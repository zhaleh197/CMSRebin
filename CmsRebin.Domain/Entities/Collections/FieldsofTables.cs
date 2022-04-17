using CmsRebin.Domain.Entities.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Domain.Entities.Collections
{
    public class FieldsofTables : BaseEntity<long>
    {
        //[Key]
        //public long id { get; set; }

        [Display(Name = "نام")]
        //[MaxLength(100, ErrorMessage = "مقدار{0} نباید بیشتر از {1} کاراکتر باشد")]
        //public string tablename { get; set; }

        ///////
        [ForeignKey("Tables")]
        public long IdTable { get; set; }
        //public virtual Tables Tables { get; set; }
        ////////

        public string fieldname { get; set; }
        public string relation { get; set; }// type n-m . m-n . 1-m. m-1
        public string interfaces { get; set; }
        //public string options { get; set; }
        //public string display { get; set; }
        //public string display_options { get; set; }
        //public bool readonlystatus { get; set; }
        //public bool hidden { get; set; }
        //public string sort { get; set; }
        //public string width { get; set; }
        //public string group { get; set; }
        //public string translations { get; set; }
        //public string note { get; set; }


    }
}
