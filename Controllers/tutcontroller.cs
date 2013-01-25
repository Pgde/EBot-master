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
        string TutAgent = "Abishi Tian";









        public tutcontroller()
        {
            Frame.Log("Starting a new tutcontroller");

        }


        public override void DoWork()
        {



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

               //     Frame.Log(Frame.Client.wealth());
             //     Frame.Log(Frame.Client.Session.LocationId);
                   Tuple<int,int> tmp2 = new Tuple<int,int> (22578,1);
                    BuyController.buylist2.Add(tmp2);
                   _States.BuyControllerState = BuyControllerStates.buy;
                    _States.tutstates = tutstates.Idle;


                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;

       
                /*
                 *         _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
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

