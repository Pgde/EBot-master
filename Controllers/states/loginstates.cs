using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.states
{

    public enum loginstate
    {
        Idle,
        login,
        waitforcharsel,
        selectchar,
        waitforig,
        Error,
        wait
    }

}

