using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveModule : EveItem
    {
        public EveModule(EveObject parent, long itemId) : base(parent)
        {
            this.PointerToObject = parent.PointerToObject;
            this.TypeId = this["sr"]["moduleInfo"]["typeID"].GetValueAs<int>();
            ItemId = itemId;
        }

        void FindMaxRange()
        {
            var info = this["sr"]["moduleInfo"];
            var vals = Frame.Client.Tactical.CallMethod("FindMaxRange", new object[] { info, Charge }).GetListFromTuple<double>();

            _optimalRange = vals[0];
            _fallOff = vals[1];
            _bombRadius = vals[2];
        }

        #region OptimalRange
        double? _optimalRange;
        public double OptimalRange
        {
            get
            {
                if (!_optimalRange.HasValue)
                {
                    FindMaxRange();
                }
                return _optimalRange.Value;
            }
        }
        #endregion

        #region FallOff
        double? _fallOff;
        public double FallOff
        {
            get
            {
                if (!_fallOff.HasValue)
                {
                    FindMaxRange();
                }
                return _fallOff.Value;
            }
        }
        #endregion

        #region BombRadius
        double? _bombRadius;
        public double BombRadius
        {
            get
            {
                if (!_bombRadius.HasValue)
                {
                    FindMaxRange();
                }
                return _bombRadius.Value;
            }
        }
        #endregion


        #region CapacitorNeed
        double? _capacitorNeed;
        /// <summary>
        /// Capacitor amount to activate/reactivate
        /// </summary>
        public double CapacitorNeed
        {
            get
            {
                if (!_capacitorNeed.HasValue)
                {
                    EveObject attrib;
                    _capacitorNeed = this.Attributes.TryGetValue("capacitorNeed", out attrib) ? attrib.GetValueAs<double>() : 0;
                }
                return _capacitorNeed.Value;
            }
        }
        #endregion

        #region Duration
        double? _duration;
        /// <summary>
        /// Activation duration often called cycle time
        /// </summary>
        public double Duration
        {
            get
            {
                if (!_duration.HasValue)
                {
                    EveObject attrib;
                    _duration = this.Attributes.TryGetValue("duration", out attrib) ? attrib.GetValueAs<double>() / 1000.0 : 0;
                }
                return _duration.Value;
            }
        }
        #endregion

        #region MiningAmount
        double? _miningAmount;
        /// <summary>
        /// Amount of ore mined per cycle
        /// </summary>
        public double MiningAmount
        {
            get
            {
                if (!_miningAmount.HasValue)
                {
                    EveObject attrib;
                    _miningAmount = this.Attributes.TryGetValue("miningAmount", out attrib) ? attrib.GetValueAs<double>() / 1000.0 : 0;
                }
                return _miningAmount.Value;
            }
        }
        #endregion

        #region IsOnline
        bool? _isOnline;
        /// <summary>
        /// Is the module online
        /// </summary>
        public bool IsOnline
        {
            get
            {
                if (!_isOnline.HasValue)
                    _isOnline = this["online"].GetValueAs<bool>();
                return _isOnline.Value;
            }
        }
        #endregion

        #region IsGoingOnline
        bool? _isGoingOnline;
        /// <summary>
        /// Is the module onlining
        /// </summary>
        public bool IsGoingOnline
        {
            get
            {
                if (!_isGoingOnline.HasValue)
                    _isGoingOnline = this["goingOnline"].GetValueAs<bool>();
                return _isGoingOnline.Value;
            }
        }
        #endregion

        #region ReloadingAmmo
        bool? _isReloadingAmmo;
        /// <summary>
        /// Is the module reloading the exisiting type of ammo. If loading a new type of ammo use IsChangingAmmo
        /// </summary>
        public bool IsReloadingAmmo
        {
            get
            {
                if (!_isReloadingAmmo.HasValue)
                    _isReloadingAmmo = this["reloadingAmmo"].GetValueAs<bool>();
                return _isReloadingAmmo.Value;
            }
        }
        #endregion

        public void ChangeAmmo(EveItem charge)
        {
            //if (charge._itemId <= 0L)
            //    return;
            //this.CallMethod("ChangeAmmoType", new object[] { charge.TypeId, charge.Stacksize }, true);
        }

        #region IsChangingAmmo
        bool? _isChangingAmmo;
        /// <summary>
        /// Is the module changing ammo
        /// </summary>
        public bool IsChangingAmmo
        {
            get
            {
                if (!_isChangingAmmo.HasValue)
                    _isChangingAmmo = this["changingAmmo"].GetValueAs<bool>();
                return _isChangingAmmo.Value;
            }
        }
        #endregion

        #region IsActive
        bool? _isActive;
        /// <summary>
        /// Is the module active
        /// </summary>
        public bool IsActive
        {
            get
            {
                if (!_isActive.HasValue)
                    _isActive = this["def_effect"]["isActive"].GetValueAs<bool>();
                return _isActive.Value;
            }
        }
        #endregion

        #region IsDeactivating
        bool? _isDeactivating;
        /// <summary>
        /// Is the module deactivating
        /// </summary>
        public bool IsDeactivating
        {
            get
            {
                if (!_isDeactivating.HasValue)
                    _isDeactivating = this["def_effect"]["isDeactivating"].GetValueAs<bool>();
                return _isDeactivating.Value;
            }
        }
        #endregion

        public void Activate()
        {
            if (!this.IsActive && !this.IsDeactivating && !this.IsChangingAmmo && !this.IsReloadingAmmo && !this.IsGoingOnline && this.IsOnline)
                this.CallMethod("ActivateEffect", new object[] { this["def_effect"] }, true);
        }
        public void Activate(long targetId)
        {
            if (!this.IsActive && !this.IsDeactivating && !this.IsChangingAmmo && !this.IsReloadingAmmo && !this.IsGoingOnline && this.IsOnline)
                this.CallMethod("ActivateEffect", new object[] { this["def_effect"], targetId }, true);
        }
        public void DeActivate()
        {
            if (this.IsActive && !this.IsDeactivating && !this.IsChangingAmmo && !this.IsReloadingAmmo && !this.IsGoingOnline && this.IsOnline)
                this.CallMethod("DeactivateEffect", new object[] { this["def_effect"] }, true);
        }

    }
}
