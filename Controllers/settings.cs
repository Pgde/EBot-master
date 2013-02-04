
namespace Controllers.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Globalization;
    using EveModel;
    using System.Diagnostics;

    public class Settings
    {
        public int i = 0;
        public string path = "C:/botxml.xml";
        public string bottopid = "C:/bottopid.xml";
        public string botxmlpath { get; set; }
        public static Settings Instance = new Settings();
        public bool DebugIdle { get; set; }
        public string Charname { get; set; }
        public string Accname { get; set; }
        public string Pw { get; set; }
        public long homesys { get; set; }
        public long homesys1 { get; set; }
        public long homesys2 { get; set; }
        public long homesys3 { get; set; }
        public long homesys4 { get; set; }
        public long homesys5 { get; set; }
        public long homesys6 { get; set; }
        public long homesys7 { get; set; }
        public long homesys8 { get; set; }
        public long homesys9 { get; set; }
        public long homesys10 { get; set; }
        public long homesys11 { get; set; }
        public long homesys12 { get; set; }
        public long homesys13 { get; set; }
        public long homesys14 { get; set; }
        public long homesys15 { get; set; }
        public long homesys16 { get; set; }
        public long homesys17 { get; set; }
        public long homesys18 { get; set; }
        public long homesys19 { get; set; }
        public long homesys20 { get; set; }
        public long homesys21 { get; set; }
        public long homesys22 { get; set; }
        public long homesys23 { get; set; }
        public long homesys24 { get; set; }
        public long homesys25 { get; set; }
        public long homesys26 { get; set; }
        public long homesys27 { get; set; }
        public long homesys28 { get; set; }
        public long homesys29 { get; set; }
        public long homesys30 { get; set; }
        public long homesys31 { get; set; }
        public long homesys32 { get; set; }
        public long homesys33 { get; set; }
        public long homesys34 { get; set; }
        public long homesys35 { get; set; }
        public long homesys36 { get; set; }
        public long homesys37 { get; set; }
        public long homesys38 { get; set; }
        public long homesys39 { get; set; }
        public long homesys40 { get; set; }
        public long homesys41 { get; set; }
        public long homesys42 { get; set; }
        public long homesys43 { get; set; }
        public long homesys44 { get; set; }
        public long homesys45 { get; set; }
        public long homesys46 { get; set; }
        public long homesys47 { get; set; }
        public long homesys48 { get; set; }
        public long homesys49 { get; set; }
        public long homesys50 { get; set; }

        public List<String> Skilllist {get; set;}
        public string timechecktmp { get; set; }
        public bool timecheckbool { get; set; }
        public void rndhomesys()
        {
            Random random = new Random();
            int i = random.Next(1, 50); 
            if (i == 1)
            {
                homesys = homesys1;
            }
            if (i == 2)
            {
                homesys = homesys2;
            }
            if (i == 3)
            {
                homesys = homesys3;
            }
            if (i == 4)
            {
                homesys = homesys4;
            }
            if (i == 5)
            {
                homesys = homesys5;
            }
            if (i == 6)
            {
                homesys = homesys6;
            }
            if (i == 7)
            {
                homesys = homesys7;
            }

            if (i == 8)
            {
                homesys = homesys8;
            }
            if (i == 9)
            {
                homesys = homesys9;
            }
            if (i == 10)
            {
                homesys = homesys10;
            }
            if (i == 11)
            {
                homesys = homesys11;
            }
            if (i == 12)
            {
                homesys = homesys12;
            }
            if (i == 13)
            {
                homesys = homesys13;
            }
            if (i == 14)
            {
                homesys = homesys14;
            }
            if (i == 15)
            {
                homesys = homesys15;
            }
            if (i == 16)
            {
                homesys = homesys16;
            }
            if (i == 17)
            {
                homesys = homesys17;
            }
            if (i == 18)
            {
                homesys = homesys18;
            }
            if (i == 19)
            {
                homesys = homesys19;
            }
            if (i == 20)
            {
                homesys = homesys20;
            }
            if (i == 21)
            {
                homesys = homesys21;
            }
            if (i == 22)
            {
                homesys = homesys22;
            }
            if (i == 23)
            {
                homesys = homesys23;
            }
            if (i == 24)
            {
                homesys = homesys24;
            }
            if (i == 25)
            {
                homesys = homesys25;
            }
            if (i == 26)
            {
                homesys = homesys26;
            }
            if (i == 27)
            {
                homesys = homesys27;
            }
            if (i == 28)
            {
                homesys = homesys28;
            }
            if (i == 29)
            {
                homesys = homesys29;
            }
            if (i == 30)
            {
                homesys = homesys30;
            }
            if (i == 31)
            {
                homesys = homesys31;
            }
            if (i == 32)
            {
                homesys = homesys32;
            }
            if (i == 33)
            {
                homesys = homesys33;
            }
            if (i == 34)
            {
                homesys = homesys34;
            }
            if (i == 35)
            {
                homesys = homesys35;
            }
            if (i == 36)
            {
                homesys = homesys36;
            }
            if (i == 37)
            {
                homesys = homesys37;
            }
            if (i == 38)
            {
                homesys = homesys38;
            }
            if (i == 39)
            {
                homesys = homesys39;
            }
            if (i == 40)
            {
                homesys = homesys40;
            }
            if (i == 41)
            {
                homesys = homesys41;
            }
            if (i == 42)
            {
                homesys = homesys42;
            }
            if (i == 43)
            {
                homesys = homesys43;
            }
            if (i == 44)
            {
                homesys = homesys44;
            }
            if (i == 45)
            {
                homesys = homesys45;
            }
            if (i == 46)
            {
                homesys = homesys46;
            }
            if (i == 47)
            {
                homesys = homesys47;
            }
            if (i == 48)
            {
                homesys = homesys48;
            }
            if (i == 49)
            {
                homesys = homesys49;
            }
            if (i == 50)
            {
                homesys = homesys50;
            }
        }


        public void LoadSettings()
        {


            Skilllist = new List<string>();
            Frame.Log(getbotxml());
                XElement xml = XDocument.Load(getbotxml()).Root;
                if (xml == null)
                {
                return; 
                }
                else
                {
                    Frame.Log("Loading Settings from [" + path + "]");                   
                    try
                    {
                        Charname = (string)xml.Element("Charname") ?? "";
                        Accname = (string)xml.Element("Accname") ?? "";
                        Pw = (string)xml.Element("Pw") ?? "";
                        timechecktmp = (string)xml.Element("timecheck") ?? "true";
                        homesys1 = (long)xml.Element("homesys1");
                        homesys2 = (long)xml.Element("homesys2");
                        homesys3 = (long)xml.Element("homesys3");
                        homesys4 = (long)xml.Element("homesys4");
                        homesys5 = (long)xml.Element("homesys5");
                        homesys6 = (long)xml.Element("homesys6");
                        homesys7 = (long)xml.Element("homesys7");
                        homesys8 = (long)xml.Element("homesys8");
                        homesys9 = (long)xml.Element("homesys9");
                        homesys10 = (long)xml.Element("homesys10");
                        homesys11 = (long)xml.Element("homesys11");
                        homesys12 = (long)xml.Element("homesys12");
                        homesys13 = (long)xml.Element("homesys13");
                        homesys14 = (long)xml.Element("homesys14");
                        homesys15 = (long)xml.Element("homesys15");
                        homesys16 = (long)xml.Element("homesys16");
                        homesys17 = (long)xml.Element("homesys17");
                        homesys18 = (long)xml.Element("homesys18");
                        homesys19 = (long)xml.Element("homesys19");
                        homesys20 = (long)xml.Element("homesys20");
                        homesys21 = (long)xml.Element("homesys21");
                        homesys22 = (long)xml.Element("homesys22");
                        homesys23 = (long)xml.Element("homesys23");
                        homesys24 = (long)xml.Element("homesys24");
                        homesys25 = (long)xml.Element("homesys25");
                        homesys26 = (long)xml.Element("homesys26");
                        homesys27 = (long)xml.Element("homesys27");
                        homesys28 = (long)xml.Element("homesys28");
                        homesys29 = (long)xml.Element("homesys29");
                        homesys30 = (long)xml.Element("homesys30");
                        homesys31 = (long)xml.Element("homesys31");
                        homesys32 = (long)xml.Element("homesys32");
                        homesys33 = (long)xml.Element("homesys33");
                        homesys34 = (long)xml.Element("homesys34");
                        homesys35 = (long)xml.Element("homesys35");
                        homesys36 = (long)xml.Element("homesys36");
                        homesys37 = (long)xml.Element("homesys37");
                        homesys38 = (long)xml.Element("homesys38");
                        homesys39 = (long)xml.Element("homesys39");
                        homesys40 = (long)xml.Element("homesys40");
                        homesys41 = (long)xml.Element("homesys41");
                        homesys42 = (long)xml.Element("homesys42");
                        homesys43 = (long)xml.Element("homesys43");
                        homesys44 = (long)xml.Element("homesys44");
                        homesys45 = (long)xml.Element("homesys45");
                        homesys46 = (long)xml.Element("homesys46");
                        homesys47 = (long)xml.Element("homesys47");
                        homesys48 = (long)xml.Element("homesys48");
                        homesys49 = (long)xml.Element("homesys49");
                        homesys50 = (long)xml.Element("homesys50");
                       
                    }
                    catch (Exception exception)
                    {
                       Frame.Log("Error Loading Ship Name Settings [" + exception + "]");
                    }






                    XElement xmlSkilllist = xml.Element("skilllist");
                    if (xmlSkilllist != null)
                    {
                        Frame.Log("xmlSkilllist");
                        int i = 1;
                        foreach (XElement Skill in xmlSkilllist.Elements("skill"))
                        {
                            Skilllist.Add((string)Skill);
                            i++;
                        }
                        Frame.Log("Skilllistcount: "+ Skilllist.Count );
                    }


                }
                rndhomesys();

                if (timechecktmp == "true")
                {
                    timecheckbool = true;
                }
                else
                {
                    timecheckbool = false;
                }

            Frame.Log(homesys);
        }

     


                   public string getbotxml()
                    {
                        Process currentProcess = Process.GetCurrentProcess();
                        int pid = currentProcess.Id;
                        string bottag = "a" + pid;
                        Frame.Log(bottag);

                       XElement xml = XDocument.Load(bottopid).Root;
                       if (xml == null)
                       {
                           return "error";
                       }
                       else
                       {
                           try
                           {
                               botxmlpath = (string)xml.Element(bottag) ?? "C:/botxml.xml";
                           }
                           catch (Exception exception)
                           {
                               Frame.Log("Error Loading Ship Name Settings [" + exception + "]");
                           }
                           Frame.Log("botxml @ " + botxmlpath);
                       }





                        return botxmlpath;
                    }












                /*
     
                    //
                    // Fittings chosen based on the faction of the mission
                    //
                    FactionFitting.Clear();
                    XElement factionFittings = xml.Element("factionfittings");
                    if (UseFittingManager) //no need to look for or load these settings if FittingManager is disabled
                    {
                        if (factionFittings != null)
                        {
                            foreach (XElement factionfitting in factionFittings.Elements("factionfitting"))
                                FactionFitting.Add(new FactionFitting(factionfitting));
                            if (FactionFitting.Exists(m => m.Faction.ToLower() == "default"))
                            {
                                DefaultFitting = FactionFitting.Find(m => m.Faction.ToLower() == "default");
                                if (string.IsNullOrEmpty(DefaultFitting.Fitting))
                                {
                                    UseFittingManager = false;
                                    Logging.Log("Settings", "Error! No default fitting specified or fitting is incorrect.  Fitting manager will not be used.", Logging.Orange);
                                }
                                Logging.Log("Settings", "Faction Fittings defined. Fitting manager will be used when appropriate.", Logging.White);
                            }
                            else
                            {
                                UseFittingManager = false;
                                Logging.Log("Settings", "Error! No default fitting specified or fitting is incorrect.  Fitting manager will not be used.", Logging.Orange);
                            }
                        }
                        else
                        {
                            UseFittingManager = false;
                            Logging.Log("Settings", "No faction fittings specified.  Fitting manager will not be used.", Logging.Orange);
                        }
                    }

                    */
                    // Fitting based on the name of the mission
                    //
          
            

     

        

   
        
    
}}