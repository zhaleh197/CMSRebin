using CmsRebin.Application.Service.Database.Queris.GetDB;
using CmsRebin.Application.Service.Persons.Commands.EditUser;
using CmsRebin.Application.Service.Persons.Commands.RegisteUser;
using CmsRebin.Application.Service.Persons.Queries.GetRoles;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using CmsRebin.Domain.Entities.Persons;
using Endpoint.WebClient.Controllers;
using Endpoint.WebClient.Models;
using Microsoft.AspNetCore.Authorization;
//using Endpoint.WebClient.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.WebClient.Area.Controllers.UserController
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserRepository userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            string token = User.FindFirst("AccessToken").Value;

            return View(_userRepository.Getalluser(token));
        }

        public IActionResult CreatUser()
        {
            string token = User.FindFirst("AccessToken").Value;
            //List<RolesDto> roles = _userRepository.GetRoles(token);
            //  ViewData["Roles"] = roles;

            ViewBag.Roles = new SelectList(_userRepository.GetRoles(token), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreatUser(RequestRegisterUserDto user)
        {
            string token = User.FindFirst("AccessToken").Value;
            
            _userRepository.Adduser(user, token);
            return RedirectToAction("Index");
        }
        public IActionResult GetRoles()
        {
            string token = User.FindFirst("AccessToken").Value;

            return View(_userRepository.GetRoles(token));
        }
        [Area("Admin")]
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            string token = User.FindFirst("AccessToken").Value;
            var customer = _userRepository.Getuserbyid(id, token);
            return View(customer);
        }

        //[HttpGet("{id}")]
        //[Route("Show")]


        //public IActionResult Show(int id)
        //{
        //    string token = User.FindFirst("AccessToken").Value;
        //    var customer = _userRepository.Getuserbyid(id, token);
        //    return View(customer);


        //}


        public IActionResult Edit(int id)
        {
            string token = User.FindFirst("AccessToken").Value;
            var customer = _userRepository.Getuserbyid(id, token);

            ViewBag.Roles = new SelectList(_userRepository.GetRoles(token), "Id", "Name");
            return View(customer);
        }

        //[HttpPut]
        [HttpPost]
        public IActionResult Edit(GetUsersDto customer)
        {
            string token = User.FindFirst("AccessToken").Value;

            _userRepository.Updateuser(customer,token);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// ////////////////////////////////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public IActionResult Update(int id)
        {
            string token = User.FindFirst("AccessToken").Value;
            var customer = _userRepository.Getuserbyid(id, token);

            ViewBag.Roles = new SelectList(_userRepository.GetRoles(token), "Id", "Name");

            return View(customer);
        }

        [HttpPut] 
        public IActionResult Update(GetUsersDto customer)
        {
            string token = User.FindFirst("AccessToken").Value;

            _userRepository.Updateuser(customer, token);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// /////////////////
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //[HttpDelete]
        //[HttpPost]
        public IActionResult Delete(int id)
        {

            string token = User.FindFirst("AccessToken").Value;

            _userRepository.DeleteUser(id, token);
            return RedirectToAction("Index");
        }
        
        /// ////////////////////////////////////
        //[HttpPost]
        //[HttpPut]
        public IActionResult ChangeStatuse(GetUsersDto customer)
        {
            string token = User.FindFirst("AccessToken").Value;

            _userRepository.UserSatusChange(customer, token);
            return RedirectToAction("Index");
        } 

        /// ////////////////////////////////////
       

        public IActionResult Privacy()
        {
            return View();
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult showdialog(int idy)
        {
            string token = User.FindFirst("AccessToken").Value;
            var y = _userRepository.Getuserbyid(idy, token);
            return PartialView(y);
        }

    }
}
