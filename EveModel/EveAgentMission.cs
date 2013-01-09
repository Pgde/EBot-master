using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveAgentMission : EveObject
    {
        public enum MissionState { Offered = 1, Accepted = 2 }
        public enum MissionType { Courier, Encounter, Mining, Unknown }

        public MissionState State
        {
            get
            {
                return (MissionState)this.GetListFromTuple<EveObject>()[0].GetValueAs<int>();
            }
        }

        public bool Important
        {
            get
            {
                return this.GetListFromTuple<EveObject>()[1].GetValueAs<bool>();
            }
        }

        MissionType? _type;
        public MissionType Type
        {
            get
            {
                if (!_type.HasValue)
                {
                    string missionType = this.GetListFromTuple<EveObject>()[2].GetValueAs<string>();
                    if (missionType.EndsWith("Courier"))
                        _type = MissionType.Courier;
                    else if (missionType.EndsWith("Encounter"))
                        _type = MissionType.Encounter;
                    else if (missionType.EndsWith("Mining"))
                        _type = MissionType.Mining;
                    else
                        _type = MissionType.Unknown;
                }
                return _type.Value;
            }
        }
        public string Name
        {
            get
            {
                string missionName = string.Empty;
                var name = this.GetListFromTuple<EveObject>()[3];
                if (name.ObjectType == PyType.StringType)
                    missionName = name.GetValueAs<string>();
                else if (name.ObjectType == PyType.IntType)
                {
                    int nameId = name.GetValueAs<int>();
                    missionName = Frame.Client.Localization.CallMethod("GetByMessageID", new object[] { nameId }).GetValueAs<string>();
                }
                return missionName.Replace("\\u2013", "-");
            }
        }
        public int AgentId
        {
            get
            {
                return this.GetListFromTuple<EveObject>()[4].GetValueAs<int>();
            }
        }
        public DateTime ExpiresOn
        {
            get
            {
                return this.GetListFromTuple<EveObject>()[5].GetValueAs<DateTime>();
            }
        }
        List<EveAgentBookmark> _bookmarks;
        public List<EveAgentBookmark> Bookmarks
        {
            get
            {
                if (_bookmarks == null || (_bookmarks != null && _bookmarks.Count == 0))
                {
                    _bookmarks = this.GetListFromTuple<EveObject>()[6].GetList<EveObject>().ConvertAll<EveAgentBookmark>(new Converter<EveObject,EveAgentBookmark>(EveClient.EveObject2EveAgentBookmark));
                }
                return _bookmarks;
            }
        }

        public bool Attrib7
        {
            get
            {
                return this.GetListFromTuple<EveObject>()[7].GetValueAs<bool>();
            }
        }
        public bool Attrib8
        {
            get
            {
                return this.GetListFromTuple<EveObject>()[8].GetValueAs<bool>();
            }
        }
    }
}
