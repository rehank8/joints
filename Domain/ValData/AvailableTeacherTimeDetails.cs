using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AvailableTeacherTimeDetails
    {
        [DataType(DataType.Date)]
        public System.DateTime FromAvailableDate { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime ToAvailableDate { get; set; }
    }
    [MetadataType(typeof(AvailableTeacherTimeDetails))]
    public partial class AvailableTeacherTime
    { }
}
