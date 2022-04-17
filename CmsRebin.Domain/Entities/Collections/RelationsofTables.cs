using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Domain.Entities.Collections
{
   public class RelationsofTables
    {
        [Key]
        public long id { get; set; }


        ////Table1
        //public string one_collection { get; set; }
        //public string one_field { get; set; }
        ////Table2
        //public string many_collection { get; set; }
        //public string many_field { get; set; }

        //Table1
        public long one_collection { get; set; }
        public long one_field { get; set; }
        //Table2
        public long many_collection { get; set; }
        public long many_field { get; set; }

        //////public virtual TypeofReleation TypeofReleation { get; set; }
        //public string TypeofReleation { get; set; }

        
        public long TypeofReleation { get; set; }


    }
}
