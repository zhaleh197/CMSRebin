using Endpoint.WebClient.Models.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using CmsRebin.Common.Dto;
using System.Text.RegularExpressions;
using Endpoint.WebClient.Models;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Common;
using CmsRebin.Application.Service.Persons.Commands.LoginUser;
using Microsoft.AspNetCore.Identity;

namespace Endpoint.WebClient.Controllers
{
    public class Auth2Controller : Controller
    {
        IHttpClientFactory _httpClientFactory;

        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;


        public Auth2Controller(IHttpClientFactory httpClientFactory
            //, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager
            )
        {
            //_signInManager = signInManager;
            //_userManager = userManager;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult LoginAsync()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Login(LoginViewModel login)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return View(login);
        //    }
        //    var _client = _httpClientFactory.CreateClient("CMSRebinClient");
        //    var jasonBody = JsonConvert.SerializeObject(login);
        //    var content = new StringContent(jasonBody,Encoding.UTF8,"application/json");
        //    var responce = _client.PostAsync("/Api/Auth",content).Result;
        //    if(responce.IsSuccessStatusCode)
        //    {
        //        var token = responce.Content.ReadAsAsync<TokenModel>().result;
        //    }
        //    return View();
        //}


        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var _client = _httpClientFactory.CreateClient("CMSRebinClient");

            var jsonBody = JsonConvert.SerializeObject(login);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = _client.PostAsync("/api/Auth2/Post", content).Result; // In api Token midahad da sorat Login.
            if (response.IsSuccessStatusCode)
            {
                var resTok = response.Content.ReadFromJsonAsync<ResultUserloginDto>().Result;

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,resTok.UserId.ToString()),
                    new Claim(ClaimTypes.Email,login.Username),
                    new Claim(ClaimTypes.Name,resTok.Name),
                    new Claim(ClaimTypes.Role,resTok.Role),
                    new Claim("AccessToken",resTok.Token)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.Now.AddMinutes(5.0),
                };

                //this is login my user.
                await HttpContext.SignInAsync(principal, properties);//saveincooki
                //return  Redirect("/User");
                return Redirect("/Home/Index");
                //return RedirectToAction("Home","");
            }
            else
            {
                ModelState.AddModelError("Username", "User Not Valid");
                return View(login);
            }





        }


        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(SignUpViewModel request)
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

            // var signeupResult = _registerUserService.Execute();

            var sinhupForm = new RequestRegisterUserDto
            {
                Email = request.Email,
                FirstName = request.Firstname,
                LastName = request.Lastname,
                Password = request.Password,
                RePasword = request.RePassword,
                role = new PasswordHasher().HashPassword("user"),
                isactive = true,
                //roles = new List<RolesInRegisterUserDto>()
                //{
                //     new RolesInRegisterUserDto { Id = 3},
                //}
            };


            var _client = _httpClientFactory.CreateClient("CMSRebinClient");

            var jsonBody = JsonConvert.SerializeObject(sinhupForm);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = _client.PostAsync("/api/Auth2/Signup", content).Result; // In api Token midahad da sorat Login.
                                                                                  // List<GetDBDto> DbsList = JsonConvert.DeserializeObject<List<GetDBDto>>(result);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Auth", "LoginAsync");

                return Ok(new ResultDto { IsSuccess = true, Message = "کاربر اضافه شد" });
                ///////////////////////////////login 
                //    if (signeupResult.IsSuccess == true)
                ////{
                //    var claims = new List<Claim>()
                //{
                //    new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                //    new Claim(ClaimTypes.Email, request.Email),
                //    new Claim(ClaimTypes.Name, request.Firstname),
                //    new Claim(ClaimTypes.Role, "operator"),
                //};


                //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var principal = new ClaimsPrincipal(identity);
                //var properties = new AuthenticationProperties()
                //{
                //    IsPersistent = true
                //};
                //HttpContext.SignInAsync(principal, properties);

            }
            //return RedirectToAction("Auth", "login");
            //return View(request);
            return Ok(new ResultDto { IsSuccess = false, Message = "کاربر اضافه نشد" });
            //return Redirect("/Auth/login");

        }

        public IActionResult SignOutt()
        {
            //_signInManager.SignOutAsync();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Auth2");
        }
    }
}
