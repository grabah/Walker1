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

namespace WalkerSimulator.Tubesheet
{
    /// <summary>
    /// Interaction logic for TubesheetControl.xaml
    /// </summary>
    public partial class TubesheetView : UserControl
    {
        public TubesheetView()
        {
            InitializeComponent();
        }
        public void LoadTubeSheet(string path)
        {
            TubeSheetVM vm = new TubeSheetVM();
            vm.LoadTubeSheet(path);
            CreateTubeCtrls(vm);

            Walker1.DataContext = vm.Walker;
            vm.Walker.PropertyChanged += Walker1.WalkerVM_PropertyChanged;
            this.DataContext = vm;
        }

        private void CreateTubeCtrls(TubeSheetVM vm)
        {
            int rows = vm.RowsNum;
            int columns = vm.ColumnsNum;
            TubeVM[,] tubeVms = vm.Tubes;
            TubesGrid.Children.Clear();
            for (int i = rows - 1; i >= 0; i--)
            {
                for (int j = 0; j < columns; j++)
                {
                    TubeView ctrl = new TubeView();
                    TubesGrid.Children.Add(ctrl);
                    ctrl.DataContext = tubeVms[i, j];
                }
            }
        }
       
    }
}