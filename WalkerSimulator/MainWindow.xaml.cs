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

namespace WalkerSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TubeSheetCtrl1_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileOk += Dialog_FileOk;
            dialog.ShowDialog();
           
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path =  AppDomain.CurrentDomain.BaseDirectory + "\\files\\Tubesheet.xml";
            TubeSheetCtrl1.LoadTubeSheet(path);
        }
        private void Dialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TubeSheetCtrl1.LoadTubeSheet(((OpenFileDialog)sender).FileName);
        }

       
    }
}
