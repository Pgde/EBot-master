using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveAgentDialogWindow : EveWindow
    {
        public string Briefing
        {
            get { return this["sr"]["briefingBrowser"]["sr"]["currentTXT"].GetValueAs<String>(); }
        }
        public string Objective
        {
            get { return this["sr"]["objectiveBrowser"]["sr"]["currentTXT"].GetValueAs<String>(); }
        }
        public int AgentId
        {
            get { return this["sr"]["agentID"].GetValueAs<int>(); }
        }
        public bool MissionIsThroughLowSec
        {
            get
            {
                var ma = System.Text.RegularExpressions.Regex.Matches(Objective, @"//\d{8}");

                var pickup = int.Parse(ma[0].ToString().Substring(2));
                var dropoff = int.Parse(ma[1].ToString().Substring(2));

                var pathfinder = Frame.Client.GetService("pathfinder");
                var path = pathfinder.CallMethod("GetPathBetween", new object[] { pickup, dropoff }).GetList<int>();

                return path.Any(p => Frame.Client.MapService.CallMethod("GetSecurityStatus", new object[] { p }).GetValueAs<double>() < 0.5);
            }
        }
    }
}
