using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WalkerSimulator.Tubesheet.Models
{
    public class TubeModel
    {
        public TubeModel(XmlNode node)
        {
            this.Column = int.Parse(node.SelectSingleNode("Column").InnerText);
            this.Row = int.Parse(node.SelectSingleNode("Row").InnerText);
            this.Status = (TubeStatus)Enum.Parse(typeof(TubeStatus), node.SelectSingleNode("Status").InnerText);
        }

        /////////////////////////////////////////

        //Model
        internal TubeStatus Status { get; set; }
        internal int Row { get; set; }
        internal int Column { get; set; }
    }

    enum TubeStatus
    { 
        Unknown = 0,
        Plugged = 1,
        Critical = 2
    }
}