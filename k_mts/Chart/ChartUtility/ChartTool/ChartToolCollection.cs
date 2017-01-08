using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public class ChartToolCollection : ChartNamedUtilityCollection<ChartTool>
    {
        #region Variable and Property

        protected ChartTool _SelectedTool;
        public ChartTool SelectedTool { get { return _SelectedTool; } }

        private ChartTool _DefaultTool;
        public ChartTool DefaultTool { get { return _DefaultTool; } }

        #endregion

        #region Event

        public event EventHandler<ToolChangedEventArgs> ToolChanged;

        #endregion

        #region Constructor

        public ChartToolCollection(IChartUtility parent) : base(parent) { }

        #endregion

        #region Method        

        public ChartTool GetCompatiabe(Annotation anot)
        {
            var toolName = anot.GetAttr<string>("ToolName");
            if (toolName == null) return null;

            return this.FindByName(toolName);
        }

        public new void Add(ChartTool tool)
        {
            this.Remove(FindByName(tool.Name));
            base.Add(tool);
        }

        public void Select(string toolName)
        {
            ChartTool newTool = this.FindByName(toolName);
            ChartTool oldTool = _SelectedTool;

            if (oldTool == newTool) return;

            if (oldTool != null)
                oldTool.Release();

            _SelectedTool = _DefaultTool;

            if (newTool != null && newTool.Select())
                _SelectedTool = newTool;

            if (ToolChanged != null)
                ToolChanged(this, new ToolChangedEventArgs(oldTool, _SelectedTool));

            if (_SelectedTool != null)
                Chart.Cursor = _SelectedTool.Cursor;
            else Chart.Cursor = Cursors.Default;
        }

        public void Release()
        {
            this.Select(_DefaultTool == null ? null : _DefaultTool.Name);
        }

        public void SetDefault(string toolName)
        {
            ChartTool tool = this.FindByName(toolName);
            _DefaultTool = tool;

            if (_SelectedTool == null)
                _SelectedTool = _DefaultTool;
        }

        #endregion

        #region Update

        public override void Update()
        {

        }

        #endregion        
    }
}
