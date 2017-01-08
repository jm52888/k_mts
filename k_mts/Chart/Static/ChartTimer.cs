using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public static class ChartTimer
	{
		[DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
		private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

		#region Internal Vars and Props

		private static DateTime _RefTime;
		private static long _RefTick;
		private static long _RefFreq;

		public static DateTime Now
		{
			get
			{
				return _RefTime.AddTicks(
						(Stopwatch.GetTimestamp() - _RefTick) * 10000000 / _RefFreq);
			}
		}

		#endregion

		#region Initialize

		static ChartTimer()
		{
			Refresh();
		}

		static DateTime Refresh()
		{
			try
			{//Windows 8 or Server 2012 or higher.
				long fileTime;
				GetSystemTimePreciseAsFileTime(out fileTime);

				_RefTick = Stopwatch.GetTimestamp();
				_RefTime = DateTime.FromFileTime(fileTime);
				_RefFreq = Stopwatch.Frequency;

				return _RefTime;
			}
			catch (EntryPointNotFoundException)
			{// GetSystemTimePreciseAsFileTime is not supported.
				_RefTime = DateTime.Now;
				_RefTick = Stopwatch.GetTimestamp();
				_RefFreq = Stopwatch.Frequency;

				return _RefTime;
			}
		}

		#endregion
	}
}
