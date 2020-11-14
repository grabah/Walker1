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
        public const int MainAxisMaxTranslate = 11;
        public const int SecAxisMaxTranslate = 6;


        public event PropertyChangedEventHandler PropertyChanged;

        public WalkerModel(TubesheetModel tubeSheet)
        {
            _tubeSheet = tubeSheet;
            //set to default position
            SetDefaultPosition();
        }

        public AxisPosition MainAxisAngle { get; set; }
        public int MainAxisTranslation { get; set; }
        public AxisPosition SecAxisAngle { get; set; }
        public int SecAxisTranslation { get;set; }
        private bool axisLock;
        public bool MainAxisLocked { get { return axisLock; } set { axisLock = value; OnPropertyChange("MainAxisLocked"); OnPropertyChange("SecAxisLockedLocked"); } }
        public bool SecAxisLocked { get { return !MainAxisLocked; } set { MainAxisLocked = !value; } }

        public Point RotationCenterPosition { get; set; }
 
        internal Point GetWorkHeadPosition()
        {
            Point result = GetMainAxisPincerPoints()[1];
            switch (MainAxisAngle)
            {
                case Models.AxisPosition.RightUp:
                    result.X = result.X + 1;
                    result.Y = result.Y + 1;
                    break;
                case AxisPosition.LeftUp:
                    result.X = result.X - 1;
                    result.Y = result.Y + 1;
                    break;
                case AxisPosition.LeftDown:
                    result.X = result.X - 1;
                    result.Y = result.Y - 1;
                    break;
                case AxisPosition.RightDown:
                    result.X = result.X + 1;
                    result.Y = result.Y - 1;
                    break;
            }
            return result;
        }
        internal Point[] GetMainAxisPincerPoints()
        {
            Point[] result = new Point[2] { new Point( ), new Point( ) };
            switch (MainAxisAngle)
            {
                case AxisPosition.RightUp:
                    result[0].X = RotationCenterPosition.X - (MainAxisLenght / 2 + SecAxisTranslation);
                    result[0].Y = RotationCenterPosition.Y - (MainAxisLenght / 2 + SecAxisTranslation );
                    result[1].X = result[0].X + MainAxisLenght;
                    result[1].Y = result[0].Y + MainAxisLenght;
                    break;
                case AxisPosition.LeftUp:
                    result[0].X = RotationCenterPosition.X + (MainAxisLenght / 2 + SecAxisTranslation);
                    result[0].Y = RotationCenterPosition.Y - (MainAxisLenght / 2 + SecAxisTranslation);
                    result[1].X = result[0].X - MainAxisLenght;
                    result[1].Y = result[0].Y + MainAxisLenght;
                    break;
                case AxisPosition.LeftDown:
                    result[0].X = RotationCenterPosition.X + (MainAxisLenght / 2 + SecAxisTranslation);
                    result[0].Y = RotationCenterPosition.Y + (MainAxisLenght / 2 + SecAxisTranslation);
                    result[1].X = result[0].X - MainAxisLenght;
                    result[1].Y = result[0].Y - MainAxisLenght;
                    break;
                case AxisPosition.RightDown:
                    result[0].X = RotationCenterPosition.X - (MainAxisLenght / 2 + SecAxisTranslation);
                    result[0].Y = RotationCenterPosition.Y + (MainAxisLenght / 2 + SecAxisTranslation);
                    result[1].X = result[0].X + MainAxisLenght;
                    result[1].Y = result[0].Y - MainAxisLenght;
                    break;
            }
            return result;
        }
         

        internal Point[] GetSecAxisPincerPoints()
        {
            Point[] result = new Point[2] { new Point(RotationCenterPosition.X, RotationCenterPosition.Y), new Point(RotationCenterPosition.X, RotationCenterPosition.Y) };

            switch (SecAxisAngle)
            {
                case AxisPosition.Right:
                    result[0].X = RotationCenterPosition.X - (SecAxisLenght / 2 + MainAxisTranslation);
                    result[1].X = result[0].X + SecAxisLenght;
                    break;
                case AxisPosition.Up:
                    result[0].Y = RotationCenterPosition.Y - (SecAxisLenght / 2 + MainAxisTranslation);
                    result[1].Y = result[0].Y + SecAxisLenght;
                    break;
                case AxisPosition.Left:
                    result[0].X = RotationCenterPosition.X + (SecAxisLenght / 2 + MainAxisTranslation);
                    result[1].X = result[0].X - SecAxisLenght;
                    break;
                case AxisPosition.Down:
                    result[0].Y = RotationCenterPosition.Y + (SecAxisLenght / 2 + MainAxisTranslation);
                    result[1].Y = result[0].Y - SecAxisLenght;
                    break;
            }
            return result;
        }

        private void SetDefaultPosition()
        {
            int x = _tubeSheet.MaxColumns / 2;
            int y = _tubeSheet.MaxRows / 2;
            RotationCenterPosition = new Point(x, y);
            MainAxisAngle = Models.AxisPosition.RightUp;
            SecAxisAngle = Models.AxisPosition.Right;
            MainAxisTranslation = 0;
            SecAxisTranslation =0;
            MainAxisLocked = false;
        }

        internal void WalkerMoveCenter(int x,int y)
        {
            RotationCenterPosition = new System.Drawing.Point(x, y);
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
                    DoMainAxisTranslation(move.translation);
                    break;
                case WalkerMoveType.SecondaryAxisRotate:
                    DoSecAxisRotation();
                    break;
                case WalkerMoveType.SecondaryAxisTranslate:
                    DoSecAxisTranslation(move.translation);
                    
                    break;
            }
            if (IsPointOutsideOfSheet(GetWorkHeadPosition()))
            {
                OnPropertyChange("WalkerMakeMove");//temp
                UndoMove(move);
                return false;
            }
            OnPropertyChange("WalkerMakeMove");//temp
            return true;
        }

       
        private void DoSecAxisTranslation(int translation)
        {
            MainAxisLocked = true;
            if (SecAxisTranslation + translation > SecAxisMaxTranslate) //over the maximum translation
                translation = SecAxisMaxTranslate - SecAxisTranslation; //just set it to max

            if (SecAxisTranslation + translation < -SecAxisMaxTranslate) //over the maximum translation
                translation = -(-SecAxisMaxTranslate - SecAxisTranslation); //just set it to -max

            this.SecAxisTranslation = this.SecAxisTranslation + translation;
            RecalculateCenterForAxisTranslation(MainAxisAngle, translation);
        }

        private void DoMainAxisTranslation(int translation)
        {
            SecAxisLocked = true;
            if (MainAxisTranslation + translation > MainAxisMaxTranslate) //over the maximum translation
                translation = MainAxisMaxTranslate - MainAxisTranslation; //just set it to max

            if (MainAxisTranslation + translation < -MainAxisMaxTranslate) //over the maximum translation
                translation = -(-MainAxisMaxTranslate - MainAxisTranslation); //just set it to max

            this.MainAxisTranslation = this.MainAxisTranslation + translation;
            RecalculateCenterForAxisTranslation(SecAxisAngle,translation);
        }
 
        private void RecalculateCenterForAxisTranslation(AxisPosition pos, int t)
        {
            Point result = new Point(RotationCenterPosition.X,RotationCenterPosition.Y);
            if(pos== AxisPosition.LeftUp || pos== AxisPosition.Up || pos== AxisPosition.RightUp)
            {
                result.Y += t;
            }
            if (pos == AxisPosition.Down || pos == AxisPosition.RightDown || pos == AxisPosition.LeftDown)
            {
                result.Y -= t;
            }
            if (pos == AxisPosition.Right || pos == AxisPosition.RightDown || pos == AxisPosition.RightUp)
            {
                result.X += t;
            }
            if (pos == AxisPosition.Left || pos == AxisPosition.LeftUp || pos == AxisPosition.LeftDown)
            {
                result.X -= t;
            }
            RotationCenterPosition = result;
        }

        private void DoMainAxisRotation()
        {
            SecAxisLocked = true;
            int rotation;

            if (CircularRotation(MainAxisAngle, 1) == SecAxisAngle)
                rotation = -2;
            else rotation = 2;

            this.MainAxisAngle = CircularRotation(MainAxisAngle, rotation);
        }
        private void DoSecAxisRotation()
        {
            MainAxisLocked = true;
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

        

        private void OnPropertyChange([CallerMemberName] string property=null)
        {
            if(PropertyChanged !=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        //MovingChecks
        private void UndoMove(WalkerMove move)
        {
            move.translation = -move.translation;
            WalkerMakeMove(move);
        }

        private bool IsPointOutsideOfSheet(Point pt)
        {
            if (pt.X > _tubeSheet.MaxColumns || pt.X < 0)
                return true;
            if (pt.Y > _tubeSheet.MaxRows || pt.Y < 0)
                return true; ;
            return false;
        }

        private bool IsMoveLegal(WalkerMove move)
        {
            if (move.type == WalkerMoveType.MainAxisRotate || move.type == WalkerMoveType.MainAxisTranslate)
            {
                if (CanPincersBeLocked(GetSecAxisPincerPoints()))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (CanPincersBeLocked(GetMainAxisPincerPoints()))
                {
                    return true;
                }
                return false;
            }
        }

        private bool CanPincersBeLocked(Point[] points)
        {
            if (IsPointOutsideOfSheet(points[0]) || IsPointOutsideOfSheet(points[1]))
                return false;

            return _tubeSheet.Tubes[points[0].Y, points[0].X].CanPincersLock() && _tubeSheet.Tubes[points[1].Y, points[1].X].CanPincersLock();
        }
    }
    public enum AxisPosition
    {
        RightUp = 0,
        Right,
        RightDown,
        Down,
        LeftDown,
        Left,
        LeftUp,
        Up
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