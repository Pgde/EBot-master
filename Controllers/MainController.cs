using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using global::Controllers.states;
using Controllers.Settings;
using Controllers;
namespace Controllers
{

    public class MainController : BaseController
    {



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        DateTime errorwait = DateTime.Now;

        public MiningState backupmining;
        public TravelerState backuptravel;
        public DroneState backupdrone;
        public fittingstate backupfitting;
        public loginstate backuplogin;
        public SkillState backupskill;
        public tutstates backuptut;
        public static bool pausebot { get; set; }







        public MainController()
        {
            Frame.Log("Starting a new MainController");

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

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                case maincontrollerStates.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(200000000, 350000000));      //dirty 
                    Frame.Log("Error maincontroller");
                    _States.tutstates = tutstates.Error;
                    break;

                case maincontrollerStates.wait:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

                case maincontrollerStates.endminingcycle:

                    waitallstates();
                    _States.maincontrollerState = maincontrollerStates.skillcheck;
                    _States.SkillState = SkillState.Initialise;
                    //checks for skilltraining etc
                   // _States.MiningState = MiningState.letzgo;
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;


                case maincontrollerStates.skillcheck:

            
                    if (_States.SkillState == SkillState.done)
                    {
                        restorestates();
                    _States.MiningState = MiningState.letzgo;
                    }
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
                        _States.maincontrollerState = maincontrollerStates.Startup;
                        break;
                    }

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
            
        }
    }

