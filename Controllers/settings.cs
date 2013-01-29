
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
        public List<String> Skilllist {get; set;}

        public void rndhomesys()
        {
            Random random = new Random();
            int i = random.Next(1, 4); 
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
                        homesys1 = (long)xml.Element("homesys1");
                        homesys2 = (long)xml.Element("homesys2");
                        homesys3 = (long)xml.Element("homesys3");
                        homesys4 = (long)xml.Element("homesys4");
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