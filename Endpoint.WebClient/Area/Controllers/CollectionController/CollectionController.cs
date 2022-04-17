using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Collection.Commands.EditTable;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Common.Fainances.Commands.AddRequestPay;
using CmsRebin.Common.Dto;
using Endpoint.WebClient.Models.Collections;
using Endpoint.WebClient.Models.DBs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.RegularExpressions;

namespace Endpoint.WebClient.Area.Controllers.CollectionController
{
    [Authorize]
    public class CollectionController : Controller
    {

        private readonly ICollectionRepository _collectionRepository;
        private readonly IDBRepository _dBRepository;
        private readonly ILogger<CollectionController> _logger;
        public CollectionController(ICollectionRepository collectionRepository, IDBRepository dBRepository, ILogger<CollectionController> logger)
        {
            _collectionRepository = collectionRepository;
            _dBRepository = dBRepository;
            _logger = logger;
        }


        /// ///////ADMI - 1400-11-12
        public IActionResult Index()
        {

            string token = User.FindFirst("AccessToken").Value;

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            return View(_collectionRepository.GetallCollection(token));
        }
        public IActionResult EditAdmin(int id)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            var c = _collectionRepository.GetCollectionbyid(id, token);
            return View(c);
        }
        //[HttpPut]
        [HttpPost]
        public IActionResult EditAdmin(PostTableDto c)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            //string db = TempData["DBname"].ToString();
            //TempData.Keep();

            string db = c.DbName;

            _collectionRepository.UpdateCollectionAdmin(
                new RequestEdittableAdminDto
                { collection = c.collection, note = c.note, TableId = c.Id, DBname = db,isremove=c.IsRemoved },
                token);

            int id = Convert.ToInt32(TempData["DBid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });


        }
        public IActionResult DeleteAdmin(int TableId)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _collectionRepository.DeleteCollectionAdmin(TableId, token);

            int id = Convert.ToInt32(TempData["DBid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });
        } 


        /// ////////////////

        public IActionResult Index2()
        {

            string token = User.FindFirst("AccessToken").Value;

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            return View(_collectionRepository.GetallCollection(token));
        }

        [HttpGet]
        public IActionResult Select(int id)
        {
            string token = User.FindFirst("AccessToken").Value;

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            TempData["DBid"] = id;
            TempData.Keep();
            ViewBag.DBId = id;
            var dbnam = _dBRepository.GetDBbyid2(id, token);

            ViewBag.DBname = dbnam[0].DB;

            TempData["DBname"] = ViewBag.DBname;
            TempData.Keep();

            return View(_collectionRepository.GetallCollectionbyDBid(id, token));
        }
        [HttpGet]
        public IActionResult Select2(int id)
        { 
            string token = User.FindFirst("AccessToken").Value;

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            TempData["DBid"] = id;
            TempData.Keep();


            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();

            return View(_collectionRepository.GetallCollectionbyDBid(id, token));
        }



        public IActionResult CreatCollection()
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();

            return View();
        }

        [HttpPost]
        public IActionResult CreatCollection(RequestCreateTableDto table)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            int id = Convert.ToInt32(TempData["DBid"].ToString());
            TempData.Keep();
            table.DbName = TempData["DBname"].ToString();
            TempData.Keep();

            ViewData["message"] = " ";


            /////////
            string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";
            var match = Regex.Match(table.collection, emailRegex, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                //return Json(new ResultDto { IsSuccess = true, Message = "نام جدول  خودرا به درستی وارد نمایید" });
                ViewData["message"] = "!!!! نام جدول خودرا به درستی وارد نمایید";
            }

            ViewData["message"] = "";
            //////
            var res= _collectionRepository.AddCollectionAsync(table, token);
            /////
            //Json(new ResultDto { IsSuccess = true, Message = res.Result });
            ViewData["message"] = res.Result;
            ////
            return  RedirectToAction("Select2", new { id = id });
        }


        public IActionResult Edit(int id)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            var c = _collectionRepository.GetCollectionbyid(id, token);
            return View(c);
        }

        //[HttpPut]
        [HttpPost]
        public IActionResult Edit(PostTableDto c)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            string db = TempData["DBname"].ToString();
            TempData.Keep();

            _collectionRepository.UpdateCollection(
                new RequestEdittableDto
                { collection = c.collection, note = c.note, TableId = c.Id ,DBname= db },
                token);

            int id = Convert.ToInt32(TempData["DBid"].ToString());
            TempData.Keep();

            return RedirectToAction("Select2", new { id = id });


        }
        public IActionResult Delete(int TableId)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _collectionRepository.DeleteCollection(TableId, token);

            int id = Convert.ToInt32(TempData["DBid"].ToString());
            TempData.Keep();

            return RedirectToAction("Select2", new { id = id });
        }

        [HttpGet]
        public IActionResult Payment()

        {
            ViewData["db"] = TempData["DBname"].ToString();
            TempData.Keep();

            return View();
        }
        [HttpGet]
        public IActionResult Paymentcreat(string db)
        {

            string token = User.FindFirst("AccessToken").Value;

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();
            string iduser = TempData["IDUsernameLogined"].ToString();
            TempData.Keep();
            long idowner = Convert.ToInt32(iduser);
            db = TempData["DBname"].ToString();
            _collectionRepository.Payment(new RequestCreatPayDto { DBName = db, IdUserOwner = idowner }, token);
            long id =Convert.ToInt32( TempData["DBid"].ToString());
            return RedirectToAction("Select2", new { id = id });
        }

       
        //[HttpPost]
        //public IActionResult Paymentcreat()
        //{

        //    string token = User.FindFirst("AccessToken").Value;

        //    ViewData["UsernameLogined"] = TempData["UsernameLogined"];
        //    TempData.Keep();
        //    string iduser = TempData["IDUsernameLogined"].ToString();
        //    TempData.Keep();
        //    long idowner = Convert.ToInt32(iduser);
        //    string db = TempData["DBname"].ToString();
        //    TempData.Keep();
        //    _collectionRepository.Payment(new RequestCreatPayDto {DBName=db,IdUserOwner= idowner }, token);
        //    return View();
        //}



    }
}
