using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Domain.Entities.Persons
{
    public class Tokens
    {
        public string Token { get; set; }
        public DateTime CreatDateTime { get; set; }
        [Key, ForeignKey("Users")]
        public long id { get; set; }
        public virtual Users Users { get; set; }


    }
}
