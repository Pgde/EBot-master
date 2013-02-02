using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;
using Controllers.states;


namespace Controllers
{
    public class MiningController : BaseController
    {
     //   int stationbook, counti;
        double usdcapcargo, fullcapcargo, restofcargo;
        long _destinationId, _currentLocation, _currentDestGateId, currentbelt;//, curritemid, leereid;
        long? bookmaid;
        bool _waitforsessionChange, targetda;//, minersac;
        enum TravelStates
        {
            Initialise, Start, Travel, ArrivedAtDestination, Opencargall, carunload, letzgo, schelling,
            Opencargstation, Mining, warping, warphome, warpnextbelt, unload, warptobelt, travStart, changebook,
            sqlchecktime, sqlchecken
        }
        EveEntity targetast;
        EveBookmark statbm;
    //    TravelStates _state;
        List<long> EmptyBelts = new List<long>();
        List<EveItem> items;         // <--- Cargohold
       // List<EveItem> items2;        // <--- OreHold
        List<long?> Emptysys = new List<long?>();
        int miningcount = 0;
        long itemzahl = 0;
        long itemwert = 0;
        string minersactiv = "Aus";
        int stationtrip = 0;
        List<string> skilltotrainid = new List<string>();
        List<string> skillZ = new List<string>();
        List<long> syssis = new List<long>();
        double verkaufswertinsg;
        int verkaufsintemszahl;
        EveMarketOrder marketitem = null;

        /////////////////////////////////////////////                           Dronen
        float maxactivdronen = 0;
      //  int dronenanzahl = 0;
      //  int droneninbay = 0;

//        bool dronen = false;

        /////////////////////////////////////////////                           Verkaufspreise ca.




        int Veldsparwert = 15;
        int ConcentratedVeldsparwert = 15;
        int DenseVeldsparwert = 16;

        int Scorditewert = 25;
        int CondensedScordite = 28;
        int MassiveScordite = 30;

        int Pyroxeres = 50;
        int SolidPyroxeres = 53;
        int ViscousPyroxeres = 60;

        int Plagioclase = 55;
        int AzurePlagioclase = 66;
        int RichPlagioclase = 25;

     //   int verkaufsintemszahl = 0;


       // int sinctest = 3402;   // Skill sience
        int minertest = 483;



        public MiningController()
        {
            Frame.Log("Starting a new MiningController");

            syssis.Add(Controllers.Settings.Settings.Instance.homesys1);                // Homesys1
            syssis.Add(Controllers.Settings.Settings.Instance.homesys2);                // Homesys2
            syssis.Add(Controllers.Settings.Settings.Instance.homesys3);                // Homesys3
            syssis.Add(Controllers.Settings.Settings.Instance.homesys4);                // Homesys4



                _States.MiningState = MiningState.wait;
        }
        public MiningController(long destinationId)
            : this()
        {
            _destinationId = destinationId;
        }



        ///       Bot Code start    ////
        ///                         ////


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_States.MiningState)
            {
                /////////////////////////////////////////////////////////////////////////////////////////
                case MiningState.Initialise:
    
           //         Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
      
                     List<EveWindow> mywindow;
                  mywindow = Frame.Client.GetWindows;
                  foreach (EveWindow tmp in mywindow)
                  {
                      Frame.Log("window name =  " + tmp.Name);
                  }
                   



                    if (Frame.Client.getinvopen() == false)
                    {
                        Frame.Client.Getandopenwindow("leer");
                        break;
                    }

                    if (Frame.Client.getoreopen() == false)
                    {

                        Frame.Client.Getandopenwindow("Orehold");
                        break;
                    }
                    EveInventoryContainer cargoho = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                    usdcapcargo = cargoho.UsedCapacity;   //bugged manchmal nullcheck fehlt                               // Variablen werden gesetzt Verbrauchtes Cargo <--
                    fullcapcargo = cargoho.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                    Frame.Log(usdcapcargo);
                    Frame.Log(fullcapcargo);



                    if (Frame.Client.Session.InSpace)                                                                           // Wenn wir im WEltraum sind gehe zur mining State
                    {
                        _States.MiningState = MiningState.letzgo;
                        Frame.Log("State zu letzgo");
                         break;
                    }


                    if (Frame.Client.Session.InStation)                                                                           // Wenn wir im WEltraum sind gehe zur mining State
                    {
                        if (usdcapcargo != 0)                                                                          // Wenn das cargo voll gehe heim
                        {
                            _States.MiningState = MiningState.unload;                                                                                                        // Abladen
                            Frame.Log("State zu unload");
                            break;
                        }
                        _States.MiningState = MiningState.letzgo;
                        //  Losfliegen minen gehen :D
                        Frame.Log("State zu letzgo");
                        break;
                    }
                    break;

               
                /////////////////////////////////////////////////////////////////////////////////////////


                case MiningState.Mining:

               

                 
                    if (Frame.Client.GetActiveShip.Miners.Count == 0)
                    {
                            _States.MiningState = MiningState.warphome;
                            break;
                    }


                    List<EveQskill> qskillmining = Frame.Client.GetMyQueue();
                    if (miningcount == 10)
                    {
                        Frame.Log("Starte SQLtimecheck");
                        //  sqltimecheck();
                        miningcount = 0;
                    }
                    miningcount = (miningcount + 1);


                    Frame.Log("Travelstate.Mining");
                    Frame.Log("Miningcount " + miningcount.ToString());


                    if (Frame.Client.getinvopen() == false)
                    {
                        Frame.Client.Getandopenwindow("leer");
                        break;
                    }

                        if (Frame.Client.getoreopen() == false)
                        {
                        Frame.Client.Getandopenwindow("Orehold");
                        break;
                          } 
                                                          
                        EveInventoryContainer cargoho2 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                        usdcapcargo = cargoho2.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                        fullcapcargo = cargoho2.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                        Frame.Log(usdcapcargo);
                        Frame.Log(fullcapcargo);
                        double carggoo = (fullcapcargo * 0.95);
                        Frame.Log("Maximal ladung(95% vom cargo" + carggoo);
                          Frame.Log(" usd cargo " + usdcapcargo);
                            Frame.Log(" qskillminingcount  =  " + qskillmining.Count);
                            double money = Frame.Client.wealth();
                    if (money > 500000)
                    {
                        Frame.Log("Genug geld vorhanden checke skillliste");
                        if (usdcapcargo != 0 && qskillmining.Count == 0)
                        {
                            if (Frame.Client.GetActiveShip.ActiveDrones.Count > 0)
                            {
                                _States.DroneState = states.DroneState.dronesback;
                                break;
                            }
                            DroneController.astro = null;
                            Frame.Log("");
                            minersactiv = "Aus";
                            //  EmptyBelts.Add(currentbelt);   // State um zurück zur Stations Bookmark zu warpen
                            _States.MiningState = MiningState.warphome;
                            break;
                        }
                            }
                        if (usdcapcargo > carggoo)                                                                          // Wenn das cargo voll gehe heim
                        {
                            if (Frame.Client.GetActiveShip.ActiveDrones.Count > 0)
                            {
                                _States.DroneState = states.DroneState.dronesback;
                                    break;
                            }
                            DroneController.astro = null;

                            Frame.Log("TravelState.Warphome");                           
                            _States.MiningState = MiningState.warphome;
                            // State um zurück zur Stations Bookmark zu warpen
                             break;
                        }
                    

                    // Wenn nicht suche iene liste mit Asterioden raus
                    List<EveEntity> test3 = Frame.Client.Entities;                                                                   // Und sortier sie nach namen distanz etc

                    if (targetast != null)
                    {
                        DroneController.astro = targetast;   // Target übergeben an dronen
                        targetda = test3.Where(i => (i.Id == targetast.Id)).Any();
                    }
                    if (targetda == false)
                    {
                      targetast = null;
                    }
                    if (targetast == null)
                    {

                        Frame.Log("suche asteroiden");
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                        && (
                        i.Name.ToLower().Contains("pyroxeres")                                                                           // Magic dont touch
                           )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }

                   


                     if (targetast == null)
                    {
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                           && (
                        i.Name.ToLower().Contains("massive scordite")                                                                            // Magic dont touch
                        )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }
                   


                         if (targetast == null)
                    {
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                           && (
                        i.Name.ToLower().Contains("condensed scordite")                                                                            // Magic dont touch
                        )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }



                    if (targetast == null)
                    {
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                           && (
                        i.Name.ToLower().Contains("scordite")                                                                            // Magic dont touch
                        )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }

                  

                           if (targetast == null)
                    {
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                        && (
                        i.Name.ToLower().Contains("concentrated veldspar")                                                                              // Magic dont touch
                        )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }

                   

                        if (targetast == null)
                    {
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                        && (
                        i.Name.ToLower().Contains("dense veldspar")                                                                              // Magic dont touch
                        )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }


                    if (targetast == null)
                    {
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                        && (
                        i.Name.ToLower().Contains("veldspar")                                                                              // Magic dont touch
                        )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }

                    if (targetast == null)
                    {
                        targetast = test3.Where(i =>
                        i.Distance < 65000
                        && (
                        i.Name.ToLower().Contains("plagioclase")                                                                                // Magic dont touch
                        )
                        ).OrderBy(i => i.Distance).FirstOrDefault();
                    }


                    if (targetast == null)
                    {
                        EmptyBelts.Add(currentbelt);

                        if (Frame.Client.getinvopen() == false)
                        {
                            Frame.Client.Getandopenwindow("leer");
                            break;
                        }

                                if (Frame.Client.getoreopen() == false)
                               {

                            Frame.Client.Getandopenwindow("Orehold");
                            break;
                              }

                            EveInventoryContainer cargoho3 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                            usdcapcargo = cargoho3.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                            fullcapcargo = cargoho3.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                            Frame.Log(" Used Cargo " + usdcapcargo);                                                                // Logausgabe Verbrauchtes Cargo
                            Frame.Log(" Used Cargo " + fullcapcargo);                                                               // Logausgabe Cargo insgesammt
                            restofcargo = (fullcapcargo * 0.95);                                                                     // Berechne 80% Des gesamten Cargos
                            Frame.Log(" 95% Cargo entspricht " + restofcargo);                                                       // Logausgabe Cargo insgesammt
                       

                           if (usdcapcargo > restofcargo)                                                                          // Wenn das benutze cargo 80% übersteigt
                            {
                                if (Frame.Client.GetActiveShip.ActiveDrones.Count > 0)
                                {
                                    _States.DroneState = states.DroneState.dronesback;
                                    break;
                                }
                                DroneController.astro = null;
                                Frame.Log("");
                                minersactiv = "Aus";
                                EmptyBelts.Add(currentbelt);   // State um zurück zur Stations Bookmark zu warpen
                                _States.MiningState = MiningState.warphome;
                                break;
                            }
                            else
                            {
                                if (Frame.Client.GetActiveShip.ActiveDrones.Count > 0)
                                {
                                    _States.DroneState = states.DroneState.dronesback;
                                    break;
                                }
                                DroneController.astro = null;
                                minersactiv = "Aus";
                                _States.MiningState = MiningState.warptobelt;  // Warpe zum nächsten Mining Belt Bookmark
                                break;
                            }
                    }
                    else
                    {
                        if (targetast.Distance > 10000)                                                                         // weiter weg als 10km
                        {
                            double dist = Frame.Client.Entities.Where(i => i.Id == targetast.Id).FirstOrDefault().Distance;
                            Frame.Log(dist);

                            targetast.Approach();
                            if (dist > 10000)
                            {
                                _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));                                     // Warte zwischen 2 und 3.5 Secunden
                                break;
                            }
                        }
                     
                        if (Frame.Client.GetActiveTargetId == -1)
                        {
                            if (!targetast.IsBeingTargeted)
                            {
                                targetast.LockTarget();
                     //           DroneController.astro = targetast;   // Target übergeben an dronen
                                Frame.Log("Locke Target");
                                break;
                            }
                        }
                        ////// Miner

                       
                        List<EveModule> minerlist = Frame.Client.GetActiveShip.Miners;
                        int cnt = minerlist.Where(i => (i.IsActive == true)).Count();
                        if (minerlist.Count > cnt)
                        {
                          
                         //   _States.DroneState = states.DroneState.Startdrones;
                            if (minersactiv == "Aus") { minersactiv = "An"; }

                            minerlist.Where(i => (i.IsActive == false)).FirstOrDefault().Activate(targetast.Id);
                            Frame.Log("Miner Aktiviern");                                                               // Logausgabe Miner aktviviern
                            break;
                        }
                            double dist33 = Frame.Client.Entities.Where(i => i.Id == targetast.Id).FirstOrDefault().Distance;
                            Frame.Log("test " + dist33);

                          
                            if (targetast.Distance < 7000)
                            {
                                if (Frame.Client.GetActiveShip.ToEntity.MovementMode != EveEntity.EntityMovementState.Stopped)
                                {
                                    Frame.Log("Schiff gestoppt");
                                    Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdStopShip);
                                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 2500));     
                                }
                                break;
                            }

                            




                    }   
                 _States.MiningState = MiningState.Mining;    // Wieder zu Mining gehen
                    break;
                /////////////////////////////////////////////////////////////////////////////////////////




                case MiningState.warphome:                                                                                         // Heimwarpen
                    minersactiv = "Aus";
                    if (Frame.Client.getinvopen() == false)
                    {
                        Frame.Client.Getandopenwindow("leer");
                        break;
                    }
                    if (Frame.Client.getoreopen() == false)
                    {

                        Frame.Client.Getandopenwindow("Orehold");
                        break;
                    }

                    if (Frame.Client.Session.InStation)                                                                             // Bin ich in der station? 
                    {                                                                                                                // Wenn ja
                        Frame.Log("In Station angekommen");                                                                          // Log
                        _States.MiningState = MiningState.unload;                                                                 // setze state zu unload
                       _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4500, 6500));                                           // Warte zwischen 4,5 und 6,5 secunden
                        break;                                                                                                      // Breake
                    }                                                                                                               // Wenn nicht in Station
                    if (Frame.Client.GetActiveShip.ToEntity.MovementMode != EveEntity.EntityMovementState.InWarp)                    // UND nicht im Warp
                    {                                                                                                               // Dann
                        if (Frame.Client.Entities.Where(i => i.Group == Group.Station).Any())                                       // und irgendwo ne Station ist
                        {                                                                                                           // Dann 
                            EveEntity station = Frame.Client.Entities.Where(i => i.Group == Group.Station).FirstOrDefault();        // Setze variablen der Station
                            Frame.Log("Docking");                                                                                   // Logbuch
                            station.Dock();                                                                                         // Docke an dieser station/Warp dahin
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 5000));                                      // Warte zwischen 3 und 5 Sekunden
                            _States.MiningState = MiningState.warphome;                                                           // Setze state auf warphome
                            break;                                                                                                  // break
                        }
                        else
                        {
                            List<EveBookmark> mybook = Frame.Client.GetMyBookmarks();                                        // Erstelle liste mit aktuellen Bookmarks                           
                            statbm = mybook.Where(i => (i.Title.Contains("station"))).FirstOrDefault();
                            Frame.Log("Warp to Station");
                            statbm.WarpTo();
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4500, 6500));                                     // Warte zwischen 4.5 und 6.5 Secunden
                            _States.MiningState = MiningState.warphome;   
                          break;
                        }
                    }
                    _States.MiningState = MiningState.warphome;  
                    break;
                /////////////////////////////////////////////////////////////////////////////////////////



                case MiningState.warping:

                    if (Frame.Client.GetActiveShip.ToEntity.MovementMode == EveEntity.EntityMovementState.InWarp)
                    {
                        break;
                    }
                    Frame.Log("wechsel state zu mining state");
                    _States.MiningState = MiningState.Mining;      
                   Frame.Log("Starte SQLtimecheck");
                    //          sqltimecheck();
                    break;
                /////////////////////////////////////////////////////////////////////////////////////////



                case MiningState.letzgo:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 50000));
                    targetast = null;                                                                                       // Setze Astroiden Target == Null
                    
                        List<EveWindow> mywindow2;
                        mywindow2 = Frame.Client.GetWindows;
                        string con = "incursion";
                        EveWindow ne = mywindow2.Where(x => x.Name.Contains(con)).FirstOrDefault();
                        if (ne != null)
                        {
                            Frame.Log("NPC in Belt WARP HOME and blacklist");
                            _States.MiningState = MiningState.changebook;
                            break;

                        }

                    if (Frame.Client.Session.InStation)
                    {

                        Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdExitStation);
                        Frame.Log("Undocking from station");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 25000));
                    }
                    {
                        if (Frame.Client.Session.InSpace)                   
                         _States.MiningState = MiningState.warptobelt;
                        }

                    Frame.Log("Starte SQLtimecheck");
                    //      sqltimecheck();
                    break;




                case MiningState.warptobelt:
                    if (Frame.Client.getinvopen() == false)
                    {
                        Frame.Client.Getandopenwindow("leer");
                        break;
                    }
                    if (Frame.Client.getoreopen() == false)
                    {

                        Frame.Client.Getandopenwindow("Orehold");
                        break;
                    }


                    var belts = Frame.Client.Entities.Where(i => i.Group == Group.AsteroidBelt && !i.Name.ToLower().Contains("ice") && !EmptyBelts.Contains(i.Id));
                    var belt = belts.OrderBy(x => x.Distance).FirstOrDefault();
                    if (belt == null)
                    {
                        Frame.Log("Alle belts auf Blacklist oder Keine Belts im System");
                        _States.MiningState = MiningState.backandaway;
                 //       _States.MiningState = MiningState.changebook;
                                                                                                                        // Dann 
                            EveEntity station = Frame.Client.Entities.Where(i => i.Group == Group.Station).FirstOrDefault();        // Setze variablen der Station
                            Frame.Log("Docking");                                                                                   // Logbuch
                            station.Dock();                                                                                         // Docke an dieser station/Warp dahin
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 5000));                                      // Warte zwischen 3 und 5 Sekunden
                                                                               // Setze state auf warphome
   
                        break;
                    }
                    currentbelt = belt.Id;
                    long countbelt = EmptyBelts.Count();
                    countbelt = countbelt - 1;
                    long countbeltZ = belts.Count();
                    countbeltZ = countbeltZ - 1;
                    Frame.Log("Belts auf Blacklist " + countbelt + " von " + countbeltZ);




                    if (Frame.Client.GetActiveShip.ToEntity.MovementMode == EveEntity.EntityMovementState.InWarp)
                    {
                        Frame.Log("Fertig gewarpt weiter zur Warping State");
                        _States.MiningState = MiningState.warping;
                        break;
                    }


                    if (Frame.Client.GetActiveShip.ToEntity.MovementMode != EveEntity.EntityMovementState.InWarp)                    // UND nicht im Warp
                    {
                        Frame.Log("Warping to Belt...");
                        long test = EmptyBelts.FirstOrDefault();
                        Frame.Log(test);
                        Frame.Log(belt.Id);
                        if (belt.Distance > 65000)
                        {
                            belt.WarpTo();
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(10000, 15000));
                            _States.MiningState = MiningState.warptobelt;
                           break;
                        }
                        else
                        {
                            _States.MiningState = MiningState.Mining;
                         }
                    }

                    break;

                case MiningState.backandaway:
                     _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4500, 6500));
                     if (Frame.Client.Session.InStation)                                                                             // Bin ich in der station? 
                     {                                                                                                                // Wenn ja
                         Frame.Log("In Station angekommen setzte neue desti");                                                                          // Log
                         _States.MiningState = MiningState.changebook;
                         break;                            // Warte zwischen 4,5 und 6,5 secunden
                     }                           
                    break;

                case MiningState.unload:

                    if (Frame.Client.GetService("marketQuote") == null)
                    {
                        break;
                    }

                    if (Frame.Client.GetService("wallet") == null)
                    {
                        break;
                    }

                    if (Frame.Client.getinvopen() == false)
                    {
                        Frame.Client.Getandopenwindow("leer");
                        break;
                    }
                    if (Frame.Client.getoreopen() == false)
                    {

                        Frame.Client.Getandopenwindow("Orehold");
                        break;
                    }

                    Frame.Log("bin bei Unload");
                    items = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip.Items;                                                     // Get itemslist check fehlt
                    Frame.Log(items.Count);                                                                                                         // Logbuch Items zahl
                    if (items.Count == 0)
                    {
                        items = Frame.Client.GetPrimaryInventoryWindow.CargoHoldOfActiveShip.Items;
                    }


                    if (items.Count != 0)                                                                                                          // Wenn items zahl ungleich 0 ist dann
                    {
                        Frame.Log("item gefunden");                                                                                                 // Logbuch items gefunden
                        EveItem itemZ = items.OrderBy(x => x.ItemId).FirstOrDefault();                                                              // Nimm das erste items in der liste
                        itemzahl = (itemZ.Quantity);
                        Frame.Log("itemzahl = " + itemzahl);
                        string namee = itemZ.TypeName;
                        Frame.Log("givenname = " + namee);

                        if (namee == "Veldspar")
                        {
                            itemwert = itemwert + (itemzahl * Veldsparwert);
                            Frame.Log(namee + " = " + (itemzahl * Veldsparwert) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Concentrated Veldspar")
                        {
                            itemwert = itemwert + (itemzahl * ConcentratedVeldsparwert);
                            Frame.Log(namee + " = " + (itemzahl * ConcentratedVeldsparwert) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Dense Veldspar")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * DenseVeldsparwert);
                            Frame.Log(namee + " = " + (itemzahl * DenseVeldsparwert) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }

                        if (namee == "Scordite")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * Scorditewert);
                            Frame.Log(namee + " = " + (itemzahl * Scorditewert) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Condensed Scordite")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * CondensedScordite);
                            Frame.Log(namee + " = " + (itemzahl * CondensedScordite) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Massive Scordite")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * MassiveScordite);
                            Frame.Log(namee + " = " + (itemzahl * MassiveScordite) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }

                        if (namee == "Pyroxeres")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * Pyroxeres);
                            Frame.Log(namee + " = " + (itemzahl * Pyroxeres) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Solid Pyroxeres")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * SolidPyroxeres);
                            Frame.Log(namee + " = " + (itemzahl * SolidPyroxeres) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Viscous Pyroxeres")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * ViscousPyroxeres);
                            Frame.Log(namee + " = " + (itemzahl * ViscousPyroxeres) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }

                        if (namee == "Plagioclase")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * Plagioclase);
                            Frame.Log(namee + " = " + (itemzahl * Plagioclase) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Azure Plagioclase")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * AzurePlagioclase);
                            Frame.Log(namee + " = " + (itemzahl * AzurePlagioclase) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }
                        if (namee == "Rich Plagioclase")
                        {
                            itemwert = itemwert = itemwert + (itemzahl * RichPlagioclase);
                            Frame.Log(namee + " = " + (itemzahl * RichPlagioclase) + " = " + itemwert);
                            MysqlController.itemwert = itemwert;
                        }

                        Frame.Client.GetItemHangar();
                        Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Add(itemZ);                                                               // und füge es dem itemshangar hinzu
                        items.Remove(itemZ);
                        Frame.Client.GetPrimaryInventoryWindow.ItemHangar.StackAll();
                        Frame.Log("Stackall");
              

                        _States.MiningState = MiningState.unload;                                                                                   // wiederhole unload
                       break;
                    }                                                                                                                               // Wenn kein items mehr da ist / oder da war 

                        Frame.Client.GetItemHangar();
                        List<EveItem> itemlischt = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                        EveItem itemsZZ = itemlischt.Where(x => x.Stacksize > 1000).FirstOrDefault();
                        if (itemsZZ == null)
                        {
                            Frame.Log("Keine items in der menge vorrätig");
                        }
                        
                        if (itemsZZ != null)
                        {

                            if (Frame.Client.getmarketopen() == false)
                            {
                                Frame.Client.Getandopenwindow("market");
                                return;

                            }
                            
                            verkaufsintemszahl = (itemsZZ.Quantity);
                            if (!Frame.Client.GetService("marketQuote").IsValid)
                            {
                                break;
                            }
                        //    sellitemsZ(verkaufsintemszahl, itemsZZ);
                            Frame.Log("Verkaufe itemZZ typ id =  " + itemsZZ.TypeId);
                            Frame.Log("Verkaufe items" + verkaufsintemszahl + "  " + itemsZZ.Quantity);


                            Frame.Client.refreshorders(itemsZZ);
                            List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();
                            if (markyord.Count == 0)
                            {
                                break;
                            }
                           
                             double t = 1228;
                            List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == itemsZZ.TypeId).Where(x => x.inrange == true).Where(x => x.bid == true).ToList();
                            if (marketitemZ != null)
                            {
                                marketitem = marketitemZ.OrderByDescending(x => x.price).Where(x => x.price > 15).First();        // first or default        
                                }


                            if (marketitem == null)
                            {
                                Frame.Log("kein eintrag mit -1");
                                return;
                            }

                            Frame.Log("Marketitem Name =  " + marketitem.Name);                                                                                                // Funktion für verkaufen infos
                            Frame.Log("Marketitem Price =  " + marketitem.price);
                            Frame.Log("Marketitem Volentered =  " + marketitem.volEntered);
                            Frame.Log("Marketitem Remain =  " + marketitem.volRemaining);
                            Frame.Log("Marketitem OrderId =  " + marketitem.orderID);
                            Frame.Log("Marketitem Jumpes =  " + marketitem.jumps);
                            Frame.Log("Marketitem Range =  " + marketitem.range);
                            Frame.Log("Marketitem inrange =  " + marketitem.inrange);
                            marketitem.sell(verkaufsintemszahl, itemsZZ);
                            verkaufswertinsg = (marketitem.price * verkaufsintemszahl);
                            Frame.Log("Verkaufswert =   " + verkaufswertinsg);
                            Frame.Log("setze marketitem wieder auf null");
                            marketitem = null;
                            itemlischt.Remove(itemsZZ);
                            Frame.Log("Loesche Item aus Liste");
                            break;

                        }
                     
                        


                    stationtrip = (stationtrip + 1);
                    Frame.Log("keine items mehr wieder losfliegen");                                                                                // Log buchausgabe
                                                                        // Abdocken und losfliegen
                    _States.MiningState = MiningState.wait;
                    Frame.Log("_States.MiningState = MiningState.wait;");  
                    _States.maincontrollerState = maincontrollerStates.endminingcycle;
                    Frame.Log("_States.maincontrollerState = maincontrollerStates.endminingcycle;");
                    break;



                case MiningState.changebook:
                    EmptyBelts.Clear();


                    long desti = syssis.FirstOrDefault();
                    if (desti == Frame.Client.Session.LocationId)
                    {
                        Frame.Log("wir befinden uns in diese system");
                        int remove = Math.Min(syssis.Count, 1);
                        syssis.RemoveRange(0, remove);
                        Frame.Log("ersten listeneintrag gelöscht");
                        break;
                    }
                    Frame.Log("setze Destination");
                    Frame.Client.SetDestination(desti);


                   


                    /*
                    List<EveBookmark> test2 = new List<EveBookmark>();
                     test2 = Frame.Client.GetMyBookmarks();
                     if (test2.Count <= 0)
                     {
                         _States.MiningState = MiningState.wait;
                         //keine bookmarks mehr
                     }
                    EveBookmark booky1 = test2.Where(x => !Emptysys.Contains(x.LocationId)).FirstOrDefault();
                    bookmaid = booky1.LocationId;
                    Frame.Log(bookmaid);
                    Frame.Log(booky1.LocationId + " = " + Frame.Client.Session.LocationId);
                    if (booky1.LocationId == Frame.Client.Session.LocationId)
                    {
                        Frame.Log("booky == momentane Loc");
                        Emptysys.Add(bookmaid);
                        _States.MiningState = MiningState.changebook;               // Abdocken und losfliegen
                        break;
                    }
                    
                    booky1.SetDestination();
                     * */

                    _States.MiningState = MiningState.travStart;
                    break;


                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////                           Travling to another Bookmark System             /////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*
                case MiningState.schelling:


                    Frame.Client.refreshorders(itemsZZ);

                 if (Frame.Client.GetCachedOrders() == null)
                 {
                     break;
                 }
                 List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();

                 List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == itemsZZ.TypeId).Where(x => x.inrange == true).Where(x => x.bid == true).ToList();
                 if (marketitemZ != null)
                 {
                      marketitem = marketitemZ.OrderByDescending(x => x.price).FirstOrDefault();
                 }

                        
                 if (marketitem == null)
                 {
                     Frame.Log("kein eintrag mit -1");
                     return;
                 }

                 Frame.Log("Marketitem Name =  " + marketitem.Name);                                                                                                // Funktion für verkaufen infos
                 Frame.Log("Marketitem Price =  " + marketitem.price);
                 Frame.Log("Marketitem Volentered =  " + marketitem.volEntered);
                 Frame.Log("Marketitem Remain =  " + marketitem.volRemaining);
                 Frame.Log("Marketitem OrderId =  " + marketitem.orderID);
                 Frame.Log("Marketitem Jumpes =  " + marketitem.jumps);
                 Frame.Log("Marketitem Range =  " + marketitem.range);
                 Frame.Log("Marketitem inrange =  " + marketitem.inrange);
                 marketitem.sell(verkaufsintemszahl, itemsZZ);
                 verkaufswertinsg = (marketitem.price * verkaufsintemszahl);
                 Frame.Log("Verkaufswert =   " +  verkaufswertinsg );

                    break;
                    */

                case MiningState.travStart:

                    if (Frame.Client.Session.LocationId != syssis.FirstOrDefault())
                    {
                        TravelController.desti = syssis.FirstOrDefault();
                        _States.TravelerState = TravelerState.Initialise;
                        _States.maincontrollerState = maincontrollerStates.homesysarriv;
                    }
                    else
                    {
                        _States.maincontrollerState = maincontrollerStates.Idle;
                        _States.MiningState = MiningState.Initialise;
                    }

/*
                    _currentLocation = 0;

                     if (Frame.Client.Session.InStation)
                    {

                        Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdExitStation);
                        Frame.Log("Undocking from station");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 25000));
                         break;
                    }

                    _destinationId = _destinationId > 0 ? _destinationId : Frame.Client.GetLastWaypointLocationId();
                    if (_destinationId == -1 || Frame.Client.Session.LocationId == _destinationId)
                    {
                        Frame.Log("No destination found, shutting down");
                        _States.MiningState = MiningState.ArrivedAtDestination; 
                        return;
                    }
                    if (Frame.Client.GetLastWaypointLocationId() == -1)
                    {
                        Frame.Log("Setting destination");
                        Frame.Client.SetDestination(_destinationId);
                    }
                    _States.MiningState = MiningState.Start; 
 * */
                    break;


                case MiningState.Start:

                   
                    if (Frame.Client.Session.InStation)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdExitStation);
                        Frame.Log("Undocking from station");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 25000));
                    }
                    _States.MiningState = MiningState.Travel; 
                    break;

                case MiningState.Travel:
                    if (_destinationId == -1 || Frame.Client.Session.LocationId == _destinationId)
                    {
                        _States.MiningState = MiningState.ArrivedAtDestination; 
                        EveEntity destEntity = Frame.Client.Entities.Where(ent => ent.Id == _currentDestGateId).FirstOrDefault();

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
                    }

                    break;

                case MiningState.ArrivedAtDestination:
                    Frame.Log("Destination reached");
                    _States.MiningState = MiningState.letzgo; 
                    break;

                case MiningState.wait:
                //    Frame.Log("Miningstate.wait erreicht");
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;


            }


        }

        public void buyitems(int typid,int menge)
        {



            Frame.Client.refreshorders(typid);
            List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();

            List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == typid).Where(x => x.jumps < 5).Where(x => x.bid == false).Where(x => x.range == -1).ToList();
            EveMarketOrder marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();

            if (marketitemZ == null)
            {
                marketitemZ = markyord.Where(x => x.typeID == minertest).OrderByDescending(x => x.jumps).Where(x => x.bid == false).Where(x => x.range == -1).ToList();
                marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();
            }
            if (marketitemZ == null)
            {
                Frame.Log("Kein items in der Station und auchnicht in der nähe (nicht im Storecach)");
                return;
            }
                       
            Frame.Log("Marketitem Name =  " + marketitem.Name);                                                                                                // Funktion für verkaufen infos
            Frame.Log("Marketitem Price =  " + marketitem.price);
            Frame.Log("Marketitem Volentered =  " + marketitem.volEntered);
            Frame.Log("Marketitem Remain =  " + marketitem.volRemaining);
            Frame.Log("Marketitem OrderId =  " + marketitem.orderID);
            Frame.Log("Marketitem Jumpes =  " + marketitem.jumps);
            Frame.Log("Marketitem Range =  " + marketitem.range);
            marketitem.buy(menge);


          

        }

        /*
             public void sellitemsZ(int menge, EveItem typeid)
             {


            //     return what == 'sell' and (order.jumps <= order.range or eve.session.stationid and order.range == -1 and order.stationID == eve.session.stationid or eve.session.solarsystemid and order.jumps == 0)

                 Frame.Client.refreshorders(typeid);

                 if (Frame.Client.GetCachedOrders() == null)
                 {
                     break;
                 }
                 List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();

                 List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == typeid.TypeId).Where(x => x.inrange == true).Where(x => x.bid == true).ToList();
                 if (marketitemZ != null)
                 {
                      marketitem = marketitemZ.OrderByDescending(x => x.price).FirstOrDefault();
                 }


                 
                 if (marketitem == null)
                 {
                     Frame.Log("kein eintrag mit -1");
                     return;
                 }

                 Frame.Log("Marketitem Name =  " + marketitem.Name);                                                                                                // Funktion für verkaufen infos
                 Frame.Log("Marketitem Price =  " + marketitem.price);
                 Frame.Log("Marketitem Volentered =  " + marketitem.volEntered);
                 Frame.Log("Marketitem Remain =  " + marketitem.volRemaining);
                 Frame.Log("Marketitem OrderId =  " + marketitem.orderID);
                 Frame.Log("Marketitem Jumpes =  " + marketitem.jumps);
                 Frame.Log("Marketitem Range =  " + marketitem.range);
                 Frame.Log("Marketitem inrange =  " + marketitem.inrange);
                 marketitem.sell(menge, typeid);
                 verkaufswertinsg = (marketitem.price * verkaufsintemszahl);
                 Frame.Log("Verkaufswert =   " +  verkaufswertinsg );

             }
        */

           public void dronencheck()
        {
          maxactivdronen =  Frame.Client.maxActiveDrones;
          Frame.Client.DroneMineRepeatedly();
          
        }
           

        }


    }




/*
                                                                                                                        items chekcne id etc die im hangar liegen
  List<EveItem> bugitems = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;


            foreach (EveItem tmp in bugitems)
            {
                Frame.Log("typenamename = " + tmp.TypeName + ",  " + "typid " + tmp.TypeId + " , " + "Givenname  = " + tmp.GivenName);
            }

*/