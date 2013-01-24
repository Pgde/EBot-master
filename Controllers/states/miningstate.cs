using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers.states
{


  //  enum TravelStates
  //  {
  //      Initialise, Start, Travel, ArrivedAtDestination, Opencargall, carunload, letzgo,
  //      Opencargstation, Mining, warping, warphome, warpnextbelt, unload, warptobelt, travStart, changebook,
  //      sqlchecktime, sqlchecken
  //  }
    public enum MiningState
    {
        Initialise,
        Start,
        Travel,
        ArrivedAtDestination,
        letzgo,
        Mining,
        warping,
        warphome,
        unload,
        warptobelt,
        travStart,
        changebook,
        wait,       
        schelling
    }

}

