using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Commands.RemoveItem;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Common.Dto;
using CmsRebin.Infrastructure.Enum;
using Endpoint.WebClient.Models.Collections;
using Endpoint.WebClient.Models.Fields;
using Endpoint.WebClient.Models.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using static CmsRebin.Application.Service.Common.SMS.SMSSender;
using CmsRebin.Application.Service.Common.SMS;

namespace Endpoint.WebClient.Area.Controllers.ItemsController
{

    [Authorize]
    public class ItemController : Controller
    {
        private readonly IItemRepository _itemRepository;
        private readonly IFieldRepository _fieldRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly ILogger<ItemController> _logger;
        public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger, ICollectionRepository collectionRepository, IFieldRepository fieldRepository)
        {
            _itemRepository = itemRepository;
            _logger = logger;
            _collectionRepository = collectionRepository;
            _fieldRepository = fieldRepository;
        }
        [HttpGet]
        public IActionResult Index(int id)
        {
            //int id = Convert.ToInt32(request.TableName);

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            TempData["idItemSelectedinTable2"] = 0;


            string token = User.FindFirst("AccessToken").Value;
           

            var Tname = _collectionRepository.GetCollectionbyid(id, token);

            ViewBag.Tname = Tname.collection;

            TempData["Tid"] =id;
            TempData.Keep();



            TempData["Tname"] = Tname.collection;
            TempData.Keep();


            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();



            var items = _itemRepository.GetallItemsbyDTId(id, token);//output==ReslutGetItemsdDto
            //                                                         //var items = _itemRepository.GetallItemsbyDTId(id, token).ITM;//output==ReslutGetItemsdDto
            //TempData["Itemsha"] = items;
            //TempData.Keep();

            return View(items);


        }

        public IActionResult ShowRecord(int id)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            string token = User.FindFirst("AccessToken").Value;
            var item = _itemRepository.GetallItemsbyDTId(id, token);

            return View(item);


        }

        [HttpGet]
        public IActionResult ShowTable2Items(int id)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            string token = User.FindFirst("AccessToken").Value;
            var item = _itemRepository.GetallItemsbyDTId(id, token);

            return View(item);

        }
        public IActionResult SaveidItem(int id)
        {
            string token = User.FindFirst("AccessToken").Value;
            TempData["idItemSelectedinTable2"] = id;
            TempData.Keep();

            return Ok();
            //return RedirectToAction("InsertItem");


        }


        //[HttpPost]
        //public IActionResult SaveidItem(int id)
        //{
        //    ViewData["UsernameLogined"] = TempData["UsernameLogined"];
        //    TempData.Keep();
        //    string token = User.FindFirst("AccessToken").Value;
        //    ViewData["idItemSelectedinTable2"] = id;


        //    ViewBag.DBname = TempData["DBname"];
        //    TempData.Keep();


        //    ViewBag.Tname = TempData["Tname"];
        //    TempData.Keep();

        //    int iid = Convert.ToInt32(TempData["Tid"]); 
        //    TempData.Keep(); 

        //    var Items_Fild = _itemRepository.GetallItemsbyDTId(iid, token);//output==ReslutGetItemsdDto
        //    var temp = Items_Fild.ITM[0].fieldnamelist;
        //    //ViewData["Filds"] = new SelectList(temp).ToList().ToString();
        //    ViewData["Filds"] = temp;
        //    ViewData["Items_Fild"] = Items_Fild;

        //    return View();
        //}


        public IActionResult Filters(string Searchkey)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            //string token = User.FindFirst("AccessToken").Value;

            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();


            ViewBag.Tname = TempData["Tname"];
            TempData.Keep();

            int id = Convert.ToInt32(TempData["Tid"]);
            TempData.Keep();

            var filters = TempData["Itemsha"];
            TempData.Keep();
            // Equation filters = new Equation();
            // _fieldRepository.GetallFieldbyDTId(id,token);

            return View(filters);
        }

        public IActionResult Search(string Searchkey)
        {
            //ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            //TempData.Keep();
            //string token = User.FindFirst("AccessToken").Value;

            //string tn = TempData["DBname"].ToString();
            //string dbn = TempData["Tname"].ToString();

            //var res = _commonRepository.Search(tn, dbn, Searchkey, token);


            //////////////////////
            //ViewBag.DBname = TempData["DBname"];
            //TempData.Keep();


            //ViewBag.Tname = TempData["Tname"];
            //TempData.Keep();

            ////int idT = Convert.ToInt32(TempData["Tid"]);
            ////TempData.Keep();



            ////var Items_Fild = TempData["Itemsha"]  ;
            ////TempData.Keep();

            //int id = Convert.ToInt32(TempData["Tid"]);
            //TempData.Keep();

            //var Items_Fild = res; //output==ReslutGetItemsdDto
            //var temp = Items_Fild.ITM[0].fieldnamelist;
            ////ViewData["Filds"] = new SelectList(temp).ToList().ToString();
            //ViewData["Filds"] = temp;
            //ViewData["Items_Fild"] = Items_Fild;

            ////if (string.IsNullOrWhiteSpace(TempData["idItemSelectedinTable2"].ToString()))
            //ViewData["idItemSelectedinTable2"] = TempData["idItemSelectedinTable2"].ToString();
            ////else
            ////    ViewData["idItemSelectedinTable2"] = 0;
            //TempData.Keep();

            //return View();



            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            //Equation filters = new Equation();
            //filters.
            //string token = User.FindFirst("AccessToken").Value;
            //var customer = _itemRepository.GetallItemsbyDTId(Searchkey, token);
            //return View(customer);
            return View();

        }

        //[Area("Admin")]
        //[HttpPost]
        //public IActionResult GetItemTable(long TableId, string Collection)
        //{
        //    ViewBag.NTableFromItem = Collection;
        //    TempData["tableNamFromItem"] = Collection;
        //    TempData.Keep();

        //    var d = new ResultDto()
        //    {
        //        IsSuccess = true,
        //        Message = "",
        //    };
        //    return Json(d);
        //}
        //[Area("Admin")]
        //public IActionResult GetItems(string searchkey, int page, string Tanlenam)
        //{

        //    ViewBag.tablenameItem = TempData["tableNamFromItem"];
        //    TempData.Keep();

        //    var items = _itemRepository.GetallItemsbyDTId(Tanlenam= ViewBag.tablenameItem);

        //    return View(items);


        //}


        //[Area("Admin")]
        [HttpGet]
        public IActionResult InsertItem()
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            string token = User.FindFirst("AccessToken").Value;




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

            var Items_Fild = _itemRepository.GetallItemsbyDTId(id, token);//output==ReslutGetItemsdDto
            var temp = Items_Fild.ITM[0].fieldnamelist;
            //ViewData["Filds"] = new SelectList(temp).ToList().ToString();
            ViewData["Filds"] = temp;
            ViewData["Items_Fild"] = Items_Fild;

            //if (string.IsNullOrWhiteSpace(TempData["idItemSelectedinTable2"].ToString()))
            ViewData["idItemSelectedinTable2"] = TempData["idItemSelectedinTable2"].ToString();
            //else
            //    ViewData["idItemSelectedinTable2"] = 0;
            TempData.Keep();

            return View();
            //var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0];
            //  var itemss = _itemRepository.AddItem(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;
            // return View(new StringLsetModel { DataItems = itemss });

        }
        //[Area("Admin")]

        [HttpPost]
        public IActionResult InsertItem(RequestCreateItemdDto req)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();
            string token = User.FindFirst("AccessToken").Value;

            req.TableName = TempData["Tname"].ToString();

            TempData.Keep();
            req.DbName = TempData["DBname"].ToString();
            TempData.Keep();


            int id = Convert.ToInt32(TempData["Tid"]);
            TempData.Keep();

            for (int i=0;i<req.I.Count;i++)
            if (string.IsNullOrWhiteSpace(req.I[i]))
            {
                return Json(new ResultDto { IsSuccess = false, Message = "لطفا تمامی موارد رو ارسال نمایید" });
            }

            var Items_Fild = _itemRepository.GetallItemsbyDTId(id, token);//output==ReslutGetItemsdDto


            //var temp = Items_Fild.ITM[0].fieldnamelist;
           // temp.RemoveAt(0);
           // req.S = temp;


            List<RequestCreateItemdDto> REqs = new List<RequestCreateItemdDto>();
            RequestCreateItemdDto req1 = new RequestCreateItemdDto { I = new List<string>(), S = new List<string>(), DbName = "",TableName="" };
            for (int i =0; i < Items_Fild.ITM[0].fieldnamelist.Count; i++)
                if (Items_Fild.ITM[0].releationfiled[i] != "m-n")
                {
                    if(Items_Fild.ITM[0].fieldnamelist[i] != "id")
                    req1.S.Add(Items_Fild.ITM[0].fieldnamelist[i]);
                    //string temS = "";
                    //foreach(var val in req.I)
                    //    temS += val;
                    if(i>4)
                      req1.I.Add(req.I[i-5]);
                }
            req1.DbName = req.DbName;
            req1.TableName = req.TableName;
            // insert iteme in tabler1 and table vaset.
            //1. 
           var signeupResult= _itemRepository.AddItem(req1, token);


            //////////////////////insert Token with 1400-11-27
            //if (req.TableName == "Users")
            //{


            //    _itemRepository.Add(new RequestCreateItemdDto { DbName = req.DbName, TableName = req.TableName, });


            //}
            ////////////////////////
           



                ///Find Id of this new item. that inserted.
                //////var Items_Fild2 = _itemRepository.GetallItemsbyDTId(id, token);
                var idthisfiled = Items_Fild.ITM.Count;
            //////for (int counteritem = 0; counteritem < Items_Fild2.ITM.Count; counteritem++)
            //////    //foreach (var itm in Items_Fild2.ITM[counteritem].valuefiledlist)
            //////        if (Items_Fild2.ITM[counteritem].valuefiledlistList[5][5]==req1.I[0]) 
            //////        {
            //////            idthisfiled = counteritem; break;
            //////        }
            ////////////////////////
            for (int i = 0; i < Items_Fild.ITM[0].fieldnamelist.Count; i++)
                if (Items_Fild.ITM[0].releationfiled[i] == "m-n")
                {

                    var c = _collectionRepository.GetCollectionbyid((int)Items_Fild.ITM[0].table2id[i], token);
                    var nametable2 = c.collection;

                    List<string> splitI = new List<string>();
                    splitI = req.I[i-5].Split(',').ToList();

                    for (int ii = 0; ii < splitI.Count; ii++)
                    {
                        RequestCreateItemdDto req2 = new RequestCreateItemdDto { I = new List<string>(), S = new List<string>() { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime"}, DbName = "", TableName = "" };

                        //get name table by id;


                        req2.S.Add(Items_Fild.ITM[0].fieldnamelist[i] + "_id");
                        req2.S.Add(nametable2 + "_id");
                        //
                        

                        req2.I.Add(idthisfiled.ToString());
                        req2.I.Add(splitI[ii]);

                        req2.TableName = req.TableName + "_" + nametable2;
                        req2.DbName = req.DbName;
                        REqs.Add(req2);



                    }
                }


            // insert iteme in tabler1 and table vaset.
            //1. 
            //_itemRepository.AddItem(req1, token);  
            //2. 
            var response=new ResultCreateItemdDto();
            foreach (var R in REqs)
                response =  _itemRepository.AddItem(R, token).Result;
             
            //if (response.ItemId>0)
            //{
            //    //return RedirectToAction("Auth", "LoginAsync");

            //    return Ok(new ResultDto { IsSuccess = true, Message = "item اضافه شد" });


            //}
            //return Ok(new ResultDto { IsSuccess = false, Message = "itm اضافه نشد" });

            return RedirectToAction("Index", new { id = id });
        }


        //public IActionResult InsertItem2(RequestCreateItemdDto req)
        //{
        //    ViewData["UsernameLogined"] = TempData["UsernameLogined"];
        //    TempData.Keep();
        //    string token = User.FindFirst("AccessToken").Value;

        //    req.TableName = TempData["Tname"].ToString();

        //    TempData.Keep();
        //    req.DbName = TempData["DBname"].ToString();
        //    TempData.Keep();


        //    int id = Convert.ToInt32(TempData["Tid"]);
        //    TempData.Keep();

        //    var Items_Fild = _itemRepository.GetallItemsbyDTId(id, token);//output==ReslutGetItemsdDto
        //    var temp = Items_Fild.ITM[0].fieldnamelist;

        //    temp.RemoveAt(0);
        //    req.S = temp;

        //    _itemRepository.AddItem(req, token);

        //    return RedirectToAction("Index", new { id = id });
        //}



        //[Area("Admin")]
        //[HttpPost]
        //public IActionResult InsertItem(StringLsetModel S)
        //{
        //    //var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;
        //    ////var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = ViewBag.tablenamit, Page = 0, SearchKey = "" }).ITM[0];

        //    //ViewBag.tablenamit = TempData["tableNamFromItem"] as string;
        //    //TempData.Keep();
        //    //var result = _createItem.Execute(new RequestCreateItemdDto { I = itemss, S = S.DataItems, TableName = ViewBag.tablenamit });
        //    return View(S);
        //}


        ////////////////////////////1401-1-06
        ///
        [HttpGet]
        public IActionResult Edit(int id)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            string token = User.FindFirst("AccessToken").Value;


            ViewBag.DBname = TempData["DBname"];
            TempData.Keep();


            ViewBag.Tname = TempData["Tname"];
            TempData.Keep();

            int Tid = Convert.ToInt32(TempData["Tid"]);
            TempData.Keep();

            var Items_Fild = _itemRepository.GetallItemsbyDTId(Tid, token);//output==ReslutGetItemsdDto

            var temp = Items_Fild.ITM[0].fieldnamelist;
            //ViewData["Filds"] = new SelectList(temp).ToList().ToString();
            ViewData["Filds"] = temp;
            TempData["Filds"] = temp;
            TempData.Keep();
            ViewData["Itemsha"] = Items_Fild.ITM[id - 1].valuefiledlistList;
            //ViewData["Items_Item"] = Items_Fild.ITM[0].valuefiledlistList[id];
            ViewData["iditemSelected"] = id;
            TempData["iditemSelected"] = id; TempData.Keep();
            ViewData["Items_Fild"] = Items_Fild;
            //if (string.IsNullOrWhiteSpace(TempData["idItemSelectedinTable2"].ToString()))
            ViewData["idItemSelectedinTable2"] = TempData["idItemSelectedinTable2"].ToString();
            //else
            //    ViewData["idItemSelectedinTable2"] = 0;
            TempData.Keep();

            //GetItemsDto dt = new GetItemsDto { fieldnamelist = Items_Fild.ITM[id - 1].fieldnamelist, releationfiled = Items_Fild.ITM[id - 1].releationfiled, table2id = Items_Fild.ITM[id - 1].table2id, valuefiledlist = Items_Fild.ITM[id - 1].valuefiledlist, valuefiledlistList = Items_Fild.ITM[id - 1].valuefiledlistList };

            return View();
        }

        //[Area("Admin")]
        [HttpPost]
        public IActionResult Edit(RequestCreateItemdDto dt)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            string db = TempData["DBname"].ToString();
            TempData.Keep();

            string t = TempData["Tname"].ToString();
            TempData.Keep();
            //List<string> s = new List<string>();
            //for (int i = 0; i < dt.fieldnamelist.Count; i++)
            //    s.Add(dt.valuefiledlist[i]);
              int Tid = Convert.ToInt32(TempData["Tid"]);
            TempData.Keep();
            var Items_Fild = _itemRepository.GetallItemsbyDTId(Tid, token);//output==ReslutGetItemsdDto


            //var temp = Items_Fild.ITM[0].fieldnamelist;
            // temp.RemoveAt(0);
            // req.S = temp;
            dt.S = new List<string>();
             for (int i = 0; i < Items_Fild.ITM[0].fieldnamelist.Count; i++)
                if (Items_Fild.ITM[0].releationfiled[i] != "m-n")
                {
                    if (Items_Fild.ITM[0].fieldnamelist[i] != "id")
                        dt.S.Add(Items_Fild.ITM[0].fieldnamelist[i]);
                }



            _itemRepository.UpdateItem(new ItemdDto { DbName = db, TableName = t, ItemId = Convert.ToInt32(TempData["iditemSelected"].ToString()), I = dt.I, S = dt.S}, token);

            //_itemRepository.UpdateItem(dt,token);

            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });


        }

        /////////////////////////////

        //////////////////////////////
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{

        //    ViewData["UsernameLogined"] = TempData["UsernameLogined"];
        //    TempData.Keep();

        //    string token = User.FindFirst("AccessToken").Value;




        //    ViewBag.DBname = TempData["DBname"];
        //    TempData.Keep();


        //    ViewBag.Tname = TempData["Tname"];
        //    TempData.Keep();

        //    int Tid = Convert.ToInt32(TempData["Tid"]);
        //    TempData.Keep();

        //    var Items_Fild = _itemRepository.GetallItemsbyDTId(Tid, token);//output==ReslutGetItemsdDto

        //    var temp = Items_Fild.ITM[0].fieldnamelist;
        //    //ViewData["Filds"] = new SelectList(temp).ToList().ToString();
        //    ViewData["Filds"] = temp;
        //    ViewData["Itemsha"] = Items_Fild.ITM[id-1].valuefiledlistList;
        //    //ViewData["Items_Item"] = Items_Fild.ITM[0].valuefiledlistList[id];
        //    ViewData["iditemSelected"] = id;
        //    TempData["iditemSelected"]=id; TempData.Keep();
        //    ViewData["Items_Fild"] = Items_Fild;
        //    //if (string.IsNullOrWhiteSpace(TempData["idItemSelectedinTable2"].ToString()))
        //    ViewData["idItemSelectedinTable2"] = TempData["idItemSelectedinTable2"].ToString();
        //    //else
        //    //    ViewData["idItemSelectedinTable2"] = 0;
        //    TempData.Keep();

        //    GetItemslistDto dt = new GetItemslistDto { fieldnamelist = Items_Fild.ITM[id - 1].fieldnamelist, releationfiled = Items_Fild.ITM[id - 1].releationfiled, table2id = Items_Fild.ITM[id - 1].table2id, valuefiledlist = Items_Fild.ITM[id - 1].valuefiledlist, valuefiledlistList = Items_Fild.ITM[id - 1].valuefiledlistList };
        //    dt.valuefiledlist = new List<string>();
        //    dt.valuefiledlist.Add("");
        //    return View(dt);
        //    //return View();
        //}

        ////[Area("Admin")]
        //[HttpPost]
        //public IActionResult Edit(GetItemslistDto dt)
        //{

        //    ViewData["UsernameLogined"] = TempData["UsernameLogined"];
        //    TempData.Keep();


        //    string token = User.FindFirst("AccessToken").Value;

        //    string db = TempData["DBname"].ToString();
        //    TempData.Keep();

        //    string t = TempData["Tname"].ToString();
        //    TempData.Keep();
        //    List<string> s = new List<string>();
        //    for(int i=0;i<dt.fieldnamelist.Count;i++)
        //       s.Add( dt.valuefiledlistList[i][0]);

        //    _itemRepository.UpdateItem(new ItemdDto { DbName = db, TableName = t, ItemId = Convert.ToInt32(TempData["iditemSelected"].ToString()), I =dt.fieldnamelist,S=s }, token);

        //    //_itemRepository.UpdateItem(dt,token);

        //    int id = Convert.ToInt32(TempData["Tid"].ToString());
        //    TempData.Keep();

        //    return RedirectToAction("Index", new { id = id });


        //}

        ///////////////////////////////
        /// 


        //[HttpPut]
        //[HttpPost]
        //public IActionResult Edit(ItemdDto dt)
        //{

        //    ViewData["UsernameLogined"] = TempData["UsernameLogined"];
        //    TempData.Keep();


        //    string token = User.FindFirst("AccessToken").Value;

        //    string db = TempData["DBname"].ToString();
        //    TempData.Keep();

        //    string t = TempData["Tname"].ToString();
        //    TempData.Keep();

        //    _itemRepository.UpdateItem(dt,token);

        //    int id = Convert.ToInt32(TempData["Tid"].ToString());
        //    TempData.Keep();

        //    return RedirectToAction("Index", new { id = id });


        //}
        public IActionResult DeleteItem(int FieldId)
        {

            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();

            string token = User.FindFirst("AccessToken").Value;




            string db = TempData["DBname"].ToString();
            TempData.Keep();

            string t = TempData["Tname"].ToString();
            TempData.Keep();


            _itemRepository.DeleteItem(new itemDto { iditem=FieldId,dbname=db,tname=t}, token);

            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });
        }


        public IActionResult SendSMS(int FieldId)
        {

            //ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            //TempData.Keep();

            //string token = User.FindFirst("AccessToken").Value;

            //string db = TempData["DBname"].ToString();
            //TempData.Keep();

            //string t = TempData["Tname"].ToString();
            //TempData.Keep();


            //_itemRepository.SendSMS(new itemDto { iditem = FieldId, dbname = db, tname = t }, token);

            //int id = Convert.ToInt32(TempData["Tid"].ToString());
            //TempData.Keep();

            //return RedirectToAction("Index", new { id = id });
            return View();
        }
        [HttpPost]
        public IActionResult SendSMS(SMSSendRequest req)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            string db = TempData["DBname"].ToString();
            TempData.Keep();

            string t = TempData["Tname"].ToString();
            TempData.Keep();

             _itemRepository.SendSMS(req, token);


            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });
            
        }

        public IActionResult SendMail(int FieldId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMail(EmailSendRequest req)
        {
            ViewData["UsernameLogined"] = TempData["UsernameLogined"];
            TempData.Keep();


            string token = User.FindFirst("AccessToken").Value;

            string db = TempData["DBname"].ToString();
            TempData.Keep();

            string t = TempData["Tname"].ToString();
            TempData.Keep();

            _itemRepository.SendMail(req, token);


            int id = Convert.ToInt32(TempData["Tid"].ToString());
            TempData.Keep();

            return RedirectToAction("Index", new { id = id });

        }

    }
}
