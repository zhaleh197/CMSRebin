using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using Endpoint.Site.Models.ViewModels.AuthenticationViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Endpoint.Site.Controllers
{

    public class AuthenticationController : Controller
    {

        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;

        public AuthenticationController(IRegisterUserService registerUserService, IUserLoginService userLoginService)
        {
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(SignupViewModel request)
        {

            ////autontiction
            if (string.IsNullOrWhiteSpace(request.Firstname) ||
                string.IsNullOrWhiteSpace(request.Lastname) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.RePassword))
              
            {
                return Json(new ResultDto { IsSuccess = false, Message = "لطفا تمامی موارد رو ارسال نمایید" });
            }

            if (User.Identity.IsAuthenticated == true)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "شما به حساب کاربری خود وارد شده اید! و در حال حاضر نمیتوانید ثبت نام مجدد نمایید" });
            }
            if (request.Password != request.RePassword)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "رمز عبور و تکرار آن برابر نیست" });
            }
            if (request.Password.Length < 8)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "رمز عبور باید حداقل 8 کاراکتر باشد" });
            }

            string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";

            var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return Json(new ResultDto { IsSuccess = true, Message = "ایمیل خودرا به درستی وارد نمایید" });
            }
            ///////////////////////////

            var signeupResult = _registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = request.Email,
                FirstName = request.Firstname,
                LastName = request.Lastname,
                Password = request.Password,
                RePasword = request.RePassword,
                role = new PasswordHasher().HashPassword("operator"),
                isactive =true,
            //roles = new List<RolesInRegisterUserDto>()
            //{
            //     new RolesInRegisterUserDto { Id = 3},
            //}
        });


            ///////////////////////////////login 
            if (signeupResult.IsSuccess == true)
            {
                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Name, request.Firstname),
                new Claim(ClaimTypes.Role, "operator"),
            };


                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, properties);

            } 
            return Json(signeupResult);
        }


        public IActionResult Signin(string ReturnUrl = "/")
        {
            ViewBag.url = ReturnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Signin(string Email, string Password, string url = "/")
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest("Modle is not valid.");
                ModelState.AddModelError(Email,"Model not valid");
                return View();
            }

            var signupResult = _userLoginService.Execute(Email, Password);

            if (signupResult.IsSuccess == false)
            {
                ModelState.AddModelError(Email,"user not found");
                //return Unauthorized();
                return View();
            }
            else
            {
                //var claims = new List<Claim>()
                //{
                //new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
                //new Claim(ClaimTypes.Email, Email),
                //new Claim(ClaimTypes.Name, signupResult.Data.Name),
                //new Claim(ClaimTypes.Surname, signupResult.Data.LastName),
                //};

                ////foreach (var item in signupResult.Data.Roles)
                ////{
                ////    claims.Add(new Claim(ClaimTypes.Role, item));
                ////}

                //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var principal = new ClaimsPrincipal(identity);
                //var properties = new AuthenticationProperties()
                //{
                //    IsPersistent = true,
                //    ExpiresUtc = DateTime.Now.AddDays(5),
                //};
                //HttpContext.SignInAsync(principal, properties);


                var searchkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OurVerifyCMSREBIN"));
                var signinCrifentioal = new SigningCredentials(searchkey, SecurityAlgorithms.HmacSha256);
                var tokenoption = new JwtSecurityToken(
                    issuer: "https://localhost:44332",
                    claims: new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, Email),
                        new Claim(ClaimTypes.Role,"admin")
                    },
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: signinCrifentioal
                    );

                var tokenstring = new JwtSecurityTokenHandler().WriteToken(tokenoption);

                //////////////Add in to Token Table 
                _userLoginService.AddToken(tokenstring, signupResult.Data.UserId);
                ///////////////////////////////////
                ///
                /////login mimanad. 
                //var claims = new List<Claim>()
                //{
                //new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
                //new Claim(ClaimTypes.Name, signupResult.Data.Name),
                //new Claim("AccessToken",tokenstring)

                //};


                //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var principal = new ClaimsPrincipal(identity);
                //var properties = new AuthenticationProperties()
                //{
                //    IsPersistent = true,
                //    ExpiresUtc = DateTime.Now.AddDays(5),
                //    AllowRefresh=true
                //};
                //HttpContext.SignInAsync(principal, properties);
                /////////////////////////////////////////

                return Json(new ResultDto { IsSuccess=true, Message= tokenstring ,});

            }
            //return Json(signupResult);
        }


        public IActionResult SignOutt()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
