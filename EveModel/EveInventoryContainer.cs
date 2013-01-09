using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveInventoryContainer : EveObject
    {
        public EveInventoryContainer(EveObject parent)
        {
            this.PointerToObject = parent.PointerToObject;
        }
        public string ShortName
        {
            get { return this["invController"]["name"].GetValueAs<string>(); }
        }
        public string Name
        {
            get { return this["invController"].CallMethod("GetName", new object[0]).GetValueAs<string>(); }
        }

        public List<EveItem> Items
        {
            get
            {
                List<EveItem> items = new List<EveItem>();
                foreach (var item in this["invController"].CallMethod("GetItems", new object[0]).GetList<EveObject>())
                {
                    items.Add(new EveItem(item));
                }
                return items;
            }
        }
        public double Capacity
        {
            get { return this["invController"].CallMethod("GetCapacity", new object[0])["capacity"].GetValueAs<double>(); }
        }
        public double UsedCapacity
        {
            get { return this["invController"].CallMethod("GetCapacity", new object[0])["used"].GetValueAs<double>(); }
        }
        public void Add(EveItem item)
        {
            this["invController"].CallMethod("_AddItem", new object[] { item }, true);
        }
        public void Add(EveItem item, int quantity)
        {
            this["invController"].CallMethod("_BaseInvContainer__AddItem", new object[] { item.ItemId, item.LocationId, quantity }, true);
        }
        public void StackAll()
        {
            this["invController"].CallMethod("StackAll", new object[0], true);
        }
    }
}
