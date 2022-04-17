using CmsRebin.Domain.Entities.Commons;
using CmsRebin.Domain.Entities.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Domain.Entities.Persons
{
    public class Users : BaseEntity<long>
    {
        //[Key]
        //public long id { get; set; }

        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        public bool IsActive { get; set; }

        [EmailAddress(ErrorMessage = ("ایمیل معتبر وارد نمایید"))]

        public string Email { get; set; }

        public string Password { get; set; }

        public string location { get; set; }
        //public string title { get; set; }

        public virtual Role Role { get; set; }

        //public string description { get; set; }
        //public string tags { get; set; }
        //public string avatar { get; set; }
        //public string language { get; set; }
        //public string theme { get; set; }
        //public string tfa_secret { get; set; }
        //public string status { get; set; }
        ////public string token { get; set; }
        //public string last_access { get; set; }
        //public string last_page { get; set; }


        /// //////////////////////////////////
        /// 




        /// /////////////////////////////////

        //public virtual Tokens Token { get; set; }



        /////
        //public virtual DatabaseList DatabaseList { get; set; }
        public virtual ICollection<DatabaseList> DatabaseLists { get; set; }

        ///

    }
}
