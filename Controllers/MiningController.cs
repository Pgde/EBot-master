using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;


namespace Controllers
{
    public class MiningController : BaseController
    {
        int stationbook, counti;
        double usdcapcargo, fullcapcargo, restofcargo;
        long _destinationId, _currentLocation, _currentDestGateId, currentbelt, curritemid, leereid;
        long? bookmaid;
        bool _waitforsessionChange, targetda, minersac;
        enum TravelStates
        {
            Initialise, Start, Travel, ArrivedAtDestination, Opencargall, carunload, letzgo,
            Opencargstation, Mining, warping, warphome, warpnextbelt, unload, warptobelt, travStart, changebook,
            sqlchecktime, sqlchecken
        }
        EveEntity targetast;
        EveBookmark statbm;
        TravelStates _state;
        List<long> EmptyBelts = new List<long>();
        List<EveItem> items;         // <--- Cargohold
        List<EveItem> items2;        // <--- OreHold
        List<long?> Emptysys = new List<long?>();
        int miningcount = 0;
        long itemzahl = 0;
        long itemwert = 0;
        string minersactiv = "Aus";
        int stationtrip = 0;
        List<string> skilltotrainid = new List<string>();
        List<string> skillZ = new List<string>();


        /////////////////////////////////////////////                           Dronen

        int dronenanzahl = 0;
        int droneninbay = 0;



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


        int sinctest = 3402;   // Skill sience
        int minertest = 483;



        public MiningController()
        {
            Frame.Log("Starting a new travelcontroller");
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
            switch (_state)
            {
                /////////////////////////////////////////////////////////////////////////////////////////
                case TravelStates.Initialise:




                    Frame.Client.OreHoldOfActiveShip();
                    if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenOreHoldOfActiveShip);// Wenn ja
                        Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                        Frame.Log("Open Cargo of Activ Ship");                                                                  // Logbuchausgabe das das inventory geöffnet wurde             
                        break;
                    }
                    Frame.Client.OreHoldOfActiveShip();
                   EveInventoryContainer cargoho = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                    usdcapcargo = cargoho.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                    fullcapcargo = cargoho.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                    Frame.Log(usdcapcargo);
                    Frame.Log(fullcapcargo);



                    if (Frame.Client.Session.InSpace)                                                                           // Wenn wir im WEltraum sind gehe zur mining State
                    {
                        _state = TravelStates.letzgo;
                        Frame.Log("State zu letzgo");
                         break;
                    }


                    if (Frame.Client.Session.InStation)                                                                           // Wenn wir im WEltraum sind gehe zur mining State
                    {
                        if (usdcapcargo != 0)                                                                          // Wenn das cargo voll gehe heim
                        {
                            _state = TravelStates.unload;                                                                               // Abladen
                            Frame.Log("State zu unload");
                            break;
                        }
                        _state = TravelStates.letzgo;                                                                            //  Losfliegen minen gehen :D
                        Frame.Log("State zu letzgo");
                        break;
                    }
                    break;

               
                /////////////////////////////////////////////////////////////////////////////////////////


                case TravelStates.Mining:
                    if (miningcount == 10)
                    {
                        Frame.Log("Starte SQLtimecheck");
                        //  sqltimecheck();
                        miningcount = 0;
                    }
                    miningcount = (miningcount + 1);


                    Frame.Log("Travelstate.Mining");
                    Frame.Log("Miningcount " + miningcount.ToString());
                    if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                    {                                                                                                          // Wenn ja
                        Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                        Frame.Log("Open Cargo of Activ Ship");                                                                 // Logbuchausgabe das das inventory geöffnet wurde
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));                                     // Warte zwischen 2 und 3.5 Secunden
                        break;
                    }
                    Frame.Client.OreHoldOfActiveShip();
                    if (Frame.Client.IsUnifiedInventoryOpen == true)                                                            // Checken ob Inventory Fenster geöffnet ist 
                    {                                                                                                           // Wenn ja
                        EveInventoryContainer cargoho2 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                        usdcapcargo = cargoho2.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                        fullcapcargo = cargoho2.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                        Frame.Log(usdcapcargo);
                        Frame.Log(fullcapcargo);
                        double carggoo = (fullcapcargo * 0.95);
                        Frame.Log("Maximal ladung(95% vom cargo" + carggoo);
                        if (usdcapcargo > carggoo)                                                                          // Wenn das cargo voll gehe heim
                        {
                            Frame.Log("TravelState.Warphome");
                            _state = TravelStates.warphome;                                                                     // State um zurück zur Stations Bookmark zu warpen
                             break;
                        }
                    }

                    // Wenn nicht suche iene liste mit Asterioden raus
                    List<EveEntity> test3 = Frame.Client.Entities;                                                                   // Und sortier sie nach namen distanz etc

                    if (targetast != null)
                    {
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
                        i.Name.ToLower().Contains("scordite")                                                                            // Magic dont touch
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
                        if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                        {                                                                                                          // Wenn ja
                            Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                            Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                            Frame.Log("Open Cargo of Activ Ship / Mining");                                                                 // Logbuchausgabe das das inventory geöffnet wurde
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));                                     // Warte zwischen 2 und 3.5 Secunden
                            break;
                        }
                        Frame.Client.OreHoldOfActiveShip();
                        if (Frame.Client.IsUnifiedInventoryOpen == true)                                                            // Checken ob Inventory Fenster geöffnet ist 
                        {                                                                                                           // Wenn ja

                            EveInventoryContainer cargoho3 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                            usdcapcargo = cargoho3.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                            fullcapcargo = cargoho3.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                            Frame.Log(" Used Cargo " + usdcapcargo);                                                                // Logausgabe Verbrauchtes Cargo
                            Frame.Log(" Used Cargo " + fullcapcargo);                                                               // Logausgabe Cargo insgesammt
                            restofcargo = (fullcapcargo * 0.95);                                                                     // Berechne 80% Des gesamten Cargos
                            Frame.Log(" 95% Cargo entspricht " + restofcargo);                                                       // Logausgabe Cargo insgesammt
                            if (usdcapcargo > restofcargo)                                                                          // Wenn das benutze cargo 80% übersteigt
                            {
                                Frame.Log("");
                                minersactiv = "Aus";
                                EmptyBelts.Add(currentbelt);
                                _state = TravelStates.warphome;                                                                     // State um zurück zur Stations Bookmark zu warpen
                                break;
                            }
                            else
                            {
                                minersactiv = "Aus";
                                _state = TravelStates.warptobelt;                                                                 // Warpe zum nächsten Mining Belt Bookmark
                                break;
                            }
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
                                targetast.LockTarget();                                                                                 // Target Locken
                                Frame.Log("Locke Target");
                            }
                        }
                        ////// Miner
                        List<EveModule> minerlist = Frame.Client.GetActiveShip.Miners;
                        int cnt = minerlist.Where(i => (i.IsActive == true)).Count();
                        if (minerlist.Count > cnt)
                        {
                            if (minersactiv == "Aus") { minersactiv = "An"; }

                            minerlist.Where(i => (i.IsActive == false)).FirstOrDefault().Activate(targetast.Id);
                            Frame.Log("Miner Aktiviern");                                                               // Logausgabe Miner aktviviern
                            break;
                        }
                    }
                    _state = TravelStates.Mining;                                                                                    // Wieder zu Mining gehen
                    break;
                /////////////////////////////////////////////////////////////////////////////////////////




                case TravelStates.warphome:                                                                                         // Heimwarpen
                    minersactiv = "Aus";
                    if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                    {                                                                                                          // Wenn ja
                        Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                        Frame.Log("Open Cargo of Activ Ship / Mining");                                                                 // Logbuchausgabe das das inventory geöffnet wurde
                        break;

                    }
                    if (Frame.Client.Session.InStation)                                                                             // Bin ich in der station? 
                    {                                                                                                                // Wenn ja
                        Frame.Log("In Station angekommen");                                                                          // Log
                        _state = TravelStates.unload;                                                                                // setze state zu unload
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
                            _state = TravelStates.warphome;                                                                         // Setze state auf warphome
                            break;                                                                                                  // break
                        }
                        else
                        {
                            List<EveBookmark> mybook = Frame.Client.GetMyBookmarks();                                        // Erstelle liste mit aktuellen Bookmarks                           
                            statbm = mybook.Where(i => (i.Title.Contains("station"))).FirstOrDefault();
                            Frame.Log("Warp to Station");
                            statbm.WarpTo();
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(4500, 6500));                                     // Warte zwischen 4.5 und 6.5 Secunden
                            _state = TravelStates.warphome;
                          break;
                        }
                    }
                    _state = TravelStates.warphome;
                    break;
                /////////////////////////////////////////////////////////////////////////////////////////



                case TravelStates.warping:

                    if (Frame.Client.GetActiveShip.ToEntity.MovementMode == EveEntity.EntityMovementState.InWarp)
                    {
                        break;
                    }
                    Frame.Log("wechsel state zu mining state");
                    _state = TravelStates.Mining;
                   Frame.Log("Starte SQLtimecheck");
                    //          sqltimecheck();
                    break;
                /////////////////////////////////////////////////////////////////////////////////////////



                case TravelStates.letzgo:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 50000));
                    targetast = null;                                                                                       // Setze Astroiden Target == Null
                    if (Frame.Client.Session.InStation)
                    {

                        Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdExitStation);
                        Frame.Log("Undocking from station");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 25000));
                    }
                    {
                        if (Frame.Client.Session.InSpace)
                            _state = TravelStates.warptobelt;
                      }

                    Frame.Log("Starte SQLtimecheck");
                    //      sqltimecheck();
                    break;




                case TravelStates.warptobelt:

                    if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                    {                                                                                                          // Wenn ja
                        Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                        Frame.Log("Open Cargo of Activ Ship / Mining");
                        break;                              // Logbuchausgabe das das inventory geöffnet wurde

                    }


                    var belts = Frame.Client.Entities.Where(i => i.Group == Group.AsteroidBelt && !i.Name.ToLower().Contains("ice") && !EmptyBelts.Contains(i.Id));
                    var belt = belts.OrderBy(x => x.Distance).FirstOrDefault();
                    if (belt == null)
                    {
                        Frame.Log("Alle belts auf Blacklist oder Keine Belts im System");
                        _state = TravelStates.changebook;
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
                        _state = TravelStates.warping;
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
                            _state = TravelStates.warptobelt;
                           break;
                        }
                        else
                        {
                            _state = TravelStates.Mining;
                         }
                    }

                    break;


                case TravelStates.unload:
               
                    Frame.Client.OreHoldOfActiveShip();
                    if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory); // Wenn ja
                        Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                        //     Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                        Frame.Log("Open Cargo of Activ Ship / Mining");                                                                 // Logbuchausgabe das das inventory geöffnet wurde
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenOreHoldOfActiveShip);
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
              

                        _state = TravelStates.unload;                                                                                              // wiederhole unload
                       break;
                    }                                                                                                                               // Wenn kein items mehr da ist / oder da war 

                        Frame.Client.GetItemHangar();
                        List<EveItem> itemlischt = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                        EveItem itemsZZ = itemlischt.Where(x => x.Stacksize > 20000).FirstOrDefault();
                        if (itemsZZ == null)
                        {
                            Frame.Log("Keine items in der menge vorrätig");
                        }
                        
                        if (itemsZZ != null)
                        {
                            
                            int verkaufsintemszahl = (itemsZZ.Quantity);
                            sellitemsZ(verkaufsintemszahl, itemsZZ);
                            Frame.Log("Verkaufe itemZZ typ id =  " + itemsZZ.TypeId);
                            Frame.Log("Verkaufe items" + verkaufsintemszahl + "  " + itemsZZ.Quantity);
                        }
                     
                        


                    stationtrip = (stationtrip + 1);
                    Frame.Log("keine items mehr wieder losfliegen");                                                                                // Log buchausgabe
                    _state = TravelStates.letzgo;                                                                                                   // Abdocken und losfliegen
                  
                    break;



                case TravelStates.changebook:
                    EmptyBelts.Clear();


                    Frame.Log("state changebook");

                    List<EveBookmark> test2 = Frame.Client.GetMyBookmarks();
                    EveBookmark booky1 = test2.Where(x => !Emptysys.Contains(x.LocationId)).FirstOrDefault();
                    bookmaid = booky1.LocationId;
                    Frame.Log(bookmaid);
                    Frame.Log(booky1.LocationId + " = " + Frame.Client.Session.LocationId);
                    if (booky1.LocationId == Frame.Client.Session.LocationId)
                    {
                        Frame.Log("booky == momentane Loc");
                        Emptysys.Add(bookmaid);
                        _state = TravelStates.changebook;                                                                                                   // Abdocken und losfliegen
                        break;
                    }

                    booky1.SetDestination();

                    _state = TravelStates.travStart;
                    break;


                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////                           Travling to another Bookmark System             /////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                case TravelStates.travStart:

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

                case TravelStates.ArrivedAtDestination:
                    Frame.Log("Destination reached");
                    _state = TravelStates.letzgo;
                    break;



            }


        }

        public void buyitems(int typid,int menge)
        {
            Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenMarket);

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

             public void sellitemsZ(int menge, EveItem typeid)
             {


                 Frame.Client.refreshorders(typeid);
                 List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();

                 List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == typeid.TypeId).Where(x => x.jumps < 5).Where(x => x.bid == true).Where(x => x.range == -1).ToList();
                 EveMarketOrder marketitem = marketitemZ.OrderByDescending(x => x.price).FirstOrDefault();

                 if (marketitem == null)
                 {
                     marketitemZ = markyord.Where(x => x.typeID == typeid.TypeId).OrderByDescending(x => x.jumps).Where(x => x.bid == true).Where(x => x.range == -1).ToList();
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
                 marketitem.sell(menge, typeid);

             }
        

           public void dronencheck()
        {
            
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