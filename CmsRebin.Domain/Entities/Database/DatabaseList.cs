using CmsRebin.Domain.Entities.Collections;
using CmsRebin.Domain.Entities.Commons;
using CmsRebin.Domain.Entities.Persons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Domain.Entities.Database
{
    public class DatabaseList:BaseEntity<long>
    {
        public string DBName { get; set; }
        // public DateTime CreatDateTime { get; set; }

        /////??or
        //[ForeignKey("UserId")]
        //public long UserId { get; set; }


        //???or

        public virtual Users User { get; set; }
        //

        //public virtual Users Users { get; set; }

        //public virtual Collection<Tables>  Tables { get; set; }
    }
}
