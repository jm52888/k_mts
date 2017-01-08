using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms.DataVisualization.Charting
{
	public static class ChartExceptionFactory
	{
        public static ArgumentException NameAlreadyExists(string name, string collection)
		{
			return new ArgumentException(string.Format(
				"{0}은 {1}에 이미 등록된 이름입니다.",
				name, collection));
		}
		public static ArgumentException NameNotFound(string name, string collection)
		{
			return new ArgumentException(string.Format(
				"{0}은 {1}에 등록되지 않은 이름입니다.",
				name, collection));
		}
	}
}
