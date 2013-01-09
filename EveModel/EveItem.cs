using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveItem : EveInvType
    {
        public EveItem(EveObject obj)
            : base(obj)
        {
            this.TypeId = this["typeID"].GetValueAs<int>();
        }

        #region CharId
        int? _charId;
        public int CharId
        {
            get
            {
                if (!_charId.HasValue)
                    _charId = this["charID"].GetValueAs<int>();
                return _charId.Value;
            }
        }
        #endregion

        #region FlagId
        int? _flagId;
        public int FlagId
        {
            get
            {
                if (!_flagId.HasValue)
                    _flagId = this["flagID"].GetValueAs<int>();
                return _flagId.Value;
            }
        }
        #endregion

        #region GivenName
        string _givenName;
        public string GivenName
        {
            get
            {
                if (string.IsNullOrEmpty(_givenName))
                    _givenName = Frame.Client.GetLocation(new object[] { ItemId })["name"].GetValueAs<string>();
                return _givenName ?? string.Empty;
            }
        }
        #endregion

        #region IsSingleton
        bool? _isSingleton;
        public bool IsSingleton
        {
            get
            {
                if (!_isSingleton.HasValue)
                    _isSingleton = this["singleton"].GetValueAs<bool>();
                return _isSingleton.Value;
            }
        }
        #endregion

        #region ItemId
        internal long? _itemId;
        public long ItemId
        {
            get
            {
                if (!_itemId.HasValue || _itemId == -1)
                    _itemId = this["itemID"].GetValueAs<long>();
                return _itemId.Value;
            }
            internal set { _itemId = value; }
        }
        #endregion

        #region LocationId
        long? _locationId;
        public long LocationId
        {
            get
            {
                if (!_locationId.HasValue)
                    _locationId = this["locationID"].GetValueAs<long>();
                return _locationId.Value;
            }
        }
        #endregion

        #region OwnerId
        long? _ownerId;
        public long OwnerId
        {
            get
            {
                if (!_ownerId.HasValue)
                    _ownerId = this["ownerID"].GetValueAs<long>();
                return _ownerId.Value;
            }
        }
        #endregion

        #region Quantity
        int? _quantity;
        public int Quantity
        {
            get
            {
                if (!_quantity.HasValue)
                    _quantity = this["quantity"].GetValueAs<int>();
                return _quantity.Value;
            }
        }
        #endregion

        #region Stacksize
        int? _stacksize;
        public int Stacksize
        {
            get
            {
                if (!_stacksize.HasValue)
                    _stacksize = this["stacksize"].GetValueAs<int>();
                return _stacksize.Value;
            }
        }
        #endregion

        #region MetaLevel
        int? _metaLevel;
        public int MetaLevel
        {
            get
            {
                if (!_metaLevel.HasValue)
                {
                    EveObject val;
                    _metaLevel = Attributes.TryGetValue("metaLevel", out val) ? val.GetValueAs<int>() : -1;
                }
                return _metaLevel.Value;
            }
        }
        #endregion

        #region RateOfFire
        double? _rateOfFire;
        /// <summary>
        /// ROF in seconds
        /// </summary>
        public double RateOfFire
        {
            get
            {
                if (!_rateOfFire.HasValue)
                {
                    EveObject val;
                    _rateOfFire = Attributes.TryGetValue("speed", out val) ? val.GetValueAs<double>() / 1000D : -1;
                }
                return _rateOfFire.Value;
            }
        }
        #endregion

        #region ChargeGroups
        List<int> _chargeGroups;
        public List<int> ChargeGroups
        {
            get
            {
                if (_chargeGroups == null)
                {
                    _chargeGroups = new List<int>();
                    if (this.Attributes["chargeGroup1"].IsValid)
                    {
                        _chargeGroups.Add(this.Attributes["chargeGroup1"].GetValueAs<int>());
                    }
                    if (this.Attributes["chargeGroup2"].IsValid)
                    {
                        _chargeGroups.Add(this.Attributes["chargeGroup2"].GetValueAs<int>());
                    }
                    if (this.Attributes["chargeGroup3"].IsValid)
                    {
                        _chargeGroups.Add(this.Attributes["chargeGroup3"].GetValueAs<int>());
                    }
                    if (this.Attributes["chargeGroup4"].IsValid)
                    {
                        _chargeGroups.Add(this.Attributes["chargeGroup4"].GetValueAs<int>());
                    }
                }
                return _chargeGroups;
            }
        }
        #endregion


        EveItem _charge;
        public EveItem Charge
        {
            get
            {
                if (_charge == null)
                {
                    var charge = this["charge"];
                    if (charge.IsValid)
                        _charge = new EveItem(charge);
                }
                return _charge;
            }
        }

        Dictionary<string, EveObject> _attributes;
        public Dictionary<string, EveObject> Attributes
        {
            get
            {
                if (_attributes == null)
                {
                    if (ItemId <= 0)
                        ItemId = LocationId;
                    if (ItemId > 0)
                        _attributes = Frame.Client.GetService("godma")["stateManager"].CallMethod("GetAttributes", new object[] { ItemId }).GetDictionary<string>();
                    else
                        _attributes = new Dictionary<string, EveObject>();
                }
                return _attributes;
            }
        }
        public void ActivateShip()
        {
            if (Frame.Client.Session.InStation && this.Category == Category.Ship)
                Frame.Client.StationService.CallMethod("TryActivateShip", new object[] { this }, true);

        }
    }
}
