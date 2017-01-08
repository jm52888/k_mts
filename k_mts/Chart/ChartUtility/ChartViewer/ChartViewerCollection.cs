using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartViewerCollection : ChartNamedUtilityCollection<ChartViewer>
	{
		#region Variable and Property

		private ChartViewer _DefaultViewer;
		public ChartViewer DefalutViewer { get { return _DefaultViewer; } }

		#endregion

		#region Constructor

		public ChartViewerCollection(IChartUtility parent)
			: base(parent)
		{
			
		}

		#endregion

		#region Method

		public void SetDefault(string name)
		{
			ChartViewer viewer = this.FindByName(name);
			_DefaultViewer = viewer;
		}

		#endregion

		#region Update

		public override void Update()
		{
			foreach (var area in Common.ChartAreas)
			{
				var view = area as ChartView;
				if (view == null) continue;

				ChartViewer viewer = this.FindByName(view.ChartViewer) ?? _DefaultViewer;
				if (viewer == null) continue;

				viewer.UpdateView(view);
			}
		}

		#endregion
	}
}
