using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
    public static class AxisExtension
    {
        public static double PositionToPixelPosition(this Axis axis, double position)
        {
            return axis.ValueToPixelPosition(axis.PositionToValue(position));
        }
    }
}
