using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveActiveShip : EveItem
    {
        public EveActiveShip(EveObject parent) : base(parent) { }

        List<EveEntity> _activeDrones;
        public List<EveEntity> ActiveDrones
        {
            get
            {
                if (_activeDrones == null)
                    _activeDrones = Frame.Client.Entities.Where(ent =>
                        Frame.Client.Michelle.CallMethod("GetDrones", new object[0])["items"].GetDictionary<long>().Keys.ToList().Contains(ent.Id)).ToList();
                return _activeDrones;
            }
        }

        public void Fit(string fittingName)
        {
            if (Frame.Client.GetFittingWindow == null)
                return;
            var fs = Frame.Client.GetService("fittingSvc")["fittings"].GetDictionary<int>().Where(f => f.Key == Frame.Client.Session.CharId).FirstOrDefault().Value;
            if (fs != null)
            {
                foreach (var item in fs.GetDictionary<int>())
                {
                    if (fittingName.ToLower() == item.Value["name"].GetValueAs<string>().ToLower())
                    {
                        Frame.Client.GetService("fittingSvc").CallMethod("LoadFitting", new object[] { Frame.Client.Session.CharId, item.Key }, true);
                        break;
                    }
                }
            }

        }

        public int DronesInBay
        {
            get
            {
                var shipInv = Frame.Client.InvCache.CallMethod("GetInventoryFromId", new object[] { Frame.Client.Session.ShipId });
                return shipInv.CallMethod("ListDroneBay", new object[0]).GetList<EveObject>().Count;
            }
        }

        public void ReleaseDrones()
        {
            var shipInv = Frame.Client.InvCache.CallMethod("GetInventoryFromId", new object[] { Frame.Client.Session.ShipId });
            var drones = shipInv.CallMethod("ListDroneBay", new object[0]);
            Frame.Client.MenuService.CallMethod("LaunchDrones", new object[] { drones }, true);
        }

        EveEntity _entity;
        public EveEntity ToEntity
        {
            get
            {
                if (_entity == null)
                    _entity = Frame.Client.Entities.Where(en => en.Id == this.ItemId).FirstOrDefault();
                return _entity;
            }
        }
        List<EveModule> _modules;
        public List<EveModule> Modules
        {
            get
            {
                if (_modules == null)
                {
                    _modules = new List<EveModule>();
                    foreach (var item in Frame.Client.Builtin["uicore"]["layer"]["shipui"]["sr"]["modules"].GetDictionary<long>())
                    {
                        _modules.Add(new EveModule(item.Value, item.Key));
                    }
                }
                return _modules;
            }
        }
        public void ManipulateModuleGroup(Group group, bool activate)
        {
            foreach (var module in Modules.Where(mo => mo.Group == group))
            {
                if (activate && module.CapacitorNeed < Capacitor)
                {
                    module.Activate();
                }
                else
                {
                    module.DeActivate();
                }
            }
        }
        public double WeaponsFiringRange
        {
            get { return Weapons.Max(w => w.OptimalRange + w.FallOff); }
        }
        public double WeaponsOptimalRange
        {
            get { return Weapons.Average(w => w.OptimalRange); }
        }

        public double MiningOptimalRange
        {
            get
            {
                return Miners.Min(en => en.OptimalRange);
            }
        }

        public List<EveModule> Miners
        {
            get
            {
                return Modules.Where(m => m.Group == Group.MiningLaser || m.Group == Group.StripMiner).ToList<EveModule>();
            }
        }

        int[] _weaponGroups = { 53, 55, 56, 72, 74, 506, 507, 508, 509, 510, 511, 512 };
        public List<EveModule> Weapons
        {
            get
            {
                return Modules.Where(m => _weaponGroups.Contains(m.GroupId)).ToList<EveModule>();
            }
        }
        double? charge;
        public double Capacitor
        {
            get
            {
                if (!charge.HasValue)
                {
                    EveObject attrib;
                    charge = this.Attributes.TryGetValue("charge", out attrib) ? attrib.GetValueAs<double>() : 0;
                }
                return charge.Value;
            }
        }
        public bool HasTravelCloak { get { return Cloak != null && Cloak.TypeName == "Covert Ops Cloaking Device II"; } }
        public bool HasCloak { get { return Cloak != null; } }
        public bool IsCloaked { get { return Cloak != null && Cloak.IsActive; } }
        EveModule _cloak;
        public EveModule Cloak
        {
            get
            {
                if (_cloak == null)
                    _cloak = this.Modules.Where(m => m.Group == EveModel.Group.CloakingDevice).FirstOrDefault();
                return _cloak;
            }
        }

        public bool IsMissileBoat
        {
            get
            {
                bool allLaunchers = false;
                foreach (var weapon in Weapons)
                {
                    allLaunchers = weapon.TypeName.Contains("Missile Launcher") |
                                        weapon.TypeName.Contains("Missile Bay") |
                                        weapon.TypeName.Contains("Rocket Launcher") |
                                        weapon.TypeName.Contains("Cruise Launcher");
                }
                return allLaunchers;
            }
        }
        public bool IsOutOfAmmo
        {
            get
            {
                if (!Frame.Client.IsUnifiedInventoryOpen)
                {
                    Frame.Log("Can't check ammo when inventory is closed");
                    Frame.Client.ExecuteCommand(EveCommand.OpenInventory);
                    return false;
                }
                double chargesUsed = 0, chargesInCargo = 0, chargeId = -1;
                foreach (var weapon in Weapons)
                {
                    if (weapon.IsReloadingAmmo || weapon.Charge.Stacksize == 0 || weapon.Charge == null)
                        return false;
                    chargesUsed += weapon.Capacity / weapon.Charge.Volume;
                    chargeId = weapon.Charge.TypeId;
                }
                foreach (var item in Frame.Client.GetCargoOfActiveShip().Items)
                {
                    if (item.TypeId == chargeId)
                    {
                        chargesInCargo += item.Quantity;
                    }
                }
                return chargesInCargo < Weapons.Count;
            }
        }
        public double MaxLockedTargets
        {
            get
            {
                return Attributes["maxLockedTargets"].GetValueAs<double>();
            }
        }
    }
}
