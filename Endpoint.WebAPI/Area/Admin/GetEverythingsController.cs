using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Infrastructure.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Endpoint.WebAPI.Area.Admin
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GetEverythingsController : ControllerBase
    {
        private readonly IGetEverythings _getEverythings;//get

        private readonly ILogger<GetEverythingsController> _logger;

        public GetEverythingsController(IGetEverythings getEverythings, ILogger<GetEverythingsController> logger)
        {

            _logger = logger;
            _getEverythings = getEverythings;

        }
        [Area("Admin")]
        [HttpPost]
        //[ResponseCache(Duration =60)]// 60 second this data is in server copmuter(no client). and if DB ic chanded, this is quiqlu update.
        public IActionResult GetEverythingsa([FromBody] Equation Filtrs, string Tablename, string DBname)
        {
            var result = _getEverythings.Execute(new RequestGetDto { filters = Filtrs, nametable = Tablename, DbName = DBname });


            _logger.LogInformation(" Get {0} from {1} of {1} ", Filtrs.Filname, Tablename, DBname);
            return new ObjectResult(result);


        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult GetEverythingsa2([FromBody] Equation Filtrs)
        {

            var result = _getEverythings.Execute2(new RequestGetDto { filters = Filtrs, nametable = Filtrs.Tablename, DbName = Filtrs.DBname });



            _logger.LogInformation(" Get {0} from {1} of {1} ", Filtrs.Filname, Filtrs.Tablename, Filtrs.DBname);
            return new ObjectResult(result);


        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult GetEverythingsa3([FromBody] Equation Filtrs)
        {

            var result = _getEverythings.Execute3(new RequestGetDto { filters = Filtrs, nametable = Filtrs.Tablename, DbName = Filtrs.DBname });

            _logger.LogInformation(" Get {0} from {1} of {1} ", Filtrs.Filname, Filtrs.Tablename, Filtrs.DBname);
            return new ObjectResult(result);

        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult GetEverythingsagetLists([FromBody] Equation Filtrs)
        {
            var result = new List<List<ReslutGetItemsdDto>>();

            var result1 = new List<ReslutGetItemsdDto>();

            var resultStack = new Stack<List<ReslutGetItemsdDto>>();

            var result11 = _getEverythings.Execute4(new RequestGetDto { filters = Filtrs, nametable = Filtrs.Tablename, DbName = Filtrs.DBname });
            result1.Add(result11);
            resultStack.Push(new List<ReslutGetItemsdDto> { result11 });
            result.Add(result1);
            var tep = result11;
            var level1 = _getEverythings.DFS(result11, Filtrs.DBname);

            result.Add(level1);

            resultStack.Push(level1);
            while (resultStack.Count>0)// == is empty
            {
                //for (int l = 0; l < 3; l++)
                //{
                // var result1temp = new List<ReslutGetItemsdDto>();
                if (level1 != null||level1.Count>0)
                    for (int i = 0; i < level1.Count; i++)
                    {
                        var D = _getEverythings. DFS(level1[i], Filtrs.DBname);
                        //for (int ii = 0; ii < D.Count; ii++)
                        //{

                        //}
                        if (D != null&&D.Count>0) { result.Add(D);resultStack.Push(D); }
                    }
                if (resultStack.Count == 0) break;
                level1 = resultStack.Pop();

            }

                //foreach (var t in result1.ITM[0].table2id)
                //for (int i = 0; i < tep.ITM[0].table2id.Count; i++)//deep search
                //{
                //    long t = tep.ITM[0].table2id[i];
                //    if (t != -1)
                //    {
                //        List<string> value = new List<string>();
                //        for (int v=0; v<tep.ITM[0].valuefiledlistList.Count;v++)
                //            value.Add(tep.ITM[0].valuefiledlistList[0][v]);
                //        //get table name
                //        string tname = _getEverythings.getTablename(Convert.ToInt32(t), Filtrs.DBname);
                //        //
                //        var restep = _getEverythings.Execute4(new RequestGetDto { filters = new Equation { Filname = new List<string> { result1.ITM[0].fieldnamelist[i] },Value=value }, nametable = tname, DbName = Filtrs.DBname });
                //        result1.Add(restep);
                //        //result2.Push(restep);
                //       //tep = result2.Pop();
                //    }
                //}
               // result.Add(result1);

            //}


            _logger.LogInformation(" Get {0} from {1} of {1} ", Filtrs.Filname, Filtrs.Tablename, Filtrs.DBname);
            return new ObjectResult(result);

        }
        //public List<ReslutGetItemsdDto> DFS(ReslutGetItemsdDto table, string DBname)
        //{
        //    List<ReslutGetItemsdDto> result1 = new List<ReslutGetItemsdDto>();
        //    for (int i = 0; i < table.ITM[0].table2id.Count; i++)//deep search
        //    {
        //        long t = table.ITM[0].table2id[i];
        //        if (t != -1)
        //        {
        //            List<string> value = new List<string>();
        //            for (int v = 0; v < table.ITM[0].valuefiledlistList.Count; v++)
        //                value.Add(table.ITM[0].valuefiledlistList[0][v]);
        //            //get table name
        //            string tname = _getEverythings.getTablename(Convert.ToInt32(t), DBname);
        //            //
        //            var restep = _getEverythings.Execute4(new RequestGetDto { filters = new Equation { Filname = new List<string> { table.ITM[0].fieldnamelist[i] }, Value = value }, nametable = tname, DbName = DBname });
        //            result1.Add(restep);
        //            //result2.Push(restep);
        //            //tep = result2.Pop();
        //        }
        //    }
        //    return result1;
        //}


    }

}
