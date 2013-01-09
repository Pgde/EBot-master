using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveSkill : EveObject
    {


        public EveSkill(IntPtr ptr)
            : base()
        {
            PointerToObject = ptr;
            ItemId = this["itemID", false].GetValueAs<long>();
            typeID = this["typeID", false].GetValueAs<long>();
            OwnerId = this["ownerID", false].GetValueAs<long>();
            locationId = this["locationID", false].GetValueAs<long>();
            Skilllvl =  Frame.Client.GetService("godma").CallMethod("GetItem", new object[] { ItemId })["skillLevel"].GetValueAs<int>();
            groupID = this["groupID", false].GetValueAs<long>();
            Skillpoints = Frame.Client.GetService("godma").CallMethod("GetItem", new object[] { ItemId })["skillPoints"].GetValueAs<int>();
            SkillTimeConstant = Frame.Client.GetService("godma").CallMethod("GetItem", new object[] { ItemId })["skillTimeConstant"].GetValueAs<int>();
        }
            

        public long? ItemId { get; private set; }
        public long? typeID { get; private set; }
        public long? OwnerId { get; private set;  }
        public long? locationId { get; private set; }
        public long? groupID { get; private set; }
        public string _name { get; private set; }
        public int? Skillpoints { get; private set; }
        public int Skilllvl { get; private set; }
        public int? SkillTimeConstant { get; private set; }
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                    _name = Frame.Client.GetInvType(new object[] { typeID })["typeName"].GetValueAs<string>();
                return _name;
            }
        }

       
    }
}
