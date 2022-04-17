using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Infrastructure.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Common.Queries
{
   public interface IGetEverythings
    {
        ReslutGetDto Execute(RequestGetDto request);
        //List<object> Execute2(RequestGetDto request);
        ReslutGetItemsdDto Execute2(RequestGetDto request);
        public string getonevalueofitem2(Filterrequestonefild2 request);
        public ReslutGetItemsdDto Execute3(RequestGetDto request);
        public ReslutGetItemsdDto Execute4(RequestGetDto request);
        public string getTablename(int id, string DbName);
        public List<ReslutGetItemsdDto> DFS(ReslutGetItemsdDto table, string DBname);
        
        }
}
