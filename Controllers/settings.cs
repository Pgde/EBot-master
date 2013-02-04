﻿
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
        public List<String> Skilllist {get; set;}
        public string timechecktmp { get; set; }
        public bool timecheckbool { get; set; }
        public void rndhomesys()
        {
            Random random = new Random();
            int i = random.Next(1, 7); 
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