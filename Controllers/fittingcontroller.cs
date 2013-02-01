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



        public static int fitcount = 0;

        public static Dictionary<long, List<double>> dictXYZ;
        public static double bestdistance = 999999999999;
        public static EveEntity bestentity;

        public static List<string> blacklist = new List<string>();

        public bool ventu { get; set; }
        public bool conv { get; set; }
        public bool mlu1fittet { get; set; }
        public bool miner2fittet { get; set; }

        public fittingcontroller()
        {
            Frame.Log("Starting a new FittingController");
            ventu = false;
            conv = false;
            mlu1fittet = false;
            miner2fittet = false;
        
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
           
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 5000));
                    break;



                case fittingstate.shipitemscheck:
                  _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 1500));

                  /* Frame.Log("dronestate == done");
                                    if (shipname == "Venture")
                                    {
                                        Frame.Log("Schiff = " + shipname);
                                        _States.fittingstate = fittingstate.FitVult;                                  schiff ei name, und alles zurücksetzen wenn man im ei sitzt
                                        break;

                                    }
                  */

                  Frame.Log("Fittingcontroller shipitemcheck");
                    if (mlu1fittet == true && miner2fittet == true)
                    {
                        Frame.Log("Schiff wurde schon gefittet wie gewuenscht");
                         _States.fittingstate = fittingstate.done;                                 
                           break;
                    }
                    Frame.Log("mlu = " + mlu1fittet + " " + " miner2fittet =  " + miner2fittet);

                  string shipname = Frame.Client.GetActiveShip.TypeName;
                  long shipid = Frame.Client.Session.ShipId;

                  if (shipname == "Venture")
                  {
                      _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 5000));
                      Frame.Log("Schiff = " +shipname);
                      Frame.Client.StripFitting(shipid);                         // alles runter
                      _States.fittingstate = fittingstate.FitVult;
                      break;

                  }
                  if (shipname == "Coventor")
                  {
                      _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 5000));
                      Frame.Log("Schiff = " + shipname);
                      Frame.Client.StripFitting(shipid);   
                      _States.fittingstate = fittingstate.FitCovetor;
                      break;
                  }
                  Frame.Log("unbekanntes schiff = " + shipname);
                  Frame.Log("Beende Fitting");
                    _States.fittingstate = fittingstate.done;
                      break;




          



                case fittingstate.FitVult:



                      if (miner2fittet == false)
                      {


                          _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 5000));
                          if (!Frame.Client.IsUnifiedInventoryOpen)
                          {
                              Frame.Client.ExecuteCommand(EveCommand.OpenInventory);
                              break;
                          }
                          double minertyp = 482;
                          var listMinerII = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items.Where(x => x.TypeId == minertyp);
                          int countminerII = 0;

                          foreach (EveItem tmp in listMinerII)
                          {
                              countminerII += tmp.Stacksize;
                          }
                          if (countminerII < 2)
                          {
                              //einkaufen 
                              Tuple<int, int> tmp = new Tuple<int, int>(482, 2 - countminerII);
                              BuyController.buylist.Add(tmp);
                              _States.BuyControllerState = BuyControllerStates.buy;
                              _States.fittingstate = fittingstate.fitbuyt2miner;
                              Frame.Log("weniger als 2 geh einkaufen etc");
                              break;
                          }
                          Frame.Client.StripFitting(Frame.Client.GetActiveShip.ItemId);
                          _States.fittingstate = fittingstate.fitt2miner2;
                          break;
                      }
                      if (mlu1fittet == false)
                      {

                          if (!Frame.Client.IsUnifiedInventoryOpen)
                          {
                              Frame.Client.ExecuteCommand(EveCommand.OpenInventory);
                              break;
                          }
                          double mluid = 22542;
                          var MluI = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items.Where(x => x.TypeId == mluid);
                          int countmluI = 0;

                          foreach (EveItem tmp2 in MluI)
                          {
                              countmluI += tmp2.Stacksize;
                          }
                          if (countmluI < 1)
                          {
                              //einkaufen 
                              Tuple<int, int> tmp2 = new Tuple<int, int>(22542, 1 - countmluI);
                              BuyController.buylist.Add(tmp2);
                              _States.BuyControllerState = BuyControllerStates.buy;
                              _States.fittingstate = fittingstate.fitbuymluI;
                              Frame.Log("weniger als 1 geh einkaufen etc");
                              break;
                          }
                          Frame.Client.StripFitting(Frame.Client.GetActiveShip.ItemId);
                          _States.fittingstate = fittingstate.fitt2miner2;
                          break;
                      }
                      _States.fittingstate = fittingstate.done;
                    break;


                  //    _States.fittingstate = fittingstate.Idle;
                  


                case fittingstate.fitt2miner:
                    fitcount = 0;

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 5500));
                    if (!Frame.Client.IsUnifiedInventoryOpen)
                    {
                        Frame.Client.ExecuteCommand(EveCommand.OpenInventory);
                        break;
                    }
                    double minertyp2 = 482;
                    var list = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items.Where(x => x.TypeId == minertyp2);
                    int count = 0;

                    foreach (EveItem tmp in list)
                    {
                        count += tmp.Stacksize;
                    }
                    if (count < 2)
                    {
                        //einkaufen 
                        Tuple<int, int> tmp = new Tuple<int, int>(482, 2-count);
                        BuyController.buylist.Add(tmp);
                        _States.BuyControllerState = BuyControllerStates.buy;
                        _States.fittingstate = fittingstate.fitbuyt2miner;
                        Frame.Log("weniger als 2 geh einkaufen etc");
                        break;
                    }
                    Frame.Client.StripFitting(Frame.Client.GetActiveShip.ItemId);
                    _States.fittingstate = fittingstate.fitt2miner2;
                    break;



                case fittingstate.fitt2miner2:
                  
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 5000));

                    foreach (EveItem testc in Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items)
                    {
                        Frame.Log(testc.TypeName); 
                    }
                    double minertyp3 = 482;
                    var tofit = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items.Where(x => x.TypeId == minertyp3).FirstOrDefault();
                    
                    List<EveItem> temp = new List<EveItem>();
                   
                    if (tofit != null && fitcount <2)
                    {
                    fitcount = fitcount + 1;
                    temp.Add(tofit);
                    Frame.Client.tryfit(temp);
                    break;
                    }
                    miner2fittet = true;
                    Frame.Log("Miner II Gefittet schaue nach mlu");
                    _States.fittingstate = fittingstate.shipitemscheck;
                    break;

                case fittingstate.fittmlu1:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(3000, 5000));

                    foreach (EveItem testc in Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items)
                    {
                        Frame.Log(testc.TypeName);
                    }
                    double mluid2 = 22542;
                    var tofitmlu = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items.Where(x => x.TypeId == mluid2).FirstOrDefault();

                    List<EveItem> temp2 = new List<EveItem>();

                    if (tofitmlu != null && fitcount < 2)
                    {
                        fitcount = fitcount + 1;
                        temp2.Add(tofitmlu);
                        Frame.Client.tryfit(temp2);
                        break;
                    }
                    mlu1fittet = true;
                    Frame.Log("Mlu gefittet");
                    _States.fittingstate = fittingstate.shipitemscheck;
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

                case fittingstate.fitbuyt2miner:

                      _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 7000));
                     if (_States.BuyControllerState == BuyControllerStates.done)
                     {
                         _States.BuyControllerState = BuyControllerStates.Idle;
                         _States.fittingstate = fittingstate.fitt2miner;
                     }
                         break;


                case fittingstate.fitbuymluI:

                         _localPulse = DateTime.Now.AddMilliseconds(GetRandom(5000, 7000));
                         if (_States.BuyControllerState == BuyControllerStates.done)
                         {
                             _States.BuyControllerState = BuyControllerStates.Idle;
                             _States.fittingstate = fittingstate.fittmlu1;
                         }
                         break;


            }

            
        }
    }
}

/*

            dictXYZ = new Dictionary<long, List<double>>();
            dictXYZ.Add(50013913, new List<double>() { 265806765196,468, 6079769755972.77, 1593583406762.97 });
            dictXYZ.Add(50013921, new List<double>() { -4067679064385.72, -710575103661.441, -3956620123726.59 });
            dictXYZ.Add(40009104, new List<double>() { -640312480017.509, -111854720599.657, -1118671397965.55 });
            dictXYZ.Add(50013928, new List<double>() { -5746110748887.02, 1545356205485.13, -2041530899914.52 });
            dictXYZ.Add(40009123, new List<double>() { -4067658172390.46, -710581041107.649, -3956618450360.03 });
            dictXYZ.Add(50001248, new List<double>() { -4067658421738.88, -710585179304.733, -3956625039281.93 });
            dictXYZ.Add(50001249, new List<double>() { -4067676852161.68, -710577070288.397, -3956622580910.29 });
            dictXYZ.Add(50001250, new List<double>() { -3206301916990.8, -560097155703.378, -3055499914589.86 });
            dictXYZ.Add(40009076, new List<double>() { -161497525.549517, 667022766.595441, -492284137.94 });
            dictXYZ.Add(40009077, new List<double>() { -35645832520.589, -6226168942.25576, 20545489368.61 });
            dictXYZ.Add(40009078, new List<double>() { 29468908605.2457, 5148946350.45066, -46419955269.15 });
            dictXYZ.Add(40009080, new List<double>() { 124057011796.684, 21671295515.1673, 16226299335.02 });
            dictXYZ.Add(40009081, new List<double>() { 124367298646.148, 21726349545.6506, 16088633148.41 });
            dictXYZ.Add(40009082, new List<double>() { -107383402083.898, -18754164490.7629, 436791136802.75 });
            dictXYZ.Add(40009083, new List<double>() { -107312367349.955, -18746271087.4538, 436845650195.07 });
            dictXYZ.Add(40009084, new List<double>() { -107403061223.43, -18761761159.0887, 436865310845.87 });
            dictXYZ.Add(40009085, new List<double>() { -107462029810.209, -18772680918.7352, 436739336653.60 });
            dictXYZ.Add(40009087, new List<double>() { -107298932018.661, -18744168085.6132, 436487539693.63 });
            dictXYZ.Add(40009088, new List<double>() { -107680009501.041, -18810658304.9223, 436988183845.98 });
            dictXYZ.Add(40009089, new List<double>() { -107068256862.562, -18703669617.9543, 436341057813.62 });
            dictXYZ.Add(40009090, new List<double>() { -106631880140.433, -18626807009.0581, 436835095911.72 });
            dictXYZ.Add(40009091, new List<double>() { -107427428275.454, -18764670875.7504, 435789572288.21 });
            dictXYZ.Add(40009092, new List<double>() { -107621062127.339, -18798855660.2674, 435571878513.69 });
            dictXYZ.Add(40009093, new List<double>() { -105993237441.343, -18516154474.207, 435948160823.01 });
            dictXYZ.Add(40009094, new List<double>() { -108278525604.429, -18913088043.1484, 434928329348.44 });
            dictXYZ.Add(40009097, new List<double>() { -108258836986.947, -18910601003.8298, 439593692906.40 });
            dictXYZ.Add(40009098, new List<double>() { -639868035020.294, -111797279390.029, -1118442589677.87 });
            dictXYZ.Add(40009099, new List<double>() { -639886661111.57, -111781743563.333, -1118255553748.73 });
            dictXYZ.Add(40009100, new List<double>() { -640079356139.162, -111815013193.25, -1118332284788.00 });
            dictXYZ.Add(40009101, new List<double>() { -640123916599.977, -111823842021.336, -1118425684004.15 });
            dictXYZ.Add(40009102, new List<double>() { -640216941931.61, -111838124278.334, -1118361672043.49 });
            dictXYZ.Add(40009103, new List<double>() { -640311727496.964, -111855960845.064, -1118354182440.35 });
            dictXYZ.Add(40009105, new List<double>() { -640404840455.755, -111871186867.556, -1118659515598.05 });
            dictXYZ.Add(40009106, new List<double>() { -639513040427.327, -111715483216.022, -1118845217803.76 });
            dictXYZ.Add(40009107, new List<double>() { -639199155722.87, -111660481627.988, -1118793667089.69 });
            dictXYZ.Add(40009108, new List<double>() { -638675201357.118, -111568653273.995, -1119021472264.15 });
            dictXYZ.Add(40009109, new List<double>() { -640898323632.249, -111955144966.736, -1117145946735.62 });
            dictXYZ.Add(40009110, new List<double>() { -639085099967.767, -111638406706.758, -1121462075199.79 });
            dictXYZ.Add(40009111, new List<double>() { -640084893370.251, -111813744050.525, -1113595819717.14 });
            dictXYZ.Add(40009112, new List<double>() { -646277814495.641, -112894177928.326, -1120263799375.62 });
            dictXYZ.Add(40009113, new List<double>() { -642949492105.225, -112314577450.997, -1109833306574.33 });
            dictXYZ.Add(40009114, new List<double>() { -632093817754.155, -110418143478.075, -1126789317984.56 });
            dictXYZ.Add(40009115, new List<double>() { -625396264266.51, -109247888578.96, -1118589437797.56 });
            dictXYZ.Add(40009116, new List<double>() { 2907919838455.15, 507985314200.774, -950952949115.87 });
            dictXYZ.Add(40009118, new List<double>() { 2907468475858.59, 507905582110.733, -951774991367.22 });
            dictXYZ.Add(40009119, new List<double>() { -2275016633260.11, -397421219576.098, 3223727988733.45 });
            dictXYZ.Add(40009121, new List<double>() { -2273988198792.45, -397242078960.07, 3224088805718.38 });
            dictXYZ.Add(40009122, new List<double>() { -2277966302211.9, -397937690816.383, 3227277770949.16 });
            dictXYZ.Add(50013876, new List<double>() { -3487431103763.74, 4569656642143.58, 2555351898925.20 });
            dictXYZ.Add(60004423, new List<double>() { -107074575929.299, -18705411998.215, 436344493843.365 });
            dictXYZ.Add(60000361, new List<double>() { -107072607313.546, -18704916429.2734, 436343522211.515 });
            dictXYZ.Add(60000364, new List<double>() { -646276163059.858, -112899558944.768, -1120272713241.6 });
            dictXYZ.Add(60000451, new List<double>() { -107072363807.271, -18704182446.6701, 436343510658.231 });
            dictXYZ.Add(60000463, new List<double>() { 2907928977527.33, 507986279167.911, -950941396001.758 });
            dictXYZ.Add(60002953, new List<double>() { -625388771037.531, -109250761116.837, -1118586051936.06 });
            dictXYZ.Add(60002959, new List<double>() { -105999375646.751, -18516666648.5268, 435947837334.116 });
            dictXYZ.Add(60003055, new List<double>() { -108271428458.765, -18913567216.5518, 434931375550.01 });
            dictXYZ.Add(60003460, new List<double>() { -105998384903.814, -18517158840.0871, 435945651610.689 });
            dictXYZ.Add(60003463, new List<double>() { -2277973142439.15, -397939383639.452, 3227280119457.61 });
            dictXYZ.Add(60003466, new List<double>() { -107302640477.428, -18745225438.4038, 436489741282.982 });
            dictXYZ.Add(60003469, new List<double>() { -107356460911.218, -18742031198.894, 436825947906.69 });
            dictXYZ.Add(60003760, new List<double>() { -107303381304.694, -18744979965.8158, 436489002907.524 });
            foreach (var tmp in Frame.Client.Entities)
            {
                //Frame.Log(tmp.velo);
            }
                    foreach (var tmp in Frame.Client.Entities.Where(x => x.MovementMode == EveEntity.EntityMovementState.InWarp && x.gotox != 0 && x.gotoy != 0 && x.gotoz != 0 && x.x != 0 && x.y != 0 && x.z != 0 && !blacklist.Contains(x.Name) && x.velo > 500 ))
                    {
                      //  if (tmp.gotox != 0 && tmp.gotoy != 0 && !blacklist.Contains(tmp.Name) && tmp.MovementMode == EveEntity.EntityMovementState.InWarp && tmp.Id > 0)
                        {
                            bestdistance = 9999999999999;
                            blacklist.Add(tmp.Name);
                          // Frame.Log(tmp.Name + ": warping to" + "x: " + tmp.gotox + " y: " + tmp.gotoy + " z: " + tmp.gotoz);      
                            foreach (var ent in Frame.Client.Entities.Where(x => x.Group == Group.Station || x.Group == Group.Stargate || x.Group == Group.Moon || x.Group == Group.Planet ))
                            {
        
                               var dist = Math.Sqrt((ent.x - tmp.gotox) * (ent.x - ent.gotox) + (ent.y - tmp.gotoy) * (ent.y - tmp.gotoy) + (ent.z - tmp.gotoz) * (ent.z - tmp.gotoz));
                       
                                if (dist < bestdistance)
                                {
                               //     Frame.Log("debug: " + ent.x + "//" + ent.y + "//" + ent.z +"//dist: " + dist);
                                    bestentity = ent;
                                    bestdistance = dist;
                                }
                            }
                            if (bestentity.Group == Group.Moon )
                            {
                                if (!bestentity.Name.Contains("Jita IV - Moon 4") && !bestentity.Name.Contains("Jita IV - Moon 2"))
                                {
                                    Frame.Log(tmp.Name + "  warping to: " + bestentity.Name + " Shiptype: " + tmp.Group);
                                _States.fittingstate = fittingstate.FitVult;
                                    bestentity.WarpTo();
                                    break;
                                }
                             }
                            Frame.Log(tmp.Name + "  warping to: " + bestentity.Name + " Shiptype: " + tmp.Group);
                        }
                        

                      //  double dist = Math.Sqrt((tmp.x - tmp.gotox) * (tmp.x - tmp.gotox) + (tmp.y - tmp.gotoy) * (tmp.y - tmp.gotoy) + (tmp.z - tmp.gotoz) * (tmp.z - tmp.gotoz));


*/