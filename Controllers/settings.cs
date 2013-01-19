
namespace Controllers.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Globalization;
    using EveModel;

    public class Settings
    {
       
        public string path = "C:/botxml.xml";
        public static Settings Instance = new Settings();
        public bool DebugIdle { get; set; }
        public string Charname { get; set; }
        public string Accname { get; set; }
        public string Pw { get; set; }
        public List<String> Skilllist {get; set;}
        public void LoadSettings()
        {


                
                XElement xml = XDocument.Load(path).Root;
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
          
            

     

        

   
        }
    }
}}