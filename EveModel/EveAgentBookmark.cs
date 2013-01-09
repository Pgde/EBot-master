using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveAgentBookmark : EveBookmark
    {
        public EveAgentBookmark(IntPtr ptr) : base(ptr)
        {
            SolarSystemId =  this["solarsystemID"].GetValueAs<long>();
            AgentId =  this["agentID"].GetValueAs<int>();
            IsDeadspace = this["deadspace"].GetValueAs<bool>();
            Flag =  this["flag"].GetValueAs<int>();
            LocationNumber =  this["locationNumber"].GetValueAs<int>();
            LocationType = this["locationType"].GetValueAs<string>();
        }

        public long SolarSystemId { get; private set; }
        public int AgentId { get; private set; }
        public bool IsDeadspace { get; private set; }
        public int Flag { get; private set; }
        public int LocationNumber { get; private set; }
        public string LocationType { get; private set; }

    }
}
