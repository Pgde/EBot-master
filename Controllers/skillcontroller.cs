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

        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////













     

        public SkillController()
        {
            Frame.Log("Starting a new SkillController");
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
                    int skillsinlist = Settings.Settings.Instance.Skilllist.Count;
                    skilltotrainid = Settings.Settings.Instance.Skilllist;
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
                    string a2 = a[1];                                                                                           // [1]
                    Frame.Log("Skillz Typid = " + a1);                                                                              // log
                    Frame.Log("Skillz gewuenschter lvl = " + a2);                                                                              // log
                    long? blub = long.Parse(a1);
                    EveSkill bugg = neueskill2.Where(i => i.typeID == blub).FirstOrDefault();
                    if (bugg == null)
                    {
                        Frame.Log("Skill nicht vorhanden muss einkaufen");
                        // Funktion zum einkaufen schreiben
                       // _States.SkillState = SkillState.buyskill;
                        int remove = Math.Min(skilltotrainid.Count, 1);
                        skilltotrainid.RemoveRange(0, remove);
                        Frame.Log("Remove First entry of list ");
                       // _States.SkillState = SkillState.wait;
                        break;

                    }
                    if (bugg.Skilllvl > int.Parse(a2)) 
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

                case SkillState.done:
                    _localPulse = DateTime.Now.AddMilliseconds(GetRandom(1000, 2500));
                    break;
            }
        }
    }
}

