using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
  public class Citymodel
	{

		public long PKCityId { get; set; }
		[Required(ErrorMessage = "please select your state")]
		public long FKStateId { get; set; }
		[Required(ErrorMessage ="please enter your city")]
		public string CityName { get; set; }
		public bool IsActive { get; set; }
	}
	[MetadataType(typeof(Citymodel))]
	public partial class city
	{
	}
}
