using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Collection.Commands.EditItem;
using CmsRebin.Application.Service.Collection.Commands.EditTable;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Collection.Commands.RemoveItem;
using CmsRebin.Application.Service.Collection.Commands.RemoveTable;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Application.Service.Common.SMS;
using CmsRebin.Application.Service.Database.Queris.GetDB;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Application.Service.Filed.Commands.EditField;
using CmsRebin.Application.Service.Filed.Commands.RemoveField;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.WebAPI.Area.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class CollectionController : ControllerBase
    {
        //////////14001019
        private readonly IElasticClient _elasticClient;
        ///////////
        private readonly IPostTable _postTable;
        private readonly ICreatTable _creatTable;
        private readonly IRemoveTableService _removeTableService;
        private readonly IEditTableService _editTableService;

        private readonly IGetFiledsService _getFiledsService;
        private readonly ICreateFiled _createFiled;
        private readonly IEditedField _editedField;
        private readonly IRemoveField _removeField;

        private readonly IGetItemsService _getItemsService;
        private readonly ICreateItem _createItem;
        private readonly IEditItemService _editItemService;
        private readonly IRemoveItem _removeItem;
        private readonly ILogger<CollectionController> _logger;

        private readonly IGetDB _getDB;
        //14001221
        private readonly ISMSSender _iSMSSender;
        private readonly IGetEverythings _getEverythings;
        //

        public CollectionController
            (
            IPostTable postTable, ICreatTable creatTable, IRemoveTableService removeTableService,
            IEditTableService editTableService, IGetFiledsService getFiledsService, ICreateFiled createFiled,
            IGetItemsService getItemsService, ICreateItem createItem,
            IEditedField editedField, IRemoveField removeField,
            IEditItemService editItemService,
            IRemoveItem removeItem,
            ILogger<CollectionController> logger,
            IElasticClient elasticClient,
            IGetEverythings getEverythings,
            IGetDB getDB,
            ISMSSender iSMSSender
            )
        {
            ///////
            ///
            _elasticClient = elasticClient;
            ////
            _logger = logger;
            _postTable = postTable;
            _creatTable = creatTable;
            _removeTableService = removeTableService;
            _editTableService = editTableService;
            _getFiledsService = getFiledsService;
            _createFiled = createFiled;
            _getItemsService = getItemsService;
            _createItem = createItem;
            _editedField = editedField;
            _removeField = removeField;
            _editItemService = editItemService;
            _removeItem = removeItem;
            _getEverythings = getEverythings;
             _iSMSSender=  iSMSSender;
            _getDB= getDB;
        }

        /////////////////////////////////////////////////////// Table

        [Area("Admin")]
        [HttpGet]
        public IActionResult GetAllCollection(string dbn, int page, string searchkey)
        {
            //return new ObjectResult(_postTable.Execute(new RequestDto { Page = page, SearchKey = searchkey,DbName=dbn }));
            var result = new ObjectResult(_postTable.Executeall().TableDtos);

            _logger.LogInformation("get collection of {0}", dbn);
            return result;
        }

        /////////////////////////////////////////////////////// Table

        [Area("Admin")]
        [HttpGet]
        public IActionResult Collections(string dbn, int page, string searchkey)

        {
            //return new ObjectResult(_postTable.Execute(new RequestDto { Page = page, SearchKey = searchkey,DbName=dbn }));

            var result = new ObjectResult(_postTable.Execute(new RequestDto { Page = page, SearchKey = searchkey, DbName = dbn }).TableDtos);

            _logger.LogInformation("get collection of {0}", dbn);
            return result;
        }

        [Area("Admin")]
        [HttpGet]
        //[Route("/Tables")]
        public IActionResult Collections2([FromBody] RequestDto db)
        {
            var result = new ObjectResult(_postTable.Execute(db).TableDtos);



            _logger.LogInformation("get collection of {0}", db.DbName);
            return result;

        }


        /// /////////////////////////////////////// FOr Test Elastic search=you can Delect it

        [Area("Admin")]
        [HttpGet]
        //[Route("/Tables")]
        public async Task<RequestDto> GetCollbyElastic([FromBody] RequestDto db)
        {
            var result = new ObjectResult(_postTable.Execute(db).TableDtos);

            var responce = await _elasticClient.SearchAsync<RequestDto>(s => s.Query(q => q.Term(t => t.TableName, db.DbName) || q.Match(m => m.Field(f => f.TableName).Query(db.DbName))));

            responce?.Documents?.FirstOrDefault();

            _logger.LogInformation("get collection of {0}", db.DbName);
            return (RequestDto)responce;

        }


        ////////////////////////////////////////////////


        [Area("Admin")]
        [HttpGet("{dbid}")]
        //[Route("/Tabless")]
        public IActionResult Collections3([FromRoute] int dbid)
        {

            var result = new ObjectResult(_postTable.Execute2(dbid).TableDtos);


            _logger.LogInformation("get collection by thid databasaeid: {0} ", dbid);
            return result;

        }
        [Area("Admin")]
        [HttpGet("{DB}")]
        //[Route("/Tabless")]
        public IActionResult Collections4([FromRoute] string DB)
        {
            var re=_getDB.GetDbbyIdAsync3(DB);
            int dbid = Convert.ToInt32(re);
            var result = new ObjectResult(_postTable.Execute2(dbid).TableDtos);

            _logger.LogInformation("get collection by thid databasaeid: {0} ", dbid);
            return result;

        }

        [Area("Admin")]
        [HttpGet("{id}")]
        public IActionResult SelectTable([FromRoute] int id)///////GET Table Dy ID
        {

            if (TableExist(id))
            {
                var db = _postTable.ExecuteIDs(new RequestDtoIDs { TableName = id });


                _logger.LogInformation("get collection of {0}", id);
                return new ObjectResult(db.TableDtos[0]);
            }
            else
                return NotFound();
        }

        [Area("Admin")]
        [HttpGet("{id}")]
        //[Route("/Cheked")]
        private bool TableExist([FromRoute] int id)
        {
            var result = _postTable.ISTableExist(id);



            _logger.LogInformation("Chek collextion of {0} is Exist", id);
            return result;
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult CreatTable([FromBody] RequestCreateTableDto req)

        {
            //var req = new RequestCreateTableDto
            //{
            //    collection = c,
            //    note = n,
            //    singleton = t

            //};
            var result = _creatTable.Execute(req);


            if (result.IsSuccess == true)
                _logger.LogInformation(" Collection of {0} in {1} is Create", req.collection, req.DbName);

            return Ok(result.Message);
            //return new ObjectResult(result);
        }

        [Area("Admin")]
        [HttpDelete("{TableId}")]
        public IActionResult DeleteTable(long TableId)
        {
            var result = _removeTableService.Execute(TableId);


            _logger.LogInformation(" Collection of {0}   is REmoved", TableId);
            return Ok(result);
        }


        [Area("Admin")]
        [HttpDelete("{TableId}")]
        public IActionResult DeleteTableAdmin(long TableId)
        {
            var result = _removeTableService.ExecuteAdmin(TableId);
            _logger.LogInformation(" Collection of {0}   is REmoved", TableId);
            return Ok(result);
        }
        /// ///////////

        [Area("Admin")]
        [HttpPut]
        public IActionResult EditTable([FromBody] RequestEdittableDto req)
        {
            //var req = new RequestEdittableDto
            //{
            //    collection = Collection,
            //    note = Note,
            //    TableId = TableId,
            //};

            var result = _editTableService.Execute(req);


            _logger.LogInformation(" Collection of {0} in {1} is Edited", req.collection, req.DBname);
            return Ok(result);
        }

        [Area("Admin")]
        [HttpPut]
        public IActionResult EditTableAdmin([FromBody] RequestEdittableAdminDto req)
        {
            //var req = new RequestEdittableDto
            //{
            //    collection = Collection,
            //    note = Note,
            //    TableId = TableId,
            //};

            var result = _editTableService.ExecuteAdmin(req);


            _logger.LogInformation(" Collection of {0} in {1} is Edited", req.collection, req.DBname);
            return Ok(result);
        }

        /////////////////////////////////////////////Fileds
        /// /////
        /// GetFiledsInallDB
        [Area("Admin")]

        [HttpGet]
        public IActionResult GetFiledsInallDB()
        {
            //get fild from table
            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = TableName, Page = 0, SearchKey = "" });
            //return Ok(itemss);

            //get fild from fildofTables. this is correct.

            var result = _getFiledsService.ExecuteaafildinallTable();



            _logger.LogInformation(" The fileds get by admin");
            return Ok(result);
        }
        //////////

        [Area("Admin")]
        [Route("/Filed")]
        [HttpGet]
        public IActionResult GetFileds()
        {
            //get fild from table
            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = TableName, Page = 0, SearchKey = "" });
            //return Ok(itemss);

            //get fild from fildofTables. this is correct.

            var result = _getFiledsService.ExecuteaafildinallTable().Fileds;



            _logger.LogInformation(" The all fileds by admin");
            return Ok(result);
        }

        [Area("Admin")]
        //[Route("/Filed")]
        [HttpGet("{id}")]
        public IActionResult GetFiledsIdDT([FromRoute] int id)
        {
            //get fild from table
            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = TableName, Page = 0, SearchKey = "" });
            //return Ok(itemss);

            //get fild from fildofTables. this is correct.
            var result = _getFiledsService.ExecutebyTid(new RequestGetFiledDtobyIdT { Tableid = id }).Fileds;

            _logger.LogInformation(" The fileds of {0}  is Get", id);
            return Ok(result);
        }



        [Area("Admin")]
        //[Route("/Filed")]
        [HttpPost]
        public IActionResult GetFiledsbtNameTable([FromBody] RequestGetFiledDto request)
        {
            //get fild from table
            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = TableName, Page = 0, SearchKey = "" });
            //return Ok(itemss);

            //get fild from fildofTables. this is correct.
            var result = _getFiledsService.Execute(request);

            _logger.LogInformation(" The fileds of {0}  is Get", request.nametable);
            return Ok(result);
        }
        [Area("Admin")]
        //[Route("/Filed")]
        [HttpPost]
        public IActionResult GetFiledsbtNameTablejustfildname([FromBody] RequestGetFiledDto request)
        {
            //get fild from table
            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = TableName, Page = 0, SearchKey = "" });
            //return Ok(itemss);

            //get fild from fildofTables. this is correct.
            var result = _getFiledsService.Executejustfildname(request);
            _logger.LogInformation(" The fileds of {0}  is Get", request.nametable);
            return Ok(result);
        }
        

        [Area("Admin")]
        //[Route("/Filed")]
        [HttpGet("{Fid}")]
        public IActionResult GetFiledbyid([FromRoute] int Fid)
        {
            //get fild from table
            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = TableName, Page = 0, SearchKey = "" });
            //return Ok(itemss);

            //get fild from fildofTables. this is correct.
            var result = _getFiledsService.GetFiledbyIdAsync2(Fid);

            _logger.LogInformation(" Get Filed {0}", Fid);
            return Ok(result);
        }
        /////////////////////////////////////////////////

        [Area("Admin")]
        [HttpDelete("{FId}")]
        public IActionResult DeleteFieldAdmin(long FId)
        {
            var result = _removeField.ExecuteAdmin(FId);

            _logger.LogInformation(" Delete Filed  {0} by admin", FId);
            return Ok(result);
        }

        //[Area("Admin")]
        //[HttpPut]
        //public IActionResult EditFiledAdmin([FromBody] FiledDto req)
        //{
        //    //var req = new RequestEdittableDto
        //    //{
        //    //    collection = Collection,
        //    //    note = Note,
        //    //    TableId = TableId,
        //    //};
        //    var result = _editedField.ExecuteAdmin(req);

        //    _logger.LogInformation(" Collection of {0} in {1} is Edited by admin", req.tablename, req.DbName);
        //    return Ok(result);
        //}

        [Area("Admin")]
        [HttpDelete("{FId}")]
        public IActionResult DeleteField(long FId)
        {
            var result = _removeField.Execute(FId);

            _logger.LogInformation(" Delete Filed {0}", FId);
            return Ok(result);
        }

        [Area("Admin")]
        [HttpPut]
        public IActionResult EditFiled([FromBody] FiledDto req)
        {
            //var req = new RequestEdittableDto
            //{
            //    collection = Collection,
            //    note = Note,
            //    TableId = TableId,
            //};
            var result = _editedField.Execute(req);

            _logger.LogInformation("Collection of {0} in {1} is Edited", req.tablename, req.DbName);
            return Ok(result);
        }


        /// /////////////////////////////////////////


        [Area("Admin")]
        //[Route("/TypeFiled")]
        [HttpGet]
        public IActionResult Typefileds()
        {

            var listtypefile = _createFiled.gettypefilse();
            _logger.LogInformation(" GEt types of filed");
            return Ok(listtypefile);
        }

        [Area("Admin")]
        //[Route("/TypeRelation")]
        [HttpGet]
        public IActionResult Typerelation()
        {

            var listtyperelatin = _createFiled.gettyperelation();
            _logger.LogInformation(" GEt types of filed");
            return Ok(listtyperelatin);
        }


        [Area("Admin")]
        [HttpGet]
        //[Route("/TableList")]
        public IActionResult ListTable()
        {
            List<T_FDto> T_F = new List<T_FDto>();

            var listTables = _createFiled.gettables();

            foreach (var t in listTables)
            {

                var listfildofthistable = _getFiledsService.ExecutebyTid(new RequestGetFiledDtobyIdT { Tableid = t.id }).Fileds;
                List<string> fildss = new List<string>();
                foreach (var f in listfildofthistable)
                    fildss.Add(f.fieldname);

                T_F.Add(new T_FDto { TName = t.collection, Filds = fildss });
            }

            _logger.LogInformation(" GEt all Tables and all fileds ");
            return Ok(T_F);
        }


        [Area("Admin")]
        [HttpPost]
        //[Route("/Filed")]
        public IActionResult addFiled([FromBody] RequestCreateFieldDto req)
        {
            //var req = new RequestCreateFieldDto
            //{
            //    name = name,
            //    tablename = Tname,
            //    Nullable = nullable,
            //    Relation = rel,
            //    type = type,
            //    Uniqe = uniqe,
            //    forignkey = forignkey,
            //    table2 = table2,
            //};
            var result = _createFiled.Execute(req);

            if (result.IsSuccess == true)
                _logger.LogInformation(" Add filed to {0} from {1} Database", req.tablename, req.DbName);

            return Ok(result);
        }

        ////////////////////////////////////////////////////////Items


        [Area("Admin")]
        [HttpGet]
        //[Route("/Items")]
        public IActionResult GetItems(string searchkey, int page, string Tanlenam, string DBname)
        {

            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = Tanlenam, Page = 0, SearchKey = "" });

            var items = _getItemsService.Execute(new RequestGetItemsdDto { Page = page, SearchKey = searchkey, nametable = Tanlenam, DbName = DBname });

            _logger.LogInformation(" Get Items of {0} from {1} Database", Tanlenam, DBname);
            return Ok(items);
        }

        [Area("Admin")]
        //[Route("/Filed")]
        [HttpGet("{id}")]
        public IActionResult GetItemsbyIDT([FromRoute] int id)
        {

            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = Tanlenam, Page = 0, SearchKey = "" });

            var items = _getItemsService.ExecutebyTID(id);



            _logger.LogInformation(" Get Items of Table {0} ", id);
            return Ok(items);
        }




        //[Area("Admin")]
        ////[Route("/Filed")]
        //[HttpGet("{id}")]
        //public IActionResult GetItemsbyIDTSearchkey([FromBody] RequestDtoIDs req)
        //{

        //    //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = Tanlenam, Page = 0, SearchKey = "" });

        //    var items = _getItemsService.ExecutebyTIDSeachkey(req);

        //    _logger.LogInformation(" Get Items of Table {0} ", req.TableName);
        //    return Ok(items);
        //}



        [Area("Admin")]
        [HttpPost]
        [Route("/Items")]
        public IActionResult CreateItem([FromBody] RequestGetItemsdDto req)
        {
            //var req = new RequestGetItemsdDto
            //{
            //    nametable = c,
            //    Page = 0,
            //    SearchKey = "",

            //};

            var itemss = _getItemsService.Execute(req);
            return Ok(itemss);
        }

        /////////////////////////////////////////////////////////////////////////OK



        [Area("Admin")]
        [HttpGet]
        public IActionResult InsertItem(string Tablename)
        {
            var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = Tablename, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;

            return Ok(new StringLsetModel { FildsNamae = itemss });
            //return View();
        }



        [Area("Admin")]
        [HttpPost]
        ////[Route("/Items")]
        public IActionResult InsertItemGet(StringLsetModel S, string Tablename, string DbName)
        {
            var itemss = _getItemsService.Execute(new RequestGetItemsdDto { nametable = Tablename, Page = 0, SearchKey = "" }).ITM[0].fieldnamelist;

            //var itemss = _getItemsService.Execute2(new RequestGetItemsdDto { nametable = Tablename, Page = 0, SearchKey = "" ,DbName=DbName});

            var result = _createItem.Execute(new RequestCreateItemdDto { I = S.DataItems, S = S.FildsNamae, TableName = Tablename });

            _logger.LogInformation(" Inser Items to {0} from {1} Database", Tablename, DbName);
            return Ok(result);
        }

        [Area("Admin")]
        [HttpPost]
        ////[Route("/Items")]
        public IActionResult InsertItemFinal([FromBody] StringLsetModel req)
        {
            ////RequestPayDto
            if (req.Tname == "RequsetPay")
            {
                List<string> Fs = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Guid", "IdUser", "Amount", "IsPay", "PayDate", "RefId" };
                var r = new RequestCreateItemdDto { DbName = req.DBname, I = new List<string> { Guid.NewGuid().ToString(), req.DataItems[1], req.DataItems[2], "false", DateTime.Now.ToString(), "0" }, S = Fs, TableName = "RequsetPay" };
                //    req.Guid = Guid.NewGuid().ToString();
                //    req.PayDate = DateTime.Now;
                //    req.RefId = 0;
                //    req.IsPay = false;

                var result = _createItem.Execute(r);

                var insertedRequestpay = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "Guid" }, Compare = new List<string>() { "=" }, Value = new List<string>() { r.I[0].ToString() }, }, nametable = "RequsetPay", DbName = req.DBname });
                if (!string.IsNullOrWhiteSpace(req.DataItems[1]) && Convert.ToInt32(req.DataItems[1]) != 0)
                {
                    //var result = _createItem.Execute(new RequestCreateItemdDto { DbName = req.DBName, I = new List<string> { req.Guid, req.IdUser.ToString(), req.Amount.ToString(), req.IsPay.ToString(), req.PayDate.ToString(), req.RefId.ToString() }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Guid", "IdUser", "Amount", "IsPay", "PayDate", "RefId" }, TableName = "RequsetPay" });
                    var result1 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.DataItems[1].ToString() }, }, nametable = "Users", DbName = req.DBname });


                    // if (result1 != null || result1.ITM[0].valuefiledlistList[0].Count > 0)
                    // {

                    //var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUser.ToString() }, }, nametable = "Users", DbName = req.DBName });
                    string username = result1.ITM[0].valuefiledlistList[5][0];

                    string SmsText = username + " عزیز فاکتور شما در لینک زیر قابل مشاده است لطفه جهت پرداخت کلیک کنید" + "\n" + "https://localhost:4432/Home/ViewUser/?Guid=" + r.I[0];//https://localhost:44332/ViewRepository/ViewData/
                    var mobile = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.DataItems[1].ToString() }, }, nametable = "Users", DbName = req.DBname });

                    _iSMSSender.SMS(new SMSSendRequest { to = mobile.ITM[0].valuefiledlistList[5][0], txt = SmsText });


                    for (int i = 5; i < insertedRequestpay.ITM[0].fieldnamelist.Count; i++)
                    {
                        result1.ITM[0].fieldnamelist.Add(insertedRequestpay.ITM[0].fieldnamelist[i]);
                        result1.ITM[0].valuefiledlistList.Add(insertedRequestpay.ITM[0].valuefiledlistList[i]);
                    }

                    _logger.LogInformation("Creat RequsetPay {0} in {1}", result1.ITM[0].valuefiledlistList[0][0]
                        , req.DBname);
                    return Ok(result1);
                }
                else
                {
                    string user = req.DataItems[0];
                    // string user = _getEverythings.getonevalueofitem2(new Filterrequestonefild2 { DbName = req.DBname, DTname = "Orders", fildgeted = "CustomerMobile", filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "idRequestPay" }, Compare = new List<string>() { "=" }, DBname = req.DBname, Value = new List<string>() { insertedRequestpay.ITM[0].valuefiledlistList[0][0] }, Addcon = new List<string>(), Tablename = "Orders" } });
                    string SmsText = user + " عزیز فاکتور شما در لینک زیر قابل مشاده است لطفه جهت پرداخت کلیک کنید" + "\n" + "https://localhost:4431/home/viewuserInsta/?guid=" + r.I[0];//https://localhost:44332/ViewRepository/ViewData/
                    _iSMSSender.SMS(new SMSSendRequest { to = user, txt = SmsText });
                    _logger.LogInformation("Creat RequsetPay for {0} in {1}", user, req.DBname);
                    return Ok(insertedRequestpay);
                }

            }
            else
            {
                ////////1401-01-11
                if (req.FildsNamae == null || req.FildsNamae.Count == 0)
                {
                    req.FildsNamae = new List<string>();
                    req.FildsNamae = _getFiledsService.Executejustfildname(new RequestGetFiledDto { DbName = req.DBname, nametable = req.Tname });
                }
                ///////
                var result = _createItem.Execute(new RequestCreateItemdDto { I = req.DataItems, S = req.FildsNamae, TableName = req.Tname, DbName = req.DBname });

                ////////////////////////////////////////////////////////////////////////////////
                if (req.Tname == "Users")
                {
                    ///
                    if (result != null)
                    {

                        string role = _getEverythings.getonevalueofitem2(new Filterrequestonefild2 { DbName = req.DBname, DTname = "Role", fildgeted = "Rolename", filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, DBname = req.DBname, Value = new List<string>() { req.DataItems[2] }, Addcon = new List<string>(), Tablename = "Role" } });

                        var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
                        var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
                        var tokenoption = new JwtSecurityToken(
                            //issuer: "https://localhost:44332",
                            issuer: "http://192.168.1.2:44332",
                            claims: new List<Claim>()
                            {
                                new Claim(ClaimTypes.NameIdentifier,result.Data.ItemId.ToString()),
                                new Claim(ClaimTypes.Email, req.DataItems[0]),
                                new Claim(ClaimTypes.MobilePhone, req.DataItems[0]),
                                new Claim(ClaimTypes.Name, req.DataItems[0]),
                                new Claim(ClaimTypes.Role, role),
                            },

                            expires: DateTime.Now.AddMinutes(5.0),
                            signingCredentials: signinCrifentioal
                            );
                        var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

                        //////////////Add in to Token Table 

                        ////////////////////insert Token with 1400-11-27
                        ///
                        var token1 = new RequestCreateItemdDto { DbName = req.DBname, TableName = "Tokens", I = new List<string>(2) { result.Data.ItemId.ToString(), tokenstring }, S = new List<string>(6) { "", "", "", "", "", "Token" } };
                        //token1.I.Add( tokenstring);
                        //token1.S.Add("Token");
                        var result2 = _createItem.Execute2token(token1).Data;//InsertItemtoTokenLevel2

                    }

                    ///////////////////////////////////////////////////////////////////////////////////////
                    ///
                }


                _logger.LogInformation(" Inser Items to {0} from {1} Database", req.Tname, req.DBname);
                return Ok(result.Data);
            }
        }
        ///////////////////////////////////////////////////


        //public IActionResult InsertItemFinal([FromBody] StringLsetModel req)
        //{
        //    ////RequestPayDto
        //    if (req.Tname == "RequsetPay")
        //    {
        //        var r = new RequestCreateItemdDto { DbName = req.DBname, I = new List<string> { Guid.NewGuid().ToString(), req.DataItems[1], req.DataItems[2], "false", DateTime.Now.ToString(), "0" }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Guid", "IdUser", "Amount", "IsPay", "PayDate", "RefId" }, TableName = "RequsetPay" };
        //        //    req.Guid = Guid.NewGuid().ToString();
        //        //    req.PayDate = DateTime.Now;
        //        //    req.RefId = 0;
        //        //    req.IsPay = false;
        //        var result = _createItem.Execute(r);

        //        //var result = _createItem.Execute(new RequestCreateItemdDto { DbName = req.DBName, I = new List<string> { req.Guid, req.IdUser.ToString(), req.Amount.ToString(), req.IsPay.ToString(), req.PayDate.ToString(), req.RefId.ToString() }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Guid", "IdUser", "Amount", "IsPay", "PayDate", "RefId" }, TableName = "RequsetPay" });
        //        var result1 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.DataItems[1].ToString() }, }, nametable = "Users", DbName = req.DBname });
        //        var result2 = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "Guid" }, Compare = new List<string>() { "=" }, Value = new List<string>() { r.I[0].ToString() }, }, nametable = "RequsetPay", DbName = req.DBname });

        //        //var result = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUser.ToString() }, }, nametable = "Users", DbName = req.DBName });


        //        string username = result1.ITM[0].valuefiledlistList[5][0];

        //        string SmsText = username + " عزیز فاکتور شما در لینک زیر قابل مشاده است لطفه جهت پرداخت کلیک کنید" + "\n" + "https://localhost:4432/Home/ViewUser/" + r.I[0];//https://localhost:44332/ViewRepository/ViewData/
        //        var mobile = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.DataItems[1].ToString() }, }, nametable = "Users", DbName = req.DBname });

        //        _iSMSSender.SMS(new SMSSendRequest { to = mobile.ITM[0].valuefiledlistList[8][0], txt = SmsText });
        //        for (int i = 5; i < result2.ITM[0].fieldnamelist.Count; i++)
        //        {
        //            result1.ITM[0].fieldnamelist.Add(result2.ITM[0].fieldnamelist[i]);
        //            result1.ITM[0].valuefiledlistList.Add(result2.ITM[0].valuefiledlistList[i]);
        //        }

        //        _logger.LogInformation("Creat RequsetPay {0} in {1}", result1.ITM[0].valuefiledlistList[0][0]
        //            , req.DBname);
        //        return Ok(result1);
        //    }
        //    else
        //    {
        //        ////////1401-01-11
        //        if (req.FildsNamae == null || req.FildsNamae.Count == 0)
        //        {
        //            req.FildsNamae = new List<string>();
        //            req.FildsNamae = _getFiledsService.Executejustfildname(new RequestGetFiledDto { DbName = req.DBname, nametable = req.Tname });
        //        }
        //        ///////
        //        var result = _createItem.Execute(new RequestCreateItemdDto { I = req.DataItems, S = req.FildsNamae, TableName = req.Tname, DbName = req.DBname });

        //        ////////////////////////////////////////////////////////////////////////////////
        //        if (req.Tname == "Users")
        //        {
        //            ///
        //            if (result != null)
        //            {
        //                var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
        //                var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
        //                var tokenoption = new JwtSecurityToken(
        //                    //issuer: "https://localhost:44332",
        //                    issuer: "https://localhost:44332",
        //                    claims: new List<Claim>()
        //                    {
        //                        new Claim(ClaimTypes.NameIdentifier,result.Data.ItemId.ToString()),
        //                        new Claim(ClaimTypes.Email, req.DataItems[0]),
        //                        new Claim(ClaimTypes.MobilePhone, req.DataItems[0]),
        //                        new Claim(ClaimTypes.Name, req.DataItems[0]),
        //                        new Claim(ClaimTypes.Role, "user"),
        //                    },

        //                    expires: DateTime.Now.AddMinutes(5.0),
        //                    signingCredentials: signinCrifentioal
        //                    );
        //                var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

        //                //////////////Add in to Token Table 

        //                ////////////////////insert Token with 1400-11-27
        //                ///
        //                var token1 = new RequestCreateItemdDto { DbName = req.DBname, TableName = "Tokens", I = new List<string>(2) { result.Data.ItemId.ToString(), tokenstring }, S = new List<string>(6) { "", "", "", "", "", "Token" } };
        //                //token1.I.Add( tokenstring);
        //                //token1.S.Add("Token");
        //                var result2 = _createItem.Execute2token(token1).Data;//InsertItemtoTokenLevel2

        //            }

        //            ///////////////////////////////////////////////////////////////////////////////////////
        //            ///
        //        }


        //        _logger.LogInformation(" Inser Items to {0} from {1} Database", req.Tname, req.DBname);
        //        return Ok(result.Data);
        //    }
        //}
        /////////////////////////////////////////////////////


        [Area("Admin")]
        [HttpPut]
        public IActionResult EditItem([FromBody] ItemdDto req)
        {
            //var req = new RequestEdittableDto
            //{
            //    collection = Collection,
            //    note = Note,
            //    TableId = TableId,
            //};

            var result = _editItemService.Execute(req);


            _logger.LogInformation(" item of {0} in {1} is Edited", req.ItemId, req.TableName);
            return Ok(result);
        }
        [Area("Admin")]
        [HttpPut]
        public IActionResult EditoneItem([FromBody] ItemdDto req)
        {
            //var req = new RequestEdittableDto
            //{
            //    collection = Collection,
            //    note = Note,
            //    TableId = TableId,
            //};

            var result = _editItemService.Executeone(req);


            _logger.LogInformation(" item of {0} in {1} is Edited", req.ItemId, req.TableName);
            return Ok(result);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult DeleteItem([FromBody] itemDto req)
        {
            var result = _removeItem.Execute(req);

            _logger.LogInformation(" Delete item  {0}", req.iditem);
            return Ok(result);
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult GetItembyFiltergetallfile([FromBody] Filterrequest req)
        {
            var result = _getItemsService.getitembyfilters(req);

            _logger.LogInformation(" getitem from item  {0}", req.DTname);
            return Ok(result);
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult GetItembyFilteronefile([FromBody] Filterrequestonefild req)
        {
            var result = _getItemsService.getitembyonfilte(req);

            _logger.LogInformation(" getitem from item  {0}", req.DTname);
            return Ok(result);
        } 
        
        [Area("Admin")]
        [HttpPost]
        public IActionResult getonevalueofitem([FromBody] Filterrequestonefild req)
        {
            var result = _getItemsService.getonevalueofitem(req);

            _logger.LogInformation(" getitem from item  {0}", req.DTname);
            return Ok(result);
        }

    }
}
