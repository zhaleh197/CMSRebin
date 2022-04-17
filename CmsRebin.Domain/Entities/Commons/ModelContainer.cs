using CmsRebin.Domain.Entities.Collections;

using CmsRebin.Domain.Entities.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using CmsRebin.Domain.Entities.Persons;

namespace CmsRebin.Domain.Entities.Commons
{
    public class ModelContainer
    {
        [Key]
        public long id { get; set; }
        public FieldsofTables fieldsofTables { get; set; }
        public RelationsofTables relationsofTables { get; set; }
        public Tables tables { get; set; }
        public TypeofReleation typeofReleation { get; set; }
        public TypeofField typeofField { get; set; }
        public PermitionstoActivities permitionstoActivities { get; set; }
        public Role role { get; set; }
        public Users user { get; set; }
    }
}
