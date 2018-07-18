using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public class TeacherProfileDetails
    {
        [Required(ErrorMessage ="Experience is Required")]
        public string Experience { get; set; }
        [Required(ErrorMessage ="Description is Required")]
        public string Description { get; set; }
        [Required]
        public decimal? Rating { get; set; }
    }
   [MetadataType(typeof(TeacherProfileDetails))]
    public partial class TeacherProfile
    { }
}
