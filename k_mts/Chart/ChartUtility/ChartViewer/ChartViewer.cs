using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public abstract class ChartViewer : ChartNamedUtility
	{
		#region Property

		private bool _Enable = true;
		[DefaultValue(true)]
		public virtual bool Enable 
		{
			get { return _Enable; }
			set { _Enable = value; }
		}		

		#endregion

		#region Constructor

		public ChartViewer() 
		{
			this.Initialize();
		}
		public ChartViewer(string name)
			: base(name)
		{
			this.Initialize();
		}

		public virtual void Initialize() { }

		#endregion

		#region Update

		public abstract void UpdateView(ChartView view);
		public override void Update()
		{
			if (!Enable || Common == null) return;

			foreach (var area in Common.ChartAreas)
			{
				var view = area as ChartView;
				if (view == null) continue;

				if (view.ChartViewer == this.Name)
					UpdateView(view);
			}
		}

        #endregion
    }
}
