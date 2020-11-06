using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WalkerSimulator.Tubesheet;
using WalkerSimulator.Tubesheet.Models;
using WalkerSimulator.Tubesheet.ViewModels;

namespace WalkerSimulator.tubesheet
{
    /// <summary>
    /// Interaction logic for TubesheetControl.xaml
    /// </summary>
    public partial class TubesheetControl : UserControl
    {
        public TubesheetControl()
        {
            InitializeComponent();  
        }
        public void LoadTubeSheet(string path)
        {
            TubeSheetVM vm = new TubeSheetVM();
            vm.LoadTubeSheet(path);
            CreateTubeCtrls(vm);
            this.DataContext = vm;
        }
        private void CreateTubeCtrls(TubeSheetVM vm)
        {
            int rows = vm.RowsNum;
            int columns = vm.ColumnsNum;
            TubeVM[,] tubeVms = vm.TubeVMs;
            mainGrid.Children.Clear();
            for (int i = rows-1; i >=0 ; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    TubeControl ctrl = new TubeControl();
                    mainGrid.Children.Add(ctrl);
                    ctrl.DataContext = tubeVms[i, j];
                }
            }
        }
    }
}