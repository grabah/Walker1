using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WalkerSimulator.Tubesheet.Models;

namespace WalkerSimulator.Tubesheet.ViewModels
{
    public class TubeSheetVM
    {
        private TubesheetModel _tubeSheet;

        public int RowsNum {get{ return _tubeSheet.MaxRows; }}
        public int ColumnsNum { get { return _tubeSheet.MaxColumns; } }
        public TubeVM[,] TubeVMs { get; set; }
        public TubeSheetVM()
        {
            _tubeSheet = new TubesheetModel();
           
        }
        public void LoadTubeSheet(string path)
        {
            LoadTubeSheetFile(path);
            CreateTubesVM();
        }
        private void CreateTubesVM()
        {
          TubeVMs = new TubeVM[RowsNum, ColumnsNum];
          for(int i=0;i<RowsNum;i++)
            {
                for(int j = 0;j< ColumnsNum; j++)
                {
                    TubeVMs[i, j] = new TubeVM(_tubeSheet.Tubes[i, j]) { Pitch = _tubeSheet.Pitch, Diameter=_tubeSheet.Diameter };
                }
            }
        }

        private void LoadTubeSheetFile(string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            _tubeSheet.LoadXml(xml.SelectSingleNode("TubesheetModel"));
        }
        
    }
}
