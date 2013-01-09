using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveAgent : EveObject
    {
        public enum AgentTypes
        {
            NonAgent = 1,
            BasicAgent = 2,
            TutorialAgent = 3,
            ResearchAgent = 4,
            CONCORDAgent = 5,
            GenericStorylineMissionAgent = 6,
            StorylineMissionAgent = 7,
            EventMissionAgent = 8,
            FactionalWarfareAgent = 9,
            EpicArcAgent = 10,
            AuraAgent = 11
        }
        public enum AgentDivisions
        {
            Accounting = 1,
            Administration = 2,
            Advisory = 3,
            Archives = 4,
            Astrosurveying = 5,
            Command = 6,
            //Distribution = 7,
            Financial = 8,
            Intelligence = 9,
            InternalSecurity = 10,
            Legal = 11,
            Manufacturing = 12,
            Marketing = 13,
            //Mining = 14,
            Personnel = 15,
            Production = 16,
            PublicRelations = 17,
            RD = 18,
            //Security = 19,
            Storage = 20,
            Surveillance = 21,
            Distribution = 22,
            Mining = 23,
            Security = 24
        }
        internal EveAgent(IntPtr ptr, string name)
            : base(ptr, name, false)
        {
            Name = name;
            AgentId = this["agentID"].GetValueAs<int>();
            AgentType = (AgentTypes)this["agentTypeID"].GetValueAs<int>();
            Division = (AgentDivisions)this["divisionID"].GetValueAs<int>();
            Level = this["level"].GetValueAs<int>();
            StationId = this["stationID"].GetValueAs<int>();
            BloodlineId = this["bloodlineID"].GetValueAs<int>();
            CorporationId = this["corporationID"].GetValueAs<int>();
            FactionId = this["factionID"].GetValueAs<int>();
            SolarSystemId = this["solarsystemID"].GetValueAs<int>();
            LocationId = StationId <= 0 ? SolarSystemId : StationId;
        }

        public int AgentId { get; private set; }
        public AgentTypes AgentType { get; private set; }
        public int BloodlineId { get; private set; }
        public int CorporationId { get; private set; }
        public AgentDivisions Division { get; private set; }
        public int FactionId { get; private set; }
        public int Level { get; private set; }
        public string Name { get; private set; }
        public int StationId { get; private set; }
        public int SolarSystemId { get; private set; }
        public int LocationId { get; private set; }
        public int Quality { get { return 20; } }

        public void StartConversation()
        {
            Frame.Client.GetService("agents").CallMethod("InteractWith", new object[] { AgentId }, true);
        }

        public void PopupMissionJournal()
        {
            Frame.Client.GetService("agents").CallMethod("PopupMissionJournal", new object[] { AgentId }, true);
        }
    }
}
