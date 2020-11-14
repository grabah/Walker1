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
    /// Interaction logic for WalkerCtontrol.xaml
    /// </summary>
    public partial class WalkerView : UserControl
    {
        bool mouseDrag;
        Point mousDragLastPoint;
        Shape DragTarget;
        WalkerVM walkerVM { get { return ((WalkerVM)this.DataContext); } }
        public WalkerView()
        {
            InitializeComponent();
        }

        private bool MakeTheMove(Point newPoint)
        {
             if (DragTarget == CenterHead)
               return walkerVM.WalkerMoveCenter(newPoint);
            if (DragTarget == MainAxis)
                return walkerVM.SlideMainAxis(newPoint,mousDragLastPoint);
            if (DragTarget == SecAxis)
                return walkerVM.SlideSecAxis(newPoint, mousDragLastPoint);
            return false;
        }
        private void MouseDrag()
        {
            Point newPoint = Mouse.GetPosition(WalkerCanvas);
            if (IsMoveSignificant(newPoint))
            {
                if (MakeTheMove(newPoint))
                {//there was a move
                    mousDragLastPoint = newPoint;
                }
            }
            PositionDragHead(newPoint);
        }
        private bool IsMoveSignificant(Point newPoint)
        {
            if (Math.Abs(newPoint.X - mousDragLastPoint.X)+
                Math.Abs(newPoint.Y - mousDragLastPoint.Y) > 5)
                return true;
            return false;
        }

        private void WalkerCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDrag && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                 MouseDrag();
            }
            else
            {
                mouseDrag = false;//security
                mousDragLastPoint = Mouse.GetPosition(WalkerCanvas);
            }
        }

        private void CenterHead_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() != typeof(Shape))
                StartMouseDrag((Shape)sender);
        }

        private void WalkerCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseDrag)
                EndMouseDrag();
        }

        private void EndMouseDrag()
        {
            mouseDrag = false;
            WalkerCanvas.ReleaseMouseCapture();
            HideDragPoint();
        }
 
        private void StartMouseDrag(Shape target)
        {
            if (!mouseDrag)
            {
                mouseDrag = true;
                Mouse.Capture(WalkerCanvas);
                mousDragLastPoint = Mouse.GetPosition(WalkerCanvas);
                ShowDragPoint();
                DragTarget = target;
            }
            else
            {
                //something went wrong with the mouse mouseDrag allready in effect
            }
        }

        private void HideDragPoint()
        {
            DragHead.Visibility = Visibility.Collapsed;
            
        }
        private void ShowDragPoint()
        {
             DragHead.Height = walkerVM.Pitch * 2;
             DragHead.Width = walkerVM.Pitch * 2;
            DragHead.Visibility = Visibility.Visible;
            PositionDragHead(mousDragLastPoint);
        }
        private void PositionDragHead(Point newPoint)
        {
            Canvas.SetLeft(DragHead, newPoint.X - walkerVM.Pitch);
            Canvas.SetTop(DragHead, newPoint.Y - walkerVM.Pitch);
          
        }
        private void Shape_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void Shape_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        //this is tmp 
        public void ResetLayout()
        {
            WalkerVM tmp = walkerVM;
            this.DataContext = null;
            this.DataContext = tmp;
        }

        public void WalkerVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ResetLayout();
        }
    }
}
