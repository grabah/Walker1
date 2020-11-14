using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WalkerSimulator.Tubesheet.Models
{
    public class WalkerModel : INotifyPropertyChanged
    {
        private TubesheetModel _tubeSheet { get; }
        public const int SecAxisLenght = 24;
        public const int MainAxisLenght = 14;

        public event PropertyChangedEventHandler PropertyChanged;

        public WalkerModel(TubesheetModel tubeSheet)
        {
            _tubeSheet = tubeSheet;
            //set to default position
            SetDefaultPosition();
        }

        private AxisPosition mainAxisAngle;
        public AxisPosition MainAxisAngle { get { return mainAxisAngle; } set { mainAxisAngle = value; } }
        public int MainAxisTranslation { get; set; }

        private AxisPosition secAxisAngle;
        public AxisPosition SecAxisAngle { get { return secAxisAngle; } set { secAxisAngle = value; }}
        private int secAxisTrans;
        public int SecAxisTranslation
        {
            get { return secAxisTrans; }
            set { secAxisTrans = value; { OnPropertyChange(); } }
        }

        private bool axisLock;
        public bool MainAxisLocked { get { return axisLock; } set { axisLock = value; OnPropertyChange("MainAxisLocked"); OnPropertyChange("SecAxisLockedLocked"); } }
        public bool SecAxisLocked { get { return !MainAxisLocked; } set { MainAxisLocked = !value; } }

        public Point CenterPosition { get; set; }
        internal Point GetWorkHeadPosition()
        {
            Point result = new Point(CenterPosition.X, CenterPosition.Y);
            switch (MainAxisAngle)
            {
                case Models.AxisPosition.RightUp:
                    result.X = CenterPosition.X + (MainAxisLenght / 2 + MainAxisTranslation + SecAxisTranslation+1);
                    result.Y = CenterPosition.Y + (MainAxisLenght / 2 - SecAxisTranslation+1);
                    break;
                case AxisPosition.LeftUp:
                    result.X = CenterPosition.X - (MainAxisLenght / 2 + MainAxisTranslation + SecAxisTranslation + 1);
                    result.Y = CenterPosition.Y + (MainAxisLenght / 2 - SecAxisTranslation + 1);
                    break;
                case AxisPosition.LeftDown:
                    result.X = CenterPosition.X - (MainAxisLenght / 2 + MainAxisTranslation + SecAxisTranslation + 1);
                    result.Y = CenterPosition.Y - (MainAxisLenght / 2 - SecAxisTranslation + 1);
                    break;
                case AxisPosition.RightDown:
                    result.X = CenterPosition.X + (MainAxisLenght / 2 + MainAxisTranslation + SecAxisTranslation + 1);
                    result.Y = CenterPosition.Y - (MainAxisLenght / 2 - SecAxisTranslation + 1);
                    break;
            }
            return result;
        }
        internal Point[] GetMainAxisPincerPoints()
        {
            Point[] result = new Point[2] { new Point(CenterPosition.X, CenterPosition.Y), new Point(CenterPosition.X, CenterPosition.Y) };
            switch (MainAxisAngle)
            {
                case AxisPosition.RightUp:
                    result[0].X = CenterPosition.X - (-MainAxisTranslation + MainAxisLenght / 2 - SecAxisTranslation );
                    result[0].Y = CenterPosition.Y - (SecAxisTranslation + MainAxisLenght / 2);
                    result[1].X = result[0].X + MainAxisLenght;
                    result[1].Y = result[0].Y + MainAxisLenght;
                    break;
                case AxisPosition.LeftUp:
                    result[0].X = CenterPosition.X + (-MainAxisTranslation + MainAxisLenght / 2 - SecAxisTranslation );
                    result[0].Y = CenterPosition.Y - (SecAxisTranslation + MainAxisLenght / 2);
                    result[1].X = result[0].X - MainAxisLenght;
                    result[1].Y = result[0].Y + MainAxisLenght;
                    break;
                case AxisPosition.LeftDown:
                    result[0].X = CenterPosition.X + (-MainAxisTranslation + MainAxisLenght / 2 - SecAxisTranslation );
                    result[0].Y = CenterPosition.Y + (SecAxisTranslation + MainAxisLenght / 2);
                    result[1].X = result[0].X - MainAxisLenght;
                    result[1].Y = result[0].Y - MainAxisLenght;
                    break;
                case AxisPosition.RightDown:
                    result[0].X = CenterPosition.X - (-MainAxisTranslation + MainAxisLenght / 2 - SecAxisTranslation );
                    result[0].Y = CenterPosition.Y + (SecAxisTranslation + MainAxisLenght / 2);
                    result[1].X = result[0].X + MainAxisLenght;
                    result[1].Y = result[0].Y - MainAxisLenght;
                    break;
            }
            return result;
        }

        internal Point[] GetSecAxisPincerPoints()
        {
            Point[] result = new Point[2] { new Point(CenterPosition.X, CenterPosition.Y), new Point(CenterPosition.X, CenterPosition.Y) };

            switch (SecAxisAngle)
            {
                case AxisPosition.Up:
                    result[0].X = CenterPosition.X - SecAxisLenght / 2;
                    result[1].X = CenterPosition.X + SecAxisLenght / 2;
                    break;
                case AxisPosition.Left:
                    result[0].Y = CenterPosition.Y - SecAxisLenght / 2;
                    result[1].Y = CenterPosition.Y + SecAxisLenght / 2;
                    break;
                case AxisPosition.Down:
                    result[0].X = CenterPosition.X + SecAxisLenght / 2;
                    result[1].X = CenterPosition.X - SecAxisLenght / 2;

                    break;
                case AxisPosition.Right:
                    result[0].Y = CenterPosition.Y + SecAxisLenght / 2;
                    result[1].Y = CenterPosition.Y - SecAxisLenght / 2;
                    break;
            }
            return result;
        }

        private void SetDefaultPosition()
        {
            int x = _tubeSheet.MaxColumns / 2;
            int y = _tubeSheet.MaxRows / 2;
            CenterPosition = new Point(x, y);
            MainAxisAngle = Models.AxisPosition.RightUp;
            SecAxisAngle = Models.AxisPosition.Up;
            MainAxisTranslation = 0;
            SecAxisTranslation =0;
            MainAxisLocked = false;
        }

        internal void WalkerMoveCenter(int x,int y)
        {
             CenterPosition = new System.Drawing.Point(x, y);
            OnPropertyChange("WalkerMakeMove");
        }

        public bool WalkerMakeMove(WalkerMove move)
        {
            if (!IsMoveLegal(move))
                return false;
            switch (move.type)
            {
                case WalkerMoveType.MainAxisRotate:
                    DoMainAxisRotation();
                    break;
                case WalkerMoveType.MainAxisTranslate:
                    this.MainAxisTranslation = this.MainAxisTranslation + move.translation;
                    break;
                case WalkerMoveType.SecondaryAxisRotate:
                    DoSecAxisRotation();
                   
                    break;
                case WalkerMoveType.SecondaryAxisTranslate:
                    this.SecAxisTranslation = this.SecAxisTranslation + move.translation;
                    break;
            }
            OnPropertyChange("WalkerMakeMove");//temp
            return true;
        }

        private void DoMainAxisRotation()
        {
            int rotation;

            if (CircularRotation(MainAxisAngle, 1) == SecAxisAngle)
                rotation = -2;
            else rotation = 2;

            this.MainAxisAngle = CircularRotation(MainAxisAngle, rotation);
        }
        private void DoSecAxisRotation()
        {
            int rotation;

            if (CircularRotation(SecAxisAngle, -1) == MainAxisAngle)
                rotation = 2;
            else rotation = -2;

            this.SecAxisAngle = CircularRotation(SecAxisAngle, rotation);
        }

        private AxisPosition CircularRotation(AxisPosition axisAngle, int rotation)
        {
            int result = (int)axisAngle + rotation;
            if (result > 7)
                result = result - 8;
            if (result < 0)
                result = result + 8;
            return (AxisPosition)result;
        }

        private bool IsMoveLegal(WalkerMove move)
        {
            return true;
        }

        private void OnPropertyChange([CallerMemberName] string property=null)
        {
            if(PropertyChanged !=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
         
    }
    public enum AxisPosition
    {
        RightUp = 0,
        Up,
        RightDown,
        Right,
        LeftDown,
        Down,
        LeftUp,
        Left
    }
    public enum WalkerMoveType
    {
        MainAxisTranslate,
        SecondaryAxisTranslate,
        MainAxisRotate,
        SecondaryAxisRotate
    }
    public class WalkerMove
    {
        public WalkerMoveType type;
        public int translation;
    }
}