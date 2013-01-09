using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveModel
{
    public class EveMe
    {
        public double MaxLockedTargets
        {
            get
            {
                return Frame.Client.GodmaService.CallMethod("GetItem", new object[] {Frame.Client.Session.CharId })["maxLockedTargets"].GetValueAs<double>();
            }
        }

    }
}
