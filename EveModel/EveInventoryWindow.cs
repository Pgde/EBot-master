using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveInventoryWindow : EveWindow
    {
        public bool IsPrimaryInvWindow
        {
            get { return this.Guid == "form.InventoryPrimary"; }
        }
        public List<EveInventoryContainer> Containers
        {
            get
            {
                List<EveInventoryContainer> containers = new List<EveInventoryContainer>();
                foreach (var cont in this["treeData"]["_children"].GetList<EveObject>())
                {
                    containers.Add(new EveInventoryContainer(cont));
                }
                return containers;
            }
        }
        public EveInventoryContainer CargoHoldOfActiveShip
        {
            get { return Containers.Where(cont => cont.ShortName.Contains("Cargo Hold")).FirstOrDefault(); }
        }
        public EveInventoryContainer OreHoldOfActiveShip
        {
            get
            {
                var containers = Frame.Client.GetPrimaryInventoryWindow.Containers[0].CallMethod("GetChildren", new object[0]).GetList<EveObject>();
                foreach (var item in containers)
                {
                    if (item["invController"].CallMethod("GetName", new object[0]).GetValueAs<string>() == "Ore Hold")
                    {
                        return new EveInventoryContainer(item);
                    }

                }
                return null;
            }
        }
        public long EntityId
        {
            get
            {
                return Frame.Client.GetPrimaryInventoryWindow["currInvID"].GetListFromTuple<EveObject>()[1].GetValueAs<long>();
            }
        }
        public EveInventoryContainer ItemHangar
        {
            get { return Containers.Where(cont => cont.Name.Contains("Item hangar")).FirstOrDefault(); }
        }
        public EveInventoryContainer ShipHangar
        {
            get { return Containers.Where(cont => cont.Name.Contains("Ship hangar")).FirstOrDefault(); }
        }
        public void LootAll()
        {
            this["invController"].CallMethod("LootAll", new object[0], true);
        }
    }
}
