using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;

namespace Controllers
{
    public class TravelController : BaseController
    {
        long _destinationId, _currentLocation, _currentDestGateId;
        bool _waitforsessionChange;

        enum TravelStates { Initialise, Start, Travel, ArrivedAtDestination }

        TravelStates _state;

        public TravelController()
        {
            Frame.Log("Starting a new travelcontroller");
        }
        public TravelController(long destinationId)
            : this()
        {
            _destinationId = destinationId;
        }

        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_state)
            {
                case TravelStates.Initialise:
                    _destinationId = _destinationId > 0 ? _destinationId : Frame.Client.GetLastWaypointLocationId();
                    if (_destinationId == -1 || Frame.Client.Session.LocationId == _destinationId)
                    {
                        Frame.Log("No destination found, shutting down");
                        _state = TravelStates.ArrivedAtDestination;
                        return;
                    }
                    if (Frame.Client.GetLastWaypointLocationId() == -1)
                    {
                        Frame.Log("Setting destination");
                        Frame.Client.SetDestination(_destinationId);
                    }
                    _state = TravelStates.Start;
                    break;


                case TravelStates.Start:
                    _state = TravelStates.Travel;
                    if (Frame.Client.Session.InStation)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdExitStation);
                        Frame.Log("Undocking from station");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 25000));
                    }
                    break;
                case TravelStates.Travel:
                    if (_destinationId == -1 || Frame.Client.Session.LocationId == _destinationId)
                    {
                        _state = TravelStates.ArrivedAtDestination;
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
                case TravelStates.ArrivedAtDestination:
                    Frame.Log("Destination reached");
                    break;
            }
        }
    }
}
