using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using global::Controllers.states;

namespace Controllers
{

    public class MainController : BaseController
    {



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        DateTime errorwait = DateTime.Now;










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


            switch (_States.maincontrollerStates)
            {
                case maincontrollerStates.Idle:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                case maincontrollerStates.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(200000000, 350000000));      //dirty 
                    Frame.Log("Error tutstates");
                    _States.tutstates = tutstates.Error;
                    break;

                case maincontrollerStates.wait:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;


            }
        }
    }
}

