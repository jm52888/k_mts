using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_Triangle : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        LineAnnotation curAnot;

        #endregion

        #region Constructor

        public ChartTool_Triangle() : base("Triangle") { }
        public ChartTool_Triangle(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.AllowMoving = true;
            this.AllowResizing = true;
            this.AllowSelecting = true;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.BackColor = Color.Red;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;
            this.LabelFormat = "0.000";
        }        

        #endregion

        #region Method

        private LineAnnotation _CreateAnnotaion()
        {
            var anot = new LineAnnotation();
            anot.AllowMoving = this.AllowMoving;
            anot.AllowResizing = this.AllowResizing;
            anot.AllowSelecting = this.AllowSelecting;
            anot.IsSizeAlwaysRelative = this.IsInfinitive;
            anot.LineColor = this.LineColor;
            anot.LineDashStyle = this.LineDashStyle;
            anot.LineWidth = this.LineWidth;

            anot.ClipToChartArea = curArea.Name;
            anot.AxisX = Common.GetMainAxisX(curArea);
            anot.AxisY = Common.GetMainAxisY(curArea);
            anot.SetAttr("ToolName", this.Name);

            return anot;
        }

        private void _EndPlacement()
        {
            if (curAnot != null)
            {
                curAnot.EndPlacement();
                Common.Annotations.Remove(curAnot);
            }
            curAnot = null;
            curArea = null;
        }

        private void _DoMagnet(Annotation anot)
        {
            if (curArea == null) return;

            var ms = Common.GetMainSeries(curArea);
            if (ms == null) return;

            var xAxis = ms.XAxisType == AxisType.Primary ? curArea.AxisX : curArea.AxisX2;
            var yAxis = ms.YAxisType == AxisType.Primary ? curArea.AxisY : curArea.AxisY2;
            var pts = ms.Points;
            if (pts.Count == 0) return;

            double x_01 = anot.X;
            double y_01 = anot.Y;
            double x_02 = anot.X + anot.Width;
            double y_02 = anot.Y + anot.Height;
            double x_min = anot.AxisX.Minimum;
            double x_max = anot.AxisX.Maximum;

            bool bCandleStock =
                    ms.ChartType == SeriesChartType.Candlestick ||
                    ms.ChartType == SeriesChartType.Stock;
            bool bFirstInRange = x_min <= x_01 && x_01 <= x_max;
            bool bSecondInRange = x_min <= x_02 && x_02 <= x_max;

            if (bFirstInRange && bSecondInRange)
            {
                var pt0 = pts.FindNearestPoint(new PointF((float)anot.X, 0));
                var pt1 = pts.FindNearestPoint(new PointF((float)(anot.X + anot.Width), 0));
                if (pt0 == null || pt1 == null) return;

                anot.X = pt0.XValue;
                anot.Y = bCandleStock ?
                    (Math.Abs(pt0.YValues[0] - y_01) < Math.Abs(pt0.YValues[1] - y_01) ?
                    pt0.YValues[0] : pt0.YValues[1])
                    : pt0.YValues[0];
                anot.Width = pt1.XValue - pt0.XValue;
                anot.Height = bCandleStock ?
                    ((Math.Abs(pt1.YValues[0] - y_02) < Math.Abs(pt1.YValues[1] - y_02) ?
                    pt1.YValues[0] : pt1.YValues[1]) - anot.Y)
                    : pt1.YValues[0] - anot.Y;
            }
            else if (bFirstInRange)
            {
                var pt0 = pts.FindNearestPoint(new PointF((float)anot.X, 0));
                if (pt0 == null) return;

                anot.X = pt0.XValue;
                anot.Y = bCandleStock ?
                    (Math.Abs(pt0.YValues[0] - y_01) < Math.Abs(pt0.YValues[1] - y_01) ?
                    pt0.YValues[0] : pt0.YValues[1])
                    : pt0.YValues[0];
                anot.Width += anot.X - x_01;
                anot.Height += anot.Y - y_01;

            }
            else if (bSecondInRange)
            {
                var pt1 = pts.FindNearestPoint(new PointF((float)(anot.X + anot.Width), 0));
                if (pt1 == null) return;

                anot.Width = pt1.XValue - x_01;
                anot.Height = bCandleStock ?
                    ((Math.Abs(pt1.YValues[0] - y_02) < Math.Abs(pt1.YValues[1] - y_02) ?
                    pt1.YValues[0] : pt1.YValues[1]) - y_01)
                    : pt1.YValues[0] - y_01;
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
            return true;
        }

        public bool OnMouseDoubleClick(MouseEventArgs e)
        {
            return true;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return false;

            Chart.Focus();

            curArea = Common.GetCurrentArea(e.X, e.Y);
            if (curArea == null) return true;

            if (curAnot == null)
            {
                curAnot = _CreateAnnotaion();
                Common.Annotations.Add(curAnot);
                curAnot.BeginPlacement();
            }
            else
            {
                var xAxis = Common.GetMainAxisX(curArea);
                var yAxis = Common.GetMainAxisY(curArea);
                
                double x1 = curAnot.X;
                double y1 = curAnot.Y;
                double x2 = curAnot.X + curAnot.Width;
                double y2 = curAnot.Y + curAnot.Height;
                double x3 = xAxis.PixelPositionToValue(e.X);
                double y3 = yAxis.PixelPositionToValue(e.Y);

                var anot2 = _CreateAnnotaion();
                anot2.X = x2;
                anot2.Y = y2;
                anot2.Width = x3 - x2;
                anot2.Height = y3 - y2;
                if (this.UseMagnet) _DoMagnet(anot2);

                var anot3 = _CreateAnnotaion();
                anot3.X = x3;
                anot3.Y = y3;
                anot3.Width = x1 - x3;
                anot3.Height = y1 - y3;

                Common.Annotations.Add(anot2);
                Common.Annotations.Add(anot3);

                if (this.UseMagnet)
                {
                    _DoMagnet(anot2);
                    _DoMagnet(anot3);
                }                
                curAnot.AllowResizing = anot2.AllowResizing = anot3.AllowResizing = false;
                List<Annotation> lst = new List<Annotation>();
                lst.Add(curAnot);
                lst.Add(anot2);
                lst.Add(anot3);                
                lst.ForEach(a => a.SetAttr("AnotList", lst));
                curAnot = null;
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
                Common.Annotations.Remove(curAnot);
                curAnot = null;               
                return;
            }            
        }

        public void AnnotationPositionChanged(Annotation anot)
        {
            if (this.UseMagnet)
                _DoMagnet(anot);
        }

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {
            var anot = e.Annotation as LineAnnotation;
            var lst = anot.GetAttr<List<Annotation>>("AnotList");
            if (lst == null) return;

            double dx = e.NewLocationX - anot.X;
            double dy = e.NewLocationY - anot.Y;

            lst.ForEach(a =>
            {
                if (a == anot) return;
                a.X += dx;
                a.Y += dy;
            });
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
                .Where(a => this.Name.Equals(a.GetAttr("ToolName")))
                .ToArray();

            if (anots.Length == 0) return;

            Common.Annotations.SuspendUpdates();
            foreach (var anot in anots) Remove(anot);
            Common.Annotations.ResumeUpdates();
        }

        public bool Remove(Annotation anot)
        {
            if (anot.GetAttr<string>("ToolName") != this.Name ||
                !Common.Annotations.Remove(anot)) return false;
            
            var lst = anot.GetAttr<List<Annotation>>("AnotList");
            if (lst != null) lst.ForEach(a =>
            {   
                a.SetAttr("AnotList", null);
                Remove(a);
            });
            return true;
        }

        public Annotation Translate(Annotation anot, double transX, double transY, bool bCopy = false)
        {
            return null;
        }
        
        public Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false)
        {
            return null;
        }

        #endregion
    }
}
