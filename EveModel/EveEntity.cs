using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveEntity : EveObject
    {
        List<EveObject> damageList;

        public enum EntitySizes { Unknown, Frigate, Cruiser, BattleCruiser, Battleship }
        
        int[] _frigateSize = { 25, 105, 520, 527, 550, 557, 562, 567, 572, 597, 606, 615, 624, 633, 665, 671, 677, 683, 687, 693, 699, 759, 789, 792, 800, 805, 810, 814, 818, 819, 826, 847, 100, 105 };

        public enum EntityMovementState
        {
            ManualFlying = 0,
            Approaching = 1,
            Stopped = 2,
            InWarp = 3,
            Orbitting = 4,
        }

        public EveItem _item;
        public EveObject _ball;

        public EveEntity(EveObject ball, EveItem item, long id)
        {
            _item = item;
            _ball = ball;
            Id = id;
            this.PointerToObject = ball.PointerToObject;
        }

        public bool IsActiveTarget { get; internal set; }
        public bool IsTarget { get; internal set; }
        public bool IsBeingTargeted { get; internal set; }
        public bool IsTargetingMe { get; internal set; }
        public bool IsAbandoned { get; internal set; }
        public bool HaveLootRights { get; internal set; }
        public bool IsWreckEmpty { get; internal set; }
        public bool IsWreckAlreadyViewed { get; internal set; }

        public bool IsWebbingMe { get; internal set; }
        public bool IsWarpScramblingMe { get; internal set; }
        public bool IsTargetPaintingMe { get; internal set; }
        public bool IsEnergyNeutingMe { get; internal set; }
        public bool IsEnergyNOSingMe { get; internal set; }
        public bool IsJammingMe { get; internal set; }
        public bool IsTrackingDisruptingMe { get; internal set; }
        public bool IsSensorDampeningMe { get; internal set; }

        #region Id
        public long Id { get; private set; }
        #endregion

        public EntitySizes EntitySize
        {
            get
            {
                if (_frigateSize.Contains(_item.GroupId))
                    return EntitySizes.Frigate;
                return EntitySizes.Unknown;
            }
        }

        #region Name
        string _name;
        public string Name
        {
            get
            {
                if (_name == null)
                    _name = Frame.Client.Uix.CallMethod("GetSlimItemName", new object[] { this._item }).GetValueAs<string>();
                return _name;
            }
        }
        #endregion

        #region Group
        public Group Group
        {
            get
            {
                return _item.Group;
            }
        }
        #endregion

        #region IsNPC
        public bool IsNpc
        {
            get
            {
                return _item.OwnerId < 90000000 && _item.OwnerId > 10000;
            }
        }
        #endregion

        public bool IsEvePlayer
        {
            get
            {
                return _item.CharId > 0;
            }
        }

        #region HasExploded
        bool? _hasExploded;
        public bool HasExploded
        {
            get
            {
                if (!_hasExploded.HasValue)
                    _hasExploded = _ball["exploded"].GetValueAs<bool>();
                return _hasExploded.Value;
            }
        }
        #endregion

        #region LocationId
        public long LocationId
        {
            get
            {
                return _item.LocationId;
            }
        }
        #endregion

        #region MovementMode
        EntityMovementState? _mode;
        public EntityMovementState MovementMode
        {
            get
            {
                if (!_mode.HasValue)
                    _mode = (EntityMovementState)_ball["mode"].GetValueAs<int>();
                return _mode.Value;
            }
        }
        #endregion

        #region Armor
        double? _armor;
        public double Armor
        {
            get
            {
                if (!_armor.HasValue)
                {
                    if (damageList == null)
                        damageList = Frame.Client.GetService("michelle").CallMethod("GetBallpark", new object[0]).CallMethod("GetDamageState", new object[] { Id }).GetList<EveObject>();
                    if (damageList.Count == 3)
                    {
                        _armor = damageList[1].GetValueAs<double>();
                    }
                }
                return _armor.Value;
            }
        }
        #endregion

        #region Structure
        double? _structure;
        public double Structure
        {
            get
            {
                if (!_structure.HasValue)
                {
                    if (damageList == null)
                        damageList = Frame.Client.GetService("michelle").CallMethod("GetBallpark", new object[0]).CallMethod("GetDamageState", new object[] { Id }).GetList<EveObject>();
                    if (damageList.Count == 3)
                    {
                        _structure = damageList[2].GetValueAs<double>();
                    }
                }
                return _structure.Value;
            }
        }
        #endregion

        #region Shield
        double? _shield;
        public double Shield
        {
            get
            {
                if (!_shield.HasValue)
                {
                    if (damageList == null)
                        damageList = Frame.Client.GetService("michelle").CallMethod("GetBallpark", new object[0]).CallMethod("GetDamageState", new object[] { Id }).GetList<EveObject>();
                    if (damageList.Count == 3)
                    {
                        _shield = damageList[0].GetValueAs<double>();
                    }
                }
                return _shield.Value;
            }
        }
        #endregion

        #region FollowId
        long? _followId;
        public long FollowId
        {
            get
            {
                if (!_followId.HasValue)
                    _followId = _ball["followId"].GetValueAs<long>();
                return _followId.Value;
            }
        }
        #endregion

        #region GivenName
        public string GivenName
        {
            get
            {
                return _item.GivenName ?? string.Empty;
            }
        }
        #endregion

        #region Distance
        double? _distance;
        public double Distance
        {
            get
            {
                if (!_distance.HasValue)
                {
                    _distance = _ball["surfaceDist"] == null ? 0.0 : _ball["surfaceDist"].GetValueAs<double>();
                    if (_distance == 0)
                    {
                        _ball.CallMethod("GetVectorAt", new object[] { Frame.Client.Session.EveTime });
                        _distance = _ball["surfaceDist"] == null ? 0.0 : _ball["surfaceDist"].GetValueAs<double>();
                    }
                }
                return _distance.Value;
            }
        }
        #endregion



        public void LockTarget()
        {
            Frame.Client.MenuService.CallMethod("LockTarget", new object[] { Id }, true);
        }

        public void Orbit()
        {
            Orbit(500);
        }
        public void Orbit(int range)
        {
            Frame.Client.MenuService.CallMethod("Orbit", new object[] { Id, range }, true);
        }

        public void Activate()
        {
            Frame.Client.MenuService.CallMethod("ActivateAccelerationGate", new object[] { Id }, true);
        }

        public void AlignTo()
        {
            Frame.Client.MenuService.CallMethod("AlignTo", new object[] { Id }, true);
        }

        public void Approach()
        {
            this.Approach(50);
        }

        public void Approach(int range)
        {
            Frame.Client.MenuService.CallMethod("Approach", new object[] { this.Id, range }, true);
        }

        public void OpenCargo()
        {
            Frame.Client.MenuService.CallMethod("OpenCargo", new object[] { this.Id }, true);
        }

        public void WarpTo()
        {
            WarpTo(0.0);
        }
        public void WarpTo(double distance)
        {
            Frame.Client.MenuService.CallMethod("WarpToItem", new object[] { this.Id, distance }, true);
        }

        public void JumpStargate()
        {
            var jumpsIndex = new EveObject(PyCall.PyList_GetItem(_item["jumps"].PointerToObject, 0), "jumps", false);
            int toCelestialId = jumpsIndex["toCelestialID"].GetValueAs<int>();
            int locationId = jumpsIndex["locationID"].GetValueAs<int>();
            Frame.Client.MenuService.CallMethod("StargateJump", new object[] { this.Id, toCelestialId, locationId }, true);
        }
        public void Dock()
        {
            Frame.Client.MenuService.CallMethod("DockOrJumpOrActivateGate", new object[] { Id }, true);
        }
        public bool IsInTargetingRange
        {
            get
            {
                return Frame.Client.TargetManager.CallMethod("IsInTargetingRange", new object[] { Id }).GetValueAs<bool>();
            }
        }
        public void SetAsActiveTarget()
        {
            if (this.IsTarget)
                Frame.Client.TargetManager.CallMethod("_SetSelected", new object[] { this.Id });
        }

    }
}
