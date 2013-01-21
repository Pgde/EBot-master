using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.states
{

    public enum BuyControllerStates
    {
        Idle,
        Initialise,
        buy,
        Error,
        wait,
        done,
        setup,
        buyjita,
        traveljita,
        gojita,
        travelhome,
        gohome,
        shiphome,
        unload
    }

}
