using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveInvType : EveObject
    {
        public EveInvType(EveObject parent) : base()
        {
            this.PointerToObject = parent.PointerToObject;
        }

        #region TypeId
        public int TypeId
        {
            get;
            set;
        }
        #endregion

        #region GroupsId - Group
        int? _groupId;
        public int GroupId
        {
            get
            {
                if (!_groupId.HasValue)
                    _groupId = Frame.Client.GetInvType(new object[] { TypeId })["groupID"].GetValueAs<int>();
                return _groupId.Value;
            }
        }
        public Group Group
        {
            get
            {
                return (Group)GroupId;
            }
        }
        #endregion

        #region BasePrice
        double? _basePrice;
        public double BasePrice
        {
            get
            {
                if (!_basePrice.HasValue)
                    _basePrice = Frame.Client.GetInvType(new object[] { TypeId })["basePrice"].GetValueAs<double>();
                return _basePrice.Value;
            }
        }
        #endregion

        #region Capacity
        double? _capacity;
        public double Capacity
        {
            get
            {
                if (!_capacity.HasValue)
                    _capacity = Frame.Client.GetInvType(new object[] { TypeId })["capacity"].GetValueAs<double>();
                return _capacity.Value;
            }
        }
        #endregion

        #region CategoryId
        int? _categoryId;
        public int CategoryId
        {
            get
            {
                if (!_categoryId.HasValue)
                    _categoryId = Frame.Client.GetInvType(new object[] { TypeId })["categoryID"].GetValueAs<int>();
                return _categoryId.Value;
            }
        }
        public Category Category
        {
            get
            {
                return (Category)CategoryId;
            }
        }
        #endregion

        #region ChanceOfDuplicating
        double? _chanceOfDuplicating;
        public double ChanceOfDuplicating
        {
            get
            {
                if (!_chanceOfDuplicating.HasValue)
                    _chanceOfDuplicating = Frame.Client.GetInvType(new object[] { TypeId })["chanceOfDuplicating"].GetValueAs<double>();
                return _chanceOfDuplicating.Value;
            }
        }
        #endregion

        #region DataId
        int? _dataId;
        public int DataId
        {
            get
            {
                if (!_dataId.HasValue)
                    _dataId = Frame.Client.GetInvType(new object[] { TypeId })["dataID"].GetValueAs<int>();
                return _dataId.Value;
            }
        }
        #endregion

        #region Description
        string _description;
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_description))
                    _description = Frame.Client.GetInvType(new object[] { TypeId })["description"].GetValueAs<string>();
                return _description;
            }
        }
        #endregion

        #region GraphicId
        int? _graphicId;
        public int GraphicId
        {
            get
            {
                if (!_graphicId.HasValue)
                    _graphicId = Frame.Client.GetInvType(new object[] { TypeId })["graphicID"].GetValueAs<int>();
                return _graphicId.Value;
            }
        }
        #endregion

        #region IconId
        int? _iconId;
        public int IconId
        {
            get
            {
                if (!_iconId.HasValue)
                    _iconId = Frame.Client.GetInvType(new object[] { TypeId })["iconID"].GetValueAs<int>();
                return _iconId.Value;
            }
        }
        #endregion

        #region MarketGroupID
        int? _marketGroupID;
        public int MarketGroupID
        {
            get
            {
                if (!_marketGroupID.HasValue)
                    _marketGroupID = Frame.Client.GetInvType(new object[] { TypeId })["marketGroupID"].GetValueAs<int>();
                return _marketGroupID.Value;
            }
        }
        #endregion

        #region Mass
        double? _mass;
        public double Mass
        {
            get
            {
                if (!_mass.HasValue)
                    _mass = Frame.Client.GetInvType(new object[] { TypeId })["mass"].GetValueAs<double>();
                return _mass.Value;
            }
        }
        #endregion

        #region PortionSize
        int? _portionSize;
        public int PortionSize
        {
            get
            {
                if (!_portionSize.HasValue)
                    _portionSize = Frame.Client.GetInvType(new object[] { TypeId })["portionSize"].GetValueAs<int>();
                return _portionSize.Value;
            }
        }
        #endregion

        #region Published
        bool? _published;
        public bool Published
        {
            get
            {
                if (!_published.HasValue)
                    _published = Frame.Client.GetInvType(new object[] { TypeId })["published"].GetValueAs<bool>();
                return _published.Value;
            }
        }
        #endregion

        #region RaceId
        int? _raceId;
        public int RaceId
        {
            get
            {
                if (!_raceId.HasValue)
                    _raceId = Frame.Client.GetInvType(new object[] { TypeId })["raceID"].GetValueAs<int>();
                return _raceId.Value;
            }
        }
        #endregion

        #region Radius
        double? _radius;
        public double Radius
        {
            get
            {
                if (!_radius.HasValue)
                    _radius = Frame.Client.GetInvType(new object[] { TypeId })["radius"].GetValueAs<double>();
                return _radius.Value;
            }
        }
        #endregion

        #region SoundId
        int? _soundId;
        public int SoundId
        {
            get
            {
                if (!_soundId.HasValue)
                    _soundId = Frame.Client.GetInvType(new object[] { TypeId })["soundID"].GetValueAs<int>();
                return _soundId.Value;
            }
        }
        #endregion

        #region TypeName
        string _typeName;
        public string TypeName
        {
            get
            {
                if (string.IsNullOrEmpty(_typeName))
                    _typeName = Frame.Client.GetInvType(new object[] { TypeId })["typeName"].GetValueAs<string>();
                return _typeName;
            }
        }
        #endregion

        #region Volume
        double? _volume;
        public double Volume
        {
            get
            {
                if (!_volume.HasValue)
                    _volume = Frame.Client.GetInvType(new object[] { TypeId })["volume"].GetValueAs<double>();
                return _volume.Value;
            }
        }
        #endregion

    }
}
