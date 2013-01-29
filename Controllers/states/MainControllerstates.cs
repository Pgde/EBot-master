using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.states
{

    public enum maincontrollerStates
    {
        Idle,
        endminingcycle,
        pause,
        pauseloop,
        Error,
        wait,
        skillcheck,
        Startup,
        homesysarriv,
        checkbuy,
        startbuy,
        waitbuy,
        dronencheck,
        dronenbuy,
        resumemining
    }

}

