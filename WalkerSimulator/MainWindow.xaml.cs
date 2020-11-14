using Microsoft.Win32;
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
using WalkerSimulator.tubesheet;
using WalkerSimulator.Tubesheet.ViewModels;

namespace WalkerSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TubesheetView TubeSheetCtrl1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileOk += Dialog_FileOk;
            dialog.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateTubeSheetCtrl(AppDomain.CurrentDomain.BaseDirectory + "\\files\\Tubesheet.xml");
        }
        private void Dialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CreateTubeSheetCtrl(((OpenFileDialog)sender).FileName);
        }
        private void CreateTubeSheetCtrl(string path)
        {
            TubeSheetCtrl1 = new TubesheetView();
            TubeSheetGrid.Children.Clear();
            TubeSheetGrid.Children.Add(TubeSheetCtrl1);
            TubeSheetCtrl1.LoadTubeSheet(path);
            RotateButtonsGrid.Visibility = Visibility.Visible;

        }

        private void buttonMain_Click(object sender, RoutedEventArgs e)
        {
            ((TubeSheetVM)TubeSheetCtrl1.DataContext).Walker.RotateMainAxis();
        }

        private void buttonSec_Click(object sender, RoutedEventArgs e)
        {
            ((TubeSheetVM)TubeSheetCtrl1.DataContext).Walker.RotateSecAxis();
        }
    }
}
