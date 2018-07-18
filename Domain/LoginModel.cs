using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please Enter your email address.")]
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Required(ErrorMessage = " Please Enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
