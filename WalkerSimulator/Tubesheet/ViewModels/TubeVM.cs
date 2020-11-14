using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WalkerSimulator.Tubesheet.Models;

namespace WalkerSimulator.Tubesheet.ViewModels
{
    public class TubeVM : INotifyPropertyChanged
    {
        public TubeVM(TubeModel tube)
        {
            _tube = tube;
            _tube.PropertyChanged += _tube_PropertyChanged;
        }

        private TubeModel _tube;

        public event PropertyChangedEventHandler PropertyChanged;

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
                case TubeStatus.Target:
                    return System.Windows.Media.Colors.GreenYellow.ToString();
                default:
                    return "Gray";
            }
        }

        internal void ChangeStatus(TubeStatus target)
        {
            _tube.ChangeStatus(target);
        }
        private void _tube_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
                OnPropertyChange("TubeColorCode");
        }
        private void OnPropertyChange([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
