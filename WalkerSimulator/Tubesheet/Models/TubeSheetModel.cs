using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WalkerSimulator.Tubesheet.Models
{
    public class TubesheetModel
    {
        public float Pitch { get; set; }
        public float Diameter { get; set; }
        public int MaxRows { get; set; }
        public int MaxColumns { get; set; }
        public TubeModel[,] Tubes { get; set; }

        internal void CreateTubes(List<TubeModel> tempList)
        {
            Tubes = new TubeModel[MaxRows,MaxColumns];
            foreach(TubeModel item in tempList)
            {
             Tubes[item.Row-1, item.Column-1] = item;
            }
            //Check if some tubes are missing
        }

        internal void LoadXml(XmlNode xmlNode)
        {
            Diameter = float.Parse(xmlNode.SelectSingleNode("TubesheetDiameter").InnerText,CultureInfo.InvariantCulture);
            Pitch = float.Parse(xmlNode.SelectSingleNode("TubesheetPitch").InnerText, CultureInfo.InvariantCulture);

            List<TubeModel> tempList = new List<TubeModel>();
            foreach (XmlNode node in xmlNode.SelectNodes("Tubes/Tube"))
            {
                TubeModel tube = new TubeModel(node);
                tempList.Add(tube);
                if (MaxRows < tube.Row) MaxRows = tube.Row;
                if (MaxColumns < tube.Column) MaxColumns = tube.Column;
            }
            CreateTubes(tempList);
        }
    }
}
