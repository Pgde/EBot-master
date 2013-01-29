using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.states
{

    public enum DroneState
    {
        Initialise,
        Idle,
        Startdrones,
        dronesback,
        dronesatwork,
        vorhandenkaufen,
        wait,
        waitbuy,
        sleeper,
        donebuy

    }

}
