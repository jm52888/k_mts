using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing.Drawing2D;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public static class ChartDashStyleExtension
    {
        public static DashStyle ToDashStyle(this ChartDashStyle style)
        {
            switch (style)
            {
                case ChartDashStyle.NotSet: return DashStyle.Solid;
                case ChartDashStyle.Dash: return DashStyle.Dash;
                case ChartDashStyle.DashDot: return DashStyle.DashDot;
                case ChartDashStyle.DashDotDot: return DashStyle.DashDotDot;
                case ChartDashStyle.Dot: return DashStyle.Dot;
                case ChartDashStyle.Solid: return DashStyle.Solid;                
            }
            return DashStyle.Solid;
        }
    }
}
