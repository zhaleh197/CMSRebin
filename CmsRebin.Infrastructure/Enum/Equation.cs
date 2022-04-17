using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Infrastructure.Enum
{
    public class Equation
    {
        public List<string> Filname { get; set; }
        public List<string> Value { get; set; }
        public List<string> Addcon { get; set; }
        public List<string> Compare { get; set; }

        //new
         public string Tablename { get; set; }
         public string DBname { get; set; }
        //public List<AddCondiyttion> Addcon { get; set; }
        //public List<QueryableFilterCompareEnum> Compare { get; set; }

    }

    //public enum QueryableFilterCompareEnum
    //{
    //    Equal = '=',
    //    GreaterThan = '>',
    //    LessThan = '<',
    //    GreaterThanOrEqual = '>=',
    //    LessThanOrEqual = '<=',
    //    NotEqual = '!=',
    //}
    //public enum AddCondiyttion
    //{
    //    And,
    //    oR,
    //}

}
