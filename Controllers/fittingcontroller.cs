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




                case fittingstate.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000000, 35000000));      //dirty 
                    Frame.Log("Error");
                    _States.fittingstate = fittingstate.Error;
                    break;

            }
        }
    }
}

