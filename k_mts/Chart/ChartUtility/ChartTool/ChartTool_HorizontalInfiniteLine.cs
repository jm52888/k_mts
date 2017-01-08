using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_HorizontalInfiniteLine : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
    {
        #region Variable and Property

        ChartArea curArea;
        HorizontalLineAnnotation curAnot;

        #endregion

        #region Constructor

        public ChartTool_HorizontalInfiniteLine() : base("HorizontalInfiniteLine") { }
        public ChartTool_HorizontalInfiniteLine(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.AllowMoving = true;
            this.AllowResizing = false;
            this.AllowSelecting = false;
            this.IsInfinitive = true;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;

            this.LabelOffsetX = 5.0f;
            this.LabelOffsetY = -5.0f;
            this.LabelFormat = "0.00";
        }        

        #endregion

        #region Method

        private HorizontalLineAnnotation _CreateAnnotation()
        {
            var anot = new HorizontalLineAnnotation();
            anot.AllowMoving = this.AllowMoving;
            anot.AllowResizing = this.AllowResizing;
            anot.AllowSelecting = this.AllowSelecting;
            anot.IsInfinitive = this.IsInfinitive;
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
            if (curAnot == null) return;
            
            curAnot.EndPlacement();
            Common.Annotations.Remove(curAnot);            
            curAnot = null;
        }

        private void _DoMagnet(Annotation anot)
        {
            var area = Common.ChartAreas.FindByName(anot.ClipToChartArea);
            if (area == null) return;

            var ms = Common.GetMainSeries(area);
            if (ms == null) return;

            var pts = ms.Points;
            if (pts.Count == 0) return;
            
            var dp = pts.FindNearestPoint(new PointF((float)anot.X, (float)anot.Y));

            if (ms.ChartType == SeriesChartType.Candlestick ||
                ms.ChartType == SeriesChartType.Stock)
            {
                anot.X = dp.XValue;
                anot.Y = Math.Abs(dp.YValues[0] - anot.Y) <= Math.Abs(dp.YValues[1] - anot.Y) ? dp.YValues[0] : dp.YValues[1];
            }
            else
            {
                anot.X = dp.XValue;
                anot.Y = dp.YValues[0];
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
            if (e.Button == MouseButtons.Left && curAnot == null)
            {
                Chart.Focus();

                curArea = Common.GetCurrentArea(e.X, e.Y);
                if (curArea == null) return true;
                
                curAnot = _CreateAnnotation();
                curAnot.X = curAnot.AxisX.PixelPositionToValue(e.X);
                curAnot.Y = curAnot.AxisY.PixelPositionToValue(e.Y);
                Chart.Annotations.Add(curAnot);
                curAnot.BeginPlacement();
            }
            return false;
        }

        public bool OnMouseMove(MouseEventArgs e)
        {
            if (curAnot != null)
                curAnot.Y = curAnot.AxisY.PixelPositionToValue(e.Y);

            return false;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
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

        public void AnnotationTextChanged(Annotation anot)
        {
            
        }

        public void AnnotationSelectionChanged(Annotation anot)
        {
            
        }

        public void AnnotationPositionChanging(AnnotationPositionChangingEventArgs e)
        {
            
        }

        public void AnnotationPositionChanged(Annotation anot)
        {
            
        }

        public void AnnotationPlaced(Annotation anot)
        {
            curAnot = null;
        }

        public void AnnotationPaint(Annotation anot, PaintEventArgs e)
        {
            var x = (float)anot.AxisX.ValueToPixelPosition(anot.AxisX.Maximum) + this.LabelOffsetX;
            var y = (float)anot.AxisY.ValueToPixelPosition(anot.Y) + this.LabelOffsetY;

            using (Brush b = new SolidBrush(this.ForeColor))
            {
                e.Graphics.DrawString(
                    anot.Y.ToString(this.LabelFormat),
                    this.Font, b,
                    x, y);
            }
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

            if (transY == 0.0) return null;

            LineAnnotation line = new LineAnnotation();
            ChartMethod.Assign(line, anot);
            
            line.Y += (anot.AxisY.Maximum - anot.AxisY.Minimum) * transY;
            line.SetAttr("ToolName", this.Name);
            line.Name = Common.Annotations.NextUniqueName();

            Common.Annotations.Add(line);
            return line;
        }

        public Annotation Reflect(Annotation anot, FlowDirection direction, bool bCopy = false)
        {
            return null;
        }

        #endregion
    }
}
