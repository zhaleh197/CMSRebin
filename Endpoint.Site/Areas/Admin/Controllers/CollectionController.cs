using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsRebin.Application.Service.Filed.Queries.Get;
using Newtonsoft.Json.Linq;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using System.Web.Helpers;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    public class CollectionController : Controller
    {
        private readonly IGetItemsService _getItemsService;
        private readonly IPostTable _postTable;
        private readonly IGetFiledsService _getFiledsService;
        private readonly ICreateItem _createItem;

        public CollectionController(ICreateItem createItem, IGetItemsService getItemsService, IPostTable postTable, IGetFiledsService getFiledsService)
        {
            _getItemsService = getItemsService;
            _postTable = postTable;
            _getFiledsService = getFiledsService;
            _createItem = createItem;
        }

       [Area("Admin")]
        public IActionResult Index(string searchkey, int page)
        {
            return View(_postTable.Execute(new RequestDto { Page = page, SearchKey = searchkey, }));
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult GetItemTable( long TableId, string Collection)
        {
            ViewBag.NTableFromItem = Collection;
            TempData["tableNamFromItem"] = Collection;
            TempData.Keep();

            var d = new ResultDto()
            {
                IsSuccess = true,
                Message = "",
            };
            return Json(d);
        }
        [Area("Admin")]
        public IActionResult GetItems(string searchkey, int page, string Tanlenam)
        {

            ViewBag.tablenameItem = TempData["tableNamFromItem"];
            TempData.Keep();

            var items = _getItemsService.Execute(new RequestGetItemsdDto { Page = page, SearchKey = searchkey, nametable = ViewBag.tablenameItem });
            // var filesd = _getFiledsService.Execute(new RequestGetFiledDto { Page = page, SearchKey = searchkey, nametable = ViewBag.tablenameItem });
            //List<GetItemsDto> listitems = new List<GetItemsDto>();
            //JObject json = JObject.FromObject(items.ITM[0]);
            //foreach (JProperty property in json.Properties())
            //    listitems.Add(new GetItemsDto { fieldname = property.Name, valuefiled = property.Value.ToString() });



            //ViewBag.items = listitems;
           // ViewBag.filesd = filesd;
            return View(items);


        }

        ///////////////////////////////////////////////////////////////////////////OK
        [Area("Admin")]
        public IActionResult CreateItem()
        {
            // reteun list oo fileds tis table from FiledsofTable table.

            ViewBag.tablenameItem = TempData["tableNamFromItem"];
            TempData.Keep();

            return View();
        }

      

        [Area("Admin")]
        [HttpPost]
        public IActionResult CreateItem(string c, string n, bool t)

        {
            var req = new RequestGetItemsdDto
            {
                nametable=c,
                Page=0,
                SearchKey="",

            };
            TempData["tableName"] = req.nametable;
            TempData.Keep();

            var itemss = _getItemsService.Execute(req);
            return View(itemss);
        }

        ///////////////////////////////////////////////////////////////////////////OK
        [Area("Admin")]
        public IActionResult AddItem()
        {
            ViewBag.tablenamit = TempData["tableNamFromItem"] as string;
            TempData.Keep();

             //var itemss = _getItemsService.Execute(new RequestGetItemsdDto {nametable= ViewBag.tablenamit,Page=0,SearchKey="" }).ITM[0].fieldnamelist;
            var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0];

            return View(itemss);
        }

        //[Area("Admin")]
        //[HttpPost]
        ////[Route("AddFiles")]
        //public IActionResult AddItem(List<string> S)
        //{
        //    var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;

        //    ViewBag.tablenamit = TempData["tableNamFromItem"] as string;
        //    TempData.Keep();
        //    var result = _createItem.Execute(new RequestCreateItemdDto { I = itemss, S = S, TableName = ViewBag.tablenamit });
        //    return Json(result);
        //}

        [Area("Admin")]
        [HttpPost]
        //[Route("AddFiles")]
        public IActionResult AddItem(string name, string price)
        {

            var result = "";
            ViewBag.tablenamit = TempData["tableNamFromItem"] as string;
            TempData.Keep();

            return Json(new ResultDto { IsSuccess = true, Message = "" });
        }
        ///////////////////////////////////////////////////////////////////////////


        [Area("Admin")]
        public IActionResult InsertItem()
        {
            ViewBag.tablenamit = TempData["tableNamFromItem"] as string;
            TempData.Keep();

            //var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0];
            var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;
            return View(new StringLsetModel { DataItems =  itemss });
            //return View();
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult InsertItem(StringLsetModel S)
        {
            var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;
            //var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0];

            ViewBag.tablenamit = TempData["tableNamFromItem"] as string;
            TempData.Keep();
            var result = _createItem.Execute(new RequestCreateItemdDto { I =  itemss, S = S.DataItems, TableName = ViewBag.tablenamit });
            return View(S); 
        }
        ///////////////////////////////////////////////////////////////////////////
        //[Area("Admin")]
        //public IActionResult InsertItemGet()
        //{
        //    return View();
        //}
        
        [Area("Admin")]
        [HttpPost]
        public IActionResult InsertItemGet(StringLsetModel S,string Tablename)
        {
            var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;

            //var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0];

            ViewBag.tablenamit = TempData["tableNamFromItem"] as string;
            TempData.Keep();
            var result = _createItem.Execute(new RequestCreateItemdDto { I =  itemss, S = S.DataItems, TableName = Tablename });
            return View(S);
            //return Json(result);
        }


    }
}
