using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.Site.Models.ViewModels.AuthenticationViewModel
{
    public class SignupViewModel
    {
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
