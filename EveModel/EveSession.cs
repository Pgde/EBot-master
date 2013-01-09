using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveSession : EveObject
    {
        public EveSession()
            : base()
        {
            this.PointerToObject = Frame.Client.Builtin["eve"]["session"].PointerToObject;
        }

        #region InStation
        /// <summary>
        /// Returns whether pilot is docked in station
        /// </summary>
        public bool InStation
        {
            get
            {
                return Frame.Client.MenuService.CallMethod("GetCheckInStation", new object[0]).GetValueAs<bool>();
            }
        }
        #endregion

        #region InSpace
        /// <summary>
        /// Checks to see if the pilot is in space
        /// </summary>
        public bool InSpace
        {
            get
            {
                return Frame.Client.MenuService.CallMethod("GetCheckInSpace", new object[0]).GetValueAs<bool>();
            }
        }
        #endregion

        #region LocationId
        private long? _locationId;
        /// <summary>
        /// returns -1 if no locationid was found
        /// </summary>
        public long LocationId
        {
            get
            {
                if (!_locationId.HasValue)
                {
                    _locationId = this["locationid"].GetValueAs<long>();
                }
                return _locationId.Value;
            }
        }
        #endregion

        #region NextSessionChange
        private DateTime? _nextSessionChange;
        /// <summary>
        /// returns when the next sessionchange ends, 
        /// can be in the past if it already ended
        /// </summary>
        public DateTime NextSessionChange
        {
            get
            {
                if (!_nextSessionChange.HasValue)
                {
                    _nextSessionChange = this["nextSessionChange"].GetValueAs<DateTime>();
                }
                return _nextSessionChange.Value;
            }
        }
        #endregion

        #region IsItSafe
        private bool? _isItSafe;
        /// <summary>
        /// returns whether the session is safe to use
        /// </summary>
        public bool IsItSafe
        {
            get
            {
                if (!_isItSafe.HasValue)
                {
                    _isItSafe = this.CallMethod("IsItSafe", new object[0]).GetValueAs<bool>();
                }
                return _isItSafe.Value;
            }
        }
        #endregion

        #region CharId
        private int? _charId;
        /// <summary>
        /// returns -1 if no charid was found
        /// </summary>
        public int CharId
        {
            get
            {
                if (!_charId.HasValue)
                {
                    _charId = this["charid"].GetValueAs<int>();
                }
                return _charId.Value;
            }
        }
        #endregion

        #region ShipId
        private long? _shipID;
        /// <summary>
        /// returns -1 if no charid was found
        /// </summary>
        public long ShipId
        {
            get
            {
                if (!_shipID.HasValue)
                {
                    _shipID = this["shipid"].GetValueAs<long>();
                }
                return _shipID.Value;
            }
        }
        #endregion

        #region EveTime
        DateTime? _eveTime;
        public DateTime EveTime
        {
            get
            {
                if (!_eveTime.HasValue)
                    _eveTime = Frame.Client.Blue["os"].CallMethod("GetSimTime", new object[0]).GetValueAs<DateTime>();
                return _eveTime.Value;
            }
        }
        #endregion

        int? _solarSystemId;
        public int SolarSystemId
        {
            get {
                if (!_solarSystemId.HasValue)
                {
                    _solarSystemId = this["solarsystemid"].GetValueAs<int>();
                    if (_solarSystemId == -1)
                        _solarSystemId= this["solarsystemid2"].GetValueAs<int>();
                }
                return _solarSystemId.Value;
            }
        }

        double? _systemSecurity;
        public double SystemSecurity
        {
            get
            {
                if (!_systemSecurity.HasValue)
                    _systemSecurity = Frame.Client.MapService.CallMethod("GetSecurityStatus", new object[] { Frame.Client.Session.SolarSystemId }).GetValueAs<double>();
                return _systemSecurity.Value;
            }
        }
    }
}
