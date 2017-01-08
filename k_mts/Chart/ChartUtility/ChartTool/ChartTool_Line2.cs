using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartTool_Line2 : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor//, IPaintEventHandler
    {
        #region Variable and Property

        ChartArea curArea;
        LineAnnotation curAnot;
        LineAnnotation prevAnot;

        int x0, y0;
        Label lbl = new Label();

        #endregion

        #region Constructor and Finalizer

        public ChartTool_Line2() : base("Line2") { }
        public ChartTool_Line2(string name) : base(name) { }

        ~ChartTool_Line2()
        {
            lbl.Dispose();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.Cursor = Cursors.Arrow;

            this.AllowMoving = true;
            this.AllowResizing = true;
            this.AllowSelecting = true;
            this.IsInfinitive = false;
            this.IsSizeAlwaysRelative = false;
            this.LineColor = Color.Black;
            this.ForeColor = Color.Black;
            this.BackColor = Color.Red;
            this.LineWidth = 2;
            this.LineDashStyle = ChartDashStyle.Solid;
            this.UseMagnet = false;

            this.LabelFormat = "0.000";
            this.LabelOffsetX = 7.5f;
            this.LabelOffsetY = 7.5f;

            lbl.Visible = false;
            lbl.AutoSize = true;
            lbl.Text = "";
        }

        #endregion

        #region Method

        private void _EnableCursor(bool bEnable)
        {
            if (bEnable)
            {
                foreach (var area in Common.ChartAreas)
                {
                    var ms = Common.GetMainSeries(area);
                    if (ms == null) continue;

                    area.CursorX.AxisType = ms.XAxisType;
                    area.CursorX.LineColor = this.LineColor;
                    area.CursorX.LineDashStyle = this.LineDashStyle;
                    area.CursorX.LineWidth = this.LineWidth;

                    area.CursorY.AxisType = ms.YAxisType;
                    area.CursorY.LineColor = this.LineColor;
                    area.CursorY.LineDashStyle = this.LineDashStyle;
                    area.CursorY.LineWidth = this.LineWidth;
                }
                lbl.Parent = this.Chart;
            }
            else
            {
                foreach (var area in Chart.ChartAreas)
                {
                    area.CursorX.LineColor = Color.Transparent;
                    area.CursorY.LineColor = Color.Transparent;
                }
            }
            lbl.Visible = bEnable;
        }

        private void _UpdateCursor()
        {
            var area = Common.GetCurrentArea(x0, y0);
            if (area == null) return;

            var xCur = area.CursorX;
            var yCur = area.CursorY;

            var xAxis = xCur.AxisType == AxisType.Primary ? area.AxisX : area.AxisX2;
            var yAxis = yCur.AxisType == AxisType.Primary ? area.AxisY : area.AxisY2;

            xCur.Position = xAxis.PixelPositionToValue(x0);
            yCur.Position = yAxis.PixelPositionToValue(y0);

            try
            {
                lbl.Text = yCur.Position.ToString(this.LabelFormat);
                lbl.BackColor = this.LabelBackColor;
                lbl.ForeColor = this.ForeColor;
                lbl.Location = new Point((int)xAxis.ValueToPixelPosition(xAxis.Maximum), y0);
            }
            finally { }
        }

        private LineAnnotation _CreateAnnotation()
        {
            var anot = new LineAnnotation();
            anot.AllowMoving = this.AllowMoving;
            anot.AllowResizing = this.AllowResizing;
            anot.AllowSelecting = this.AllowSelecting;
            anot.IsInfinitive = this.IsInfinitive;
            anot.IsSizeAlwaysRelative = this.IsSizeAlwaysRelative;
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
                base.IsDrawing = false;
            }
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
            _EnableCursor(false);
            _EndPlacement();
            return true;
        }

        public override void Release()
        {
            _EnableCursor(false);
            _EndPlacement();
        }

        #endregion

        #region IMouseEventHandler

        public bool OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x0 = e.X;
                y0 = e.Y;

                _UpdateCursor();
            }
            return false;
        }

        public bool OnMouseDown(MouseEventArgs e)
        {
            Chart.Focus();

            if (e.Button == MouseButtons.Left && curAnot == null)
            {
                curArea = Common.GetCurrentArea(e.X, e.Y);
                if (curArea == null) return true;

                x0 = e.X;
                y0 = e.Y;

                _EnableCursor(true && this.LabelVisible);
                _UpdateCursor();

                curAnot = _CreateAnnotation();
                curAnot.X = curAnot.AxisX.PixelPositionToValue(e.X);
                curAnot.Y = curAnot.AxisY.PixelPositionToValue(e.Y);
                curAnot.SetAttr("prevAnot", prevAnot);
                Common.Annotations.Add(curAnot);
                IsDrawing = true;
                curAnot.BeginPlacement();
            }
            return false;
        }

        public bool OnMouseUp(MouseEventArgs e)
        {
            _EnableCursor(false);

            if (e.Button == MouseButtons.Left && curAnot != null)// && !curAnot.IsValid())
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

        public bool OnMouseClick(MouseEventArgs e)
        {
            return true;
        }

        public bool OnMouseDoubleClick(MouseEventArgs e)
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
            if (this.UseMagnet)
                _DoMagnet(anot);
        }

        public void AnnotationPlaced(Annotation anot)
        {
            if (!anot.IsValid())
            {
                Common.Annotations.Remove(anot);
                return;
            }
            else prevAnot = curAnot;

            curAnot = null;
            IsDrawing = false;
        }

        public void AnnotationPaint(Annotation anot, PaintEventArgs e)
        {
            var x = (float)anot.AxisX.ValueToPixelPosition(anot.X);
            var y = (float)anot.AxisY.ValueToPixelPosition(anot.Y);
            var x2 = (float)anot.AxisX.ValueToPixelPosition(anot.X + anot.Width);
            var y2 = (float)anot.AxisY.ValueToPixelPosition(anot.Y + anot.Height);

            using (Pen pen = new Pen(this.BackColor))
                e.Graphics.DrawEllipse(pen, (x + x2) / 2 - 3, (y + y2) / 2 - 3, 6, 6);

            if (!this.LabelVisible) return;

            if (!e.ClipRectangle.Contains((int)x, (int)y) &&
                !e.ClipRectangle.Contains((int)x2, (int)y2)) return;

            using (Brush b = new SolidBrush(this.ForeColor))
            {
                float dx = x > x2 ? -this.LabelOffsetX : -this.LabelOffsetX;
                float dy = y > y2 ? this.LabelOffsetY : -this.LabelOffsetY - 10.0f;
                float dx2 = x2 > x ? -this.LabelOffsetX : -this.LabelOffsetX;
                float dy2 = y2 > y ? this.LabelOffsetY : -this.LabelOffsetY - 10.0f;

                string msg = anot.Height.ToString(this.LabelFormat);

                var anot_1 = anot.GetAttr<Annotation>("prevAnot");
                if (anot_1 != null && anot_1.ClipToChartArea == anot.ClipToChartArea)
                {
                    double ratio = anot_1.Height == 0 ? double.NaN :
                        Math.Abs(anot.Height / anot_1.Height);
                    msg += ", " + ratio.ToString(this.LabelFormat);
                }

                e.Graphics.DrawString(
                    msg,
                    this.Font, b,
                    x2 + dx2, y2 + dy2);

                if (!this.LabelVisible) return;
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

            LineAnnotation anot2 = bCopy ? new LineAnnotation() : (LineAnnotation)anot;
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

            LineAnnotation anot2 = bCopy ? new LineAnnotation() : (LineAnnotation)anot;
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
            return anot2;
        }

        #endregion

        #region IPaintEventHandler

        public void OnPaint(PaintEventArgs e)
        {
            if (curArea == null ||
                curArea.CursorX.LineColor == Color.Transparent) return;

            var xCur = curArea.CursorX;
            var yCur = curArea.CursorY;
            var axisX = xCur.AxisType == AxisType.Primary ? curArea.AxisX : curArea.AxisX2;
            var axisY = yCur.AxisType == AxisType.Primary ? curArea.AxisY : curArea.AxisY2;

            using (Brush brush_line = new SolidBrush(Color.Yellow))// new SolidBrush(this.LineColor))
            using (Brush brush_back = new SolidBrush(this.LabelBackColor))
            {
                float x = (float)axisX.ValueToPixelPosition(axisX.Maximum) + this.LabelOffsetX;
                float y = (float)axisY.ValueToPixelPosition(yCur.Position) + this.LabelOffsetY;

                string msg = yCur.Position.ToString(axisY.LabelStyle.Format);
                var size = e.Graphics.MeasureString(msg, this.Font);

                e.Graphics.FillRectangle(brush_back, new Rectangle((int)x, (int)y, (int)size.Width, (int)size.Height));
                e.Graphics.DrawString(
                    msg,
                    this.Font,
                    brush_line,
                    x, y);
            }
        }

        #endregion
    }
}