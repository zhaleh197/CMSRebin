using Abp.MimeTypes;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Application.Service.Database.Commands;
using CmsRebin.Application.Service.Database.Queris.GetDB;
using CmsRebin.Application.Service.Database.Queris.UploadDB;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.WebAPI.Area.Admin.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DBController : ControllerBase
    {
        private readonly IGetDB _getDB;
        private readonly ICreatDB _creatDB;
        private readonly IUploadBD _uploadBD;
        private readonly IRemoveDB _removeDB;
        private readonly IEditDB _editDB;
        private readonly ILogger<DBController> _logger;
        private readonly ICreatTable _creatTable;
        private readonly ICreateFiled _createFiled;

        private readonly ICreateItem _createItem;


        private readonly IGetEverythings _getEverythings;
        public DBController(ICreatDB creatDB,IGetDB getDB, IUploadBD uploadBD,IEditDB editDB,
            IRemoveDB removeDB, ILogger<DBController> logger,
            ICreatTable creatTable, ICreateFiled createFiled, ICreateItem createItem
            , IGetEverythings getEverythings)
        {

            _logger = logger;
            _getDB = getDB;
            _creatDB = creatDB;
            _uploadBD = uploadBD;
            _editDB = editDB;
            _removeDB = removeDB;
            _creatTable = creatTable;
            _createFiled = createFiled;
            _createItem = createItem;

            _getEverythings = getEverythings;
    }

        [Area("Admin")]
        [HttpGet]
        public IActionResult DBs(long owner, int page, string searchkey)
        {


            var result = _getDB.Executeall(new RequesDBDto { Page = page, SearchKey = searchkey, OwnerUser = owner }).DbsDtos;


            _logger.LogInformation("get all DBs by Owner {0}", owner);
            return new ObjectResult(result);
        }


        [Area("Admin")]
        //[Route("/Databases")]
        [HttpGet("{id}")]
        public IActionResult DBsbyUserid([FromRoute] long id)
        {

            var result = _getDB.Execute(new RequesDBDto { OwnerUser = id }).DbsDtos;


            _logger.LogInformation("get all DBs by Owner {0}", id);
            return new ObjectResult(result);
        }


        ///////////////////////////////////////////////// 

        [Area("Admin")]
        //[Route("/Select")]

        [HttpGet("{id}")]
        public IActionResult SelectDB([FromRoute] int id)
        {

            if (DBExist(id))
            {
                var db = _getDB.GetDbbyIdAsync2(id);

                _logger.LogInformation("get DB {0}", id);
                return new ObjectResult(db.DbsDtos);
            }
            else
            {

                return NotFound();
            }
                
        }
        [HttpGet("{DB}")]
        public long SelectDBbyname([FromRoute] string DB)
        {
                var db = _getDB.GetDbbyIdAsync3(DB);

                _logger.LogInformation("get DB {0}", DB);
                return db;
        }

        /////////////////////////////////////////////////

        [Area("Admin")]
        [HttpPost]
        //[Route("/CreatDB")]
        public IActionResult CreatDB([FromBody] RequestCreateDBDto req)
        {
            var result = _creatDB.Execute(req);

            ///ROLE
            //[Key, ForeignKey("Users")]
            //public long id { get; set; }
            var result4 = _creatTable.Execute(new RequestCreateTableDto { collection = "Role", DbName = req.DBName, note = "", singleton = false });
            var result5 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "Rolename", Nullable = true, Relation = null, table2 = null, tablename = "Role", type = "string", Uniqe = false });
            var result6 = _createItem.Execute(new RequestCreateItemdDto { DbName = req.DBName, I = new List<string> { "admin" }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Rolename" }, TableName = "Role" });
            var result7 = _createItem.Execute(new RequestCreateItemdDto { DbName = req.DBName, I = new List<string> { "user" }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "Rolename" }, TableName = "Role" });

            ///USER
            var result1 = _creatTable.Execute(new RequestCreateTableDto { collection="Users",DbName=req.DBName,note="",singleton=false});
            var result3u = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "UserName", Nullable = true, Relation = null, table2 = null, tablename = "Users", type = "string", Uniqe = false });
            var result3ue = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "Password", Nullable = true, Relation = null, table2 = null, tablename = "Users", type = "string", Uniqe = false });
            var result3uu = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = "id", name = "Roleid", Nullable = true, Relation = "1-n", table2 = "Role", tablename = "Users", type = "string", Uniqe = false });
            //var user =  _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { req.IdUserOwner.ToString()}, }, nametable = "Users", DbName = "swa" });

            //Token
            var result2 = _creatTable.Execute2(new RequestCreateTableDto2 { collection = "Tokens", DbName = req.DBName, note = "", singleton = false, forignkey = "id", table2 = "Users" });
            var result3 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "Token", Nullable = true, Relation = null, table2 = null, tablename = "Tokens", type = "nvarchar(MAX)", Uniqe = false });
            //var result33 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "CreatDateTime", Nullable = true, Relation = null, table2 = null, tablename = "Tokens", type = "DateTime", Uniqe = false });


            var request = new RequestCreateItemdDto { DbName = req.DBName, I = new List<string> { "admin", "admin", "1" }, S = new List<string> { "InsertTime", "UpdateTime", "IsRemoved", "RemoveTime", "UserName", "Password", "Roleid" }, TableName = "Users" };
            var result6uu = _createItem.Execute(request);
            if (result6uu.IsSuccess == true)
            {
                //////
                var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
                var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
                var tokenoption = new JwtSecurityToken(
                      //issuer: "https://localhost:44332",
                      issuer: "https://localhost:44332",
                    claims: new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,result6uu.Data.ItemId.ToString()),
                        new Claim(ClaimTypes.MobilePhone, request.I[0]),
                        new Claim(ClaimTypes.Name, request.I[0]),
                        new Claim(ClaimTypes.Role, request.I[2]),
                    },

                    expires: DateTime.Now.AddMinutes(5.0),
                    signingCredentials: signinCrifentioal
                    );
                var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

                //////////////Add in to Token Table 

                var retok = _createItem.Execute2token(new RequestCreateItemdDto { TableName = "Tokens", DbName = request.DbName, I = new List<string>() { result6uu.Data.ItemId.ToString(), tokenstring }, S = new List<string>() { "id", "", "", "", "", "Token" } });
            }

            ////////////////////////////////////////  

            //Token
            //var result2 = _creatTable.Execute2(new RequestCreateTableDto2 { collection = "Tokens", DbName = req.DBName, note = "", singleton = false, forignkey = "id", table2 = "Users" });
            //var result3 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "Token", Nullable = true, Relation = null, table2 = null, tablename = "Tokens", type = "nvarchar(MAX)", Uniqe = false });
            ////var result33 = _createFiled.Execute(new RequestCreateFieldDto { DbName = req.DBName, forignkey = null, name = "CreatDateTime", Nullable = true, Relation = null, table2 = null, tablename = "Tokens", type = "DateTime", Uniqe = false });


          
            _logger.LogInformation("Creat DB {0} by Owner {0}", req.DBName, req.IdUserOwner);
            return Ok(result);
        }



        //[Area("Admin")]
        //[HttpPost]
        //[Route("/UploadDB")]
        //public IActionResult UploadDB([FromForm] UploadDBDto req)
        //{
        //    _uploadBD.Execute(req);
        //    return Ok();
        //}

        
        [Area("Admin")]
        [HttpGet("{id}")]
        //[Route("/Cheked")]
        private bool DBExist([FromRoute] int id)
        {

            var result = _getDB.ISDBExist(id);

            _logger.LogInformation(" Check DB {0} is Exist", id);
            return result;
        }



        //[Area("Admin")]
        //[HttpDelete("{id}")]
        ////[HttpPost("{id}")]
        //[Route("/Delete")]

        [Area("Admin")]
        [HttpDelete("{iddb}")]
        //[HttpPost("{iddb}")]
        //[Route("/Delete")]
        public IActionResult DeleteDB([FromRoute] int iddb)
        {

            var result = _removeDB.Execute(iddb);

            _logger.LogInformation(" Delete DB {0} ", iddb);
            return Ok(result);
        }


        [Area("Admin")]
        [HttpPut]
        //[Route("/Edite")]
        public IActionResult EditDB([FromBody] RequestEditDBDto db)
        {

            var result = _editDB.Execute(db);

            _logger.LogInformation(" Edite DB {0} ", db.Iddb);
            return Ok(result);
        }
        ///////////////////////////////////////////////////////////////////////////Admin
        /// [Area("Admin")]
        [HttpDelete("{iddb}")]
        //[HttpPost("{iddb}")]
        //[Route("/Delete")]
        public IActionResult DeleteDBAdmin([FromRoute] int iddb)
        {

            var result = _removeDB.ExecuteADmin(iddb);

            _logger.LogInformation(" Delete DB {0} ", iddb);
            return Ok(result);
        }


        [Area("Admin")]
        [HttpPut]
        //[Route("/Edite")]
        public IActionResult EditDBAdmin([FromBody] RequestEditDBAdminDto db)
        {

            var result = _editDB.ExecuteAdmin(db);

            _logger.LogInformation(" Edite DB {0} ", db.Iddb);
            return Ok(result);
        }


        /////////////////////////////////////////////////////// 
        ///

        [Area("Admin")]
        //[Route("/Databases")]
        [HttpGet("{id}")]
        public IActionResult DownloadDB([FromRoute] int id)
        {
            string filename = DateTime.Now.ToString("ddMMyyyy").ToString() + ".Bak";
            var result = _getDB.downloadDB(id, filename);

            _logger.LogInformation("download db {0}", id);
            return Ok(result);
        }


        //[Area("Admin")]
        ////[Route("/Databases")]
        //[ HttpPost]
        //public IActionResult DownloadDB([FromBody] GetDBDto req)
        //{

        //    var result = _getDB.downloadDB(
        //        (int)req.Id,
        //        (req.Owner.id.ToString() + req.DB + DateTime.Now.ToString("ddMMyyyy").ToString())
        //        );

        //    _logger.LogInformation("download db {0}", req.Id);
        //    return Ok(result);
        //}  
        //////

    }
}
