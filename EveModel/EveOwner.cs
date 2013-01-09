using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveOwner : EveInvType
    {
        public EveOwner(EveObject obj) : base(obj) {}
        public string Name { get; internal set; }
        public long OwnerId { get; internal set; }
    }
}
