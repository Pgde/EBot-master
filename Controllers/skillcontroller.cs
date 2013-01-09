using System;
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


            List<EveSkill> tooskill;

                     _state = TravelStates.Start;
                    break;


                case TravelStates.Start:





                    break;
            }
             
            
        }
    }
}

