using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeacherApplication.Models
{
	public class Category
	{
		public Class Class { get; set; }
		public TeacherProfile TeacherProfile { get; set; }
		public AvailableTeacherTime AvalilableTeacher { get; set; }
		public UserImage userImage { get; set; }
		public UserVideo UserVideo { get; set; }
		public  List<City> Cities{get;set;}
    }
}