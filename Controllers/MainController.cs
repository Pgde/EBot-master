using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using global::Controllers.states;
using Controllers.Settings;
using Controllers;
using System.Diagnostics;
namespace Controllers
{

    public class MainController : BaseController
    {



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        DateTime errorwait = DateTime.Now;
        public static DateTime logintimer { get; set; }
        public MiningState backupmining;
        public TravelerState backuptravel;
        public DroneState backupdrone;
        public fittingstate backupfitting;
        public loginstate backuplogin;
        public SkillState backupskill;
        public tutstates backuptut;
        public static bool pausebot { get; set; }
        public static bool vollgeknallt { get; set; }
        public static bool notwenigskills { get; set; }
   






        public MainController()
        {
            Frame.Log("Starting a new MainController");

            notwenigskills = false;


        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }


            switch (_States.maincontrollerState)
            {
                case maincontrollerStates.Idle:
                    vollgeknallt = false;
                  Frame.Log("maincontrollerStates.Idle:");
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                case maincontrollerStates.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(200000000, 350000000));      //dirty 
                    Frame.Log("Error maincontroller");
                    _States.tutstates = tutstates.Error;
                    break;

                case maincontrollerStates.wait:
                    Frame.Log("maincontrollerStates.wait:");
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

                case maincontrollerStates.endminingcycle:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    Frame.Log("starte maincontroller endminingcycle");
                    waitallstates();
                    timecheck();


                    if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 1)                                    // operator wieder ändern nur für testzwecke
                    {
                        Frame.Log("kontrolle wegen zeit");
                        if (vollgeknallt == false)
                        {
                            Frame.Log("skillsvollknallen");
                            _States.SkillState = SkillState.logoutskills;
                            _States.maincontrollerState = maincontrollerStates.wait;
                            vollgeknallt = true;
                            break;
                        }
                     
                       }
                 
                        _States.maincontrollerState = maincontrollerStates.skillcheck;
                        _States.SkillState = SkillState.Initialise;
                        //checks for skilltraining etc
                        // _States.MiningState = MiningState.letzgo;
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                        break;
               

                case maincontrollerStates.skillcheck:
                    Frame.Log("maincontroller.skillcheck angekommen");
                    
                    if (_States.SkillState == SkillState.done)
                    {
                        Frame.Log("skillstate done");
                //        restorestates();
                //    _States.MiningState = MiningState.letzgo;
                //alt        _States.maincontrollerState = maincontrollerStates.checkbuy;
                        bool ak = Frame.Client.dronconaktiv();
                        Frame.Log("ak == " + ak);
                        if (ak == true)
                        {
                            Frame.Log("true starte dronestate initi");
                            DroneController.aktiv = true;
                            _States.DroneState = DroneState.Initialise;
                        }
                        if (ak == false)
                        {
                            Frame.Log("done .... dronestate donebuy");
                            DroneController.aktiv = false;
                            _States.DroneState = DroneState.donebuy;
                        }
                        Frame.Log("done ....dronencheck");
                        _States.maincontrollerState = maincontrollerStates.dronencheck;

                    }
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

                case maincontrollerStates.dronencheck:
                    Frame.Log("bei....dronencheck");
                    if (_States.DroneState == DroneState.donebuy)
                    {
                        if (SkillController.miner2rdy == true && SkillController.mlu1rdy == true)
                        {
                            Frame.Log("dronestate == done");
                            _States.maincontrollerState = maincontrollerStates.fittincheck;
                            _States.fittingstate = fittingstate.shipitemcheck2;
                            break;
                        }
                         _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                         _States.fittingstate = fittingstate.done;
                         _States.maincontrollerState = maincontrollerStates.fittincheck;
                              break;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;


                case maincontrollerStates.fittincheck:
                     if (_States.fittingstate == fittingstate.done)
                    {
                        _States.maincontrollerState = maincontrollerStates.checkbuy;
                         _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                        break;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;



                case maincontrollerStates.checkbuy:


             // checks ob etwas gekauft werden muss, evtl eigener controller ?
            // wenn nein
                    _States.maincontrollerState = maincontrollerStates.resumemining;
            // wenn ja
            //        _States.maincontrollerState = maincontrollerStates.startbuy;
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

                case maincontrollerStates.startbuy:

                    Tuple<int,int> tmp = new Tuple<int,int> (483,1); // miner, 1 -> typeid, menge
                     BuyController.buylist.Add(tmp);
                    _States.BuyControllerState = BuyControllerStates.buy;
                    _States.maincontrollerState = maincontrollerStates.waitbuy;
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

                case maincontrollerStates.waitbuy:


                    if (_States.BuyControllerState == BuyControllerStates.done)
                    {

                        _States.maincontrollerState = maincontrollerStates.resumemining;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

            case maincontrollerStates.resumemining:

                
                        restorestates();
                        _States.MiningState = MiningState.letzgo;
                        _States.maincontrollerState = maincontrollerStates.Idle;
                        _States.DroneState = DroneState.Idle;
                    //    _States.DroneState = DroneState.Initialise;
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;


                case maincontrollerStates.pause:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    waitallstates();
                    pausebot = true;
                    _States.maincontrollerState = maincontrollerStates.pauseloop;
                    break;

                case maincontrollerStates.pauseloop:
                    if (!pausebot)
                    {
                        restorestates();
                        _States.maincontrollerState = maincontrollerStates.Idle;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;
            

             case maincontrollerStates.Startup:
                  

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                   
                   if (DateTime.Now < logintimer)
                   {
                        Frame.Log("warte noch");
                       break;
                    }
                    if (Frame.Client.Session.LocationId != Settings.Settings.Instance.homesys)
                    {
                        TravelController.desti = Settings.Settings.Instance.homesys;
                        _States.TravelerState = TravelerState.Initialise;
                        _States.maincontrollerState = maincontrollerStates.homesysarriv;
                    }
                    else
                    {
                        _States.maincontrollerState = maincontrollerStates.Idle;
                        _States.MiningState = MiningState.Initialise;
                    }
                    break;

             case maincontrollerStates.homesysarriv:


                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    Frame.Log(Frame.Client.Session.LocationId.ToString());
                    if (_States.TravelerState == TravelerState.ArrivedAtDestination)
                    {
                        _States.TravelerState = TravelerState.wait;
                        _States.maincontrollerState = maincontrollerStates.Startup;
                        break;
                    }

                    break;

             case maincontrollerStates.logo:

                    break;
            }
       
        }
        


        public void waitallstates()
        {
            backupstates();
            setwaitstates();
        }

        public void setwaitstates()
        {
            _States.TravelerState = TravelerState.wait;
            _States.MiningState = MiningState.wait;
            _States.DroneState = DroneState.wait;
            _States.fittingstate = fittingstate.wait;
            _States.LoginState = loginstate.wait;
            _States.SkillState = SkillState.wait;
            _States.tutstates = tutstates.wait;
        }

            public void backupstates ()
            {
                backuptravel = _States.TravelerState;
                backupmining = _States.MiningState;
                backupdrone = _States.DroneState;
                backupfitting = _States.fittingstate;
                backuplogin = _States.LoginState;
                backupskill = _States.SkillState;
                backuptut = _States.tutstates;
            }

            public void restorestates()
            {
                _States.TravelerState = backuptravel;
                _States.MiningState = backupmining;
                _States.DroneState = backupdrone;
                _States.fittingstate = backupfitting;
                _States.LoginState = backuplogin;
                _States.SkillState = backupskill;
                _States.tutstates = backuptut;
            }
            public static void timecheck()
            {
                if (DateTime.Now.Hour > 1 && DateTime.Now.Hour < 12)
                {
                   
                    Process currentProcess = Process.GetCurrentProcess();
                    currentProcess.Kill();
                }

            }
            
        }
    }

