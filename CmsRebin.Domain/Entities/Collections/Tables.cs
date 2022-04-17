using CmsRebin.Domain.Entities.Commons;
using CmsRebin.Domain.Entities.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Domain.Entities.Collections
{
   public class Tables : BaseEntity<long>
    {
        //[Key]
        //public long id { get; set; }
        public string collection { get; set; }
        //public string icon { get; set; }
        public string note { get; set; }
        //public string display_template { get; set; }

        //public bool hidden { get; set; }
        public bool singleton { get; set; }


        [ForeignKey("DatabaseList")]
        public long IdDBase { get; set; }
        //public virtual DatabaseList DatabaseList { get; set; }

        //public virtual Collection<FieldsofTables> FieldsofTables { get; set; }


        //public string translations { get; set; }
        //public string archive_field { get; set; }
        //public bool archive_app_filter { get; set; }
        //public string archive_value { get; set; }
        //public string unarchive_value { get; set; }
        //public string sort_field { get; set; }
        //public string accountability { get; set; }
        //public string color { get; set; }
        //public string item_duplication_fields { get; set; }
    }
}
