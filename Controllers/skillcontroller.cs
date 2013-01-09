﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveModel;
using MySql.Data.MySqlClient;

namespace Controllers
{

    public class SkillController : BaseController
    {
        long _destinationId, _currentLocation, _currentDestGateId;
        bool _waitforsessionChange;
        enum TravelStates { Initialise, Start, Travel, ArrivedAtDestination, sqlsettime, sqlstarttime, sqlcheck, sqltimecheck }






        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////


        ////////////////////////////////////////////////////////
        ///////////           VARIABLEN        ////////













        TravelStates _state;

        public SkillController()
        {
            Frame.Log("Starting a new SkillController");
        }


        public override void DoWork()
        {
            if (IsWorkDone || _localPulse > DateTime.Now || Frame.Client.Session.NextSessionChange > DateTime.Now)
            {
                return;
            }
            switch (_state)
            {
                case TravelStates.Initialise:


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




            string skillscience = "3402";                                                                                                                            // typids der skills
            string skillsminingfrigat = "32918";
            string skillastroglo = "3410";
            string skillmining = "3386";
            string skillminingup = "22578";
            string skillindustry = "";
            

 
            List<string> skilltotrainid = new List<string>();                                                                                                 // Skill liste die wir brauchen 
            int skillsinlist = skilltotrainid.Count();
            skilltotrainid.Insert(skillsinlist, skillsminingfrigat + " " +  "5"); skillsinlist = skilltotrainid.Count();  // Miningfrigate 2
            skilltotrainid.Insert(skillsinlist, skillmining + " " + "3"); skillsinlist = skilltotrainid.Count(); // Mining 3
            skilltotrainid.Insert(skillsinlist, skillminingup + " " + "1"); skillsinlist = skilltotrainid.Count(); // Miningupgrade 1
            skilltotrainid.Insert(skillsinlist, skillsminingfrigat + " " + "3"); skillsinlist = skilltotrainid.Count();  // Mining frigate 3
            skilltotrainid.Insert(skillsinlist, skillmining + " " + "4"); skillsinlist = skilltotrainid.Count(); // Mining 4
            skilltotrainid.Insert(skillsinlist, skillsminingfrigat + " " + "4"); skillsinlist = skilltotrainid.Count(); // Mining frigate 4
            skilltotrainid.Insert(skillsinlist, skillscience + " " + "4"); skillsinlist = skilltotrainid.Count(); // Science 4
            skilltotrainid.Insert(skillsinlist, skillastroglo + " " + "3"); skillsinlist = skilltotrainid.Count(); // Astrology 3



                   foreach (string tmp in skilltotrainid)
            {
                Frame.Log("Skill = " + tmp );
            }



            List<EveSkill> tooskill;

                     _state = TravelStates.Start;
                    break;


                case TravelStates.Start:





                    break;
            }
             
            
        }
    }
}

