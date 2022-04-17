using CmsRebin.Application.Service.Database.Commands;
using CmsRebin.Application.Service.Database.Queris.UploadDB;
using CmsRebin.Domain.Entities.Persons;
using Endpoint.WebClient.Models;
using Endpoint.WebClient.Models.DBs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Endpoint.WebClient.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IDBRepository _dBRepository;
        private readonly ILogger<HomeController> _logger;
        private IUserRepository _userRepository;
        public HomeController(ILogger<HomeController> logger, IDBRepository dBRepository, IUserRepository userRepository)
        {
            _dBRepository = dBRepository;
            _logger = logger;
            _userRepository = new UserRepository();
        }

        public IActionResult Index()
        {
            string token = User.FindFirst("AccessToken").Value;
            //return View(_dBRepository.GetallDBs(token));
            var d=_dBRepository.GetallDBsbyUserid(token);

            var c = User.Identity as ClaimsIdentity;
            var v = c.FindFirst(ClaimTypes.Name);
            string username = v.Value;
            TempData["UsernameLogined"] = username ;
            TempData["IDUsernameLogined"] = c.FindFirst(ClaimTypes.NameIdentifier).Value;
            ViewData["UsernameLogined"] = username ;
            ViewData["UsernameRole"] = c.FindFirst(ClaimTypes.Role).Value;
            TempData.Keep();


            return View(d); 
        }

        public IActionResult showuser()
        {
            string token = User.FindFirst("AccessToken").Value;
            return View(_dBRepository.GetallDBs(token));

        }

        //[HttpGet("{id}")]
        public IActionResult Show( int id)
        {
            string token = User.FindFirst("AccessToken").Value;
            var customer = _userRepository.Getuserbyid(id, token);
            return View(customer);


        }

        // [HttpPost]
        //public IActionResult Index([FromRoute]Users y)
        //{
        //    ViewBag.u = y;
        //    return RedirectToAction("_UserPartialview");
        //}


        /// ////////////////////////////////////////////////////  

        public IActionResult CreatDB()
        {
            string token = User.FindFirst("AccessToken").Value;
            ViewBag.Owners = new SelectList(_userRepository.Getalluser(token), "Name", "Email");
            return View();
        }
        [HttpPost]
        public IActionResult CreatDB(RequestCreateDBDto db)
        {
            string token = User.FindFirst("AccessToken").Value;

            _dBRepository.AddDB(db, token);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult UploadBD()
        {
            string token = User.FindFirst("AccessToken").Value;

            return View();
        }

        [HttpPost]
        public IActionResult UploadBD(UploadDBDto db)
        {
            string token = User.FindFirst("AccessToken").Value;
            //if (Path.GetExtension(db.fileDB.FileName) != ".mdf")
            //{
            //    ModelState.AddModelError("fileDB", "فایل با پسوند mdf بارگزاری شود");
            //}
            string filePath = "";
            //db.fileDBName = Path.GetExtension(db.fileDB.FileName).ToString();
            db.fileDBName = (db.fileDB.FileName).ToString();

            filePath = Path.Combine(Directory.GetCurrentDirectory(), "/DBS/", db.fileDBName);
            if (db.fileDB != null)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    db.fileDB.CopyTo(stream);
                }

            }
            db.UserId = 1;
            _dBRepository.UploadDB(db, token);

            return RedirectToAction("Index");
            //return Ok();
        }

        public IActionResult Delete(int DBid)
        {
            string token = User.FindFirst("AccessToken").Value;
            _dBRepository.DeletDB(DBid, token);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Edit(int id)
        {
            string token = User.FindFirst("AccessToken").Value;
            var db = _dBRepository.GetDBbyid(id, token);
            return View(db);
        }

        //[HttpPut]
        [HttpPost]
        public IActionResult Edit(RequestEditDBDto customer)
        {
            string token = User.FindFirst("AccessToken").Value;

            _dBRepository.EditDB(customer, token);
            return RedirectToAction("Index", "Home");
        }

    }
    ///////////////////////////////////////////////


    //public IActionResult Index()
    //{
    //    string token = User.FindFirst("AccessToken").Value;

    //    return View(_UserRepository.Getalluser(token));
    //}

    //public IActionResult CreatUser()
    //{
    //    return View();
    //}
    //[HttpPost]
    //public IActionResult CreatUser(GetUsersDto user)
    //{
    //    _UserRepository.Adduser(user);
    //    return RedirectToAction("Index");
    //}


    //public IActionResult Edit(int id)
    //{
    //    var customer = _UserRepository.Getuserbyid(id);
    //    return View(customer);
    //}

    //[HttpPost]
    //public IActionResult Edit(GetUsersDto customer)
    //{
    //    _UserRepository.Updateuser(customer);
    //    return RedirectToAction("Index");
    //}


    //public IActionResult Delete(int id)
    //{
    //    _dBRepository.DeletDB(id);

    //    return RedirectToAction("Index");
    //}





    //public IActionResult Privacy()
    //{
    //    return View();
    //}

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
}

