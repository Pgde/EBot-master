﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.states
{
   public class _States
    {
        public static  TravelerState TravelerState { get; set; }
        public static MiningState MiningState { get; set; }
        public static DroneState DroneState { get; set; }
        public static loginstate LoginState { get; set; }
        public static SkillState SkillState { get; set; }
        public static MysqlState MysqlState { get; set; }
        public static fittingstate fittingstate { get; set; }
        public static tutstates tutstates { get; set; }
       public static maincontrollerStates maincontrollerState { get; set; }
       public static BuyControllerStates BuyControllerState { get; set; }
    }
}
