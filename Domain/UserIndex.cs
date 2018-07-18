using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UserIndex
    {
        public long PKUserId { get; set; }
		[Required(ErrorMessage = "Please  enter Teacher Name")]
		public string TeacherName { get; set; }

		[Required(ErrorMessage = "Please Select Picture")]
		public string PhotoUrl { get; set; }

		[Required(ErrorMessage = "Please  enter Business Name")]
		public string ClassName { get; set; }

		[Required(ErrorMessage = "Please  enter class Name")]
		public string SubjectName { get; set; }

		[Required(ErrorMessage = "Please  enter Experiance")]
		public string Experience { get; set; }

		[Required(ErrorMessage = "Please  enter description")]
		public string Description { get; set; }


		[Required(ErrorMessage = "Please  select from date")]
		[DataType(DataType.Date)]
        public string AvailableDate { get; set; }

		[Required(ErrorMessage = "Please Select Time")]
		[DataType(DataType.Time)]
        public string AvailableTime { get; set; }

		[Required(ErrorMessage = "Please  Select to Date")]
		[DataType(DataType.Date)]
        public string ToDate { get; set; }

		[Required(ErrorMessage = "Please  Select To time")]
		[DataType(DataType.Time)]
        public string ToTime { get; set; }

		[Required(ErrorMessage = "Please  enter Location")]
		public string Location { get; set; }
        public decimal Rating { get; set; }
        public long Teacherid { get; set; }
        public long PKAvailableTeacherId { get; set; }

		[Required(ErrorMessage = "Please  enter emialID")]
		public string EmailId { get; set; }
        public long FKRoleId { get; set; }
        public long CityId { get; set; }
        public long StateId { get; set; }
        public long ClassId { get; set; }
		[Required(ErrorMessage ="Please  enter city Name")]
        public string CityName { get; set; }
    }
}
