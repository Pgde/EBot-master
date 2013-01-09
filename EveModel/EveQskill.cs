using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveQskill : EveObject
    {


        public EveQskill(IntPtr ptr)
            : base()
        {
            PointerToObject = ptr;

            List<int> tmp = this.GetListFromTuple<int>();
            typeID = tmp[0];
            Skilllvl = tmp[1];
          
        }


       
        public long? typeID { get; private set; }
      
        public string _name { get; private set; }
      
        public int? Skilllvl { get; private set; }
   

    }
}
