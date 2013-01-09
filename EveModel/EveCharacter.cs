using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveCharacter : EveObject
    {
        public int CharacterId { get; internal set; }
        public int CorporationId { get; internal set; }
        public int AllianceId { get; internal set; }
        public int WarFactionId { get; internal set; }
        string _name;
        public string Name {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                    _name = Frame.Client.GetOwner(new object[] { CharacterId })["ownerName"].GetValueAs<string>();
                return _name;
            }
        }
    }
}
