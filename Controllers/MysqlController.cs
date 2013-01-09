using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;

namespace Controllers
{

    public class MysqlController : BaseController
    {
        long _destinationId, _currentLocation, _currentDestGateId;
        bool _waitforsessionChange;
        enum TravelStates { Initialise, Start, Travel, ArrivedAtDestination, sqlsettime, sqlstarttime }






        ////////////////////////////////////////////////////////
        ///////////          MySql VARIABLEN        ////////

        int stationtrip = 0;
        int charid;
        bool charon = false;
        bool needupdate = false;
        string row = "";
        string rowid = "";

        string minersactiv = "Aus";
        string shipname = "";
        string stringstate = "";
        string aktime = "";
        string starttime = "";


        ////////////////////////////////////////////////////////
        ///////////          MySql VARIABLEN        ////////













        TravelStates _state;

        public MysqlController()
        {
            Frame.Log("Starting a new mysqlcontroller");
        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_state)
            {
                case TravelStates.Initialise:

                    DateTime currentDate = DateTime.Now;
                    int h = currentDate.Hour;
                    int m = currentDate.Minute;
                    Frame.Log(h + ":" + m);
                    starttime = (h + ":" + m);



                    _state = TravelStates.Start;
                    break;




                case TravelStates.Start:




                case TravelStates.sqlstarttime:


                case TravelStates.sqlsettime:

















                case TravelStates.ArrivedAtDestination:
                    Frame.Log("Destination reached");
                    IsWorkDone = true;
                    break;
            }
        }
    }
}

