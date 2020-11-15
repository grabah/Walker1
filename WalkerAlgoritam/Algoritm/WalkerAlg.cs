using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkerSimulator.Tubesheet.Models;

namespace WalkerAlgoritam.WalkerAlg
{
    class WalkerAlg
    {
        WalkerModel _walkerModel;
        TubesheetModel _tubeSheet;
        public  WalkerAlg(WalkerModel walkerModel, TubesheetModel tubeSheet)
        {
            _walkerModel = walkerModel;
            _tubeSheet = tubeSheet;
        }

        internal async Task RunAsync( )
        {
            _walkerModel.ClearLog();
            _walkerModel.WriteToLog("****Alg Start****");
            await Task.Delay(5000);
            _walkerModel.WriteToLog("****Alg End******");

        }
    }
}
