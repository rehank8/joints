using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public class ClassDetails
    {
        [Required(ErrorMessage ="Class Name is Required")]
        public string ClassName { get; set; }

	

	}
    [MetadataType(typeof(ClassDetails))]
    public partial class Class
    { }
	public partial class Class
	{
		public string Experience { get; set; }
		public string Description { get; set; }
	}

}
