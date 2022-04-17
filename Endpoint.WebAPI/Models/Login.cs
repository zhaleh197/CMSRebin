using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.WebAPI.Models
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }


    }
    public class Login2
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        //
        public string DB { get; set; }

    }
}
