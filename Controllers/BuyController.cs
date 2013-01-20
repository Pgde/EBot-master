using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using global::Controllers.states;

namespace Controllers
{

    public class BuyController : BaseController
    {



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        DateTime errorwait = DateTime.Now;
        public static List<Tuple<int, int>> buylist { get; set; }
        public static List<Tuple<int, int>> buylist2 { get; set; }
        int i;
        long Jitastationid = 60003760;







        public BuyController()
        {
            Frame.Log("Starting a new BuyController");

        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }


            switch (_States.BuyControllerState)
            {
                case BuyControllerStates.Idle:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                     case BuyControllerStates.setup:
                    i = 0;
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                case BuyControllerStates.buy:

                    
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    if (buylist.Count <= 0 && buylist2.Count > 0)
                    {
                        _States.BuyControllerState = BuyControllerStates.gojita;
                        
                    }
                  
                   int typeid = buylist.First().Item1;
                   int menge = buylist.First().Item2;
          
                    Frame.Client.refreshorders(typeid);
                    List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();

                    List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == typeid).Where(x => x.jumps < 1).Where(x => x.bid == false).Where(x => x.range == -1).ToList();
                    EveMarketOrder marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();

                /*     if (marketitemZ == null)
                    {
                       marketitemZ = markyord.Where(x => x.typeID == minertest).OrderByDescending(x => x.jumps).Where(x => x.bid == false).Where(x => x.range == -1).ToList();
                        marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();
                    }
                 */
                    if (marketitemZ == null)
                    {
                    Frame.Log("Kein items in der Station und auchnicht in der nähe (nicht im Storecach)");
                    buylist2.Add(buylist.First());
                    buylist.RemoveAt(0);                   
                    break;
                    }
                       
            Frame.Log("Marketitem Name =  " + marketitem.Name);                                                                                                // Funktion für verkaufen infos
            Frame.Log("Marketitem Price =  " + marketitem.price);
            Frame.Log("Marketitem Volentered =  " + marketitem.volEntered);
            Frame.Log("Marketitem Remain =  " + marketitem.volRemaining);
            Frame.Log("Marketitem OrderId =  " + marketitem.orderID);
            Frame.Log("Marketitem Jumpes =  " + marketitem.jumps);
            Frame.Log("Marketitem Range =  " + marketitem.range);
            marketitem.buy(menge);


            break;

                case BuyControllerStates.gojita:

            _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    TravelController.desti = Jitastationid;
                    _States.TravelerState = TravelerState.Initialise;
                    _States.BuyControllerState = BuyControllerStates.traveljita;

                    break;


                case BuyControllerStates.traveljita:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    if (_States.TravelerState == TravelerState.ArrivedAtDestination && Frame.Client.Session.LocationId == Jitastationid)
                    {
                        _States.BuyControllerState = BuyControllerStates.buyjita;
                        _States.TravelerState = TravelerState.wait;
                    }
                    break;




                    case BuyControllerStates.buyjita:

                   _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    if (buylist2.Count <= 0)
                    {
                        _States.BuyControllerState = BuyControllerStates.gohome;
                        
                    }
                  
                   int typeid2 = buylist2.First().Item1;
                   int menge2 = buylist2.First().Item2;
          
                    Frame.Client.refreshorders(typeid2);
                    markyord = Frame.Client.GetCachedOrders();

                    marketitemZ = markyord.Where(x => x.typeID == typeid2).Where(x => x.jumps < 1).Where(x => x.bid == false).Where(x => x.range == -1).ToList();
                    marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();

                /*     if (marketitemZ == null)
                    {
                       marketitemZ = markyord.Where(x => x.typeID == minertest).OrderByDescending(x => x.jumps).Where(x => x.bid == false).Where(x => x.range == -1).ToList();
                        marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();
                    }
                 */
                    if (marketitemZ == null)
                    {
                    Frame.Log("Kein items in der Station und auchnicht in der nähe (nicht im Storecach)");
                    buylist2.RemoveAt(0);                   
                    break;
                    }
                       
            Frame.Log("Marketitem Name =  " + marketitem.Name);                                                                                                // Funktion für verkaufen infos
            Frame.Log("Marketitem Price =  " + marketitem.price);
            Frame.Log("Marketitem Volentered =  " + marketitem.volEntered);
            Frame.Log("Marketitem Remain =  " + marketitem.volRemaining);
            Frame.Log("Marketitem OrderId =  " + marketitem.orderID);
            Frame.Log("Marketitem Jumpes =  " + marketitem.jumps);
            Frame.Log("Marketitem Range =  " + marketitem.range);
            marketitem.buy(menge2);


            break;


                    case BuyControllerStates.gohome:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    TravelController.desti = Settings.Settings.Instance.homesys;
                    _States.TravelerState = TravelerState.Initialise;
                    _States.BuyControllerState = BuyControllerStates.travelhome;
                    break;


                    case BuyControllerStates.travelhome:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    if (_States.TravelerState == TravelerState.ArrivedAtDestination && Frame.Client.Session.LocationId == Settings.Settings.Instance.homesys)
                    {
                        _States.BuyControllerState = BuyControllerStates.done;
                        _States.TravelerState = TravelerState.wait;
                    }
                    break;


                case BuyControllerStates.Error:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(200000000, 350000000));      //dirty 
                    Frame.Log("Error tutstates");
                    _States.tutstates = tutstates.Error;
                    break;

                case BuyControllerStates.wait:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

                case BuyControllerStates.done:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(10000, 25000));
                    break;


            }
        }
    }
}

