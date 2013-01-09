using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveBookmark : EveObject
    {
        string _title;

        public EveBookmark(IntPtr ptr) : base()
        {
            PointerToObject = ptr;

            BookmarkId = this["bookmarkID", false].GetValueAs<long>();
            ItemId =  this["itemID", false].GetValueAs<int>();
            LocationId = this["locationID", false].GetValueAs<long>();
            FolderId = this["folderID", false].GetValueAs<long>();
            OwnerId = this["ownerID", false].GetValueAs<long>();
            _title = this["memo", false].GetValueAs<string>();
            if (!string.IsNullOrWhiteSpace(_title) && _title.Contains("\t"))
            {
                Memo = _title.Substring(_title.IndexOf("\t") + 1);
                _title = _title.Substring(0, _title.IndexOf("\t"));
            }
            Hint = this["hint", false].GetValueAs<string>();
            X = this["x", false].GetValueAs<double>();
            Y = this["y", false].GetValueAs<double>();
            Z = this["z", false].GetValueAs<double>();
        }

        public long? BookmarkId { get; private set; }
        public long? ItemId { get; private set; }
        public long? LocationId { get; private set; }
        public long? FolderId { get; private set; }
        public long? OwnerId { get; private set;  }
        public string Title { get { return _title ?? Hint; } }
        public string Memo { get; private set; }
        public string Hint { get; private set; }
        public double? X { get; private set; }
        public double? Y { get; private set; }
        public double? Z { get; private set; }

        public void AlignTo()
        {
            Frame.Client.MenuService.CallMethod("AlignToBookmark", new object[] { BookmarkId }, true);
        }
        public void SetDestination()
        {
            Frame.Client.GetService("starmap").CallMethod("SetWaypoint", new object[] { this.ItemId, true, true }, true);
        }
        public void WarpTo()
        {
            WarpTo(0.0);
        }
        public void WarpTo(double distance)
        {
            Frame.Client.MenuService.CallMethod("WarpToBookmark", new object[] { this, distance }, true);
        }
        public void Delete()
        {
            Frame.Client.AddressBook.CallMethod("DeleteBookmarks", new object[] { new List<object>() { BookmarkId } }, true);
        }
    }
}
