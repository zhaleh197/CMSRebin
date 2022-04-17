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

namespace Endpoint.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase

    {

        private readonly ILogger<AuthController> _logger;


        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;

        private readonly IGetUsersService _getUsersService;
        public AuthController(IGetUsersService getUsersService, IRegisterUserService registerUserService, IUserLoginService userLoginService, ILogger<AuthController> logger)
        {
            _logger = logger;
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
            _getUsersService = getUsersService;
        }

        //[HttpPost("singnup")]
        [HttpPost]
        public IActionResult Signup([FromBody] RequestRegisterUserDto request)
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
            request.role =  "user";
            request.isactive = true;
            ////////////////////
            ///
            /// 
            ///////   check email 
            var chekemai = _getUsersService.IsUserExistbyemail(request.Email);
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

                _logger.LogInformation(" Signup {0} ", request.Email );

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
        public IActionResult Post([FromBody] Login login)//// = Login ast.
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid.");
            }

            var signupResult = _userLoginService.Execute(login.Username, login.Password);


            if (signupResult.IsSuccess == false)
            {
                return Unauthorized();
            }
            else
            {
                //var claims = new List<Claim>()
                //{
                //new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
                //new Claim(ClaimTypes.Email, login.Email),
                //new Claim(ClaimTypes.Name, signupResult.Data.Name),
                //new Claim(ClaimTypes.Surname, signupResult.Data.LastName),
                //};


                //  //////
                //  var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
                //  var signinCrifentioal = new SigningCredentials(searchkey,SecurityAlgorithms.HmacSha256);
                //  var tokenoption = new JwtSecurityToken(
                //      issuer:"https://localhost:44332",
                //      claims: new List<Claim>()
                //      {
                //          new Claim(ClaimTypes.Name, login.Email),
                //          new Claim(ClaimTypes.Role,"admin"),
                //      },
                //      expires: DateTime.Now.AddMinutes(30),
                //      signingCredentials: signinCrifentioal
                //      );
                //  var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

                //  //////////////Add in to Token Table 

                ////var tokenstring =_userLoginService.GetToken(signupResult.Data.UserId);
                var tokenstring = signupResult.Data.Token;

                //  ///////////////////////////////////
                ///


                _logger.LogInformation(" Signup {0} ", signupResult.Data.UserId);
                return Ok(signupResult.Data);


                //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var principal = new ClaimsPrincipal(identity);
                //var properties = new AuthenticationProperties()
                //{
                //    IsPersistent = true,
                //    ExpiresUtc = DateTime.Now.AddDays(5),
                //};
                //HttpContext.SignInAsync(principal, properties);

            }
            //return Ok(signupResult);
        }


        //public IActionResult SignOutt()
        //{
        //    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        //    return RedirectToAction("Index", "Home");
        //}
    }
}
