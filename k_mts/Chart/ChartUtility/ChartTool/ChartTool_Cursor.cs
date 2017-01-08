using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartTool_Cursor : ChartTool, IMouseEventHandler, IPaintEventHandler
	{
		#region Variable and Property

		int x0, y0;        
		bool _bLock = false;        

		#endregion

		#region Method

		private void _DoMagnet()
		{
			var area = Common.GetCurrentArea(x0, y0) as ChartView;
			if (area == null) return;

            var ms = Common.GetMainSeries(area);
            if (ms == null) return;

            var xAxis = ms.XAxisType == AxisType.Primary ? area.AxisX : area.AxisX2;
            var yAxis = ms.YAxisType == AxisType.Primary ? area.AxisY : area.AxisY2;            
            var pts = ms.Points;

            var pt0 = new PointF((float)xAxis.PixelPositionToValue(x0), (float)yAxis.PixelPositionToValue(y0));
            var pt = pts.FindNearestPoint(pt0);
            if (pt == null) return;

            area.CursorX.Position = pt.XValue;

            if (ms.ChartType == SeriesChartType.Candlestick ||
                ms.ChartType == SeriesChartType.Stock)
            {
                var yVal = yAxis.PixelPositionToValue(y0);
                if (Math.Abs(pt.YValues[0] - yVal) <= Math.Abs(pt.YValues[1] - yVal))
                    area.CursorY.Position = pt.YValues[0];
                else area.CursorY.Position = pt.YValues[1];
            }
            else area.CursorY.Position = pt.YValues[0];
		}

        private void _SetVisible(bool bVisible)
        {
            this.LabelVisible = bVisible;

            if (bVisible)
            {   
                foreach (var area in Common.ChartAreas)
                {
                    var ms = Common.GetMainSeries(area);
                    if (ms == null) continue;

                    area.CursorX.AxisType = ms.XAxisType;
                    area.CursorX.LineColor = Color.DeepSkyBlue;//his.LineColor;
                    area.CursorX.LineDashStyle = this.LineDashStyle;
                    area.CursorX.LineWidth = this.LineWidth;

                    area.CursorY.AxisType = ms.YAxisType;
                    area.CursorY.LineColor = Color.DeepSkyBlue; //this.LineColor;
                    area.CursorY.LineDashStyle = this.LineDashStyle;
                    area.CursorY.LineWidth = this.LineWidth;
                }                
            }
            else
            {   
                foreach (var area in Chart.ChartAreas)
                {
                    area.CursorX.LineColor = Color.Transparent;
                    area.CursorY.LineColor = Color.Transparent;
                }                
            }            
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
		}

        #endregion

        #region Constructor

        public ChartTool_Cursor() : base("Cursor") { }
        public ChartTool_Cursor(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            this.LabelAlignment = LabelAlignmentStyles.Right;
            this.LabelVisible = false;
            this.LabelOffsetX = 5.0f;
            this.LabelOffsetY = -5.0f;
            this.LabelFormat = "0.000";
        }

        #endregion

        #region ChartTool

        public override bool Select()
		{   
			_bLock = false;
            _SetVisible(true);
			return true;
		}

		public override void Release()
		{
            _SetVisible(false);
			Chart.Invalidate();
		}

		#endregion

		#region IMouseEventHandler

		public bool OnMouseMove(MouseEventArgs e)
		{
            //x0 = e.X;
            //y0 = e.Y;

            //if (!_bLock) _UpdateCursor();

            //return true;
            if (e.Button == MouseButtons.Left)
            {
                x0 = e.X;
                y0 = e.Y;

                if (this.LabelVisible)
                    _bLock = !_bLock;

                _SetVisible(true);

                if (_bLock && UseMagnet)
                    _DoMagnet();
                else _UpdateCursor();
            }
            return true;
        }

		public bool OnMouseDown(MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {
                x0 = e.X;
                y0 = e.Y;

                if (this.LabelVisible)
                    _bLock = !_bLock;

                _SetVisible(true);

                if (_bLock && UseMagnet)
                    _DoMagnet();
                else _UpdateCursor();
            }
            return true;
        }

		public bool OnMouseUp(MouseEventArgs e)
		{
            //if (e.Button == MouseButtons.Right)
            //{
            _SetVisible(false);

            return true;
            //}
            //return false;
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

		#region IPaintEventHandler

		public void OnPaint(PaintEventArgs e)
		{
			if (!this.LabelVisible) return;

			foreach (ChartArea area in Chart.ChartAreas)
			{
				var xCur = area.CursorX;
				var yCur = area.CursorY;
				var axisY = yCur.AxisType == AxisType.Primary ? area.AxisY : area.AxisY2;
				var axisX = xCur.AxisType == AxisType.Primary ? area.AxisX : area.AxisX2;
				
				if (this.LabelAlignment == LabelAlignmentStyles.Right)
				{
					using (Brush b = new SolidBrush(this.ForeColor))
					{
						e.Graphics.DrawString(
							yCur.Position.ToString(axisY.LabelStyle.Format),
							this.Font,
							b,
							(float)axisX.ValueToPixelPosition(axisX.Maximum) + this.LabelOffsetX,
							(float)axisY.ValueToPixelPosition(yCur.Position) + this.LabelOffsetY);
					}					
				}
				else if(this.LabelAlignment == LabelAlignmentStyles.Left)
				{
					using (Brush b = new SolidBrush(this.ForeColor))
					{
						e.Graphics.DrawString(
							yCur.Position.ToString(axisY.LabelStyle.Format),
							this.Font,
							b,
							(float)axisX.ValueToPixelPosition(axisX.Minimum) + this.LabelOffsetX,
							(float)axisY.ValueToPixelPosition(yCur.Position) + this.LabelOffsetY);
					}
				}
			}
		}

		#endregion
	}
}
