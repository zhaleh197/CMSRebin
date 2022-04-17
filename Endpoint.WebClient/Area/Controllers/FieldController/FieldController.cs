using CmsRebin.Application.Service.Filed.Commands.AddField;
using Endpoint.WebClient.Models.Collections;
using Endpoint.WebClient.Models.Fields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Endpoint.WebClient.Area.Controllers.FieldController
{
    [Authorize]
    public class FieldController : Controller
    {
        private readonly IFieldRepository _fieldRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly ILogger<FieldController> _logger;
        public FieldController(IFieldRepository fieldRepository, ILogger<FieldController> logger, ICollectionRepository collectionRepository)
        {
            _collectionRepository = collectionRepository;
            _fieldRepository = fieldRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;


            var Tname = _collectionRepository.GetCollectionbyid(id, token);

            ViewBag.Tname = Tname.collection;

            TempData["Tid"] = id;
            TempData.Keep();

           

            TempData["Tname"] = Tname.collection;
            TempData.Keep();


            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();

            var F= _fieldRepository.GetallFieldbyDTId(id, token);

            return View(F);
        }

        [HttpGet]
        public IActionResult GetAllfild()
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;


            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();

            var F = _fieldRepository.GetallFieldinallDB(token);

            return View(F);
        }

        //[HttpGet]
        //    public IActionResult CreateField()
        //    {
        //        string token = User.FindFirst("AccessToken").Value;

        //        var listTables = _fieldRepository.GetTables2andfilds(token);
        //        var listtypefile = _fieldRepository.GetTypeField(token);
        //        var listtyperelatin = _fieldRepository.GetTypeRelations(token);

        //        //List<string> s = new List<string>();
        //        //for (int i = 0; i < listTables.Count; i++)
        //        //{
        //        //    s.Add(listTables[i].collection);
        //        //}
        //        ViewData["Typ"] = new SelectList(listtypefile);
        //        //ViewBag.typefile = listtypefile;
        //        //ViewBag.typerelation = listtyperelatin;

        //        ViewData["typefile"] = new SelectList(listtypefile);
        //        ViewData["typerelation"]= new SelectList(listtyperelatin);

        //        List<string> table2liast = new List<string>();
        //        List<List<string>> filedofthisTable = new List<List<string>>();

        //        foreach(var tf in listTables)
        //{
        //            table2liast.Add(tf.TName);
        //            filedofthisTable.Add(tf.Filds);
        //        }

        //        //ViewBag.listTabl = listTables;
        //        //ViewBag.Table2list = table2liast;
        //        //ViewBag.FildsofAllTable = filedofthisTable;

        //        ViewData["listTabl "] = new SelectList(listTables);
        //        ViewData["Table2list"] = new SelectList(table2liast);
        //        ViewData["FildsofAllTable"] = new SelectList( filedofthisTable);

        //        ////////////////////////////////////
        //        ///

        //        ViewBag.Tid = TempData["Tid"];
        //        TempData.Keep();

        //        ViewBag.Tname = TempData["Tname"];
        //        TempData.Keep();

        //        ViewBag.DBname = TempData["DBname"];
        //        TempData.Keep();
        //        return View();
        //    }

        //    [HttpPost]
        //    public IActionResult CreateField(RequestCreateFieldDto Fild)
        //    {
        //        string token = User.FindFirst("AccessToken").Value;



        //        Fild.DbName = TempData["DBname"].ToString();
        //        TempData.Keep();
        //        Fild.tablename = TempData["Tname"].ToString();
        //        TempData.Keep();
        //        _fieldRepository.AddField(Fild, token);


        //        int id = Convert.ToInt32(TempData["Tid"].ToString());
        //        TempData.Keep();

        //        return RedirectToAction("Index", new { id = id });
        //    }



        /// <summary>
        /// ////////

        public JsonResult fild_Bind(string id)
        {


            string token = User.FindFirst("AccessToken").Value;
            var F = _fieldRepository.GetallFieldbyDTId(Convert.ToInt32( id), token).ToList();



            List<SelectListItem> statelist = new List<SelectListItem>();
            foreach (var dr in F)
            {
               
                statelist.Add(new SelectListItem { Text = dr.fieldname.ToString(), Value = dr.Id.ToString() });
            }
            //return Json(statelist, System.Web.Mvc.JsonRequestBehavior.AllowGet);

            return Json(statelist);

            ////return Json(new { data = statelist });
            //return new JsonResult(new { data = statelist });
            //return Json(F, System.Web.Mvc.JsonRequestBehavior.AllowGet);

        }

        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult ADDField()
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            int dbid = (int) TempData["DBid"];
            TempData.Keep();
            var table2 = _collectionRepository.GetallCollectionbyDBid(dbid, token);

            //var listTables = _fieldRepository.GetTables2andfilds(token);
            var listtypefile = _fieldRepository.GetTypeField(token);
            var listtyperelatin = _fieldRepository.GetTypeRelations(token);

            //List<string> s = new List<string>();
            //for (int i = 0; i < listTables.Count; i++)
            //{
            //    s.Add(listTables[i].collection);
            //}
            ViewData["Typ"] = new SelectList(listtypefile);
            //ViewBag.typefile = listtypefile;
            //ViewBag.typerelation = listtyperelatin;

            ViewData["typefile"] = new SelectList(listtypefile);
            ViewData["typerelation"] = new SelectList(listtyperelatin);

            //List<string> table2liast = new List<string>();
            //List<List<string>> filedofthisTable = new List<List<string>>();
            string tn = TempData["Tname"].ToString();
            TempData.Keep();
            List<SelectListItem> Tlist = new List<SelectListItem>();

            foreach (var tf in table2)
            {
                if(tf.collection!=tn)
                    Tlist.Add(new SelectListItem { Text = tf.collection.ToString(), Value = tf.Id.ToString() });
                //table2liast.Add(tf.TName);
                //filedofthisTable.Add(tf.Filds);
            }

            //ViewBag.listTabl = listTables;
            //ViewBag.Table2list = table2liast;
            //ViewBag.FildsofAllTable = filedofthisTable;

            ViewData["Table2list"] = Tlist;
          
            //ViewData["Table2list"] = new SelectList(table2liast);
            //ViewData["FildsofAllTable"] = new SelectList(filedofthisTable);

            ////////////////////////////////////
            ///

            ViewBag.Tid = TempData["Tid"];
            TempData.Keep();

            ViewBag.Tname = TempData["Tname"];
            TempData.Keep();

            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public IActionResult ADDField(RequestCreateFieldDto Fild)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;



            Fild.DbName = TempData["DBname"].ToString();
            TempData.Keep();
            Fild.tablename = TempData["Tname"].ToString();
            TempData.Keep();
            _fieldRepository.AddField(Fild, token);


            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });
        }

        public IActionResult Edit(int id)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;
            var c = _fieldRepository.GetFieldbyid(id, token);
            return View(c);
        }

        //[HttpPut]
        [HttpPost]
        public IActionResult Edit(FiledDto c)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            string db = TempData["DBname"].ToString();
            TempData.Keep();

            _fieldRepository.UpdateField(
                new FiledDto
                { tablename=c.tablename,
                DbName=c.DbName,
                table2=c.table2,
                forignkey=c.forignkey,
                name=c.name,
                Relation=c.Relation,
                Nullable=c.Nullable,
                type=c.type,
                Uniqe=c.Uniqe
                },
                token);

            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });


        }
        public IActionResult Delete(int FieldId)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _fieldRepository.DeleteField(FieldId, token);

            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });
        }


        public IActionResult DeleteAdmin(int FieldId)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            _fieldRepository.DeleteFieldAdmin(FieldId, token);

            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });
        }

    }
}
