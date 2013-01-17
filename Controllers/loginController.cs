using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;
using global::Controllers.states;

namespace Controllers
{

    public class logincontroller : BaseController
    {
    
        
        //
        //  Braucht noch Eula Handeling und Login
        //  Restarten des Clients läuft über Injector

       
      

        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        int failcount = 0;
        int maxfails = 3;
        string charname = "Neo Worm";
        DateTime errorwait = DateTime.Now;








     

        public logincontroller()
        {
            Frame.Log("Starting a new LoginController");
        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }


            switch (_States.LoginState)
            {
                case loginstate.Idle:
                    failcount = 0;
                    if (Frame.Client.isloginscreen())
                    {
                        _States.LoginState = loginstate.login;
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));
                        break;
                    }

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;



                case loginstate.login:
                    errorwait = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));
                    failcount++;
                    if (failcount > maxfails)
                    {
                        _States.LoginState = loginstate.Error;
                        break;
                    }
                    
                    Frame.Client.logintest("gwarfa", "UDh35dsF");
                    Frame.Log("Logging In ");
                    _States.LoginState = loginstate.waitforcharsel;
                    break;



                case loginstate.waitforcharsel:

                    if (DateTime.Now > errorwait)
                    {
                        _States.LoginState = loginstate.Error;
                        break;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log("Wait for Char selection");
                    if (!Frame.Client.isconnecting() && (!Frame.Client.ischarsel()))
                    {
                        _States.LoginState = loginstate.login;
                        break;
                    }

                    if (Frame.Client.ischarsel())
                    {
                        _States.LoginState = loginstate.selectchar;
                        errorwait = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                        break;
                    }
                    break;

                case loginstate.selectchar:


                    errorwait = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));
                    Frame.Client.selectchar(charname);
                    Frame.Log("Selecting Char: "+ charname);
                    _States.LoginState = loginstate.waitforig;
                    break;


                case loginstate.waitforig:
                    if (DateTime.Now > errorwait)
                    {
                        _States.LoginState = loginstate.Error;
                        break;
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log("Waiting for Ingame");
                    _States.LoginState = loginstate.waitforig;
                    break;

                case loginstate.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000000, 35000000));      //dirty 

                    Frame.Log("Error");
                    _States.LoginState = loginstate.Error;
                    break;

            }   
        }
    }
}

