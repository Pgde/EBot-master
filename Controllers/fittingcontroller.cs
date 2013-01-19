using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;
using global::Controllers.states;

namespace Controllers
{

    public class fittingcontroller : BaseController
    {


        //
        //  Braucht noch Eula Handeling
        //  Restarten des Clients läuft über Injector
        // Client wird bereits automatisch bei disconnect geschlossen
        // beim quitten müssema schaue
        // 



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        DateTime errorwait = DateTime.Now;



     






        public fittingcontroller()
        {
            Frame.Log("Starting a new FittingController");
          
        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }


            switch (_States.fittingstate)
            {
                case fittingstate.Idle:
                 
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                case fittingstate.FitVult:

                      _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                      _States.fittingstate = fittingstate.Idle;
                    break;

                case fittingstate.FitCovetor:

                      _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                      _States.fittingstate = fittingstate.Idle;
                    break;


                case fittingstate.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(200000000, 350000000));      //dirty 
                    Frame.Log("Error fittingstate");
                    _States.fittingstate = fittingstate.Error;
                    break;

                case fittingstate.wait:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

            }
        }
    }
}

