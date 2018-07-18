using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public partial class UserEnquiry
    {
        public string UserName { get; set; }
        public string TeacherName { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }
        public string Subject { get; set; }

    }
    public  class UserProfileVal
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "EmailAddress is Required")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Adress is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "State is Required")]
        public string State { get; set; }
        [Required(ErrorMessage = "Country is Required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "ZipCode is Required")]
        public string Zipcode { get; set; }
    }
    [MetadataType(typeof(UserProfileVal))]
    public partial class UserProfile
    { }
}
