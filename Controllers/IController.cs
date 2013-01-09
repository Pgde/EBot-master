using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers
{
    public interface IController
    {
        bool IsWorkDone { get; set; }
        void DoWork();
    }
}
