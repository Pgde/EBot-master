using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;
using Controllers.states;
namespace Controllers
{

    public class MysqlController : BaseController
    {

     //   enum TravelStates { Initialise, Start, Travel, ArrivedAtDestination, sqlsettime, sqlstarttime, sqlcheck, sqltimecheck }






        ////////////////////////////////////////////////////////
        ///////////          MySql VARIABLEN        ////////

       public static int stationtrip = 0;
       public static DateTime lastaction = DateTime.Now;
        int charid;
        string charname;
        bool charon = false;
 //       bool needupdate = false;
  //      string row = "";
        string rowid = "";

        string minersactiv = "Aus";
        string shipname = "";
       public static string stringstate = "";
        string aktime = "";
        string starttime = "";

      //  int miningcount = 0;
      //   itemzahl = 0;
      //   itemwert = 0;


        double usdcapcargo = 0;
        double fullcapcargo = 0;

        ////////////////////////////////////////////////////////
        ///////////          MySql VARIABLEN        ////////




        public static  long itemzahl { get; set; }
        public static long itemwert { get; set; }











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
            switch (_States.MysqlState)
            {
                    
                case MysqlState.Initialise:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(35000, 55000));
                    if (Frame.Client.Session.InStation == true || Frame.Client.Session.InSpace == true)
                    {
                        ///   MySql INIZALISIERN ///
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));
                        stringstate = _States.MysqlState.ToString();
                        shipname = Frame.Client.GetActiveShip.GivenName;
                        charid = Frame.Client.Session.CharId;
                        charname = Frame.Client.Charname;
                        fullcapcargo = Frame.Client.wealth();
                        usdcapcargo = Frame.Client.qlengdouble; //not working yet
                        string system = Frame.Client.Session.SolarSystemId.ToString(); //solid
                        DateTime currentDate = DateTime.Now;
                        int h = currentDate.Hour;
                        int m = currentDate.Minute;
                        Frame.Log(h + ":" + m);
                        starttime = (h + ":" + m);

            


                        _States.MysqlState = MysqlState.Start;
                    }
                    break;


                case MysqlState.Start:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    stringstate = "//MySqlctrl: " + _States.MysqlState.ToString() + " //Minig: " + _States.MiningState.ToString() + " //Maincont: " + _States.maincontrollerState.ToString() + " //buyctrl: " + _States.BuyControllerState.ToString() + " //travlctrl: " + _States.TravelerState.ToString();
                    sqlsettime();
                    sqlcheck();
                    MainController.emztimecheck();
                    break;

                case MysqlState.wait:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;


            }
        }

          public void sqlsettime()
        {

            DateTime currentDate = DateTime.Now;
            int h = currentDate.Hour;
            int m = currentDate.Minute;
            Frame.Log(h + ":" + m);
            aktime = (h + ":" + m);
        }


        public void sqlstarttime()
        {

            DateTime currentDate = DateTime.Now;
            int h = currentDate.Hour;
            int m = currentDate.Minute;
            Frame.Log(h + ":" + m);
            starttime = (h + ":" + m);
        }



        
    
        public void sqlcheck()
        {
            string connString = "Server=192.168.0.21;Uid=root;Pwd=;Database=evetest;";
            MySqlConnection connection = new MySqlConnection(connString);
             MySqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT id FROM evedaten";
            MySqlDataReader Reader;
                  connection.Open();
            Reader = command.ExecuteReader();
           
            while (Reader.Read())
            {
               string row = "";
               for (int i = 0; i < Reader.FieldCount; i++)
                   row = Reader.GetValue(i).ToString();
                           if (row == charid.ToString())
                 {
                     Frame.Log("Gefunden");
                     charon = true;
                     rowid = row;
                 }   
            }
            connection.Close();

            connection.Open();
       

               if (charon == true)
            {
                 sqlsettime();
                 command.CommandText = "Update evedaten SET id='" + charid.ToString() +"' WHERE id='" + rowid +"'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET name='" + charname + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET state='" + stringstate.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET shipname='test' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET cargmax='" + fullcapcargo.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET cargousd='" + usdcapcargo.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET minersactive='" + minersactiv.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET time='" + aktime.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET itemzahl='" + itemzahl.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET itemwert='" + itemwert.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET starttime='" + starttime.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
                 command.CommandText = "Update evedaten SET stationtrip='" + stationtrip.ToString() + "' WHERE id='" + rowid + "'";
                 command.ExecuteNonQuery();
      
            }
            if (charon == false)
            {
                sqlsettime();
                Frame.Log("Char id nicht Gefunden INSERT INTO");
                command.CommandText = "INSERT INTO evedaten (id,name,state,shipname,cargmax,cargousd,minersactive,time,itemzahl,itemwert,starttime,stationtrip) VALUES ('" + charid.ToString() + "','Hans','" + stringstate.ToString() + "','shipname','" + fullcapcargo.ToString() + "', '" + usdcapcargo.ToString() + "', '" + minersactiv.ToString() + "','" + aktime.ToString() + "','" + itemzahl.ToString() + "', '" + itemwert.ToString() + "', '" + starttime.ToString() + "','" + stationtrip.ToString() +"')";
               //                                                                                                                                                              charid               name            state                       shipname        usdcarg                             fullcarg                        minersactiv                         aktim                   itemwert                    itemzahl                            aktime                  stationtrip                         itemwert                        starttime                       stationtrip                         
                command.ExecuteNonQuery();
                                                                                          
            }
            
           
            connection.Close();
                return;
            }

       



            }
        }


