using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;
using Controllers.states;

namespace Controllers
{

    public class SkillController : BaseController
    {
     
     //   enum TravelStates { Initialise, Start, Travel, ArrivedAtDestination, sqlsettime, sqlstarttime, sqlcheck, sqltimecheck, buyskill }


        //   List<string> skilltotrainid = new List<string>();        



        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////

        List<string> skilltotrainid = new List<string>();
        List<string> vergleichlist = new List<string>();
        List<EveItem> itemlistee = new List<EveItem>();
        List<string> vergleichlist2 = new List<string>();
        List<EveItem> ersteitemlistee = new List<EveItem>();
        
        public static int itemid { get; set; }
        public static int dronenmoeglich { get; set; }
        long? skilldronen = 3436;
        long? skilldronenop = 3438;
        bool firstread = false;
        string[] bung2;
        

        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////



        // Mining Drone 1 Typid = 10246
        // Mining Upgrades Typid = 22578
        // Science Typid = 3402
        // Industry Typid = 3380
        // Astrology Typid = 3410
        // Mining Drone OP Typid = 3438
        // Mining Frigatte 32918
        // Mining 3386
        // Mining barge 17940

  

        public SkillController()
        {
            Frame.Log("Starting a new SkillController");
            dronenmoeglich = 0;
            itemid = 0;
            _States.SkillState = SkillState.wait;
          
        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_States.SkillState)
            {
                case SkillState.Initialise:
                    double money = Frame.Client.wealth();
                    if (money < 500000)
                    {
                        Frame.Log("Skillcontroller == nicht genug geld /  " + money);
                        _States.SkillState = SkillState.done;
                        break;
                    }
                 
                    if (!Frame.Client.GetService("skillqueue").IsValid)
                    {
                        _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));
                        break;
                    }
                    //      Frame.Client.refreshskillq();

                    List<EveSkill> neueskill = Frame.Client.GetMySkills();
                    List<EveQskill> neueQskill = Frame.Client.GetMyQueue();

                    foreach (EveSkill tmp in neueskill)
                    {
                        Frame.Log("Skill = " + tmp.Name + ",  " + "Lvl = " + tmp.Skilllvl + " , " + "skillid  = " + tmp.typeID);
                    }
                    Frame.Log("................");
                    Frame.Log("................");
                    Frame.Log("................");
                    Frame.Log("................");
                    foreach (EveQskill tmp in neueQskill)
                    {
                        Frame.Log("Skill = " + tmp._name + ",  " + "Lvl = " + tmp.Skilllvl + " , " + "skillid  = " + tmp.typeID);
                    }
                    Frame.Log("................");
                    Frame.Log("................");
                    Frame.Log("................");
                    Frame.Log("................");


                
                  
                   


                    //        string skillscience = "3402";                                                                                                                            // typids der skills
                    //        string skillsminingfrigat = "32918";
                    //      string skillastroglo = "3410";
                    //    string skillmining = "3386";
                    //  string skillminingup = "22578";
                    // string skillindustry = "";



                    //     List<string> skilltotrainid = new List<string>();  
                    // Skill liste die wir brauchen 
                    if (Settings.Settings.Instance.Skilllist == null)
                    {
                        _States.SkillState = SkillState.done;
                        break;
                    }
                    //            int skillsinlist = Settings.Settings.Instance.Skilllist.Count;
                    //            skilltotrainid = Settings.Settings.Instance.Skilllist;
                    //            vergleichlist = skilltotrainid;

                    int skillsinlist = Settings.Settings.Instance.Skilllist.Count;
                    skilltotrainid = Settings.Settings.Instance.Skilllist;
                    
                    if (firstread == false)
                    {
                        string[] bung = new string[skillsinlist];
                        vergleichlist = Settings.Settings.Instance.Skilllist;
                        skilltotrainid.CopyTo(bung);
                        bung2 = bung;
                        firstread = true;
                    }

                    /*           skilltotrainid.Insert(skillsinlist, skillsminingfrigat + " " + "2");
                               skillsinlist = skilltotrainid.Count();  // Miningfrigate 2
                               skilltotrainid.Insert(skillsinlist, skillmining + " " + "3"); skillsinlist = skilltotrainid.Count(); // Mining 3
                               skilltotrainid.Insert(skillsinlist, skillminingup + " " + "1"); skillsinlist = skilltotrainid.Count(); // Miningupgrade 1
                               skilltotrainid.Insert(skillsinlist, skillsminingfrigat + " " + "3"); skillsinlist = skilltotrainid.Count();  // Mining frigate 3
                               skilltotrainid.Insert(skillsinlist, skillmining + " " + "4"); skillsinlist = skilltotrainid.Count(); // Mining 4
                               skilltotrainid.Insert(skillsinlist, skillsminingfrigat + " " + "4"); skillsinlist = skilltotrainid.Count(); // Mining frigate 4
                               skilltotrainid.Insert(skillsinlist, skillscience + " " + "4"); skillsinlist = skilltotrainid.Count(); // Science 4
                               skilltotrainid.Insert(skillsinlist, skillastroglo + " " + "3"); skillsinlist = skilltotrainid.Count(); // Astrology 3
           */

                    foreach (string tmp in skilltotrainid)
                    {
                        Frame.Log("Skill = " + tmp);
                    }
                    _States.SkillState = SkillState.Start;
                    break;



                case SkillState.Start:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(2000, 3500));

                    Frame.Log(skilltotrainid.Count);
                    if (skilltotrainid.Count < 1)
                    {
                        Frame.Log("liste leer");
                        _States.SkillState = SkillState.done;
                        break;
                    }

                    if (Frame.Client.GetService("skillqueue").IsValid)
                    {
                        double debugg = Frame.Client.qlengdouble;
                        Frame.Log("Debugg float lenge = " + debugg);
                        if (!Frame.Client.placeinq())
                        {
                            Frame.Log("kein platz in q");
                            _States.SkillState = SkillState.done;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    List<EveSkill> neueskill2 = Frame.Client.GetMySkills();
                    List<EveQskill> neueQskill2 = Frame.Client.GetMyQueue();


                    string buk = skilltotrainid.FirstOrDefault();                                                               // ersten skill inder liste 
                     Frame.Log("Skillz = " + buk);                                                                               // logbuch
                    string[] a = buk.Split(new Char[] { });                                                                     // teile den string in typid und lvl
                    string a1 = a[0];                                                                                           // [0]
                    string a2 = a[1];        
                    string a3 = a[2];
                    int a22 = Convert.ToInt32(a2);
                    int aa3 = Convert.ToInt32(a3);
                    int bunsch1 = Convert.ToInt32(a1);
                    itemid = bunsch1;
                    Frame.Log("Skillz Typid = " + a1);                                                                              // log
                    Frame.Log("Skillz gewuenschter lvl = " + a2);       
                   Frame.Log("Skillz weitreleveln ? = " + a3);
                   EveSkill schkill = neueskill2.Where(x => x.typeID == itemid).FirstOrDefault();
                    if (schkill != null)
                    {
                        if (schkill.Skilllvl >= a22)
                        {
                            int remove = Math.Min(skilltotrainid.Count, 1);
                            skilltotrainid.RemoveRange(0, remove);
                            Frame.Log("Remove First entry of list ");
                            break;
                        }
                    }
                   

                   if (aa3 == 1 && neueQskill2.Count != 0)
                   {
                      
                       Frame.Log("Skill benötigt eine leere Q");
                       _States.SkillState = SkillState.done;
                    //  _States.MiningState = MiningState.letzgo;
                       break;
                   }
                  
                    long? blub = long.Parse(a1);
                    EveSkill bugg = neueskill2.Where(i => i.typeID == blub).FirstOrDefault();
                    if (bugg == null)
                    {
                        Frame.Log("Skill nicht vorhanden muss einkaufen");

                        if (Frame.Client.getinvopen() == false)
                        {
                            Frame.Client.Getandopenwindow("leer");
                            break;
                        }

                        Frame.Client.GetItemHangar();
                        ersteitemlistee = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;
                        EveItem skillbookitem2 = ersteitemlistee.Where(i => i.TypeId == itemid).FirstOrDefault();
                        if (skillbookitem2 != null)
                        {
                            Frame.Log("Buch schon im Hanger");
                         
                            _States.BuyControllerState = BuyControllerStates.done;
                            _States.SkillState = SkillState.buyskill;
                            break;

                        }

                       Tuple<int,int> tmp = new Tuple<int,int> (bunsch1,1);
                        BuyController.buylist.Add(tmp);
                      _States.BuyControllerState = BuyControllerStates.buy;
                        _States.SkillState = SkillState.buyskill;
                        break;

                    }
                    if (bugg.Skilllvl >= int.Parse(a2)) 
                    {
                        int remove = Math.Min(skilltotrainid.Count, 1);
                        skilltotrainid.RemoveRange(0, remove);
                        Frame.Log("Remove First entry of list ");
                        break;
                    }

                    int? bugskilllvl = bugg.Skilllvl;
                    Frame.Log("Skill vorhanden Level =  " + bugskilllvl);
                    EveQskill buggy2 = neueQskill2.Where(i => i.typeID == blub).FirstOrDefault();
                    if (buggy2 == null)
                    {
                        Frame.Client.AddSkillToEnd(bugg, bugg.Skilllvl);
                        Frame.Log("Skill vorhanden aber nicht in der Skillque");
                        Frame.Log("Skill hinzugefügt");


                        break;
                    }
                    else
                    {
                        EveQskill buggy = neueQskill2.Where(i => i.typeID == blub).OrderByDescending(i => i.Skilllvl).FirstOrDefault();
                        Frame.Log("Skill level hoechster zuerst " + buggy.Skilllvl);
                        Frame.Log("DEBUGG  " + buggy.Skilllvl + "  +  " + int.Parse(a2));
                        if (buggy.Skilllvl < int.Parse(a2))
                        {
                            Frame.Client.AddSkillToEnd(bugg, bugg.Skilllvl);
                            Frame.Log("DEBUGG Aktueller skill level =   " + buggy.Skilllvl + "  + gewünschtes level =  " + int.Parse(a2) + " Skill hinzugefügt ");
                            break;
                        }
                        int remove = Math.Min(skilltotrainid.Count, 1);
                        skilltotrainid.RemoveRange(0, remove);
                        Frame.Log("Remove First entry of list ");
                    }


                    break;


                case SkillState.wait:

                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;


                case SkillState.buyskill:
   
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    if (_States.BuyControllerState == BuyControllerStates.done)
                    {


                        Frame.Log("Test22");   
                                      if (Frame.Client.getinvopen() == false)
                                            {
                                           Frame.Client.Getandopenwindow("leer");
                                            break;
                                              }
                                Frame.Client.GetItemHangar();               
                               itemlistee = Frame.Client.GetPrimaryInventoryWindow.ItemHangar.Items;

                               foreach (string tmp in bung2)
                               {
                                   Frame.Log("Skill vergleichsliste2 = " + tmp);
                                   Frame.Log(".....");
                                    string[] aa = tmp.Split(new Char[] { });                                                                     // teile den string in typid und lvl
                                   string aa1 = aa[0];
                                   vergleichlist2.Add(aa1);
                               }

                               foreach (EveItem tmp in itemlistee)
                               {
                                   Frame.Log("Skill = " + tmp.TypeId);
                                   int buggy = tmp.TypeId;
                                   string buggy22 = buggy.ToString();
                                   string[] aa = buggy22.Split(new Char[] { });                                                                     // teile den string in typid und lvl
                                   string aa1 = aa[0];
                                   Frame.Log("Sstring nach teilung  " + aa1);
                                   if (vergleichlist2.Contains(aa1))
                                   {
                                       Frame.Log("Item gefunden");
                                        EveItem skillbookitem = itemlistee.Where(i => i.TypeId == buggy).FirstOrDefault();
                                        Frame.Client.InjectSkillIntoBrain(skillbookitem);
                                           Frame.Log("Inject skill");
                                  }
                              
                               }
                                    
                        _States.BuyControllerState = BuyControllerStates.wait;
                        _States.SkillState = SkillState.Initialise;
                    }

                    break;

                case SkillState.done:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;
            }

              }

           
            }

        }
    



