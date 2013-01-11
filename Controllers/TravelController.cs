using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using EveModel;
using global::Controllers.states;

namespace Controllers
{
    public class TravelController : BaseController
    {
        
        long _destinationId, _currentLocation, _currentDestGateId;
        bool _waitforsessionChange;
       
       
        

        public TravelController()
        {
            Frame.Log("Starting a new travelcontroller");
        }
        public TravelController(long destinationId)
            : this()
        {
           // _States.TravelerState = TravelerState.Initialise;
            _destinationId = destinationId;
        }

        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_States.TravelerState)
            {
                case TravelerState.Initialise:



         //           test = Frame.Client.GetMySkills();
          //          test2 = Frame.Client.GetMyQueue();
            //       EveSkill tmp =  test.Where(i=> i.Name == "Mechanics").FirstOrDefault();
             //      Frame.Client.AddSkillToEnd(tmp, tmp.Skilllvl);

            //        if (!Frame.Client.isconnecting())
             //       {
              //          Frame.Client.logintest("kurri92", "OQttV9Jp");
               //     }

                //    if (Frame.Client.isconnecting())
                //    {
                  //      break;
                   // }
                //    List<EveItem> items = new List<EveItem>();
               //     items = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                //    Frame.Client.tryfit(items);

              //      Frame.Client.StripFitting(Frame.Client.GetActiveShip.ItemId);
                    EveObject itemid = Frame.Client.GetShipHangar().Items.LastOrDefault();    
              Frame.Client.TryActivateShip(itemid);
                _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2500, 5000));
                   
            //    test3 =   Frame.Client.GetCharslots();
           //         if (Frame.Client.selectchar("teslon mawa"))
             //       {
                 //       _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 8000));
               //         break;
                   // }
              //      Frame.Client.DroneMineRepeatedly();

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(8000, 8000));
                  break;
             _destinationId = _destinationId > 0 ? _destinationId : Frame.Client.GetLastWaypointLocationId();
                    if (_destinationId == -1 || Frame.Client.Session.LocationId == _destinationId)
                    {
                        Frame.Log("No destination found, shutting down");
                        _States.TravelerState = TravelerState.ArrivedAtDestination;
                        return;
                    }
                    if (Frame.Client.GetLastWaypointLocationId() == -1)
                    {
                        Frame.Log("Setting destination");
                       
                        Frame.Client.SetDestination(_destinationId);
                    }
                    _States.TravelerState = TravelerState.Start;
                    break;
                case TravelerState.Start:
                    _States.TravelerState = TravelerState.Travel;
                    if (Frame.Client.Session.InStation)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdExitStation);
                        Frame.Log("Undocking from station");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 25000));
                    }
                    break;
                case TravelerState.Travel:
                    if (_destinationId == -1 || Frame.Client.Session.LocationId == _destinationId)
                    {
                        _States.TravelerState = TravelerState.ArrivedAtDestination;
                        return;
                    }
                    EveEntity destEntity = Frame.Client.Entities.Where(ent => ent.Id == _currentDestGateId).FirstOrDefault();
                    // Should I cloak
                    if (Frame.Client.GetActiveShip.ToEntity != null && Frame.Client.GetActiveShip.ToEntity.MovementMode == EveEntity.EntityMovementState.InWarp &&
                        Frame.Client.Session.SystemSecurity < 0.5 && Frame.Client.GetActiveShip.HasTravelCloak && !Frame.Client.GetActiveShip.Cloak.IsActive)
                    {
                        if (destEntity != null && destEntity.Distance > 100000)
                            Frame.Client.GetActiveShip.ManipulateModuleGroup(Group.CloakingDevice, true);
                    }
                    // Should I uncloak
                    if (Frame.Client.GetActiveShip.HasTravelCloak && !Frame.Client.GetActiveShip.Cloak.IsActive && destEntity != null && destEntity.Distance < 100000)
                    {
                        Frame.Client.GetActiveShip.ManipulateModuleGroup(Group.CloakingDevice, false);
                    }
                    if (Frame.Client.Session.LocationId != _currentLocation && Frame.Client.Session.IsItSafe)
                    {
                        if (_waitforsessionChange)
                        {
                            Frame.Log("Session changed");
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4000, 6000));
                            _waitforsessionChange = false;
                            return;
                        }
                        _waitforsessionChange = true;
                        destEntity = Frame.Client.GetNextWaypointStargate();
                        _currentDestGateId = destEntity.Id;
                        if (destEntity.Group == Group.Stargate)
                        {
                            destEntity.JumpStargate();
                            Frame.Log("Warping to and jumping through stargate - " + destEntity.Name);
                        }
                        else if (destEntity.Group == Group.Station)
                        {
                            destEntity.Dock();
                            Frame.Log("Warping to and docking at station - " + destEntity.Name);
                        }
                        _currentLocation = Frame.Client.Session.LocationId;
                    }
                    break;
                case TravelerState.ArrivedAtDestination:
                    Frame.Log("Destination reached");
                    IsWorkDone = true;
                    break;
            }
        }
    }
}
