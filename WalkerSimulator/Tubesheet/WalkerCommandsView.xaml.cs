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
        public WalkerCommandsView()
        {
            InitializeComponent();
        }
        private void buttonMain_Click(object sender, RoutedEventArgs e)
        {
            ((WalkerVM)DataContext).RotateMainAxis();
        }

        private void buttonSec_Click(object sender, RoutedEventArgs e)
        {
            ((WalkerVM)DataContext).RotateSecAxis();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            ((WalkerVM)DataContext).ClearMovesLog();
        }

        internal void SetupTextBox()
        {
            ((WalkerVM)DataContext).PropertyChanged += WalkerCommandsView_PropertyChanged;
        }

        private void WalkerCommandsView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName== "MovesLog")
            {
                textBox.ScrollToEnd();
            }
        }
    }
}
