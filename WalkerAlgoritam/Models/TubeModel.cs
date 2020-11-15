using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WalkerSimulator.Tubesheet.Models
{
    public class TubeModel:INotifyPropertyChanged
    {
        public TubeModel(XmlNode node)
        {
            this.Column = int.Parse(node.SelectSingleNode("Column").InnerText);
            this.Row = int.Parse(node.SelectSingleNode("Row").InnerText);
            this.Status = (TubeStatus)Enum.Parse(typeof(TubeStatus), node.SelectSingleNode("Status").InnerText);
        }

        /////////////////////////////////////////

        //Model
        private TubeStatus oldStatus;
        private TubeStatus status;
        public TubeStatus Status { get { return status; }  set { oldStatus = status; status = value; OnPropertyChange(); } }
        public int Row { get; set; }
        public int Column { get; set; }

        public void ChangeStatus(TubeStatus target)
        {
            if (Status == TubeStatus.Target && target == TubeStatus.Target)
                Status = oldStatus;
            else
                Status = target;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal bool CanPincersLock()
        {
            if (Status == TubeStatus.Plugged)
                return false;
            return true;
        }
        private void OnPropertyChange([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

   public  enum TubeStatus
    { 
        Unknown = 0,
        Plugged = 1,
        Critical = 2,
        Target
    }
}