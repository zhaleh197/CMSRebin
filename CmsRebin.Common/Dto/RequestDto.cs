using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Common.Dto
{
   public class RequestDto
    {
        public string DbName { get; set; }
        public string TableName { get; set; }
        public string FiledName { get; set; }
        public string SearchKey { get; set; }
        public int Page { get; set; }

    }
    public class RequestDtoIDs
    {
        public long DbName { get; set; }
        public long TableName { get; set; }
        public string FiledName { get; set; }
        public string SearchKey { get; set; }
        public int Page { get; set; }

    }
}
