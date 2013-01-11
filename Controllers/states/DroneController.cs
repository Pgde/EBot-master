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
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));

                    Frame.Client.Getandopenwindow("Orehold");

           //    _state = TravelStates.Start;
                    _States.DroneState = DroneState.Idle;
                    break;


                case DroneState.Idle:

                _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
                Frame.Log("Ídle");
                 break;



                case DroneState.Startdrones:
                 Frame.Log("Klappt / Start");
                 _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));
                 if (Frame.Client.Session.InStation)
                 {
                   
                 }
                
                 Windows = Frame.Client.GetWindows;
                 foreach (EveWindow tmp in Windows)
                 {
                     Frame.Log("EveWinow Name =   " + tmp.Name + "EveWindow Typ =   " + tmp.Type);

                 }

                     EveWindow winni = Windows.Where(x => x.Name.Contains("InventorySpace")).FirstOrDefault(); // kann auch leer sein
                 if (Frame.Client.Session.InSpace == true)
                 {
                 Windows = Frame.Client.GetWindows;
                  winni = Windows.Where(x => x.Name.Contains("InventorySpace")).FirstOrDefault();
                 }
                if (winni == null)
                {
                    Frame.Log("Kein inv gefunden öffne");
                    Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);
                    break;
                }
                if (Frame.Client.Session.InSpace == false)

                {
                  Windows = Frame.Client.GetWindows;
                  winni = Windows.Where(x => x.Name.Contains("InventorySpace")).FirstOrDefault();
                 }
                if (winni == null)
                {
                 // Inventory öffnen
                    // break;
                }
                Frame.Log("Inv offen / Fertig");
                 
                //  winni = Windows.Where(x => x.Name.Contains("ShipOreHold")).FirstOrDefault();
                 //       EveWindow wini = Windows.Where(x => x.Name == "");
                 break;
             
            }
        }
    }
}
