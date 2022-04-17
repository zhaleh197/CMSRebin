using Endpoint.WebClient.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.WebClient.Controllers
{
   
    public class SearchController : Controller
    {
        private readonly ICommonRepository _commonRepository;
        private readonly ILogger<SearchController> _logger;
        public SearchController(ILogger<SearchController> logger, ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
            _logger = logger;

        }
   
        public IActionResult Index(string SearchKey)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();
            string token = User.FindFirst("AccessToken").Value;

            string dbn = TempData["DBname"].ToString();
            TempData.Keep();
            string tn = "";

            if (TempData["Tname"] == null)
            {
                tn = "Tables";
                TempData.Keep();
            }
            else
            {
                tn = TempData["Tname"].ToString();
                TempData.Keep();
            }

            var res = _commonRepository.Search( dbn,tn, SearchKey, token).Result;

            //var res = _commonRepository.Search(dbn, tn, SearchKey, token);
            ////////////////////
            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();


            ViewBag.Tname = TempData["Tname"];
            TempData.Keep();

            //int idT = Convert.ToInt32(TempData["Tid"]);
            //TempData.Keep();



            //var Items_Fild = TempData["Itemsha"]  ;
            //TempData.Keep();

            int id = Convert.ToInt32(TempData["Tid"]);
            TempData.Keep();
          
            var Items_Fild = res; //output==ReslutGetItemsdDto
            var temp = Items_Fild.ITM[0].fieldnamelist;
            //ViewData["Filds"] = new SelectList(temp).ToList().ToString();
            ViewData["Filds"] = temp;
            ViewData["Items_Fild"] = Items_Fild;

            //if (string.IsNullOrWhiteSpace(TempData["idItemSelectedinTable2"].ToString()))
            ViewData["idItemSelectedinTable2"] = TempData["idItemSelectedinTable2"].ToString();
            //else
            //    ViewData["idItemSelectedinTable2"] = 0;
            TempData.Keep();

            return View(res);
            /////////////////////
            ///
            // return RedirectToAction(RolbackPath);

            //return View(res);
        }


        public IActionResult View(string SearchKey)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();
            string token = User.FindFirst("AccessToken").Value;

            string dbn = TempData["DBname"].ToString();
            TempData.Keep();
            string tn = "";

            if (TempData["Tname"] == null)
            {
                tn = "Tables";
                TempData.Keep();
            }
            else
            {
                tn = TempData["Tname"].ToString();
                TempData.Keep();
            }

            var res = _commonRepository.Search(dbn, tn, SearchKey, token).Result;

            //var res = _commonRepository.Search(dbn, tn, SearchKey, token);
            ////////////////////
            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();


            ViewBag.Tname = TempData["Tname"];
            TempData.Keep();

            //int idT = Convert.ToInt32(TempData["Tid"]);
            //TempData.Keep();



            //var Items_Fild = TempData["Itemsha"]  ;
            //TempData.Keep();

            int id = Convert.ToInt32(TempData["Tid"]);
            TempData.Keep();

            var Items_Fild = res; //output==ReslutGetItemsdDto
            var temp = Items_Fild.ITM[0].fieldnamelist;
            //ViewData["Filds"] = new SelectList(temp).ToList().ToString();
            ViewData["Filds"] = temp;
            ViewData["Items_Fild"] = Items_Fild;

            //if (string.IsNullOrWhiteSpace(TempData["idItemSelectedinTable2"].ToString()))
            ViewData["idItemSelectedinTable2"] = TempData["idItemSelectedinTable2"].ToString();
            //else
            //    ViewData["idItemSelectedinTable2"] = 0;
            TempData.Keep();

            return View(res);
            /////////////////////
            ///
            // return RedirectToAction(RolbackPath);

            //return View(res);
        }
    }
}


//public IActionResult Index()
//{
//    string token = User.FindFirst("AccessToken").Value;
//    //return View(_dBRepository.GetallDBs(token));
//    var d = _dBRepository.GetallDBsbyUserid(token);

//    var c = User.Identity as ClaimsIdentity;
//    var v = c.FindFirst(ClaimTypes.Name);
//    string username = v.Value;
//    TempData["UsernameLogined"] = username;
//    ViewData["UsernameLogined"] = username;
//    TempData.Keep();


//    return View(d);
//}

//public IActionResult showuser()
//{
//    string token = User.FindFirst("AccessToken").Value;
//    return View(_dBRepository.GetallDBs(token));

//}


