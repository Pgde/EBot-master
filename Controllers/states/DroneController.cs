using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using EveModel;
using global::Controllers.states;

namespace Controllers
{
    public class DroneController : BaseController
    {

        long _destinationId, _currentLocation, _currentDestGateId;
        bool _waitforsessionChange;

        int dronesinbay = 0;


        public DroneController()
        {
            Frame.Log("Starting a new Mining Controller");
        }
     

        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_States.DroneState)
            {
                case DroneState.Initialise:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));


           //    _state = TravelStates.Start;
                    _States.DroneState = DroneState.Idle;
                    break;


                case DroneState.Idle:
                    bool staat = _States.DroneState == DroneState.Idle;
                       while (staat == true)
                       {
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
                        break;

                       }

                    break;
            }
        }
    }
}
