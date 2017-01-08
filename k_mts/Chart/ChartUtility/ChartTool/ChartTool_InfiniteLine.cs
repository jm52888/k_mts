using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{   
    public class ChartTool_InfiniteLine : ChartTool, IMouseEventHandler, IAnnotationEventHandler, IAnnotationEditor
	{
        #region Variable and Property

        ChartArea curArea;
        LineAnnotation curAnot;

		#endregion

        #region Constructor

        public ChartTool_InfiniteLine() : base("InfiniteLine") { }
        public ChartTool_InfiniteLine(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.Cursor = Cursors.Arrow;

            this.Alignment = ContentAlignment.MiddleCenter;
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

            this.IsAlwaysPainting = true;
        }

        #endregion

        #region Method

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
            _EndPlacement();
            return true;
		}

		public override void Release()
		{
			_EndPlacement();
		}

        #endregion

        #region IMouseEventHandler
        
		public bool OnMouseMove(MouseEventArgs e)
		{   
			return false;
		}

		public bool OnMouseDown(MouseEventArgs e)
		{
            Chart.Focus();

            if (e.Button == MouseButtons.Left && curAnot == null)
			{
                curArea = Common.GetCurrentArea(e.X, e.Y);                
				if (curArea == null) return true;
                                
                curAnot = _CreateAnnotation();
                curAnot.X = curAnot.AxisX.PixelPositionToValue(e.X);
                curAnot.Y = curAnot.AxisY.PixelPositionToValue(e.Y);                
                Common.Annotations.Add(curAnot);
                IsDrawing = true;
				curAnot.BeginPlacement();
            }
			return false;
		}

		public bool OnMouseUp(MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left && curAnot != null)// && !curAnot.IsValid())
            {
                //if (!curAnot.IsValid())
                //    Common.Annotations.Remove(curAnot);
                //curAnot = null;

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

            //switch (this.Alignment)
            //{
            //    case ContentAlignment.TopLeft:
            //        curAnot.Height = Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = -Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.TopCenter:
            //        curAnot.Height = Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = Math.Sign(curAnot.Width) * Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.TopRight:
            //        curAnot.Height = Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.MiddleLeft:
            //        curAnot.Height = Math.Sign(curAnot.Height) * Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = -Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.MiddleCenter:
            //        curAnot.Height = Math.Sign(curAnot.Height) * Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = Math.Sign(curAnot.Width) * Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.MiddleRight:
            //        curAnot.Height = Math.Sign(curAnot.Height) * Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.BottomLeft:
            //        curAnot.Height = -Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = -Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.BottomCenter:
            //        curAnot.Height = -Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = Math.Sign(curAnot.Width) * Math.Abs(curAnot.Width * 1000);
            //        break;
            //    case ContentAlignment.BottomRight:
            //        curAnot.Height = -Math.Abs(curAnot.Height * 1000);
            //        curAnot.Width = Math.Abs(curAnot.Width * 1000);
            //        break;                
            //}
            curAnot.LineColor = Color.Transparent;
            curAnot.ShadowColor = Color.Transparent;
            curAnot = null;
            IsDrawing = false;
		}

		public void AnnotationPaint(Annotation anot, PaintEventArgs e)
		{
            // LTRB
            var x1 = (float)anot.AxisX.ValueToPixelPosition(anot.X);
            var y1 = (float)anot.AxisY.ValueToPixelPosition(anot.Y);
            var x2 = (float)anot.AxisX.ValueToPixelPosition(anot.X + anot.Width);
            var y2 = (float)anot.AxisY.ValueToPixelPosition(anot.Y + anot.Height);

            var x_min = (float)anot.AxisX.ValueToPixelPosition(anot.AxisX.Minimum);
            var x_max = (float)anot.AxisX.ValueToPixelPosition(anot.AxisX.Maximum);
            var y_min = (float)anot.AxisY.ValueToPixelPosition(anot.AxisY.Minimum);
            var y_max = (float)anot.AxisY.ValueToPixelPosition(anot.AxisY.Maximum);

            if (!this.LabelVisible) return;

            using (Pen pen = new Pen(this.LineColor))
            {
                pen.DashStyle = anot.LineDashStyle.ToDashStyle();
                pen.Width = anot.LineWidth;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var X1 = x1;
                var Y1 = y1;
                var X2 = x2;
                var Y2 = y2;

                if (x1 == x2)
                {
                    if (y1 > y2)
                    {
                        Y2 = (float)y_max;
                    }
                    else if (y1 < y2)
                    {
                        X2 = y2;
                        Y2 = (float)y_min;
                    }
                }
                else
                {
                    double r = (y2 - y1) / (x2 - x1);
                    if (x1 < x2)
                    {
                        if (r > 0)
                        {
                            var y_x_max = y1 + r * (x_max - x1);
                            bool bCrossY = y_x_max > y_min;

                            X2 = (float)(bCrossY ? x1 + (y_min - y1) / r : x_max);
                            Y2 = (float)(bCrossY ? y_min : y_x_max);
                        }
                        else if (r < 0)
                        {
                            var y_x_max = y1 + r * (x_max - x1);
                            bool bCrossY = y_x_max < y_max;

                            X2 = (float)(bCrossY ? x1 + (y_max - y1) / r : x_max);
                            Y2 = (float)(bCrossY ? y_max : y_x_max);
                        }
                        else // r == 0
                        {
                            X2 = (float)x_max;
                        }
                    }
                    else // x1 > x2
                    {
                        if (r > 0)
                        {
                            var y_x_min = y1 + r * (x_min - x1);
                            bool bCrossY = y_x_min < y_max;

                            X2 = (float)(bCrossY ? x1 + (y_max - y1) / r : x_min);
                            Y2 = (float)(bCrossY ? y_max : y_x_min);
                        }
                        else if (r < 0)
                        {
                            var y_x_min = y1 + r * (x_min - x1);
                            bool bCrossY = y_x_min > y_min;

                            X2 = (float)(bCrossY ? x1 + (y_min - y1) / r : x_min);
                            Y2 = (float)(bCrossY ? y_min : y_x_min);
                        }
                        else // r == 0
                        {
                            X2 = (float)x_min;
                        }
                    }
                }

                var left = Math.Min(X1, X2);
                var right = Math.Max(X1, X2);
                var top = Math.Min(Y1, Y2);
                var bottom = Math.Max(Y1, Y2);

                if ((left <= x_min && x_min <= right) ||
                    (left <= x_max && x_max <= right) ||
                    (top <= y_min && y_min <= bottom) ||
                    (top <= y_max && y_max <= bottom))
                {
                    e.Graphics.IntersectClip(new RectangleF(x_min, y_min, x_max - x_min, y_max - y_min));
                    e.Graphics.DrawLine(pen, X1, Y1, X2, Y2);
                }
            }

            //using (Pen pen = new Pen(this.BackColor))
            //    e.Graphics.DrawEllipse(pen, (x1 + x2) / 2 - 3, (y1 + y2) / 2 - 3, 6, 6);

            using (Brush b = new SolidBrush(this.ForeColor))
            {
                float dx = x1 > x2 ? -this.LabelOffsetX : -this.LabelOffsetX;
                float dy = y1 > y2 ? this.LabelOffsetY : -this.LabelOffsetY - 10.0f;
                //float dx2 = x2 > x1 ? -this.LabelOffsetX : -this.LabelOffsetX;
                //float dy2 = y2 > y1 ? this.LabelOffsetY : -this.LabelOffsetY - 10.0f;

                e.Graphics.DrawString(
                    anot.Y.ToString(this.LabelFormat),
                    this.Font, b,
                    x1 + dx, y1 + dy);

                //e.Graphics.DrawString(
                //	(anot.Y + anot.Height).ToString(this.LabelFormat),
                //	this.Font, b,
                //    x2 + dx2, y2 + dy2);
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
    }
}