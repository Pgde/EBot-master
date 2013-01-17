using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using global::Controllers.states;

namespace Controllers
{

    public class tutcontroller : BaseController
    {



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        DateTime errorwait = DateTime.Now;










        public tutcontroller()
        {
            Frame.Log("Starting a new tutcontroller");

        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }


            switch (_States.tutstates)
            {
                case tutstates.Idle:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                case tutstates.start:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _States.tutstates = tutstates.acceptmission;
                    break;

                case tutstates.acceptmission:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _States.tutstates = tutstates.domission;
                    break;

                case tutstates.domission:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _States.tutstates = tutstates.compmission;
                    break;

                case tutstates.compmission:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _States.tutstates = tutstates.checksecond;
                    break;


                case tutstates.checksecond:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    _States.tutstates = tutstates.Idle;
                    break;
               

                case tutstates.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(200000000, 350000000));      //dirty 
                    Frame.Log("Error tutstates");
                    _States.tutstates = tutstates.Error;
                    break;

            }
        }
    }
}

