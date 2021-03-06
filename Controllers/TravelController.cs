﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using Controllers.states;
namespace Controllers
{
    public class TravelController : BaseController
    {
        long _destinationId, _currentLocation, _currentDestGateId;
        bool _waitforsessionChange;
        public static long desti { get; set; }
        public DateTime idelout;

        public TravelController()
        {
            Frame.Log("Starting a new travelcontroller");
        }


        public override void DoWork()
        {
            if ( _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_States.TravelerState)
            {

                case TravelerState.idel:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4000, 6000));
                    break;


                case TravelerState.Initialise:
                    idelout = DateTime.Now.AddMinutes(3);
                    _currentLocation = 0;
                    _destinationId = desti;
                    _destinationId = _destinationId > 0 ? _destinationId : Frame.Client.GetLastWaypointLocationId();
                    if (_destinationId == -1 || Frame.Client.Session.LocationId == _destinationId)
                    {
                        Frame.Log("No destination found, ");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4000, 6000));
                        _States.TravelerState = TravelerState.wait;
                        break;
                    }
                  
                        Frame.Log("Setting destination");
                        Frame.Client.SetDestination(_destinationId);
                    
                    _States.TravelerState = TravelerState.Start;
                    break;


                case TravelerState.Start:
                    _States.TravelerState = TravelerState.Travel;
                    Frame.Log("travel");
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
                        Frame.Log("bug");
                        _States.TravelerState = TravelerState.ArrivedAtDestination;
                        break;
                    }
                    Frame.Log("test");
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
                            break;
                        }
                        _waitforsessionChange = true;
                        Frame.Log(Frame.Client.GetNextWaypointStargate());
                        destEntity = Frame.Client.GetNextWaypointStargate();
                        _currentDestGateId = destEntity.Id;
                        if (destEntity.Group == Group.Stargate)
                        {
                            destEntity.JumpStargate();
                            Frame.Log("Warping to and jumping through stargate - " + destEntity.Name);
                            idelout = DateTime.Now.AddMinutes(3);
                        }
                        else if (destEntity.Group == Group.Station)
                        {
                            destEntity.Dock();
                            Frame.Log("Warping to and docking at station - " + destEntity.Name);
                            idelout = DateTime.Now.AddMinutes(3);
                        }
                        _currentLocation = Frame.Client.Session.LocationId;
                        
                    }
                    if (DateTime.Now > idelout)
                    {
                        _currentLocation = 0;
                    }

                    break;


                case TravelerState.ArrivedAtDestination:
                    MysqlController.lastaction = DateTime.Now; 
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4000, 6000));
                    Frame.Log("Destination reached");
                    break;

                case TravelerState.wait:
                    MysqlController.lastaction = DateTime.Now; 
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4000, 6000));
                    Frame.Log("waiting");
                    break;
            }
        }
    }
}
