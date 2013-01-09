using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveChatWindow : EveWindow
    {
        List<EveCharacter> _members;
        public List<EveCharacter> Members
        {
            get
            {
                if (_members == null)
                {
                    _members = new List<EveCharacter>();
                    var channelId = this["channelID"];
                    var channels = Frame.Client.LSCService["channels"];

                    var channel = new EveObject(PyCall.PyDict_GetItem(channels.PointerToObject, channelId.PointerToObject), "channel", false);
                    var memberList = channel["memberList"];
                    foreach (var item in memberList.CallMethod("keys", new object[0]).GetList<EveObject>())
                    {
                        var newMember = new EveCharacter()
                        {
                            PointerToObject = memberList.CallMethod("__getitem__", new object[] { item }).PointerToObject
                        };

                        newMember.AllianceId = newMember["allianceID"].GetValueAs<int>();
                        newMember.CharacterId = newMember["charID"].GetValueAs<int>();
                        newMember.CorporationId = newMember["corpID"].GetValueAs<int>();
                        //newMember.Name = newMember["info"]["name"].GetValueAs<string>();
                        newMember.WarFactionId = newMember["warFactionID"].GetValueAs<int>();

                        _members.Add(newMember);
                    }
                }
                return _members;
            }
        }
        public int MemberCount
        {
            get
            {
                return Members.Count;
            }
        }
    }
}
