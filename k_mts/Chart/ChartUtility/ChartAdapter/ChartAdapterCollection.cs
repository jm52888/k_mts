using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public class ChartAdapterCollection : ChartNamedUtilityCollection<ChartAdapter>
	{
        #region Constructor

        public ChartAdapterCollection(IChartUtility parent)
			: base(parent)
		{

		}

		#endregion

		#region Update

		public override void Update()
		{	
			var ch = Chart;
			ChartAdapter ad = null;

			var ds = Common.DataSource;
			if (ds != null)
				ad = this.FindByName(ds.GetType().Name);


            List<Series> msList = new List<Series>();
            foreach (var area in ch.ChartAreas)
            {
                var ms = Common.GetMainSeries(area);
                if (ms == null) continue;
                
                msList.Add(ms);
            }

			foreach (var s in ch.Series)
			{
                if (!s.Enabled && !msList.Contains(s)) continue;

				var dataSource = s.GetAttr("DataSource");
				if (dataSource != null)
				{
					var ad2 = this.FindByName(ds.GetType().Name);
					if (ad2 != null)
						ad2.DataBind(s, s.XValueMember, s.YValueMembers);
				}
				else if (ad != null)
				{
					ad.DataBind(s, s.XValueMember, s.YValueMembers);
				}
			}
		}

		#endregion
	}
}
