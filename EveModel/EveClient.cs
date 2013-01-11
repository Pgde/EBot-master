using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveClient : IDisposable
    {
        public EveClient()
        {
        }

        public bool invopencheck()
        {
             if (Frame.Client.IsUnifiedInventoryOpen == false)                                                          // checken ob das Inventory geöffnet ist
            {          return false;}                                                                                                // Wenn nicht return false
             return true;
        }


        


        #region Eve Client Object and Service References

        /// <summary>
        /// Cache for already referenced EVE Client objects, will be cleared beween every frame
        /// </summary>
        public Dictionary<string, EveObject> Objects = new Dictionary<string, EveObject>();

        /// <summary>
        /// Gets the __builtin__ object reference in the EVE CLient process
        /// </summary>
        internal EveObject Builtin
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("__builtin__", out obj))
                {
                    obj = new EveObject(PyCall.PyImport_ImportModule("__builtin__"), "__builtin__");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets the utthread object reference used in creating asynchronous method calls in the EVE Client process
        /// </summary>
        internal EveObject UThread
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("uthread", out obj))
                {
                    obj = new EveObject(PyCall.PyImport_ImportModule("uthread"), "uthread");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets the services object reference
        /// </summary>
        internal EveObject Services
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("services", out obj))
                {
                    obj = Builtin["sm"]["services"];
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets the invtypes object reference
        /// </summary>
        internal EveObject InvTypes
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("instanceBuiltin", out obj))
                {
                    obj = new EveObject(PyCall.PyImport_ImportModule("__builtin__"), "instanceBuiltin");
                }
                return obj["cfg"]["invtypes"];
            }
        }
        /// <summary>
        /// Gets the Locations object reference
        /// </summary>
        internal EveObject Locations
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("evelocations", out obj))
                {
                    obj = Builtin["cfg"]["evelocations"];
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets the LocalSvc object reference
        /// </summary>
        internal EveObject LocalSvc
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("LocalSvc", out obj))
                {
                    obj = Builtin["eve"]["session"].CallMethod("ConnectToService", new object[] { "LocalSvc" }, true);
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets the scanSvc object reference
        /// </summary>
        public EveObject ScanService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("scanSvc", out obj))
                {
                    obj = GetService("scanSvc");
                }
                return obj;
            }
        }


        /// <summary>
        /// Gets the Skiilsrv object reference
        /// </summary>
        public EveObject SkillService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("skills", out obj))
                {
                    obj = GetService("skills");
                }
                return obj;
            }
        }

        /// <summary>
        /// Gets the queuesrv object reference
        /// </summary>
        public EveObject skillq
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("skillqueue", out obj))
                {
                    obj = GetService("skillqueue");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets the uix object reference in the EVE CLient process
        /// </summary>
        internal EveObject Uix
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("uix", out obj))
                {
                    obj = new EveObject(PyCall.PyImport_ImportModule("uix"), "uix");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets the blue object reference in the EVE CLient process
        /// </summary>
        internal EveObject Blue
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("blue", out obj))
                {
                    obj = new EveObject(PyCall.PyImport_ImportModule("blue"), "blue");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for a running service from the EVE Client process
        /// </summary>
        /// <param name="serviceName">Name of the service you are looking for, check IsValid property for valid references</param>
        /// <returns></returns>
        public EveObject GetService(string serviceName)
        {
            EveObject serviceObject;
            if (!Services.GetDictionary<string>().TryGetValue(serviceName, out serviceObject))
                serviceObject = LocalSvc[serviceName];
            return serviceObject;
        }
        EveSession _eveSession;
        /// <summary>
        /// Returns an EVE session object
        /// </summary>
        public EveSession Session
        {
            get
            {
                if (_eveSession == null)
                    _eveSession = new EveSession();
                return _eveSession;
            }
        }
        /// <summary>
        /// Gets a reference for the invCache service form the EVE client process
        /// </summary>
        public EveObject InvCache
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("invCache", out obj))
                {
                    obj = GetService("invCache");
                }
                return obj;
            }
        }

        /// <summary>
        /// Gets a reference for the menu service form the EVE client process
        /// </summary>
        public EveObject MenuService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("menu", out obj))
                {
                    obj = GetService("menu");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the michelle service form the EVE client process
        /// </summary>
        public EveObject Michelle
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("michelle", out obj))
                {
                    obj = GetService("michelle");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the map service form the EVE client process
        /// </summary>
        public EveObject MapService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("map", out obj))
                {
                    obj = GetService("map");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the tactical service form the EVE client process
        /// </summary>
        public EveObject Tactical
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("tactical", out obj))
                {
                    obj = GetService("tactical");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the target service form the EVE client process
        /// </summary>
        public EveObject TargetManager
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("target", out obj))
                {
                    obj = GetService("target");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the LSC service form the EVE client process
        /// </summary>
        public EveObject LSCService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("LSC", out obj))
                {
                    obj = GetService("LSC");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the godma service form the EVE client process
        /// </summary>
        public EveObject GodmaService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("godma", out obj))
                {
                    obj = GetService("godma");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the station service form the EVE client process
        /// </summary>
        public EveObject StationService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("station", out obj))
                {
                    obj = GetService("station");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the state service form the EVE client process
        /// </summary>
        public EveObject StateService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("state", out obj))
                {
                    obj = GetService("state");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the addressbook service form the EVE client process
        /// </summary>
        public EveObject AddressBook
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("addressbook", out obj))
                {
                    obj = GetService("addressbook");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the station service form the EVE client process
        /// </summary>
        public EveObject BookmarkService
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("bookmarkSvc", out obj))
                {
                    obj = GetService("bookmarkSvc");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the eveowners object in the EVE client process
        /// </summary>
        internal EveObject EveOwners
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("eveowners", out obj))
                {
                    obj = Builtin["cfg"]["eveowners"];
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the localization object in the EVE client process
        /// </summary>
        public EveObject Localization
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("localization", out obj))
                {
                    obj = new EveObject(PyCall.PyImport_ImportModule("localization"), "localization");
                }
                return obj;
            }
        }
        /// <summary>
        /// Gets a reference for the const object in the EVE client process
        /// </summary>
        public EveObject Const
        {
            get
            {
                EveObject obj;
                if (!Objects.TryGetValue("const", out obj))
                {
                    obj = Builtin["const"];
                }
                return obj;
            }
        }

        #endregion

        #region Entities

        /// <summary>
        /// Cache for already referenced entities, will be cleared between every frame
        /// </summary>
        Dictionary<long, EveEntity> _entityDictionary;

        /// <summary>
        /// populates a dictionary with current entities
        /// </summary>
        void PopulateEntityDictionary()
        {
            var activeTargets = TargetManager["targetsByID"].GetDictionary<long>().Keys;
            var beingTargeted = TargetManager["targeting"].GetDictionary<long>(); // Dictionary is <long, datetime> where datetime states when targeting started
            var targetedBy = TargetManager["targetedBy"].GetList<long>();
            var jammers = Frame.Client.Tactical["jammers"].GetDictionary<long>();

            _entityDictionary = new Dictionary<long, EveEntity>();

            var ballpark = GetService("michelle").CallMethod("GetBallpark", new object[] { });
            var balls = ballpark["balls"];
            if (!balls.IsValid)
            {
                return;
            }
            var ballKeyList = balls.CallMethod("keys", new object[0]).GetList<long>();

            foreach (var num in ballKeyList)
            {
                if (num > 0L)
                {
                    EveObject parent = ballpark.CallMethod("GetInvItem", new object[] { num });
                    if (!parent.IsValid)
                        break;
                    EveItem item = new EveItem(parent);
                    EveObject ball = ballpark.CallMethod("GetBall", new object[] { num });
                    EveEntity ent = new EveEntity(ball, item, num);
                    _entityDictionary.Add(num, ent);

                    ent.IsTarget = activeTargets.Contains(num);
                    ent.IsBeingTargeted = beingTargeted.Keys.Contains<long>(num);
                    ent.IsTargetingMe = targetedBy.Contains(num);
                    ent.IsActiveTarget = num == GetActiveTargetId;
                    ent.IsAbandoned = ballpark.CallMethod("IsAbandoned", new object[] { num }).GetValueAs<bool>();
                    ent.HaveLootRights = ballpark.CallMethod("HaveLootRight", new object[] { num }).GetValueAs<bool>();
                    ent.IsWreckEmpty = StateService.CallMethod("CheckWreckEmpty", new object[] { item }).GetValueAs<bool>();
                    ent.IsWreckAlreadyViewed = StateService.CallMethod("CheckWreckViewed", new object[] { item }).GetValueAs<bool>();

                    if (jammers.ContainsKey(num))
                    {
                        foreach (var effect in jammers[num].GetDictionary<string>())
                        {
                            switch (effect.Key)
                            {
                                case "webify":
                                    ent.IsWebbingMe = true;
                                    break;
                                case "ewTargetPaint":
                                    ent.IsTargetPaintingMe = true;
                                    break;
                                case "warpScrambler":
                                    ent.IsWarpScramblingMe = true;
                                    break;
                                case "ewEnergyNeut":
                                    ent.IsEnergyNeutingMe = true;
                                    break;
                                case "ewEnergyVampire":
                                    ent.IsEnergyNOSingMe = true;
                                    break;
                                case "electronic":
                                    ent.IsJammingMe = true;
                                    break;
                                case "ewRemoteSensorDamp":
                                    ent.IsSensorDampeningMe = true;
                                    break;
                                case "ewTrackingDisrupt":
                                    ent.IsTrackingDisruptingMe = true;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// List of entities present in the current frame, returns null if pilot is not in space
        /// </summary>
        public List<EveEntity> Entities
        {
            get
            {
                if (!Session.InSpace)
                    return null;
                if (_entityDictionary == null || _entityDictionary.Count == 0)
                {
                    PopulateEntityDictionary();
                }
                return _entityDictionary.Values.ToList<EveEntity>();
            }
        }
        #endregion

        /// <summary>
        /// Executes an EVE command, similiar to the keyboard shortcuts available int the client
        /// </summary>
        public void ExecuteCommand(EveCommand command)
        {
            GetService("cmd").CallMethod(command.ToString(), new object[0], true);
        }

        #region Navigation
        /// <summary>
        /// Gets the locationid of the last waypoint
        /// </summary>
        public long GetLastWaypointLocationId()
        {
            return GetService("starmap").CallMethod("GetDestinationPath", new object[0]).GetList<long>().Last();
        }
        /// <summary>
        /// Gets the locationid of the next waypoint
        /// </summary>
        public long GetNextWaypointLocationId()
        {
            return GetService("starmap").CallMethod("GetDestinationPath", new object[0]).GetList<long>().First();
        }
        /// <summary>
        /// Sets destionation to destinationId
        /// </summary>
        /// <param name="destinationId">Destination, can be solarsystemId or stationId</param>
        public void SetDestination(long destinationId)
        {
            GetService("starmap").CallMethod("SetWaypoint", new object[] { destinationId, true, true }, true);
        }
        /// <summary>
        /// Returns true if any waypoints has been set
        /// </summary>
        public bool IsWaypointsSet
        {
            get { return GetService("starmap").CallMethod("GetDestinationPath", new object[0]).IsValid; }
        }
        #endregion

        #region Inventories
        public bool IsUnifiedInventoryOpen
        {
            get { return GetPrimaryInventoryWindow != null; }
        }

        public List<EveInventoryWindow> InventoryWindows
        {
            get { return GetWindows.Where(w => w.Type == EveWindow.EveWindowType.Inventory).ToList<EveWindow>().ConvertAll<EveInventoryWindow>(new Converter<EveWindow, EveInventoryWindow>(EveWindow2EveInventoryWindow)); }
        }

        public EveInventoryWindow GetPrimaryInventoryWindow
        {
            get
            {
                return InventoryWindows.Where(w => w.IsPrimaryInvWindow).FirstOrDefault();
            }
        }
        public EveInventoryContainer GetCargoOfActiveShip()
        {
            return IsUnifiedInventoryOpen ? GetPrimaryInventoryWindow.CargoHoldOfActiveShip : null;
        }
        public EveInventoryContainer OreHoldOfActiveShip()
        {
            return IsUnifiedInventoryOpen ? GetPrimaryInventoryWindow.OreHoldOfActiveShip : null;
        }
        public EveInventoryContainer GetItemHangar()
        {
            return IsUnifiedInventoryOpen ? GetPrimaryInventoryWindow.ItemHangar : null;
        }
        public EveInventoryContainer GetShipHangar()
        {
            return IsUnifiedInventoryOpen ? GetPrimaryInventoryWindow.ShipHangar : null;
        }

        #endregion

        #region Scanner
        public EveWindow GetScannerWindow
        {
            get { return Frame.Client.GetWindows.Where(w => w.Type == EveWindow.EveWindowType.Scanner).FirstOrDefault(); }
        }

        public List<EveScanResult> GetScanResults
        {
            get
            {
                if (GetScannerWindow == null)
                    return null;
                return ScanService.CallMethod("GetScanResults", new object[0]).GetList<EveObject>().ConvertAll<EveScanResult>(new Converter<EveObject, EveScanResult>(EveObject2EveScanResult));
            }
        }

        public void Scan()
        {
            if (GetScannerWindow == null)
                return;
            if (Frame.Client.GetScannerWindow["sr"]["analyzeBtn"]["opacity"].GetValueAs<double>() == 0.25)
                return;
            Frame.Client.GetScannerWindow.CallMethod("Analyze", new object[0], true);
        }
        #endregion

        public void BookmarkCurrentLocation(string name, string comment)
        {
            if (Frame.Client.Session.InSpace)
                Frame.Client.BookmarkService.CallMethod("BookmarkLocation", new object[] { Frame.Client.GetActiveShip.ToEntity.Id, Frame.Client.Session.CharId, name, comment, Frame.Client.Session.SolarSystemId }, true, new Dictionary<string, object>());
        }

        public EveObject GetLocation(object[] parameters)
        {
            return new EveObject(Locations.CallMethod("GetIfExists", parameters).PointerToObject, "getlocation");
        }
        /// <summary>
        /// Gets the string represetation of an id number, eg. station names
        /// </summary>
        public string GetLocationName(long id)
        {
            return new EveObject(Locations.CallMethod("GetIfExists", new object[] { id }).PointerToObject, null)["name"].GetValueAs<string>();
        }
        /// <summary>
        /// Can only be called while in space
        /// </summary>
        /// <returns></returns>
        public EveEntity GetNextWaypointStargate()
        {
            if (Session.InSpace)
            {
                var map = GetService("starmap").CallMethod("GetDestinationPath", new object[0]).GetList<long>();
                var dest = GetLocationName(map.First());
                return Entities.Where(e => e.Name == dest).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Returns an EVE Agent object
        /// </summary>
        /// <param name="agentName"></param>
        /// <returns></returns>
        public EveAgent GetAgentByName(string agentName)
        {
            if (EveAgents == null)
            {
                return null;
            }
            var agentNameId = EveAgents.Where(a => a.Key.ToLower() == agentName.ToLower()).FirstOrDefault();
            if (agentNameId.Value == 0)
            {
                return null;
            }
            var eveagent = GetService("agents").CallMethod("GetAgentByID", new object[] { agentNameId.Value });
            return new EveAgent(eveagent.PointerToObject, agentNameId.Key);
        }
        /// <summary>
        /// Rteuns the id of your active (selected) target or -1 if you have no active target
        /// </summary>
        public long GetActiveTargetId
        {
            get { return TargetManager.CallMethod("GetActiveTargetID", new object[0]).GetValueAs<long>(); }
        }
        public List<EveAgentMission> AgentMissions
        {
            get
            {
                return GetService("journal")["agentjournal"].GetListFromTuple<EveObject>()[0].GetList<EveObject>().ConvertAll(new Converter<EveObject, EveAgentMission>(EveObject2EveAgentMission));
            }
        }
        /// <summary>
        /// Gets an invtype if it exists
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal EveObject GetInvType(object[] parameters)
        {
            return new EveObject(InvTypes.CallMethod("GetIfExists", parameters).PointerToObject, "getinvtype");
        }
        /// <summary>
        /// Gets the owner of an object, often represented by an id, eg. agentID
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal EveObject GetOwner(object[] parameters)
        {
            return new EveObject(EveOwners.CallMethod("GetIfExists", parameters).PointerToObject, "getowners");
        }
        /// <summary>
        /// Gets a list of windows from the EVE Client
        /// </summary>
        public List<EveWindow> GetWindows
        {
            get
            {
                return Builtin["uicore"]["registry"].CallMethod("GetWindows", new object[0]).GetList<EveObject>().ConvertAll(new Converter<EveObject, EveWindow>(EveObject2EveWindow));
            }
        }

        public EveObject loginfrm
        {
            get
            {
                return Builtin["uicore"]["layer"]["login"].GetValueAs<EveObject>();
            }
        }

        public EveObject charselfrm
        {
            get
            {
                return Builtin["uicore"]["layer"]["charsel"].GetValueAs<EveObject>();
            }
        }

        
        EveAgentDialogWindow _agentDialogWindow;
        /// <summary>
        /// Gets the dialog window og the provided agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <returns>EveAgentDialogWindow or Null</returns>
        public EveAgentDialogWindow GetAgentDialogWindow(int agentId)
        {
            if (_agentDialogWindow == null)
                _agentDialogWindow = GetWindows.Where(w => w.Type == EveWindow.EveWindowType.AgentDialog).ToList<EveWindow>().ConvertAll<EveAgentDialogWindow>(new Converter<EveWindow, EveAgentDialogWindow>(EveWindow2EveAgentDialogWindow)).Where(aw => aw.AgentId == agentId).FirstOrDefault();
            return _agentDialogWindow;
        }

        public EveWindow GetFittingWindow
        {
            get
            {
                return this.GetWindows.Where(w => w.Type == EveWindow.EveWindowType.FittingWindow).FirstOrDefault();
            }
        }

        EveActiveShip _activeship;
        public EveActiveShip GetActiveShip
        {
            get
            {
            
                if (_activeship == null)
                    _activeship = new EveActiveShip(Frame.Client.GetService("clientDogmaIM")["dogmaLocation"].CallMethod("GetShip", new object[0]));
                return _activeship;
            }
        }
        EveMe _eveMe;
        public EveMe EveMe
        {
            get
            {
                if (_eveMe == null)
                    _eveMe = new EveMe();
                return _eveMe;
            }
        }

        public EveChatWindow GetLocalChat
        {
            get
            {
                return Frame.Client.GetWindows.Where(w => w.WindowCaption == "Local").FirstOrDefault().ToChatWindow;
            }
        }

        public string Charname
        {
            get
            {
                return Frame.Client.EveOwners.CallMethod("GetIfExists" ,new object [] {Session.CharId})["ownerName"].GetValueAs<string>();
             
            }
        }

        public EveChatWindow GetCorpChat
        {
            get
            {
                return Frame.Client.GetWindows.Where(w => w.WindowCaption == "Corp").FirstOrDefault().ToChatWindow;
            }
        }

        public double qlengdouble
        {
            get
            {
                double tmp = Frame.Client.GetService("skillqueue").CallMethod("GetTrainingLengthOfQueue", new object[] { }).GetValueAs<double>();
                TimeSpan tmp2 = TimeSpan.FromTicks((long)tmp);
                return tmp2.TotalDays;//Frame.Client.GetService("skillqueue").CallMethod("GetTrainingLengthOfQueue", new object[] { }).GetValueAs<double>();
            }
        }

        public bool placeinq()
        {

            double qlen = qlengdouble;
             if (qlen < 1)
             {
                 return true;
             }
            return false;

        }
        public List<EveBookmark> GetMyBookmarks()
        {
            Dictionary<long, EveObject> bms;
            //bms = Frame.Client.BookmarkService.CallMethod("GetMyBookmarks", new object[0]).GetDictionary<EveObject>();
            //if (bms.Count == 0)
            bms = Frame.Client.BookmarkService["bookmarkCache"].GetDictionary<long>();
            //if (PyCall.PyErr_Occurred() != IntPtr.Zero)
            //    PyCall.PyErr_Clear();
            return bms.Values.ToList<EveObject>().ConvertAll<EveBookmark>(EveObject2EveBookmark);
        }

        public List<EveMarketOrder> GetCachedOrders()
        {
         
            Dictionary<long, EveObject> bms;
            //bms = Frame.Client.BookmarkService.CallMethod("GetMyBookmarks", new object[0]).GetDictionary<EveObject>();
            //if (bms.Count == 0)
            bms = Frame.Client.GetService("marketQuote")["orderCache"].GetDictionary<long>();
           
            //if (PyCall.PyErr_Occurred() != IntPtr.Zero)
            //    PyCall.PyErr_Clear();
            List<EveObject> tmp = bms.Values.ToList<EveObject>();
           List<EveObject> tmp2 = new List<EveObject>();
           List<EveObject> tmp3 = new List<EveObject>();
            foreach (EveObject a in tmp)
            {
           tmp2 = tmp2.Union(a.GetList<EveObject>()).ToList<EveObject>();           

            }
            foreach (EveObject a in tmp2)
            {
                tmp3 = tmp3.Union(a.GetList<EveObject>()).ToList<EveObject>();

            }
            return tmp3.ConvertAll<EveMarketOrder>(EveObject2EveMarketOrder);
        }

        public List<EveSkill> GetMySkills()
        {
            List<EveObject> mys;
           mys = Frame.Client.SkillService["myskills"].GetList<EveObject>();
           return mys.ConvertAll<EveSkill>(EveObject2EveSkill);
        }

        //  def TryFit(self, invItems, shipID = None):  tested/working
        public bool tryfit(List<EveItem> invitems)
        {
            if (Frame.Client.Session.InStation)
            {
                Frame.Client.GetService("menu").CallMethod("TryFit", new object[] { invitems, GetActiveShip.ItemId },true);
                return true;
            }
            return false;
            }

        //  def TryFit(self, invItems, shipID = None):  tested/working
        public bool tryfit(List<EveItem> invitems, long shipid)
        {
            if (Frame.Client.Session.InStation)
            {
                Frame.Client.GetService("menu").CallMethod("TryFit", new object[] { invitems, shipid });
                return true;
            }
            return false;
        }

        //def AssembleShip(self, invItems):     untested, should work
        public bool AssembleShip(List<EveItem> invitems)
        {
            if (Frame.Client.Session.InStation)
            {
                Frame.Client.GetService("menu").CallMethod("AssembleShip", new object[] { invitems });
                return true;
            }
            return false;
        }

        // self.invCache.GetInventoryFromId(shipID).StripFitting()   tested/working

        public bool StripFitting(long shipid)
        {

            if (Frame.Client.Session.InStation)
            {
                EveObject tmp = new EveObject();

                tmp = Frame.Client.GetService("invCache").CallMethod("GetInventoryFromId", new object[] { shipid }).GetValueAs<EveObject>();
                tmp.CallMethod("StripFitting", new object[] { });
                return true;
            }
            return false;
        }

        //def InjectSkillIntoBrain(self, invItems):     untested

        public void InjectSkillIntoBrain(List<EveItem> invitems)
        {
            Frame.Client.GetService("menu").CallMethod("InjectSkillIntoBrain", new object[] { invitems });
            return;
        }

        //  sm.StartService('station').TryActivateShip(invItem) (checked invItem = itemid of a ship) dunno if this works

        public void TryActivateShip(EveObject ship)
        {
            Frame.Client.GetService("station").CallMethod("TryActivateShip", new object[] { ship } ,true);
            return;
        }


        public List<EveQskill> GetMyQueue()
        {
            List<EveObject> myqs;
            myqs = Frame.Client.skillq["skillQueue"].GetList<EveObject>();
            return myqs.ConvertAll<EveQskill>(tuple2QEveSkill);
        }

        public bool logintest(string usr, string psw)
        {
            if (Frame.Client.GetService("viewState").CallMethod("IsViewActive", new object[] { "login" }).GetValueAs<bool>())
            {
                Frame.Client.loginfrm["usernameEditCtrl"].CallMethod("SetValue", new object[] { usr });
                Frame.Client.loginfrm["passwordEditCtrl"].CallMethod("SetValue", new object[] { psw });
                Frame.Client.loginfrm.CallMethod("_Connect", new object[] { },true);
            }
                return false;
        }

        public bool isconnecting()
        {             
          return  Frame.Client.loginfrm["connecting"].GetValueAs<bool>();                   
        }


        public List<Charslot> GetCharslots()
        {
            List<EveObject> mychars;
            mychars = Builtin["uicore"]["layer"]["charsel"]["sr"]["slots"].GetList<EveObject>();

            return mychars.ConvertAll<Charslot>(EveObject2Charslot);
        }

        public bool selectchar(string charname)
        {
            Charslot tmp = GetCharslots().Where(i => i.Name == charname).FirstOrDefault();

            if (tmp.idx != 0)
            {
                Builtin["uicore"]["layer"]["charsel"].CallMethod("SelectSlot", new object[] { tmp.PointerToObject }, true);
                return false;
            }
            Builtin["uicore"]["layer"]["charsel"].CallMethod("Confirm", new object[] { }, true);
            return true;
        }

        
        public void Getandopenwindow(string name)
        {
            List<EveWindow> mywindow;
            mywindow = Frame.Client.GetWindows;
            EveWindow winni;
            if (Frame.Client.Session.InSpace == true)
            {
                mywindow = Frame.Client.GetWindows;
                winni = mywindow.Where(x => x.Name.Contains("InventorySpace")).FirstOrDefault();
                if (winni == null)
                {
                    Frame.Log("Kein inv gefunden öffne");
                    Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);
                }
            }
            if (Frame.Client.Session.InStation == true)
            {
                mywindow = Frame.Client.GetWindows;
                winni = mywindow.Where(x => x.Name.Contains("InventoryStation")).FirstOrDefault();
                if (winni == null)
                {
                    Frame.Log("Kein inv gefunden öffne");
                    Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenInventory);
                }
             }
            mywindow = Frame.Client.GetWindows;
            winni = mywindow.Where(x => x.Name.Contains(name)).FirstOrDefault();
            if (name == "Orehold" && winni == null)
            {
                Frame.Log("Öffne Orehold");
                Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenOreHoldOfActiveShip);
                return;
            }
            if (name == "StationItems" && winni == null)
            {
                Frame.Log("Öffne Itemhanger");
                Frame.Client.ExecuteCommand(EveModel.EveCommand.OpenHangarFloor);
                return;
            }
            Frame.Log("Kein Befehl gefunden oder schon offen");

            return;
        }
      
   

        /*
         *  def AddSkillToEnd(self, skillID, current, nextLevel = None):
        queueLength = self.GetNumberOfSkillsInQueue()
        if queueLength >= const.skillQueueMaxSkills:
            eve.Message('CustomNotify', {'notify': localization.GetByLabel('UI/SkillQueue/QueueIsFull')})
            return
        totalTime = self.GetTrainingLengthOfQueue()
        if totalTime > const.skillQueueTime:
            eve.Message('CustomNotify', {'notify': localization.GetByLabel('UI/SkillQueue/QueueIsFull')})
            return
        if nextLevel is None:
            queue = self.GetServerQueue()
            nextLevel = self.FindNextLevel(skillID, current, queue)
        self.AddSkillToQueue(skillID, nextLevel)
*/

        public bool AddSkillToEnd(EveSkill skill, int crlvl)
        {
            Frame.Client.GetService("skillqueue").CallMethod("AddSkillToEnd", new object [] {skill.typeID, crlvl});
            return true;
        }

        public bool recallalldrones()
        {
            EveObject drones = new EveObject();
            drones = Frame.Client.GetService("michelle").CallMethod("GetDrones", new object[] { })["items"].CallMethod("keys", new object[] { }).GetValueAs<EveObject>();
            if (drones.IsValid)
            {
                Frame.Client.GetService("menu").CallMethod("ReturnToDroneBay", new object[] { drones }, true);
                return true;
            }
            return false;
           }


        //def MineRepeatedly(self, droneIDs):

        public bool DroneMineRepeatedly()
        {
            EveObject drones = new EveObject();
            drones = Frame.Client.GetService("michelle").CallMethod("GetDrones", new object[] { })["items"].CallMethod("keys", new object[] { }).GetValueAs<EveObject>();
            if (drones.IsValid)
            {
                Frame.Client.GetService("menu").CallMethod("MineRepeatedly", new object[] { drones }, true);
                return true;
            }
            return false;
        }

        // sm.RemoteSvc('userSvc').ApplyPilotLicence(itemID, justQuery=False)
        public bool ApplyPilotLicence(EveItem lic)
        {
            //needs testing
            Frame.Client.GetService("userSvc").CallMethod("ApplyPilotLicence", new object[] { lic.ItemId, true }, true);
            
            return true;
        }
        public float maxActiveDrones
        {
            get
            {
                return Frame.Client.GodmaService.CallMethod("GetItem", new object[] { Frame.Client.Session.CharId })["maxActiveDrones"].GetValueAs<float>();
            }
        }

        public List<EveEntity> GetSortedAsteroidBelts()
        {
            var list = Entities.Where(ent => ent.Group == Group.AsteroidBelt).ToList<EveEntity>();
            list.Sort(CompareAsteroidBelts);
            return list;
        }

        public void refreshorders(int typeid)
        {
            Frame.Client.GetService("marketQuote").CallMethod("RefreshOrderCache", new object[] { typeid }, true);
            Frame.Client.GetService("marketQuote").CallMethod("RefreshJumps", new object[] { typeid }, true);
        }

        public void refreshorders(EveItem item)
        {
            Frame.Client.GetService("marketQuote").CallMethod("RefreshOrderCache", new object[] { item.TypeId }, true);
            Frame.Client.GetService("marketQuote").CallMethod("RefreshJumps", new object[] { item.TypeId }, true);
        }

        public List<EveEntity> GetNPCTargets()
        {
            return Entities.Where(
                en => en.IsNpc &&
                        en.Distance < 125000 &&
                        en._item.Category == Category.Entity &&
                        en._item.Group != Group.LargeCollidableStructure &&
                        en._item.Group != Group.SpawnContainer &&
                        !en.HasExploded && en.IsValid
                        ).OrderBy(en => en.Distance).ToList<EveEntity>();
        }


        public double getcargousedback()
        {
             EveInventoryContainer cargoho2 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
             double usedcargo = cargoho2.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
             return usedcargo;
           
        }

        public double getcargofullback()
        {
          EveInventoryContainer cargoho2 = Frame.Client.GetPrimaryInventoryWindow.OreHoldOfActiveShip;           //  Container wird erstellt "cargoho" und wird mit aktivem Cargohold verknüpft
          double fullcargo = cargoho2.UsedCapacity;                                                                     // Variablen werden gesetzt Verbrauchtes Cargo <--
          return fullcargo;

        }

        #region External Resources
        static string _innerspacePath = @"C:\Program Files (x86)\InnerSpace\.NET Programs";
        static System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        static Dictionary<string, int> _eveAgentAndIds;
        public static Dictionary<string, int> EveAgents
        {
            get
            {
                if (_eveAgentAndIds == null)
                {
                    Frame.Log("Trying to load agents");
                    if (System.IO.File.Exists(_innerspacePath + @"\agents.bin"))
                    {
                        using (System.IO.FileStream stream = System.IO.File.OpenRead(_innerspacePath + @"\agents.bin"))
                        {
                            _eveAgentAndIds = (Dictionary<string, int>)formatter.Deserialize(stream);
                            Frame.Log("Succesfully loaded agents");
                        }
                    }
                }
                return _eveAgentAndIds;
            }
        }

        static List<int> _warpScramblers;
        public static List<int> WarpScramblingEntities
        {
            get
            {
                if (_warpScramblers == null)
                {
                    Frame.Log("Trying to load warpScramblers");
                    if (System.IO.File.Exists(_innerspacePath + @"\warpScramblers.bin"))
                    {
                        using (System.IO.FileStream stream = System.IO.File.OpenRead(_innerspacePath + @"\warpScramblers.bin"))
                        {
                            _warpScramblers = (List<int>)formatter.Deserialize(stream);
                            Frame.Log("Succesfully loaded warpScramblers");
                        }
                    }
                }
                return _warpScramblers;
            }
        }
        static List<int> _commanderWrecks;
        public static List<int> CommanderWrecks
        {
            get
            {
                if (_commanderWrecks == null)
                {
                    Frame.Log("Trying to load CommanderWrecks");
                    if (System.IO.File.Exists(_innerspacePath + @"\CommanderWrecks.bin"))
                    {
                        using (System.IO.FileStream stream = System.IO.File.OpenRead(_innerspacePath + @"\CommanderWrecks.bin"))
                        {
                            _commanderWrecks = (List<int>)formatter.Deserialize(stream);
                            Frame.Log("Succesfully loaded CommanderWrecks");
                        }
                    }
                }
                return _commanderWrecks;
            }
        }

        static List<int> _officerWrecks;
        public static List<int> OfficerWrecks
        {
            get
            {
                if (_officerWrecks == null)
                {
                    Frame.Log("Trying to load OfficerWrecks");
                    if (System.IO.File.Exists(_innerspacePath + @"\OfficerWrecks.bin"))
                    {
                        using (System.IO.FileStream stream = System.IO.File.OpenRead(_innerspacePath + @"\OfficerWrecks.bin"))
                        {
                            _officerWrecks = (List<int>)formatter.Deserialize(stream);
                            Frame.Log("Succesfully loaded OfficerWrecks");
                        }
                    }
                }
                return _officerWrecks;
            }
        }

        static List<int> _jammingEntities;
        public static List<int> JammingEntities
        {
            get
            {
                if (_jammingEntities == null)
                {
                    Frame.Log("Trying to load Jammers");
                    if (System.IO.File.Exists(_innerspacePath + @"\Jammers.bin"))
                    {
                        using (System.IO.FileStream stream = System.IO.File.OpenRead(_innerspacePath + @"\Jammers.bin"))
                        {
                            _jammingEntities = (List<int>)formatter.Deserialize(stream);
                            Frame.Log("Succesfully loaded Jammers");
                        }
                    }
                }
                return _jammingEntities;
            }
        }

        #endregion

        #region Clean Up
        /// <summary>
        /// Clean up the Cache
        /// </summary>
        public void Dispose()
        {
            foreach (var item in Objects.Values)
            {
                item.Dispose();
            }
            Objects = null;
        }
        #endregion

        #region Converters
        internal static EveAgentMission EveObject2EveAgentMission(EveObject obj)
        {
            return new EveAgentMission() { PointerToObject = obj.PointerToObject };
        }
        internal static EveBookmark EveObject2EveBookmark(EveObject obj)
        {
            return new EveBookmark(obj.PointerToObject);
        }
        internal static EveMarketOrder EveObject2EveMarketOrder(EveObject obj)
        {
            return new EveMarketOrder(obj.PointerToObject);
        }
        internal static EveSkill EveObject2EveSkill(EveObject obj)
        {
            return new EveSkill(obj.PointerToObject);
        }
        internal static EveQskill tuple2QEveSkill(EveObject obj)
        {
            return new EveQskill(obj.PointerToObject);
        }

        internal static Charslot EveObject2Charslot(EveObject obj)
        {
            return new Charslot(obj.PointerToObject);
        }
        internal static EveAgentBookmark EveObject2EveAgentBookmark(EveObject obj)
        {
            return new EveAgentBookmark(obj.PointerToObject);
        }
        internal static EveWindow EveObject2EveWindow(EveObject obj)
        {
            return new EveWindow() { PointerToObject = obj.PointerToObject };
        }
        internal static EveAgentDialogWindow EveWindow2EveAgentDialogWindow(EveObject obj)
        {
            return new EveAgentDialogWindow() { PointerToObject = obj.PointerToObject };
        }
        internal static EveInventoryWindow EveWindow2EveInventoryWindow(EveObject obj)
        {
            return new EveInventoryWindow() { PointerToObject = obj.PointerToObject };
        }
        internal static EveScanResult EveObject2EveScanResult(EveObject obj)
        {
            return new EveScanResult() { PointerToObject = obj.PointerToObject };
        }
        #endregion

        #region Routine for sorting asteroid belts
        private static int CompareAsteroidBelts(EveEntity e1, EveEntity e2)
        {
            int e1num = 0, e2num = 0;
            string planetE1 = e1.Name.Split(' ')[1],
                planetE2 = e2.Name.Split(' ')[1];

            string[] romans = { "N", "I", "II", "III", "IV", "V", "VI", "VII",
                                "VIII", "IX", "X", "XI", "XII", "XIII", "XIV",
                                "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

            for (int i = 0; i < romans.Length; i++)
            {

                if (romans[i] == planetE1)
                {
                    e1num = i;
                }
                if (romans[i] == planetE2)
                {
                    e2num = i;
                }
                if (e1num > 0 && e2num > 0)
                {
                    break;
                }
            }
            int i1 = int.Parse(e1.Name.Substring(e1.Name.LastIndexOf(' ') + 1));
            int i2 = int.Parse(e2.Name.Substring(e2.Name.LastIndexOf(' ') + 1));

            if (e1num > e2num)
            {
                return 1;
            }
            else if (e1num < e2num)
            {
                return -1;
            }
            else
            {
                if (i1 > i2)
                {
                    return 1;
                }
                else if (i1 < i2)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
    }
}
