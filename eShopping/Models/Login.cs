using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eShopping.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }

        // There is a ReturnUrl querystring or when the user is redirected to Login
        // if they try to access a page that needs authorization.
        public string ReturnUrl { get; set; }
    }
}
