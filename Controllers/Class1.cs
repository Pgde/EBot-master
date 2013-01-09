using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;

namespace Controllers
{
    public class MiningController : BaseController
    {
        int stationbook, counti;
        double usdcapcargo, fullcapcargo, restofcargo;
        long _destinationId, _currentLocation, _currentDestGateId, _acshipId, targetID, stationid, currentbelt, curritemid;
        bool _waitforsessionChange, targetda, minersac;
        enum TravelStates { Initialise, Start, Travel, ArrivedAtDestination, Opencargall, carunload, letzgo, Opencargstation, Mining, warping, warphome, warpnextbelt, unload, warptobelt }
        EveEntity targetast;
        EveBookmark statbm;
        TravelStates _state;
        List<long> EmptyBelts;
        List<EveItem> items;         // <--- Cargohold
        List<EveItem> items2;        // <--- OreHold

        public MiningController()
        {
            Frame.Log("Starting a new travelcontroller");
        }
        public MiningController(long destinationId)
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
                                                                                                                /////////////////////////////////////////////////////////////////////////////////////////
                case TravelStates.Initialise:

                    Frame.Log(Frame.Client.GetActiveShip.GivenName); 
                    
                    if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                    {                                                                                                          // Wenn ja
                        Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                        Frame.Log("Open Cargo of Activ Ship");                                                                 // Logbuchausgabe das das inventory geöffnet wurde
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));                                     // Warte zwischen 2 und 3.5 Secunden
                    }
                    if (Frame.Client.Session.InSpace)                                                                           // Wenn wir im WEltraum sind gehe zur mining State
                    {
                        _state = TravelStates.Mining;
                        Frame.Log("State zu Mining geaendert");
                        break;
                    }        
                          EveInventoryContainer cargoho = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                          usdcapcargo = cargoho.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                          fullcapcargo = cargoho.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                          Frame.Log(usdcapcargo);
                          Frame.Log(fullcapcargo);
                          if (usdcapcargo == fullcapcargo)                                                                          // Wenn das cargo voll gehe heim
                          {
                              _state = TravelStates.unload;                                                                               // Abladen
                              Frame.Log("State zu unload");
                              break;
                          }
                            _state = TravelStates.letzgo;                                                                            //  Losfliegen minen gehen :D
                            Frame.Log("State zu letzgo");
                             break;
                                                                                                                 /////////////////////////////////////////////////////////////////////////////////////////
        

                case TravelStates.Mining:

                             Frame.Log("Travelstate.Mining");

                      if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                      {                                                                                                          // Wenn ja
                          Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                          Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                          Frame.Log("Open Cargo of Activ Ship");                                                                 // Logbuchausgabe das das inventory geöffnet wurde
                          _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));                                     // Warte zwischen 2 und 3.5 Secunden
                      }

                      if (Frame.Client.IsUnifiedInventoryOpen == true)                                                            // Checken ob Inventory Fenster geöffnet ist 
                      {
                          _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));                                     // Warte zwischen 2 und 3.5 Secunden
                                                                                                                                // Wenn ja
                          EveInventoryContainer cargoho2 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                          usdcapcargo = cargoho2.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                          fullcapcargo = cargoho2.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                          Frame.Log(usdcapcargo);
                          Frame.Log(fullcapcargo);
                          if (usdcapcargo == fullcapcargo)                                                                          // Wenn das cargo voll gehe heim
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
                  targetda =  test3.Where(i => (i.Id == targetast.Id)).Any();
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
                    i.Name.ToLower().Contains("veldspar")                                                                           // Magic dont touch
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
                    i.Name.ToLower().Contains("pyroxeres")                                                                              // Magic dont touch
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
                        
                        if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
                        {                                                                                                          // Wenn ja
                            Frame.Log(Frame.Client.IsUnifiedInventoryOpen);                                                        // Logbuchausgabe ob das inv geöffnet ist False/true
                            Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);                                          // Öffnet das Inventory
                            Frame.Log("Open Cargo of Activ Ship / Mining");                                                                 // Logbuchausgabe das das inventory geöffnet wurde
                            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));                                     // Warte zwischen 2 und 3.5 Secunden
                        }
                                                                                    
                        if (Frame.Client.IsUnifiedInventoryOpen == true)                                                            // Checken ob Inventory Fenster geöffnet ist 
                        {                                                                                                           // Wenn ja
                            EveInventoryContainer cargoho3 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
                            usdcapcargo = cargoho3.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
                            fullcapcargo = cargoho3.Capacity;                                                                        // Variablen werden gesetzen Cargo insgesammt <---
                            Frame.Log(" Used Cargo " + usdcapcargo);                                                                // Logausgabe Verbrauchtes Cargo
                            Frame.Log(" Used Cargo " + fullcapcargo);                                                               // Logausgabe Cargo insgesammt
                            restofcargo = (fullcapcargo * 0.8);                                                                     // Berechne 80% Des gesamten Cargos
                            Frame.Log(" 80% Cargo entspricht " + restofcargo);                                                               // Logausgabe Cargo insgesammt
                            if (usdcapcargo > restofcargo)                                                                          // Wenn das benutze cargo 80% übersteigt
                            {
                                EmptyBelts.Add(currentbelt);
                                _state = TravelStates.warphome;                                                                     // State um zurück zur Stations Bookmark zu warpen
                                break;
                            }
                            else
                            {
                              
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
                            List<EveModule> minerlist = Frame.Client.GetActiveShip.Miners;
                            int cnt = minerlist.Where(i => (i.IsActive == true)).Count();
                            if (minerlist.Count > cnt)
                            {
                                minerlist.Where(i => (i.IsActive == false)).FirstOrDefault().Activate(targetast.Id);
                                Frame.Log("Miner Aktiviern");                                                               // Logausgabe Miner aktviviern
                                break;
                            }                  
                        }
                    _state = TravelStates.Mining;                                                                                    // Wieder zu Mining gehen
                    break;
                                                                                                                    /////////////////////////////////////////////////////////////////////////////////////////


              

                case TravelStates.warphome:                                                                                         // Heimwarpen
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
                   
                    
                    // if abfrage ob station da ist


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
                    break;
                                                                                                                  /////////////////////////////////////////////////////////////////////////////////////////



                case TravelStates.letzgo:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 50000));
                    if (Frame.Client.Session.InStation)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.CmdExitStation);
                        Frame.Log("Undocking from station");
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(18000, 25000));
                    }
                    if (Frame.Client.Session.InSpace)
                        _state = TravelStates.warptobelt;
                    break;




                case TravelStates.warptobelt:
                    
                    var belts = Frame.Client.Entities.Where(i => i.Group == Group.AsteroidBelt && !i.Name.ToLower().Contains("ice"));// && !EmptyBelts.Contains(i.Id));
                    var belt = belts.OrderBy(x => x.Distance).FirstOrDefault();
                    currentbelt = belt.Id;
                    

                    if (Frame.Client.GetActiveShip.ToEntity.MovementMode == EveEntity.EntityMovementState.InWarp)
                    {
                        Frame.Log("Fertig gewarpt weiter zur Warping State");
                        _state = TravelStates.warping;
                        break;
                    }


                    if (Frame.Client.GetActiveShip.ToEntity.MovementMode != EveEntity.EntityMovementState.InWarp)                    // UND nicht im Warp
                    {
                        Frame.Log("Warping to Belt...");
                        belt.WarpTo();
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(10000, 15000));
                        _state = TravelStates.warptobelt;
                        break;
                    }
                  
                        break;


                case TravelStates.unload:
                        Frame.Log("bin bei Unload");

                        items = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip.Items;                                                     // Get itemslist
                        Frame.Log(items.Count);                                                                                                         // Logbuch Items zahl

                    if (items.Count != 0)                                                                                                          // Wenn items zahl ungleich 0 ist dann
                    {
                        Frame.Log("item gefunden");                                                                                                 // Logbuch items gefunden
                        EveItem itemZ = items.OrderBy(x => x.ItemId).FirstOrDefault();                                                              // Nimm das erste items in der liste
                        Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Add(itemZ);                                                               // und füge es dem itemshangar hinzu
                        items.Remove(itemZ);      
                         _state = TravelStates.unload;                                                                                              // wiederhole unload
                         break;
                    }                                                                                                                               // Wenn kein items mehr da ist / oder da war 
                   
/*
 * 
 *                                  OREHOLD 
 *                                  
                        items2 = Frame.Client.GetPrimaryInventoryWindow.CargoHoldOfActiveShip.Items;                                                     // Get itemslist
                        Frame.Log(items2.Count);                                                                                                         // Logbuch Items zahl

                    if (items2.Count != 0)                                                                                                          // Wenn items zahl ungleich 0 ist dann
                    {
                        Frame.Log("item gefunden");                                                                                                 // Logbuch items gefunden
                        EveItem itemZ2 = items2.OrderBy(x => x.ItemId).FirstOrDefault();                                                              // Nimm das erste items in der liste
                        Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Add(itemZ);                                                               // und füge es dem itemshangar hinzu
                        items.Remove(itemZ2);      
                         _state = TravelStates.unload;                                                                                              // wiederhole unload
                         break;
                    }                                                                                                                               // Wenn kein items mehr da ist / oder da war 

*/



                    Frame.Log("keine items mehr wieder losfliegen");                                                                                // Log buchausgabe
                    _state = TravelStates.letzgo;                                                                                                   // Abdocken und losfliegen
                    break;



          



            }
        }
    }
}

/*
 List<EveBookmark> bmlist = Frame.Client.GetMyBookmarks();
List<EveBookmark> bmblacklist = null;
EveBookmark bm = null;

if (bm == null)
{
if (bmblacklist != null)
{
if (bmlist.Where(i => !(bmblacklist.Contains(i)) && i.Title.Contains("Miningbm")).FirstOrDefault() != null)
{

bm = bmlist.Where(i => !(bmblacklist.Contains(i)) && i.Title.Contains("Miningbm")).FirstOrDefault(); // fehlen noch zusätzliche bedingungen
}
}
else
{
bm = bmlist.Where(i => (i.Title.Contains("Miningbm"))).FirstOrDefault(); // fehlen noch zusätzliche bedingungen
}
}

if (bm == null)
{
//keine bookmarks mehr
}
 * 
 * 
 *  var belts = Frame.Client.Entities.Where(i => i.Group == Group.AsteroidBelt && !i.Name.ToLower().Contains("ice") && !EmptyBelts.Contains(i.Id));
    var belt = belts.OrderBy(x => x.Distance).FirstOrDefault();
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 *  List<EveModule> minerlist = Frame.Client.GetActiveShip.Miners; 
*/
