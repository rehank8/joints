using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
   public class TeacherTimeDetails
    {
        public long UserId { get; set; }
        public long ClassId { get; set; }
        public long TeacherId { get; set; }
        public long AvailableTeacherTimeId { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
        [DataType(DataType.Date)]
        public string FromAvailableDate { get; set; }
        [DataType(DataType.Date)]
        public string ToAvailableDate { get; set; }
        [DataType(DataType.Time)]
        public string FromAvailableTime { get; set; }
        [DataType(DataType.Time)]
        public string ToAvailableTime { get; set; }
        public decimal Rating { get; set; }
        public string TeacherImageUrl { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string Description { get; set; }
    }
}
