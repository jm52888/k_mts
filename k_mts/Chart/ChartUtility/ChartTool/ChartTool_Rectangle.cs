using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_Rectangle : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        RectangleAnnotation curAnot;

        #endregion

        #region Constructor

        public ChartTool_Rectangle() : base("Rectangle") { }
        public ChartTool_Rectangle(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.AllowMoving = true;
            this.AllowResizing = true;
            this.AllowSelecting = true;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.BackColor = Color.Transparent;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;
        }        

        #endregion

        #region Method

        private void _EndPlacement()
        {
            if (curAnot != null)
            {
                curAnot.EndPlacement();
                Common.Annotations.Remove(curAnot);
            }
            curAnot = null;
        }

        private void _MakePositive(Annotation anot)
        {
            if (anot.Width < 0)
            {
                anot.X += anot.Width;
                anot.Width = -anot.Width;
            }

            if (anot.Height > 0)
            {
                anot.Y += anot.Height;
                anot.Height = -anot.Height;
            }
        }

        private void _DoMagnet(Annotation anot)
        {
            if (curArea == null) return;

            var ms = Common.GetMainSeries(curArea);
            if (ms == null) return;

            var pts = ms.Points;
            if (pts.Count == 0) return;

            PointF pt1 = new PointF((float)anot.X, (float)anot.Y);
            PointF pt2 = new PointF((float)(anot.X + anot.Width), (float)(anot.Y + anot.Height));

            var dp1 = pts.FindNearestPoint(pt1);
            var dp2 = pts.FindNearestPoint(pt2);

            if (ms.ChartType == SeriesChartType.Candlestick ||
                ms.ChartType == SeriesChartType.Stock)
            {
                double y1 = Math.Abs(dp1.YValues[0] - anot.Y) <= Math.Abs(dp1.YValues[1] - anot.Y) ? dp1.YValues[0] : dp1.YValues[1];
                double y2 = Math.Abs(dp2.YValues[0] - anot.Y - anot.Height) <= Math.Abs(dp2.YValues[1] - anot.Y - anot.Height) ? dp2.YValues[0] : dp2.YValues[1];

                anot.X = dp1.XValue;
                anot.Y = y1;
                anot.Width = dp2.XValue - dp1.XValue;
                anot.Height = y2 - y1;
            }
            else
            {
                anot.X = dp1.XValue;
                anot.Y = dp1.YValues[0];
                anot.Width = dp2.XValue - dp1.XValue;
                anot.Height = dp2.YValues[0] - dp1.YValues[0];
            }
        }        

        #endregion

        #region ChartTool

        public override bool Select()
        {
            _EndPlacement();
            return true;
        }

        public override void Release()
        {
            _EndPlacement();
        }

        #endregion

        #region IMouseEventHandler

        public bool OnMouseClick(MouseEventArgs e)
        {
            return false;
        }

        public bool OnMouseDoubleClick(MouseEventArgs e)
        {
            return true;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && curAnot == null)
            {
                Chart.Focus();

                curArea = Common.GetCurrentArea(e.X, e.Y);
                if (curArea == null) return true;

                var anot = new RectangleAnnotation();
                anot.AllowMoving = this.AllowMoving;
                anot.AllowResizing = this.AllowResizing;
                anot.AllowSelecting = this.AllowSelecting;
                anot.IsSizeAlwaysRelative = this.IsInfinitive;
                anot.LineColor = this.LineColor;
                anot.BackColor = this.BackColor;
                anot.BackSecondaryColor = this.BackSecondaryColor;
                anot.BackGradientStyle = this.BackGradientStyle;
                anot.BackHatchStyle = this.BackHatchStyle;
                anot.LineDashStyle = this.LineDashStyle;
                anot.LineWidth = this.LineWidth;

                anot.ClipToChartArea = curArea.Name;
                anot.AxisX = Common.GetMainAxisX(curArea);
                anot.AxisY = Common.GetMainAxisY(curArea);
                anot.SetAttr("ToolName", this.Name);

                curAnot = anot;
                Common.Annotations.Add(anot);
                anot.BeginPlacement();
            }
            return false;
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            return false;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && curAnot != null && !curAnot.IsValid())
            {
                curAnot.EndPlacement();
                Common.Annotations.Remove(curAnot);
                curAnot = null;
            }
            else if (e.Button == MouseButtons.Right)
            {
                Common.Tools.Release();
                return true;
            }
            return false;
        }

        public bool OnMouseWheel(MouseEventArgs e)
        {
            return true;
        }

        #endregion

        #region IAnnotationEventHandler

        public void AnnotationPaint(Annotation anot, PaintEventArgs e)
        {

        }

        public void AnnotationPlaced(Annotation anot)
        {
            if (!anot.IsValid())
            {
                Common.Annotations.Remove(anot);
                return;
            }
            curAnot = null;
        }

        public void AnnotationPositionChanged(Annotation anot)
        {
            if (UseMagnet)
                _DoMagnet(anot);

            _MakePositive(anot);
        }

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {

        }

        public void AnnotationSelectionChanged(Annotation anot)
        {

        }

        public void AnnotationTextChanged(Annotation anot)
        {

        }

        #endregion

        #region IAnnotationEditor

        public void Clear()
        {
            var anots = Common.Annotations
                .Where(a => a.GetAttr<string>("ToolName") == this.Name)
                .ToArray();

            if (anots.Length == 0) return;

            Common.Annotations.SuspendUpdates();
            foreach (var anot in anots) Remove(anot);
            Common.Annotations.ResumeUpdates();
        }

        public bool Remove(Annotation anot)
        {
            if (anot.GetAttr<string>("ToolName") != this.Name) return false;

            return Common.Annotations.Remove(anot);
        }

        public Annotation Translate(Annotation anot, double transX, double transY, bool bCopy = false)
        {
            if (!Name.Equals(anot.GetAttr("ToolName"))) return null;

            RectangleAnnotation anot2 = bCopy ? new RectangleAnnotation() : (RectangleAnnotation)anot;
            if (bCopy)
            {
                ChartMethod.Assign(anot2, anot);
                anot2.Name = Common.Annotations.NextUniqueName();
                Translate(anot2, transX, transY, false);
                Common.Annotations.Add(anot2);
                return anot2;
            }
            anot2.X += (anot.AxisX.Maximum - anot.AxisX.Minimum) * transX;
            anot2.Y += (anot.AxisY.Maximum - anot.AxisY.Minimum) * transY;            
            return anot2;
        }

        public Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false)
        {
            if (!Name.Equals(anot.GetAttr("ToolName"))) return null;

            RectangleAnnotation anot2 = bCopy ? new RectangleAnnotation() : (RectangleAnnotation)anot;
            if (bCopy)
            {
                ChartMethod.Assign(anot2, anot);
                anot2.Name = Common.Annotations.NextUniqueName();
                Reflect(anot2, direction, false);
                Common.Annotations.Add(anot2);
                return anot2;
            }

            if (direction == FlowDirection.BottomUp)
            {
                if (anot2.Height > 0)
                    anot2.Y += anot2.Height * 2;
                anot2.Height = -anot2.Height;
            }
            else if (direction == FlowDirection.TopDown)
            {
                if (anot2.Height < 0)
                    anot2.Y += anot2.Height * 2;
                anot2.Height = -anot2.Height;
            }
            else if (direction == FlowDirection.LeftToRight)
            {
                if (anot2.Width > 0)
                    anot2.X += anot2.Width * 2;
                anot2.Width = -anot2.Width;
            }
            else if (direction == FlowDirection.RightToLeft)
            {
                if (anot2.Width < 0)
                    anot2.X += anot2.Width * 2;
                anot2.Width = -anot2.Width;
            }
            _MakePositive(anot2);
            return anot2;
        }

        #endregion
    }
}
