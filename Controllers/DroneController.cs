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


        int dronesinbay = 0;




        EveEntity astrodronen;
        public static EveEntity astro { get; set; }
        public static bool dronenaktiviern { get; set; }
        public static bool dronecontrolleraktiv { get; set; }

  
        List<EveWindow> Windows = new List<EveWindow>();

        public DroneController()
        {
            Frame.Log("Starting a new Drone Controller");
            DroneController.dronecontrolleraktiv = true;
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
                         _localPulse = DateTime.Now.AddMilliseconds(GetRandom(8000, 9000));
                        break;
                    }
                    if (Frame.Client.GetActiveShip.DronesInBay > 0)
                    {
         //               DroneController.dronen = true;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));

                    _States.DroneState = DroneState.Idle;
                    break;


                case DroneState.Idle:
                    if (Frame.Client.Session.InStation == true)
                    {
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
                        break;
                    }
                _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
                Frame.Log("Drones ...Ídle");

                if (astro != null)
                {
                    _States.DroneState = DroneState.Startdrones;
                    astrodronen = DroneController.astro;
                    break;
                }

                List<EveEntity> dronis = Frame.Client.GetActiveShip.ActiveDrones;
                int droni = dronis.Count;
                Frame.Log("Drones  count =   " + droni);

                int dronesbay = Frame.Client.GetActiveShip.DronesInBay;
                Frame.Log("Drones  count in bay =   " + dronesbay);
                 break;



                case DroneState.Startdrones:

                 _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
               if (Frame.Client.getdronbay() == false)
                 {
                     Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
                     break;
                 }
                         List<EveEntity> test3 = Frame.Client.Entities;                                                                   // Und sortier sie nach namen distanz etc
                         EveEntity dista2 = test3.Where(i => (i.Id == astro.Id)).FirstOrDefault();
                         double dista = dista2.Distance;

               if (dista > 7000)
               {
                   Frame.Log("Debug distanz " + dista);
                   break;
               }
                 dronesinbay = Frame.Client.GetActiveShip.DronesInBay;
                 if (dronesinbay > 0)
                 {
                     Frame.Log("Drones Startet");
                     Frame.Client.GetActiveShip.ReleaseDrones();
                     break;
                 }
     //               if (dronenaktiviern == false)
     //               {
                        Frame.Log("Drones start mining");
                     Frame.Client.DroneMineRepeatedly();
                    dronenaktiviern = true;                              // Dronen aktiv
                    _States.DroneState = DroneState.dronesatwork;
                    break;
     //             }
     //               break;


                case DroneState.dronesatwork:
                      
                    if (astrodronen.Id == DroneController.astro.Id)
                    {
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2000));
                        break;
                    }
 
                      dronenaktiviern = false;                              // Dronen aktiv
                     _States.DroneState = DroneState.dronesback;
                    break;




                case DroneState.dronesback:
                    if (Frame.Client.GetActiveShip.ActiveDrones.Count > 0)
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


/*
     double dist2 = Frame.Client.Entities.Where(i => i.Id == targetast.Id).FirstOrDefault().Distance;
                        if (DroneController.dronenaktiviern == false && dist2 < 2000)
                        {
                            _States.DroneState = states.DroneState.Startdrones;   // Target Locken
                        }
*/