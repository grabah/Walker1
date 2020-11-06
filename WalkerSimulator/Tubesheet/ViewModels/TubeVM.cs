using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkerSimulator.Tubesheet.Models;

namespace WalkerSimulator.Tubesheet.ViewModels
{
    public class TubeVM
    {
        public TubeVM(TubeModel tube)
        {
            _tube = tube;
        }

        private TubeModel _tube;
        //ViewModel
        public string ToolTipTxt { get { return "[" + _tube.Row.ToString() + "," + _tube.Column.ToString() + "]" + Environment.NewLine + _tube.Status.ToString(); } }
        public float Pitch { get; set; }
        public float Diameter { get; set; }
        public string TubeColorCode { get { return StatusToColor(_tube.Status); } }
      
 
        private string StatusToColor(TubeStatus status)
        {
            switch (status)
            {
                case TubeStatus.Critical:
                    return "Red";
                case TubeStatus.Plugged:
                    return "Black";
                case TubeStatus.Unknown:
                    return "Gray";
                default:
                    return "Gray";
            }
        }
    }
}
