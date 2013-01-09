using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class Charslot : EveObject
    {


        public Charslot(IntPtr ptr)
            : base()
        {
            PointerToObject = ptr;
            idx = this["sr"]["idx", false].GetValueAs<int>();
            ItemId = this["sr"]["rec"]["characterID", false].GetValueAs<int>();
            Name = this["sr"]["rec"]["characterName", false].GetValueAs<string>();
        }



        public int ItemId { get; private set; }
        public int idx { get; private set; }
        public string Name { get; private set; }

 


    }
}