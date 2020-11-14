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
        public TubeVM[,] Tubes { get; set; }
        public WalkerVM Walker { get; set; }
        public TubeSheetVM()
        {
         
           
        }
        public void LoadTubeSheet(string path)
        {
            LoadTubeSheetFile(path);
            CreateTubes();
            CreatteWalker();
        }

        private void CreatteWalker()
        {
            WalkerModel walkerModel = new WalkerModel(_tubeSheet);
            Walker = new WalkerVM(this, walkerModel);
        }

        internal float GetPitch()
        {
            return _tubeSheet.Pitch;
        }
        internal float GetDiameter()
        {
            return _tubeSheet.Diameter;
        }
        private void CreateTubes()
        {
          Tubes = new TubeVM[RowsNum, ColumnsNum];
          for(int i=0;i<RowsNum;i++)
            {
                for(int j = 0;j< ColumnsNum; j++)
                {
                    Tubes[i, j] = new TubeVM(_tubeSheet.Tubes[i, j]) { Pitch = _tubeSheet.Pitch, Diameter=_tubeSheet.Diameter };
                }
            }
        }

        private void LoadTubeSheetFile(string path)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            _tubeSheet = new TubesheetModel(xml.SelectSingleNode("TubesheetModel"));
        }
        
    }
}
