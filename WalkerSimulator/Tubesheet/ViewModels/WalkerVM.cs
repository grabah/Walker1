using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WalkerSimulator.Tubesheet.Models;

namespace WalkerSimulator.Tubesheet.ViewModels
{
    public class WalkerVM: INotifyPropertyChanged
    {
        public WalkerVM(TubeSheetVM tubeSheet, WalkerModel walkerModel)
        {
            _tubeSheetVM = tubeSheet;
            _walkerModel = walkerModel;
            _walkerModel.PropertyChanged += _walkerModel_PropertyChanged;
        }

        private TubeSheetVM _tubeSheetVM { get; set; }
        private WalkerModel _walkerModel { get; set; }

        private const int AxisThicknessLocked = 15;
        private const int AxisThicknessFree = 5;
        private  string PincersLockedColor = "Red";
        private string PincersUnLockedColor = "#4C991717";

        public event PropertyChangedEventHandler PropertyChanged;

        //ViewModel

        public string MovesLog { get { return _walkerModel.MovesLog; } set { } }

        public float Pitch { get { return _tubeSheetVM.GetPitch();  }}

        public float WorkHeadSize { get { return _tubeSheetVM.GetPitch(); } }

        public double WHX { get { return _walkerModel.GetWorkHeadPosition().X * Pitch; } }
        public double WHY { get { return _walkerModel.GetWorkHeadPosition().Y * Pitch; } }

        public double CenterX { get { return _walkerModel.RotationCenterPosition.X * Pitch; } }
        public double CenterY { get { return _walkerModel.RotationCenterPosition.Y * Pitch; } }
        public float CenterSize  { get { return Pitch; } }

        public int MainAxisThickness { get { if (_walkerModel.MainAxisLocked) return AxisThicknessLocked; else return AxisThicknessFree; } }
        public string MainAxisColor { get { return Colors.LightGreen.ToString(); } }
        public string MPincersColor { get { if (_walkerModel.MainAxisLocked) return PincersLockedColor.ToString(); else return PincersUnLockedColor.ToString(); } }
        public System.Drawing.Point[] MainAxisPoints { get { return _walkerModel.GetMainAxisPincerPoints();  } }
        public double MX1 { get { return _walkerModel.GetMainAxisPincerPoints()[0].X * Pitch; } }
        public double MY1 { get { return _walkerModel.GetMainAxisPincerPoints()[0].Y * Pitch; } }
        public double MX2 { get { return _walkerModel.GetMainAxisPincerPoints()[1].X * Pitch; } }
        public double MY2 { get { return _walkerModel.GetMainAxisPincerPoints()[1].Y * Pitch; } }
        public double MLX1 { get { return MX1 + 0.5 * Pitch; } }
        public double MLY1 { get {return MY1 + 0.5 * Pitch; } }
        public double MLX2 { get { return _walkerModel.GetWorkHeadPosition().X * Pitch + 0.5 * Pitch; } }
        public double MLY2 { get { return _walkerModel.GetWorkHeadPosition().Y * Pitch + 0.5 * Pitch; } }

        public string SPincersColor { get { if (_walkerModel.SecAxisLocked) return PincersLockedColor.ToString(); else return PincersUnLockedColor.ToString(); } }
        public string SecAxisColor { get { return Colors.Brown.ToString(); } }
        public int SecAxisThicness { get { if (_walkerModel.SecAxisLocked) return AxisThicknessLocked; else return AxisThicknessFree; } }
        public double SX1 { get { return _walkerModel.GetSecAxisPincerPoints()[0].X * Pitch; } }
        public double SY1 { get { return _walkerModel.GetSecAxisPincerPoints()[0].Y * Pitch; } }
        public double SX2 { get { return _walkerModel.GetSecAxisPincerPoints()[1].X * Pitch; } }
        public double SY2 { get { return _walkerModel.GetSecAxisPincerPoints()[1].Y * Pitch; } }
        public double SLX1 { get { return SX1 + 0.5 * Pitch; } }
        public double SLY1 { get { return SY1 + 0.5 * Pitch; } }
        public double SLX2 { get { return SX2 + 0.5 * Pitch; } }
        public double SLY2 { get { return SY2 + 0.5 * Pitch; } }

        internal bool SlideMainAxis(Point newPoint, Point oldPoint)
        {
            int T = 0;
            switch (_walkerModel.SecAxisAngle)
            {
                case AxisPosition.Right:
                    T = (int)((newPoint.X - oldPoint.X) / Pitch);
                    break;
                case AxisPosition.Left:
                    T = -(int)((newPoint.X - oldPoint.X) / Pitch);
                    break;
                case AxisPosition.Up:
                    T = (int)((newPoint.Y - oldPoint.Y) / Pitch);
                    break;
                case AxisPosition.Down:
                    T = -(int)((newPoint.Y - oldPoint.Y) / Pitch);
                    break;
            }
            if (Math.Abs(T) < 1)
                return false;//no move

            return _walkerModel.WalkerMakeMove(new WalkerMove() { type = WalkerMoveType.MainAxisTranslate, translation = T });
        }

        internal bool SlideSecAxis(Point newPoint, Point oldPoint)
        {
            int T = 0;
            switch (_walkerModel.SecAxisAngle)
            {
                case AxisPosition.Right:
                    T = (int)((newPoint.Y - oldPoint.Y) / Pitch);
                    break;
                case AxisPosition.Left:
                    T = -(int)((newPoint.Y - oldPoint.Y) / Pitch);
                    break;
                case AxisPosition.Up:
                    T = -(int)((newPoint.X - oldPoint.X) / Pitch);
                    break;
                case AxisPosition.Down:
                    T = (int)((newPoint.X - oldPoint.X) / Pitch);
                    break;
            }
            if (Math.Abs(T) < 1)
                return false;//no move

            return _walkerModel.WalkerMakeMove(new WalkerMove() { type = WalkerMoveType.SecondaryAxisTranslate, translation = T });
        }
        internal bool RotateMainAxis( )
        {
           return _walkerModel.WalkerMakeMove(new WalkerMove() { type = WalkerMoveType.MainAxisRotate});
        }
        internal bool RotateSecAxis( )
        {
            return _walkerModel.WalkerMakeMove(new WalkerMove() { type = WalkerMoveType.SecondaryAxisRotate});
        }
        internal bool WalkerMoveCenter(Point newPoint)
        {
            int X = (int)(newPoint.X / Pitch);
            int Y = (int)(newPoint.Y / Pitch);
            _walkerModel.WalkerMoveCenter( X,  Y);
            
            return true;
        }
        internal void ClearMovesLog()
        {
            _walkerModel.ClearLog();
        }
        internal async Task  RunAlgorithmAsync()
        {
            await _walkerModel.RunAlgorithmAsync();
        }
        private void _walkerModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            
            //call OnPropertyChange with apropriate property name/names
            OnPropertyChange(e.PropertyName);
        }

        private void OnPropertyChange([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
