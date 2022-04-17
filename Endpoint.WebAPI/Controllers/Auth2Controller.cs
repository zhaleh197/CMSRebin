using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Application.Service.Filed.Queries.Get;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using Endpoint.WebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Endpoint.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Auth2Controller : ControllerBase

    {

        private readonly ILogger<AuthController> _logger;

        private readonly IGetEverythings _getEverythings;
        private readonly IGetFiledsService _getFiledsService;

        private readonly IGetItemsService _getItemsService;
        
        private readonly ICreateItem _createItem;
        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;
        //private readonly IGetUsersService _getUsersService;

        public Auth2Controller(
            ICreateItem createItem,
            IGetEverythings getEverythings,
            IGetItemsService getItemsService,
            //IGetUsersService getUsersService, 
            IRegisterUserService registerUserService,
            IUserLoginService userLoginService,
            IGetFiledsService getFiledsService,
            ILogger<AuthController> logger)
        {
            _logger = logger;
            _getItemsService = getItemsService;
            _getEverythings = getEverythings;
            _registerUserService = registerUserService;
            _createItem = createItem;
            _userLoginService = userLoginService;
            _getFiledsService = getFiledsService;
            //_getUsersService = getUsersService;
        }
        [HttpPost]
        public IActionResult Signup([FromBody] RequestRegisterUserDto2 request)
        {

            //foreach(var n in ...)
            //{
            //if (string.IsNullOrWhiteSpace(request.Email))
            //{
            //    return new ResultDto<ResultRegisterUserDto>()
            //    {
            //        Data = new ResultRegisterUserDto()
            //        {
            //            UserId = 0,
            //        },
            //        IsSuccess = false,
            //        Message = "همه مقادیر را وارد نمایید به درستی."
            //    };
            //}
            //}

            ////autontiction
            if (string.IsNullOrWhiteSpace(request.Mobile) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.RePasword))

            {
                return Ok(new ResultDto { IsSuccess = false, Message = "لطفا تمامی موارد رو ارسال نمایید" });
            }

            if (User.Identity.IsAuthenticated == true)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "شما به حساب کاربری خود وارد شده اید! و در حال حاضر نمیتوانید ثبت نام مجدد نمایید" });
            }
            if (request.Password != request.RePasword)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "رمز عبور و تکرار آن برابر نیست" });
            }
            if (request.Password.Length < 8)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "رمز عبور باید حداقل 8 کاراکتر باشد" });
            }

            ///////////////////////////
            //request.role = new PasswordHasher().HashPassword("operator");
            if (string.IsNullOrWhiteSpace(request.role) || request.role == "string")
            { request.role = "user"; }
            request.isactive = true;
            ////////////////////
            ///
            /// 
            ///////   check email 
            //var chekemai = _getUsersService.IsUserExistbyemail(request.Mobile);
            var chekemai = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "UserName" }, Compare = new List<string>() { "=" }, Value = new List<string>() { request.Mobile }, }, nametable = "Users", DbName = request.DB });


            if (chekemai.ITM[0].valuefiledlistList[0].Count() > 0)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "موبایل وارد  شده موجود است. " });
            }
            ////////////////
            ///
            var signeupResult = _registerUserService.Execute2(request);


            //////////////////////////

            ///////////////////////////////login 
            if (signeupResult.IsSuccess == true)
            {
                //////
                var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
                var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
                var tokenoption = new JwtSecurityToken(
                      //issuer: "https://localhost:44332",
                      issuer: "https://localhost:44332",
                    claims: new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                        new Claim(ClaimTypes.MobilePhone, request.Mobile),
                        new Claim(ClaimTypes.Name, request.Mobile),
                        new Claim(ClaimTypes.Role, "user"),
                    },

                    expires: DateTime.Now.AddMinutes(5.0),
                    signingCredentials: signinCrifentioal
                    );
                var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

                //////////////Add in to Token Table 

                var retok = _createItem.Execute2token(new RequestCreateItemdDto { TableName = "Tokens", DbName = request.DB, I = new List<string>() { signeupResult.Data.UserId.ToString(), tokenstring }, S = new List<string>() { "", "", "", "", "", "Token" } });

                ///////////////////////////////////
                ///

                _logger.LogInformation(" Signup {0} ", request.Mobile);

                return Ok(new { token = tokenstring });
            }
            else
            {
                return Ok(new ResultDto { IsSuccess = true, Message = "کاربر اضافه نشد" });
            }

        }

        
        //[HttpPost("singnup")]
        [HttpPost]
        public IActionResult Signupdynamic([FromBody] RequestCreateItemdDto request)
        {

            //foreach(var n in ...)
            //{
            //if (string.IsNullOrWhiteSpace(request.Email))
            //{
            //    return new ResultDto<ResultRegisterUserDto>()
            //    {
            //        Data = new ResultRegisterUserDto()
            //        {
            //            UserId = 0,
            //        },
            //        IsSuccess = false,
            //        Message = "همه مقادیر را وارد نمایید به درستی."
            //    };
            //}
            //}

            ////autontiction
            for(int i=0; i<request.I.Count;i++)
            if (string.IsNullOrWhiteSpace(request.I[i])) 
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "لطفا تمامی موارد رو ارسال نمایید" });
            }

            if (User.Identity.IsAuthenticated == true)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "شما به حساب کاربری خود وارد شده اید! و در حال حاضر نمیتوانید ثبت نام مجدد نمایید" });
            }
            ///////////////////////////
            ///

            if (string.IsNullOrWhiteSpace(request.I[2]) || request.I[2] == "string")
            { request.I[2] = "user"; }


            try { int r = Convert.ToInt32(request.I[2]); }
            catch
            {
                request.I[2] = _getItemsService.getonevalueofitem(new Filterrequestonefild { fildgeted = "id", DbName = request.DbName, DTname = "Role", filters = new CmsRebin.Infrastructure.Enum.Equation { Compare = new List<string>() { "=" }, DBname = request.DbName, Tablename = "Role", Filname = new List<string>() { "Rolename" }, Value = new List<string>() { request.I[2] }, Addcon = new List<string>() } });
            }
            ///////   check email 
            //var chekemai = _getUsersService.IsUserExistbyemail(request.Mobile);
            var chekemai =_getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "UserName" }, Compare = new List<string>() { "=" }, Value = new List<string>() { request.I[0] }, }, nametable = "Users", DbName = request.DbName });


            if (chekemai.ITM[0].valuefiledlistList[0].Count()>0)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "موبایل وارد  شده موجود است. " });
            }
            ////////////////
            ///
            if(request.S==null||request.S.Count==0)
            {
                request.S = new List<string>();
                //ReslutGetFiledDto filds= _getFiledsService.Execute(new RequestGetFiledDto { DbName = request.DbName, nametable = request.TableName });
                //  for(int f=0;f<filds.Fileds.Count;f++)
                //      request.S.Add(filds.Fileds[f].fieldname);
                request.S = _getFiledsService.Executejustfildname(new RequestGetFiledDto { DbName = request.DbName, nametable = request.TableName });
            }

            var signeupResult = _createItem.Execute(request);


            //////////////////////////

            ///////////////////////////////login 
            if (signeupResult.IsSuccess == true)
            {
                //////
                var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
                var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
                var tokenoption = new JwtSecurityToken(
                      //issuer: "https://localhost:44332",
                      issuer: "https://localhost:44332",
                    claims: new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.ItemId.ToString()),
                        new Claim(ClaimTypes.MobilePhone, request.I[0]),
                        new Claim(ClaimTypes.Name, request.I[0]),
                        new Claim(ClaimTypes.Role, request.I[2]),
                    },

                    expires: DateTime.Now.AddMinutes(5.0),
                    signingCredentials: signinCrifentioal
                    );
                var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

                //////////////Add in to Token Table 

               var retok= _createItem.Execute2token(new RequestCreateItemdDto { TableName= "Tokens",DbName=request.DbName,I=new List<string>() { signeupResult.Data.ItemId.ToString(), tokenstring },S=new List<string>() { "id", "", "", "", "","Token" } });

                ///////////////////////////////////
                ///

                _logger.LogInformation(" Signup {0} ", request.I[0]);

                return Ok(new { token = tokenstring });
            }
            else
            {
                return Ok(new ResultDto { IsSuccess = true, Message = "کاربر اضافه نشد" });
            }

        }


        //public IActionResult Signin(string ReturnUrl = "/")
        //{
        //    ViewBag.url = ReturnUrl;
        //    return View();
        //}

        [HttpPost]
        public IActionResult Post([FromBody] Login2 login)//// = Login ast.
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Modle is not valid.");
            }

            var signupResult = _userLoginService.Execute2(login.UserName, login.Password,login.DB);


            if (signupResult.IsSuccess == false)
            {
                return Unauthorized();
            }
            else
            {
                var tokenstring = signupResult.Data.Token;

                _logger.LogInformation(" Signup {0} ", signupResult.Data.UserId);
                return Ok(signupResult.Data);


            }

        }


        //public IActionResult SignOutt()
        //{
        //    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    return RedirectToAction("Index", "Home");
        //}
    }
}
