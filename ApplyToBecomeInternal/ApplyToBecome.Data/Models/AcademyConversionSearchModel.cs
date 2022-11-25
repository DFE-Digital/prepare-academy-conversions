using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplyToBecome.Data.Models
{
	public class AcademyConversionSearchModel
	{
		public int Page { get; set; }
		public int Count { get; set; }
		public string TitleFilter { get; set; }
		public IEnumerable<string> DeliveryOfficerQueryString { get; set; }
		public IEnumerable<int> RegionUrnsQueryString { get; set; }
		public IEnumerable<string> StatusQueryString { get; set; }
	}
}

