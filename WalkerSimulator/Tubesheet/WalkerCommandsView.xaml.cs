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
using WalkerSimulator.Tubesheet.ViewModels;

namespace WalkerSimulator.Tubesheet
{
    /// <summary>
    /// Interaction logic for TubeSheetCommandsView.xaml
    /// </summary>
    public partial class WalkerCommandsView : UserControl
    {
        private WalkerVM walkerVM { get { return (WalkerVM)DataContext; } }
        public WalkerCommandsView()
        {
            InitializeComponent();
        }
        private void buttonMain_Click(object sender, RoutedEventArgs e)
        {
            walkerVM.RotateMainAxis();
        }

        private void buttonSec_Click(object sender, RoutedEventArgs e)
        {
            walkerVM.RotateSecAxis();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            walkerVM.ClearMovesLog();
        }

        internal void SetupTextBox()
        {
            walkerVM.PropertyChanged += WalkerCommandsView_PropertyChanged;
        }

        private void WalkerCommandsView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName== "MovesLog")
            {
                textBox.ScrollToEnd();
            }
        }

        private async void buttonStartWalker_ClickAsync(object sender, RoutedEventArgs e)
        {
            await  walkerVM.RunAlgorithmAsync();
        }
    }
}
