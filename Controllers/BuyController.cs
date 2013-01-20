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
            buylist = new List<Tuple<int, int>>();
            buylist2 = new List<Tuple<int, int>>();
        }


        public override void DoWork()
        {
          
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
                        break;
                    }
                  
                   int typeid = buylist.First().Item1;
                   int menge = buylist.First().Item2;
          
                    Frame.Client.refreshorders(typeid);
                    List<EveMarketOrder> markyord = new List<EveMarketOrder>();
                    markyord = Frame.Client.GetCachedOrders();

                    foreach (EveMarketOrder tmp in markyord)
                    {
                        Frame.Log(tmp.jumps +"//" + tmp.price + tmp.range ); 
                    }
                    List<EveMarketOrder> marketitemZ = new List<EveMarketOrder>();
                    marketitemZ = markyord.Where(x => x.typeID == typeid).Where(x => x.jumps < 1).Where(x => x.bid == false).Where(x => x.stationID == Frame.Client.Session.LocationId).ToList();
               
                    if (marketitemZ.Count < 1)
                    {
                        Frame.Log("Kein items in der Station und auchnicht in der nähe (nicht im Storecach)");
                        buylist2.Add(buylist.First());
                        buylist.RemoveAt(0);
                        break;
                    }

                    if (marketitemZ != null)
                    {
                        EveMarketOrder marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();

                        /*     if (marketitemZ == null)
                            {
                               marketitemZ = markyord.Where(x => x.typeID == minertest).OrderByDescending(x => x.jumps).Where(x => x.bid == false).Where(x => x.range == -1).ToList();
                                marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();
                            }
                         */

                        Frame.Log("Marketitem Name =  " + marketitem.Name);                                                                                                // Funktion für verkaufen infos
                        Frame.Log("Marketitem Price =  " + marketitem.price);
                        Frame.Log("Marketitem Volentered =  " + marketitem.volEntered);
                        Frame.Log("Marketitem Remain =  " + marketitem.volRemaining);
                        Frame.Log("Marketitem OrderId =  " + marketitem.orderID);
                        Frame.Log("Marketitem Jumpes =  " + marketitem.jumps);
                        Frame.Log("Marketitem Range =  " + marketitem.range);
                        double kosten = (marketitem.price * menge);

                        if (marketitem.volRemaining < menge)
                        {
                            if (kosten < Frame.Client.wealth())
                            {
                                marketitem.buy(menge);
                               
                            }
                            buylist.RemoveAt(0);
                            }
                        else
                        {
                            if (kosten < Frame.Client.wealth())
                            {
                                marketitem.buy(marketitem.volRemaining);
                                buylist.RemoveAt(0);
                                Tuple<int, int> tmp = new Tuple<int, int>(marketitem.typeID, (menge - marketitem.volRemaining));
                                buylist.Add(tmp);
                            }
                            else
                            {
                                buylist.RemoveAt(0);
                            }
                        }
                    }
            break;

                case BuyControllerStates.gojita:
            Frame.Log("gojita");
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
                        break;
                    }
                  
                   int typeid2 = buylist2.First().Item1;
                   int menge2 = buylist2.First().Item2;
          
                    Frame.Client.refreshorders(typeid2);
                    markyord = Frame.Client.GetCachedOrders();

                    marketitemZ = markyord.Where(x => x.typeID == typeid2).Where(x => x.jumps < 1).Where(x => x.bid == false).Where(x => x.stationID == Frame.Client.Session.LocationId).ToList();

                    if (marketitemZ != null)
                    {
                        EveMarketOrder marketitem = marketitemZ.OrderByDescending(x => x.price).LastOrDefault();


                        if (marketitemZ.Count < 1)
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
                        double kosten = (marketitem.price * menge2);

                        if (marketitem.volRemaining < menge2)
                        {
                            if (kosten < Frame.Client.wealth())
                            {
                                marketitem.buy(menge2);

                            }
                            buylist.RemoveAt(0);
                        }
                        else
                        {
                            if (kosten < Frame.Client.wealth())
                            {
                                marketitem.buy(marketitem.volRemaining);
                                buylist.RemoveAt(0);
                                Tuple<int, int> tmp = new Tuple<int, int>(marketitem.typeID, (menge2 - marketitem.volRemaining));
                                buylist2.Add(tmp);
                            }
                            else
                            {
                                buylist2.RemoveAt(0);
                            }

                        }
                        
                    }

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
                    Frame.Log("Error Buystates");
                    _States.tutstates = tutstates.Error;
                    break;

                case BuyControllerStates.wait:
                    Frame.Log("wait");
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

                case BuyControllerStates.done:
                    Frame.Log("done");
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(10000, 25000));
                    break;


            }
        }
    }
}

