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
        public static bool dronen { get; set; }
       
        List<EveWindow> Windows = new List<EveWindow>();

        public DroneController()
        {
            Frame.Log("Starting a new Drone Controller");
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

                    if (Frame.Client.getdronbay() == false)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
                        break;
                    }

                    if (Frame.Client.Session.InStation == true)
                    {
                         _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
                        break;
                    }
                    if (Frame.Client.GetActiveShip.DronesInBay > 0)
                    {
                        DroneController.dronen = true;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));

                    _States.DroneState = DroneState.Idle;
                    break;


                case DroneState.Idle:
                    dronen = false;
                _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
                Frame.Log("Drones ...Ídle");
                 break;



                case DroneState.Startdrones:
                   
                 _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
               if (Frame.Client.getdronbay() == false)
                 {
                     Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
                     break;
                 }
                 dronesinbay = Frame.Client.GetActiveShip.DronesInBay;
                 if (dronesinbay > 0)
                 {
                     Frame.Log("Drones Startet");
                     Frame.Client.GetActiveShip.ReleaseDrones();
                     break;
                 }
                    if (dronen == false)
                    {
                        Frame.Log("Drones start mining");
                 Frame.Client.DroneMineRepeatedly();
                    dronen = true;                              // Dronen aktiv
                    break;
                  }

                    break;

                case DroneState.dronesback:
                    if (Frame.Client.GetActiveShip.DronesInBay != 0)
                    {
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                        Frame.Log("Hole dronen zurück");
                        Frame.Client.recallalldrones();
                        break;
                    }
                    Frame.Log("Setzte state auf Idle");
                  _States.DroneState = states.DroneState.Idle;
                     break;
                 }
             
            }
        }
    }

