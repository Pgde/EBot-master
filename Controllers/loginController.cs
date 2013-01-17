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
    
        


       
        List<string> skilltotrainid = new List<string>();

        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////













     

        public logincontroller()
        {
            Frame.Log("Starting a new SkillController");
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

                    //check for loginscreen
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _States.LoginState = loginstate.login;
                    break;



                case loginstate.login:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log("Logging In ");
                    _States.LoginState = loginstate.waitforcharsel;
                    break;



                case loginstate.waitforcharsel:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log("Wait for Char selection");
                    _States.LoginState = loginstate.selectchar;
                    break;


                case loginstate.selectchar:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log("Selecting Char");
                    _States.LoginState = loginstate.waitforig;
                    break;


                case loginstate.waitforig:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log("Waiting for Ingame");
                    _States.LoginState = loginstate.Idle;
                    break;

                case loginstate.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log("Error");
                    _States.LoginState = loginstate.Error;
                    break;

            }   
        }
    }
}

