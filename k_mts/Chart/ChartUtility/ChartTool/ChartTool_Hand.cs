using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartTool_Hand : ChartTool, IMouseEventHandler, IKeyboardEventHandler
	{
		#region Variable and Property

		int x0, y0, x1, y1;
		int viewStart = 0;
		ChartView curView = null;

        [DefaultValue(1.0)]        
		public double DragSpeed { get; set; }

        #endregion

        #region Method

        private void _UpdateView()
        {
            if (curView == null) return;

            var axisX = Common.GetMainAxisX(curView);
            if (axisX == null) return;

            double x_ratio = curView.ViewCount /
                (axisX.ValueToPixelPosition(axisX.Maximum) - axisX.ValueToPixelPosition(axisX.Minimum));

            curView.ViewStart =
                Math.Max(0,
                (int)(viewStart + (x0 - x1) * x_ratio * DragSpeed));

            ((ChartEx)Chart).DataBind();
        }

        #endregion

        #region Constructor

        public ChartTool_Hand() : base("Hand") { }
        public ChartTool_Hand(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();

            DragSpeed = 1.0;
            Cursor = Cursors.Hand;
        }
        
		#endregion

		#region ChartTool

		public override bool Select()
		{   
			if (!(Chart is ChartEx)) return false;
			
			curView = null;
			return true;
		}

		public override void Release()
		{
			curView = null;
		}

		#endregion

		#region IMouseEventHandler

		public bool OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{   
				x1 = e.X;
				y1 = e.Y;

				_UpdateView();
			}
			return true;
		}

		public bool OnMouseDown(MouseEventArgs e)
		{
			Chart.Focus();

			if (e.Button == MouseButtons.Left)
			{
				curView = Common.GetCurrentArea(e.X, e.Y) as ChartView;
				if (curView == null) return false;
                
				viewStart = curView.ViewStart;

				x0 = x1 = e.X;
				y0 = y1 = e.Y;

				return true;
			}			
			return false;
		}

		public bool OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				x1 = e.X;
				y1 = e.Y;

				_UpdateView();
			}
			else if (e.Button == MouseButtons.Right)
			{
				Common.Tools.Release();				
			}
			return true;
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

		#region IKeyboardEventHandler

		public bool OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left)
			{
				if (curView == null) return true;

                curView.ViewStart -= 1;
				((ChartEx)Chart).DataBind();
			}
			else if (e.KeyCode == Keys.Right)
			{
				if (curView == null) return true;

                curView.ViewStart += 1;
				((ChartEx)Chart).DataBind();
			}
			return true;
		}

		public bool OnKeyUp(KeyEventArgs e)
		{
			return true;
		}

		public bool OnKeyPress(KeyPressEventArgs e)
		{
			return true;
		}

		#endregion
	}
}
