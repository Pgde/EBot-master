using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using global::Controllers.states;
using System.IO;

namespace Controllers
{

    public class tutcontroller : BaseController
    {



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        DateTime errorwait = DateTime.Now;
        string TutAgent = "Abishi Tian";
        public static int csystem = 0;
       







        public tutcontroller()
        {
            Frame.Log("Starting a new tutcontroller");

        }


        public override void DoWork()
        {
            if (_localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }


            switch (_States.tutstates)
            {
                case tutstates.Idle:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    break;

                case tutstates.start:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    if (!Frame.Client.GetService("agents").IsValid)
                    {
                        break;
                    }

                    if (!Frame.Client.GetAgentByName(TutAgent).IsValid)
                    {
                        _States.tutstates = tutstates.Error;
                        break;
                    }
                    if (Frame.Client.Session.LocationId == Frame.Client.GetAgentByName(TutAgent).StationId)
                    {
                        _States.tutstates = tutstates.getmission;
                        break;
                    }
                    else
                    {
                        TravelController.desti = Frame.Client.GetAgentByName(TutAgent).StationId;
                        _States.TravelerState = TravelerState.Initialise;
                        _States.tutstates = tutstates.travel;
                    }

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 5000));
                    break;

                case tutstates.travel:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));
                    if (_States.TravelerState == TravelerState.ArrivedAtDestination)
                    {
                        _States.TravelerState = TravelerState.wait;
                        _States.tutstates = tutstates.start;
                    }
                   break;
                

                case tutstates.getmission:
                     _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));
                    Frame.Client.GetAgentByName(TutAgent).StartConversation();
                    EveAgentDialogWindow agentwnd = Frame.Client.GetAgentDialogWindow(Frame.Client.GetAgentByName(TutAgent).AgentId);
                  //  agentwnd.ClickButton(EveWindow.Button.RequestMission);

                    foreach (EveAgentMission tmp in Frame.Client.AgentMissions)
                    {
                        if (tmp.State == EveAgentMission.MissionState.Offered && tmp.Name == "Making Mountains of Molehills (1 of 10)" || tmp.State == EveAgentMission.MissionState.Offered && tmp.Name == "Making Mountains of Molehills (2 of 10)")
                        {
                            _States.tutstates = tutstates.acceptmission;
                            break;
                        }
                        if (tmp.State == EveAgentMission.MissionState.Accepted && tmp.Name == "Making Mountains of Molehills (1 of 10)" || tmp.State == EveAgentMission.MissionState.Accepted && tmp.Name == "Making Mountains of Molehills (2 of 10)")
                        {
                            _States.tutstates = tutstates.domission;
                            break;
                        }
                    }
                   if (agentwnd.HasButton(EveWindow.Button.RequestMission))
                   {
                       agentwnd.ClickButton(EveWindow.Button.RequestMission);
                       _States.tutstates = tutstates.acceptmission;
                   }
                           
                    break;

                case tutstates.acceptmission:

                    foreach (EveAgentMission tmp in Frame.Client.AgentMissions)
                    {
                   Frame.Log( tmp.Name);
                   if (tmp.State == EveAgentMission.MissionState.Offered && tmp.Name == "Making Mountains of Molehills (1 of 10)" || tmp.State == EveAgentMission.MissionState.Offered && tmp.Name == "Making Mountains of Molehills (2 of 10)")
                   {
                       EveAgentDialogWindow agentwnd2 = Frame.Client.GetAgentDialogWindow(Frame.Client.GetAgentByName(TutAgent).AgentId);
                       if (agentwnd2.HasButton(EveWindow.Button.Accept))
                       {
                           agentwnd2.ClickButton(EveWindow.Button.Accept);
                           _States.tutstates = tutstates.getmission;
                       }
                   }
                    }
                  
                     _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
                    // agentwnd2.ClickButton(EveWindow.Button.Accept);
                    
                    break;

                case tutstates.domission:


                    foreach (EveAgentMission tmp in Frame.Client.AgentMissions)
                    {
                        Frame.Log(tmp.Name);
                        if (tmp.State == EveAgentMission.MissionState.Offered && tmp.Name == "Making Mountains of Molehills (1 of 10)")
                        {
                            _States.tutstates = tutstates.dominingmissi;
                            break;
                        }

                        if (tmp.State == EveAgentMission.MissionState.Offered && tmp.Name == "Making Mountains of Molehills (2 of 10)")
                        {
                            _States.tutstates = tutstates.dosecmissi;
                            break;
                        }
                    }
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(20000, 35000));
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



                case tutstates.wait:


                    if (Frame.Client.Session.SystemSecurity >= 0.9 && csystem != Frame.Client.Session.SolarSystemId)
                    {
                        if (Frame.Client.Entities.Where(i => i.Group == Group.Station).Any())
                        {
                            Frame.Log(Frame.Client.Entities.Where(i => i.Group == Group.Station).FirstOrDefault().Id);
                            Frame.Log(Frame.Client.Entities.Where(i => i.Group == Group.Station).FirstOrDefault().Name);
                            Frame.Log(Frame.Client.Entities.Where(i => i.Group == Group.Station).FirstOrDefault().LocationId);
                            Frame.Log(Frame.Client.GetLocalChat.MemberCount);

                            string path = @"c:\syses.txt";
                          using (StreamWriter sw = File.AppendText(path))
                          {
                               sw.WriteLine(Frame.Client.Entities.Where(i => i.Group == Group.Station).FirstOrDefault().Id);
                               sw.WriteLine(Frame.Client.Entities.Where(i => i.Group == Group.Station).FirstOrDefault().Name);
                               sw.WriteLine("People in Space: " + Frame.Client.GetLocalChat.MemberCount);
                            }    
                        }
           }


                        
                        csystem = Frame.Client.Session.SolarSystemId;
                    

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(200000000, 350000000));      //dirty 
                    break;
                
                /*
                     * 
                     * 
                     * 
                    double t = 1228;
                                List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();
                                          if (markyord.Count == 0)
                                          {
                                              break;
                                          }
                                                                                                    List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == t).Where(x => x.inrange == true).Where(x => x.bid == true).ToList();
                                         foreach (EveMarketOrder tmp in marketitemZ)
                                         {
                                            Frame.Log("Name = " + tmp.Name );
                                            Frame.Log("_name = " + tmp._name);
                                            Frame.Log("Solarsystemid = " + tmp.solarSystemID);
                                            Frame.Log("range = " + tmp.range);
                                            Frame.Log("price = " + tmp.price);
                                            Frame.Log("OrderID = " + tmp.orderID);
                                            Frame.Log("jumps = " + tmp.jumps);
                                            Frame.Log("regionID = " + tmp.regionID);
                                            Frame.Log("typeID = " + tmp.typeID);
                                            Frame.Log("inrange = " + tmp.inrange);
                                            Frame.Log("bid = " + tmp.bid);
                                            Frame.Log("..................");
                                            Frame.Log("..................");
                                            Frame.Log("..................");
                                            Frame.Log("..................");
                                         }
                                         EveMarketOrder marketitem;
                                         marketitem = marketitemZ.OrderByDescending(x => x.price).Where(x => x.price > 15).FirstOrDefault();
                                            Frame.Log("..................");
                                            Frame.Log("..................");
                                            Frame.Log("marketitem");
                                            Frame.Log("..................");
                                            Frame.Log("Name = " + marketitem.Name );
                                            Frame.Log("_name = " + marketitem._name);
                                            Frame.Log("Solarsystemid = " + marketitem.solarSystemID);
                                            Frame.Log("range = " + marketitem.range);
                                            Frame.Log("price = " + marketitem.price);
                                            Frame.Log("OrderID = " + marketitem.orderID);
                                            Frame.Log("jumps = " + marketitem.jumps);
                                            Frame.Log("regionID = " + marketitem.regionID);
                                            Frame.Log("typeID = " + marketitem.typeID);
                                            Frame.Log("inrange = " + marketitem.inrange);
                                            Frame.Log("bid = " + marketitem.bid);
                                            Frame.Log("..................");
                                            Frame.Log("..................");
                                            Frame.Log("..................");
                                            Frame.Log("..................");

                                         break;
                    */
                                      //    if (marketitemZ != null)
                                      //    {
                                      //      marketitem = marketitemZ.OrderByDescending(x => x.price).Where(x => x.price > 15).FirstOrDefault();
                                      //    }

              //      Frame.Client.Session.ShipId;
               //     Frame.Log("typname =   " + Frame.Client.GetActiveShip.TypeName);
               //     Frame.Log("typid =  " + Frame.Client.GetActiveShip.TypeId);


            /*
                                  List<EveItem> itte = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                                  foreach (EveItem tmp in itte)
                                  {
                                      Frame.Log("Items ID " + tmp.TypeId + "items name  =  " + tmp.TypeName);
                                  }
                   
            
                           _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                                  break;
                    
                    /*
                                List<EveItem> items;
                                if (Frame.Client.getinvopen() == false)
                                {
                                    Frame.Client.Getandopenwindow("leer");
                                    break;
                                }

                                  Frame.Log("bin bei Unload");
                                  items = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;                                              // Get itemslist check fehlt
                                  Frame.Log(items.Count);                                                                                                         // Logbuch Items zahl
                                  if (items.Count == 0)
                                  {
                                      items = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                                  }


                                  if (items.Count != 0)                                                                                                          // Wenn items zahl ungleich 0 ist dann
                                  {
                                      foreach (EveItem tmp in items) 
                                      {
                                          Frame.Log("Eveitem tmp =  " +  tmp.TypeName + "  " + tmp.ItemId + "  " + tmp.TypeId );

                                      }
                    
                                  }
                  

                   
                                   List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();
                                          if (markyord.Count == 0)
                                          {
                                              break;
                                          }

                                          EveMarketOrder marketitem;
                                          List<EveMarketOrder> marketitemZ = markyord.Where(x => x.typeID == "").Where(x => x.inrange == true).Where(x => x.bid == true).ToList();
                                          if (marketitemZ != null)
                                          {
                                            marketitem = marketitemZ.OrderByDescending(x => x.price).Where(x => x.price > 15).FirstOrDefault();
                                          }
                                  

                    /*


                    List<EveItem> a = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                    foreach (EveItem tmp in a)
                    {
                        Frame.Log(" Name =   " + tmp.TypeName + "   Typid =   " + tmp.TypeId);
                    }
                    Frame.Log("Fertig");
                     * 
                     * */

                   
                    /*
       
                
                       _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                      List<EveMarketOrder> markyord = Frame.Client.GetCachedOrders();
                        if (markyord.Count == 0)
                        {
                            break;
                        }
                    foreach (EveMarketOrder blub in markyord)
                    {
                        Frame.Log("marketorder name =  " + blub.Name + " + " +  blub.typeID);
                    }
                       break;

                    */

            }
        }
    }
}

