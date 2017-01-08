using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public static class DataPointCollectionExtension
	{
		#region Fast Clear

		public static void ClearEx(this DataPointCollection pts)
		{
			if (pts.Count == 0) return;

			pts.SuspendUpdates();
			while (pts.Count > 0)
				pts.RemoveAt(pts.Count - 1);
			pts.ResumeUpdates();
			pts.Clear(); // To ensure chart axis updated correctly on next plot
		}

        #endregion

        #region FindNearestPoint/Index

        public static int FindNearestPointIndex(this DataPointCollection pts, PointF pt, double weightX = 1.0, double weightY = 0.0, int index = 0)
        {
            float x = pt.X;
            float y = pt.Y;
            
            int idx = -1;
            int nPoints = pts.Count;
            if (nPoints == 0) return idx;

            double dist_min = double.MaxValue;
            double dist = double.MaxValue;

            bool bIsStringOrIndexed = !string.IsNullOrEmpty(pts[0].AxisLabel);
            bool bUseXValue = weightX != 0.0;
            bool bUseYValue = weightY != 0.0;

            if (bIsStringOrIndexed)
            {
                if (bUseXValue && bUseYValue)
                {

                }
                else if (bUseXValue)
                {
                    idx =
                        Math.Max(0,
                        Math.Min(nPoints - 1,
                        (int)(pt.X - 0.5)));
                }
                else // bUseYValue
                {

                }
            }
            else
            {
                if (bUseXValue && bUseYValue)
                {
                    for (int i = 0; i < nPoints; i++)
                    {
                        dist = weightX * Math.Abs(pts[i].XValue - x) + weightY * Math.Abs(pts[i].YValues[index] - y);
                        if (dist < dist_min)
                        {
                            dist_min = dist;
                            idx = i;
                        }
                    }
                }
                else if (bUseXValue)
                {
                    for (int i = 0; i < nPoints; i++)
                    {
                        dist = Math.Abs(pts[i].XValue - x);
                        if (dist < dist_min)
                        {
                            dist_min = dist;
                            idx = i;
                        }
                    }
                }
                else // bUseYValue
                {
                    for (int i = 0; i < nPoints; i++)
                    {
                        dist = Math.Abs(pts[i].YValues[index] - y);
                        if (dist < dist_min)
                        {
                            dist_min = dist;
                            idx = i;
                        }
                    }
                }
            }
            return idx;
        }

        public static DataPoint FindNearestPoint(this DataPointCollection pts, PointF pt, double weightX = 1.0, double weightY = 0.0, int index = 0)
        {
            int idx = FindNearestPointIndex(pts, pt, weightX, weightY, index);
            if (idx < 0) return null;

            if (string.IsNullOrEmpty(pts[idx].Label))
                return new DataPoint(pts[idx].XValue, pts[idx].YValues); 
            else return new DataPoint(idx + 1, pts[idx].YValues);
        }
        
		#endregion
	}
}
