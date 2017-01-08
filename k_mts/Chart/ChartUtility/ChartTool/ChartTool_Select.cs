using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartTool_Select : ChartTool, IMouseEventHandler, IKeyboardEventHandler
	{
        #region Constructor

        public ChartTool_Select() : base("Select") { }
        public ChartTool_Select(string name) : base(name) { }

        protected override void Initialize()
        {
            base.Initialize();
            this.Cursor = Cursors.Default;
        }

        #endregion

        #region ChartTool

        public override bool Select()
		{
			return true;
		}

		public override void Release()
		{
			return;
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
			return false;
		}

		public bool OnMouseUp(MouseEventArgs e)
		{
			return false;
		}

		public bool OnMouseWheel(MouseEventArgs e)
		{
			return false;
		}

		public bool OnMouseClick(MouseEventArgs e)
		{
			return false;
		}

		public bool OnMouseDoubleClick(MouseEventArgs e)
		{
			return false;
		}

        #endregion

        #region IKeyboardEventHandler

        public bool OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var anots = Common.Annotations.Where(a => a.IsSelected).ToArray();
                foreach (var anot in anots)
                {
                    string toolName = anot.GetAttr<string>("ToolName");
                    if (toolName == null) continue;

                    var h = Common.Tools.FindByName(toolName) as IAnnotationEditor;
                    if (h != null) h.Remove(anot);
                    else Common.Annotations.Remove(anot); 
                }
                this.Cursor = Cursors.Default;
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
