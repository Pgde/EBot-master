using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveScanResult : EveObject
    {
        public string Name
        {
            get { return this["typeName"].GetValueAs<string>(); }
        }
        public string Group
        {
            get { return this["groupName"].GetValueAs<string>(); }
        }
        public double Certainty
        {
            get { return this["certainty"].GetValueAs<double>(); }
        }
        public string ScanGroupName
        {
            get { return this["scanGroupName"].GetValueAs<string>(); }
        }
        public string Id
        {
            get { return this["id"].GetValueAs<string>(); }
        }
        public void WarpTo()
        {
            Frame.Client.MenuService.CallMethod("WarpToScanResult", new object[] { Id }, true);
        }
    }
}
