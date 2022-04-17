using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Application.Service.Collection.Commands.CreatTable;
using CmsRebin.Application.Service.Collection.Commands.EditTable;
using CmsRebin.Application.Service.Collection.Commands.RemoveTable;
using CmsRebin.Application.Service.Persons.Commands.EditUser;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Application.Service.Persons.Commands.RemoveUser;
using CmsRebin.Application.Service.Persons.Commands.UserSatusChange;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using CmsRebin.Domain.Entities.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using CmsRebin.Application.Service.Persons.Queries.GetRoles;
using CmsRebin.Common.Dto;
using Microsoft.AspNetCore.Authorization;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IGetUsersService _getUserService;
        private readonly IPostTable _postTable;
        private readonly ICreatTable _creatTable;
        private readonly ICreateFiled _createFiled;
        private readonly IRemoveUserService _removeUserService;
        private readonly IEditUserService _editeUsesrService;
        private readonly IUserSatusChangeService _userSatusChangeService;
        private readonly IRemoveTableService _removeTableService;
        private readonly IEditTableService _editTableService ;
        private readonly IGetFiledsService _getFiledsService;
        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;
        private readonly IGetRolesService _getRolesService;
        public UsersController(IGetRolesService getRolesService, IRegisterUserService registerUserService, IUserLoginService userLoginService,IGetUsersService getUsersService,IPostTable postTable, IGetFiledsService getFiledsService, ICreatTable creatTable, IEditTableService editTableService, ICreateFiled createFiled, IRemoveUserService removeUserService, IEditUserService editUserService, IUserSatusChangeService userSatusChangeService, IRemoveTableService removeTableService)
        {
            _getUserService = getUsersService;
            _postTable = postTable;
            _creatTable = creatTable;
            _createFiled = createFiled;
            _removeUserService = removeUserService;
            _editeUsesrService = editUserService;
            _userSatusChangeService = userSatusChangeService;
            _removeTableService = removeTableService;
            _editTableService = editTableService;
            _getFiledsService = getFiledsService;
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
            _getRolesService = getRolesService;

        }
        //////////////////////////////////////////////   USER
      
        public IActionResult Index(string searchkey, int page)
        {
            
            return View(_getUserService.Execute(new RequestGetUserDto { Page = page, SearchKey = searchkey, }));
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult DeleteUser(long UserId)
        {
            return Json(_removeUserService.Execute(UserId));
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult EditUser(long UserId, string firstname, string lastname)
        {
            return Json(_editeUsesrService.Execute(new RequestEdituserDto
            {
                FirstName = firstname,
                LasttName=lastname,
                UserId = UserId,
            }));
        }


        [Area("Admin")]
        [HttpPost]
        public IActionResult UserSatusChange(long UserId)
        {
            return Json(_userSatusChangeService.Execute(UserId));
        }



        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.Roles = new SelectList(_getRolesService.Execute().Data, "Id", "Name");
            return View();
        }


        


        [Area("Admin")]
        [HttpPost]
        public IActionResult CreateUser(string Email, string firstname, string lastname, string Password, string RePassword,bool isactive, string role)
        {
            var result = _registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = Email,
                FirstName = firstname,
                LastName=lastname,
                Password = Password,
                RePasword = RePassword,
                isactive=isactive,
                role=role,
            });
            return Json(result);
        }




        /////////////////////////////////////////////////////// Table

        [Area("Admin")]
        public IActionResult Models(string searchkey, int page)
        {
            return View(_postTable.Execute(new RequestDto { Page = page, SearchKey = searchkey, }));
        }

        [Area("Admin")]
        [HttpGet]
        public IActionResult creatTable()

        {

           // ViewBag.Roles = new SelectList(_creatTable.Execute().Data, "Id", "Name");
            return View();
        }

        

        [Area("Admin")]
        [HttpPost]
        public IActionResult creatTable(string c, string n, bool t)

        {
            var req = new RequestCreateTableDto
            {
                collection = c,
                note = n,
                singleton = t

            };
            TempData["tableName"] = req.collection;
            //TempData.Keep("tableName");
            TempData.Keep();
            //TempData["tableN"] = req.collection;
            //TempData.Keep("tableName");
            //TempData.Keep();
            var result = _creatTable.Execute(req);            //var result = _creatTable.Execute(new RequestCreateTableDto
            //{
            //    collection=c,
            //    note=n,
            //    singleton=t

            //});

            return Json(result);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult DeleteTable(long TableId)
        {
            return Json(_removeTableService.Execute(TableId));
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult EditTable(long TableId, string Collection, string Note)
        {
            return Json(_editTableService.Execute(new RequestEdittableDto
            {
                collection= Collection,
                note = Note,
                TableId = TableId,
            }));
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>

        /// <returns></returns>
        //[Area("Admin")]
        //public IActionResult GetFGetFiledsmodelsileds(string c)
        //{
        //    var Tanlenam = TempData["tableName"] as string;
        //    ViewBag.tablename = Tanlenam;
        //    return View();
        //}


        [Area("Admin")]

        public IActionResult GetFileds(string searchkey, int page , string Tanlenam)
        {
            var Tanlenam1 = TempData["tableName"] as string;
            //TempData.Keep();
            //ViewBag.tablename = Tanlenam1;
            if (Tanlenam1 != null)
            {
                ViewBag.tablename = TempData["tableName"];
                TempData.Keep();
            }
            else
            {
                ViewBag.tablename = TempData["tableNameee"];
                TempData.Keep();
            }
            return View(_getFiledsService.Execute(new RequestGetFiledDto { Page = page, SearchKey = searchkey, nametable= ViewBag.tablename }));
        }


        //[Area("Admin")]
        //[HttpGet]
        //public IActionResult GetInsertFiled()
        //{

        //    return View();
        //}

        [Area("Admin")]
        [HttpPost]
        public IActionResult GetInsertFiled(int page, string searchkey, long TableId, string Collection)
        {

            //var result = _getFiledsService.Execute(new RequestGetFiledDto
            //{
            //    Page = page,
            //    SearchKey = searchkey,
            //    nametable = Collection,
            //}
            //);
            //ViewBag.getfilde = result;
            ViewBag.NTableFromModel = Collection;
            TempData["tableNameee"] = Collection;
            TempData.Keep();
          
            var d = new ResultDto()
            {
                IsSuccess = true,
                Message = "",
            }; 
            return Json(d);


            //return View();
        }


        [Area("Admin")]
        [HttpGet]
        public IActionResult addFiled()
        {
            var Tanlename = TempData["tableName"] as string;
            ViewBag.tablenam = Tanlename;
            TempData.Keep();

            if (Tanlename != null)
                ViewBag.tablenam = Tanlename;
            else
            {
                var Tanlename1 = TempData["tableNameee"] as string;
                ViewBag.tablenam = Tanlename1;
                TempData.Keep();
            }


            var listtypefile = _createFiled.gettypefilse();
            var listtyperelatin = _createFiled.gettyperelation();
            var listTables = _createFiled.gettables();
            List<string> s = new List<string>();
            for (int i = 0; i < listTables.Count; i++)
            {
                s.Add(listTables[i].collection);
            }
            ViewBag.typefile = listtypefile;
            ViewBag.typerelation = listtyperelatin;
            ViewBag.listTabl = s;


            // ViewBag.Roles = new SelectList(_creatTable.Execute().Data, "Id", "Name");
            return View();
        }

        [Area("Admin")]
        [HttpPost]
        //[Route("AddFiles")]
        public IActionResult addFiled(string Tname, string name,string type, string rel, bool nullable, bool uniqe, string table2, string forignkey)
        {
          
            var result = _createFiled.Execute(new RequestCreateFieldDto
            {
                name = name,
                tablename = Tname,
                Nullable = nullable,
                Relation = rel,
                type = type,
                Uniqe = uniqe,
                forignkey = forignkey,
                table2 = table2,
            });


            return Json(result);
        }

        

        [Area("Admin")]
        [HttpPost]
        public IActionResult creatPro()
        {
            return View();
        }

    }
}
