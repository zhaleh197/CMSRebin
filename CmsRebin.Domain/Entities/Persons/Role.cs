
using CmsRebin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Domain.Entities.Persons
{
    public class Role :BaseEntityNotId
    {
        [Key]
        public string id { get; set; }
        public string rolename { get; set; }
        public virtual ICollection<Users> user { get; set; }
    }
}
