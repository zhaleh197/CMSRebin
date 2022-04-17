using CmsRebin.Application.Service.Persons.Commands.EditUser;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Persons.Commands.RemoveUser;
using CmsRebin.Application.Service.Persons.Commands.UserSatusChange;
using CmsRebin.Application.Service.Persons.Queries.GetRoles;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
 
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using Endpoint.WebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies; 
using Microsoft.IdentityModel.Tokens; 
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Endpoint.WebAPI.Area.Admin.Controllers
{
    [Area("Admin")]
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        private readonly IRemoveUserService _removeUserService;//delete
        private readonly IEditUserService _editeUsesrService;//put
        private readonly IGetUsersService _getUserService;//get
        private readonly IRegisterUserService _registerUserService;//post
        private readonly IGetRolesService _getRolesService;
        private readonly IUserSatusChangeService _userSatusChangeService;
        private readonly IUserLoginService _userLoginService;


        public UserController(ILogger<UserController> logger, IGetRolesService getRolesService, 
            IUserSatusChangeService userSatusChangeService, IUserLoginService userLoginService,
            IRemoveUserService removeUserService, IEditUserService editUserService, 
            IGetUsersService getUsersService, IRegisterUserService registerUserService)
        {
            _logger = logger;

            _removeUserService = removeUserService;
            _editeUsesrService = editUserService;
            _getUserService = getUsersService;
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
            _userSatusChangeService = userSatusChangeService;
            _getRolesService = getRolesService;

           
        }

        [Area("Admin")]
        [HttpGet]
        //[ResponseCache(Duration =60)]// 60 second this data is in server copmuter(no client). and if DB ic chanded, this is quiqlu update.
        public IActionResult GetAllUser(string searchkey, int page)
        {
            var result = new ObjectResult(_getUserService.Execute(new RequestGetUserDto { Page = page, SearchKey = searchkey, }).Users)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
            //////ارسال اطلاعات از طریق هدر
            Request.HttpContext.Response.Headers.Add("X_Count", _getUserService.numerUser().ToString());

            _logger.LogInformation("Get Alluser");

            return result;
        }

        [Area("Admin")]
        [HttpGet("{id}")]
       // [Route("/GetUser")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (UserExist(id))
            {
                var user = await _getUserService.GetuserbyIdAsync(id);


                _logger.LogInformation("Get User of {0}", id);
                return Ok(user.Users);
            }
            else
                return NotFound();
        }

        private bool UserExist(int id)
        {
            return _getUserService.IsUserExist(id);
        }

        [Area("Admin")]
        [HttpGet]
        [Route("/Role")]
        public IActionResult GetRoles()
        {
            var result = _getRolesService.Execute();

            _logger.LogInformation("Get Role");
            return Ok(result.Data);
        }


        [Area("Admin")]
        [HttpPost]
        public IActionResult InserUser([FromBody] RequestRegisterUserDto request)
        {
            ////autontiction
            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
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

            string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";

            var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "ایمیل خودرا به درستی وارد نمایید" });
            }
            ///////////////////////////
            //request.role = new PasswordHasher().HashPassword("operator");
           // request.role = "user";
         //   request.isactive = true;
            ////////////////////
            ///
            /// 
            ///////   check email 
            var chekemai = _getUserService.IsUserExistbyemail(request.Email);
            if (chekemai)
            {
                return Ok(new ResultDto { IsSuccess = false, Message = "ایمیل وارد  شده موجود است. لطفا ایمیلی دیگر وارد نمایید" });
            }
            ////////////////
            ///
            var signeupResult = _registerUserService.Execute(request);


            ///////////////////////////////login 
            if (signeupResult.IsSuccess == true)
            {
                //    var claims = new List<Claim>()
                //{
                //    new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                //    new Claim(ClaimTypes.Email, request.Email),
                //    new Claim(ClaimTypes.Name, request.FirstName),
                //    new Claim(ClaimTypes.Role, "operator"),
                //};


                //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //    var principal = new ClaimsPrincipal(identity);
                //    var properties = new AuthenticationProperties()
                //    {
                //        IsPersistent = true
                //    };
                //    HttpContext.SignInAsync(principal, properties);

                //////
                var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
                var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
                var tokenoption = new JwtSecurityToken(
                      //issuer: "https://localhost:44332",
                      issuer: "https://localhost:44332",
                    claims: new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                        new Claim(ClaimTypes.Email, request.Email),
                        new Claim(ClaimTypes.Name, request.FirstName),
                        new Claim(ClaimTypes.Role, "user"),
                    },

                    expires: DateTime.Now.AddMinutes(5.0),
                    signingCredentials: signinCrifentioal
                    );
                var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

                //////////////Add in to Token Table 

                _userLoginService.AddToken(tokenstring, signeupResult.Data.UserId);

                ///////////////////////////////////
                ///

                _logger.LogInformation(" Signup {0} ", request.Email);

                return Ok(new { token = tokenstring });

            }
            else
            {
                return Ok(new ResultDto { IsSuccess = true, Message = "کاربر اضافه نشد" });
            }

            //var result = _registerUserService.Execute(user);
            ////insert JWT
            ////////////////////////////////////////////////////////////////
            //var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
            //var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
            //var tokenoption = new JwtSecurityToken(
            //    issuer: "https://localhost:44332",
            //    claims: new List<Claim>()
            //    {
            //            new Claim(ClaimTypes.NameIdentifier,result.Data.UserId.ToString()),
            //            new Claim(ClaimTypes.Email, user.Email),
            //            new Claim(ClaimTypes.Name, user.FirstName),
            //            new Claim(ClaimTypes.Role, "operator"),
            //    },
            //    expires: DateTime.Now.AddMinutes(30),
            //    signingCredentials: signinCrifentioal
            //    );
            //var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

            ////////////////Add in to Token Table 

            //_userLoginService.AddToken(tokenstring, result.Data.UserId);
            //////////////////////////////////////////////////////////

            //_logger.LogInformation("Inser User {0}", user.FirstName);
            //return Ok(result.Data);
        }

        [Area("Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            var result = _removeUserService.Execute(id);

            _logger.LogInformation(" Delete User {0}",id);
            return Ok(result);
        }

        //[Area("Admin")]
        //[HttpPut("{id}")]
        //public IActionResult EditUser([FromRoute] int id, [FromBody] RequestEdituserDto user)
        //{
        //    return Ok(_editeUsesrService.Execute(user));
        //}

        [Area("Admin")]
        //[HttpPut]
        [HttpPost]
        [Route("/Edite")]
        public IActionResult EditUser( [FromBody] RequestEdituserDto user)
        {
            var result = (_editeUsesrService.Execute(user));

            _logger.LogInformation(" Edite User {0}", user.FirstName);
            return Ok(result);
        }

        [Area("Admin")]
        [HttpPut]
        //[HttpPost]
        //[Route("/Update")]
        public IActionResult UpdateUser([FromBody] GetUsersDto user)
        {

            var resulrt = (_editeUsesrService.Execute2(user));


            _logger.LogInformation(" Delete User {0}", user.first_name);
            return Ok(resulrt);
        }


        [Area("Admin")]
        [HttpPut("{id}")]
        //[Route("/UserSatusChange")]
        public IActionResult UserSatusChange([FromRoute] int id)
        {
            var result = _userSatusChangeService.Execute(id);


            _logger.LogInformation(" Change StatuseCode User {0}", id);
            return Ok(result);
        }

        /////////////////////////////////////////////////////// 
        ///

    }
}
