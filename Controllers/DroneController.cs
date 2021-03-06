﻿using System;
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
        int skilldronenmoeglich = 0;
        int dronen1idd = 10246;
        int dronen2idd = 0;



        EveEntity astrodronen;
        public static EveEntity astro { get; set; }
        public static bool dronenaktiviern { get; set; }
        public static bool dronecontrolleraktiv { get; set; }

        int dronenmoeglich = 0;
        long? skilldronen = 3436;
        long? skilldronenop = 3438;
        public static bool aktiv { get; set; }
  
        List<EveWindow> Windows = new List<EveWindow>();

        public DroneController()
        {
            Frame.Log("Starting a new Drone Controller");
            aktiv = false;
            _States.DroneState = DroneState.wait;
           
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

                   

                    double money = Frame.Client.wealth();
                    if (money < 500000)
                    {
                        Frame.Log("Dronenstate == Nicht genug geld //  " + money);
                        _States.DroneState = DroneState.donebuy;
                        break;
                    }

                    List<EveSkill> neueskill = Frame.Client.GetMySkills();
                    List<EveQskill> neueQskill = Frame.Client.GetMyQueue();

                        EveSkill droneskill = neueskill.Where(x => x.typeID == skilldronen).Where(x => x.Skilllvl > 0).FirstOrDefault();
                    if (droneskill == null)
                    {
                        Frame.Log("Dronenskill == null");
                         _States.DroneState = DroneState.donebuy;
                         break;
                    }
                    EveSkill droneskillop = neueskill.Where(x => x.typeID == skilldronenop).Where(x => x.Skilllvl > 0).FirstOrDefault();
                    if (droneskillop == null)
                    {
                        Frame.Log("Dronenskillop == null");
                        _States.DroneState = DroneState.donebuy;
                        break;

                    }
                    if (droneskillop == null && droneskill != null)
                    {
                        Frame.Log("Dronenskillop == null aber schon einen mining skill");
                        _States.DroneState = DroneState.donebuy;
                        break;
                    }
                    if (droneskill != null && droneskillop != null)
                    {
                        Frame.Log("Droneskill ist Aktiv");
                        int dronenlevel = droneskill.Skilllvl;
                        Frame.Log("Droneskilllevel =" + dronenlevel);
                        dronenmoeglich = dronenlevel;
                    }

                    if (Frame.Client.getdronbay() == false)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
                        break;
                    }
                    string shipname = Frame.Client.GetActiveShip.TypeName;
                    if (shipname == "Venture")
                    {
                        skilldronenmoeglich = 2;
                    }
                    if (shipname == "Startschiff")
                    {
                        skilldronenmoeglich = 0;
                    }
                    if (Frame.Client.Session.InSpace == true)
                    {
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(8000, 9000));
                        break;
                    }
                    if (Frame.Client.Session.InStation == true)
                    {
                        if (Frame.Client.getdronbay() == false)
                        {
                            Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
                            break;
                        }
                    
                        if (Frame.Client.GetPrimaryInventoryWindow.DroneBay.Items.Count < skilldronenmoeglich)
                        {
                            Frame.Log("DroneninBay =  null");
                            _States.DroneState = DroneState.vorhandenkaufen;
                            Frame.Log("starte vorhandenkaufen im dronestate");
                            break;
                        }
                      
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 3500));

                    }
                    if (Frame.Client.GetPrimaryInventoryWindow.DroneBay.Items.Count == skilldronenmoeglich)
                    {
                        Frame.Log("Change Dronestate to Idle..");
                        _States.DroneState = DroneState.donebuy;
                        break;
                    }
                    Frame.Log("hier darf ich nie ankommen !!!");
                    break;


                case DroneState.Idle:

                    if (aktiv == false)
                    {
                        _States.DroneState = DroneState.wait;
                        break;
                    }
                  
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
                    if (Frame.Client.getinvopen() == false)
                    {
                        Frame.Client.Getandopenwindow("leer");
                        break;
                    }
                    if (Frame.Client.getdronbay() == false)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
                        break;
                    }

                    List<EveEntity> dronis = Frame.Client.GetActiveShip.ActiveDrones;
                    int droni = dronis.Count;
                    Frame.Log("Drones  count =   " + droni);

                    int dronesbay = Frame.Client.GetPrimaryInventoryWindow.DroneBay.Items.Count;
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
                    else if (Frame.Client.GetActiveShip.ActiveDrones.Count > 0)
                    {
                    Frame.Log("Drones start mining");
                    Frame.Client.DroneMineRepeatedly();
                    dronenaktiviern = true;                              // Dronen aktiv
                    _States.DroneState = DroneState.dronesatwork;
                    break;
                          }
                          break;


              
                  
                case DroneState.dronesatwork:


                    if (astrodronen == null || DroneController.astro == null)
                    {
                        dronenaktiviern = false;                              // Dronen aktiv
                        _States.DroneState = DroneState.dronesback;
                        break;
                    }
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


                case DroneState.wait:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                      if (SkillController.dronenmoeglich == null)
                    {
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                        break;
                    }
    
                    break;


                case DroneState.vorhandenkaufen:
                    Frame.Log("Dronestate .Vorhandenkaufen");
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));

                    if (Frame.Client.getdronbay() == false)
                    {
                        Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenDroneBayOfActiveShip);
                        break;
                    }
                    int dronenhanga = Frame.Client.GetPrimaryInventoryWindow.DroneBay.Items.Count;
                    Frame.Log("count drones in bay = " + dronenhanga);
                    if (dronenhanga == skilldronenmoeglich)
                    {
                        Frame.Log("dronenhanga == skilldronenmoeglich ");
                        _States.DroneState = DroneState.donebuy;
                        break;
                    }
                    if (dronenhanga < SkillController.dronenmoeglich || dronenhanga < skilldronenmoeglich)
                    {
                        if (Frame.Client.getinvopen() == false)
                        {
                            Frame.Client.Getandopenwindow("leer");
                            break;
                        }
                        List<EveItem> itemlischt = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                        EveItem itemsZZ = itemlischt.Where(x => x.TypeId == dronen1idd).FirstOrDefault();
                        if (itemsZZ == null)
                        {
                            Frame.Log("Keine Dronen in der menge vorrätig");
                            Frame.Log("Auf die Einkaufsliste setzen");

                            Tuple<int, int> tmp = new Tuple<int, int>(dronen1idd, 2);
                            BuyController.buylist.Add(tmp);
                            _States.BuyControllerState = BuyControllerStates.buy;
                            _States.DroneState = DroneState.waitbuy;
                            break;
                        }

                        if (itemsZZ != null)
                        {
                            Frame.Log("Dronen im Hanga vorrätig");
                            Frame.Client.GetPrimaryInventoryWindow.DroneBay.Add(itemsZZ, 1);
                            Frame.Log("Dronen von Hanga in Dronenbay +1");
                            _States.DroneState = DroneState.Initialise;
                            break;
                        }
                    }
                    _States.DroneState = DroneState.donebuy;
                    break;


                case DroneState.waitbuy:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    if (_States.BuyControllerState == BuyControllerStates.done)
                    {
                        Frame.Log("Buycontroller == done, setze Dronestate auf Initialise");
                        _States.DroneState = DroneState.Initialise;
                        break;
                    }
                    break;


                case DroneState.donebuy:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
              //       _States.DroneState = states.DroneState.Initialise;
                    break;


                case DroneState.sleeper:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
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