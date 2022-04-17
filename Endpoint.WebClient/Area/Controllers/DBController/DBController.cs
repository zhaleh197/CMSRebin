using CmsRebin.Application.Service.Database.Commands;
using CmsRebin.Application.Service.Database.Queris.GetDB;
using CmsRebin.Application.Service.Database.Queris.UploadDB;
using Endpoint.WebClient.Models.DBs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net;

namespace Endpoint.WebClient.Area.Controllers.DBController
{
    [Authorize]
    public class DBController : Controller
    {
        private readonly IDBRepository _dBRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DBController> _logger;
        private readonly IHostingEnvironment Environment;
        public DBController(IDBRepository dBRepository, ILogger<DBController> logger, IUserRepository userRepository, IHostingEnvironment _environment)
        {
            _dBRepository = dBRepository;
            _logger = logger;
            _userRepository = userRepository;
            Environment = _environment;
        }


        ////// ///////////////////////////////  ADMIN 
        public IActionResult Index()
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            return View(_dBRepository.GetallDBs(token));
        }


        public IActionResult DeleteAdmin(int DBid)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _dBRepository.DeletDBAdmin(DBid, token);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult EditAdmin(int id)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            var db = _dBRepository.GetDBbyid2(id, token);
            return View(db[0]);
        }

        //[HttpPut]
        [HttpPost]
        public IActionResult EditAdmin(GetDBDto dbb)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _dBRepository.EditDBAdmin(new RequestEditDBAdminDto { Iddb = dbb.Id, NameDB = dbb.DB, IsRemove=dbb.IsRemoved }, token);
            return RedirectToAction("Index", "DB");
        }

        /// /////////////////////////////// /// /////////////////////////////// 
        public IActionResult Index2()
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            return View(_dBRepository.GetallDBsbyUserid(token));
        }

        public IActionResult CreatDB()
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            //var userid = _userRepositor.GetUsertbyToken(token);

            //TempData["OwnerID"] = d[0].Owner.id;

            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            //var r = claims.FirstOrDefault(s => s.Type == "UserId")?.Value;
            //long userid = long.Parse(r);

            var c = User.Identity as ClaimsIdentity;
            var v = c.FindFirst(ClaimTypes.NameIdentifier);
            long userid = long.Parse(v.Value);
            TempData["OwnerID"] = userid.ToString();
            ViewData["OwnerID"] = userid.ToString();
            TempData.Keep();

            return View();
        }
        [HttpPost]
        public IActionResult CreatDB(RequestCreateDBDto db)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            db.IdUserOwner =long.Parse( TempData["OwnerID"].ToString());
            TempData.Keep();
            _dBRepository.AddDB(db, token);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult UploadBD()
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            return View();
        }

        [HttpPost]
        public IActionResult UploadBD(UploadDBDto db)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


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
        //////////////////////////////

        //[HttpPost]
        //public FileResult DownloadFile([FromRoute] int id)
        //{
        //    ViewData["UsernameLogined"] = TempData["UsernameLogined"];
        //    TempData.Keep();


        //    string token = User.FindFirst("AccessToken").Value;

        //    //////download from sql in E// Creat BackUp in E.

        //    //_dBRepository.DownloadDB(id, token);//"E:\\DatabaseBackup";
        //    var c = User.Identity as ClaimsIdentity;
        //    var v = c.FindFirst(ClaimTypes.NameIdentifier);
        //    string userId = v.Value;
        //    GetDBDto req = new GetDBDto
        //    {
        //        Id = id,
        //        DB = id.ToString(),
        //        IsRemoved = false,
        //        Owner = new CmsRebin.Domain.Entities.Persons.Users { id = Convert.ToInt32(userId) }

        //    };
        //    _dBRepository.DownloadDB(req, token);//"E:\\DatabaseBackup";
        //    /////////////
        //    //string filePath1 = "E:\\DatabaseBackup" + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".Bak";
        //    string filePath1 = "E:\\DatabaseBackup" + "\\" + req.Owner.id.ToString() + req.Id + DateTime.Now.ToString("ddMMyyyy").ToString() + ".Bak";

        //    string fileName = req.Owner.id.ToString() + req.Id + DateTime.Now.ToString("ddMMyyyy").ToString() + ".Bak";
        //    //Build the File Path.
        //    //string path = Path.Combine(this.Environment.WebRootPath, "Files/") + fileName;

        //    //Read the File data into Byte Array.
        //    byte[] bytes = System.IO.File.ReadAllBytes(filePath1);

        //    //Send the File to Download.
        //    return File(bytes, "application/octet-stream", fileName);
        //}


        ///////////////////////////////////

        [HttpGet]
        public FileResult DownloadFile([FromRoute] int id)
        {




            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            //////download from sql in E// Creat BackUp in E.

            _dBRepository.DownloadDB(id, token);//"E:\\DatabaseBackup";
            /////////////
            string filePath1 = "E:\\DatabaseBackup" + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".Bak";


            /*
            //if (Path.GetExtension(db.fileDB.FileName) != ".mdf")
            //{
            //    ModelState.AddModelError("fileDB", "فایل با پسوند mdf بارگزاری شود");
            //}
            string filePath = "";

            string filePath2 = "";
            //db.fileDBName = Path.GetExtension(db.fileDB.FileName).ToString();

            //filePath = "https://192.168.1.1.:8000/C:ProgramFiles\\MicrosoftSQLServer\\MSSQL15.MSSQLSERVER\\MSSQL\\DATA\\" + dbname + ".mdf";
            //filePath2 = "https://192.168.1.1.:8000/C:ProgramFiles\\MicrosoftSQLServer\\MSSQL15.MSSQLSERVER\\MSSQL\\DATA\\" + dbname + ".ldf";
            Environment.CurrentDirectory = "C:/";
            filePath = "Program Files//Microsoft SQL Server//MSSQL15.MSSQLSERVER//MSSQL//DATA//" + dbname + "DBzhaleh.mdf";
            //filePath2 = "C:ProgramFiles\\MicrosoftSQLServer\\MSSQL15.MSSQLSERVER\\MSSQL\\DATA\\" + dbname + ".ldf";
            var mem = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Create))
                {
                stream.CopyToAsync(mem);
                }
            mem.Position = 0;
            //var ext = Path.GetExtension(filePath).ToLowerInvariant();
            File(mem, "MDF/mdf",Path.GetFileName(filePath));


            //var mem2 = new MemoryStream();
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    stream.CopyToAsync(mem2);
            //}
            //mem2.Position = 0; 

            //File(mem2, "LDF/LDF", Path.GetFileName(filePath2));
            */

            /*
            var mem = new MemoryStream();
            using (var stream = new FileStream(filePath1, FileMode.Create))
            {
                stream.CopyToAsync(mem);
            }
            mem.Position = 0;
            //var ext = Path.GetExtension(filePath).ToLowerInvariant();
            File(mem, "Bak", Path.GetFileName(filePath1));
            */


            string filename = DateTime.Now.ToString("ddMMyyyy") + ".Bak";
            //string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Path/To/File/" + filename;
            //byte[] filedata = System.IO.File.ReadAllBytes(filepath);

            //string contentType = MimeMapping.GetMimeMapping(filepath);

            //var cd = new System.Net.Mime.ContentDisposition
            //{
            //    FileName = filename,
            //    Inline = true,
            //};

            //Response.AppendHeader("Content-Disposition", cd.ToString());

            //return File(filedata, contentType);

            byte[] bytes = System.IO.File.ReadAllBytes(filePath1);

            //Send the File to Download.
             return File(bytes, "application/octet-stream", filename);


            //return Ok();
            // return RedirectToPage("https://192.168.1.1:8000/E:\\DatabaseBackup" + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".Bak");
        }


        public IActionResult Delete(int DBid)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _dBRepository.DeletDB(DBid, token);
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult Edit(int id)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            var db = _dBRepository.GetDBbyid2(id, token);
            return View(db[0]);
        }

        //[HttpPut]
        [HttpPost]
        public IActionResult Edit(GetDBDto dbb)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _dBRepository.EditDB(new RequestEditDBDto { Iddb = dbb.Id, NameDB = dbb.DB }, token);
            return RedirectToAction("Index","Home");
        }


        //public IActionResult Select(int id)
        //{
        //    string token = User.FindFirst("AccessToken").Value;

        //    return RedirectToAction("Index", "Home");
        //}

        //[HttpPost]
        //public IActionResult Select(GetDBDto dbb)
        //{
        //    string token = User.FindFirst("AccessToken").Value;

        //    //_dBRepository.SelectDB(new RequestEditDBDto { Iddb = dbb.Id, NameDB = dbb.DB }, token);
        //    return RedirectToAction("Index", "Home");
        //}



    }
}
