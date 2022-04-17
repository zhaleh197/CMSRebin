using CmsRebin.Application.Interface.Context;
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using CmsRebin.Domain.Entities.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Application.Service.Collection.Queris.GetItems;

namespace CmsRebin.Application.Service.Persons.Commands.RegisteUser
{
   public interface IRegisterUserService
    {
        ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request);

        ResultDto<ResultRegisterUserDto> Execute2(RequestRegisterUserDto2 request);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDatabaseContext _context;
        private readonly ICreateItem _createItem;
        private readonly IGetItemsService _getItemsService;
        

        public RegisterUserService(IDatabaseContext context,
            ICreateItem createItem, IGetItemsService getItemsService)
        {
            _context = context;
            _createItem = createItem;
            _getItemsService = getItemsService;
        }
        public ResultDto<ResultRegisterUserDto> Execute(RequestRegisterUserDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "پست الکترونیک را وارد نمایید"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.FirstName))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام را وارد نمایید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.LastName))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "نام خانوادگی را وارد نمایید"
                    };
                }
                if (string.IsNullOrWhiteSpace(request.Password))
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمز عبور را وارد نمایید"
                    };
                }
                if (request.Password != request.RePasword)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "رمز عبور و تکرار آن برابر نیست"
                    };
                }
                string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";

                var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new ResultDto<ResultRegisterUserDto>()
                    {
                        Data = new ResultRegisterUserDto()
                        {
                            UserId = 0,
                        },
                        IsSuccess = false,
                        Message = "ایمیل خودرا به درستی وارد نمایید"
                    };
                }


                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(request.Password);
                //var hashedRole= passwordHasher.HashPassword("user");

                //var rolhash = _context.Roles.Where(p => p.id.Equals(passwordHasher.HashPassword(request.role))).FirstOrDefault();//request.role="admin" .

                var rolhash = _context.Role.Where(p => p.rolename.Equals(request.role)).FirstOrDefault();//request.role="admin" .
                if (rolhash == null)
                    rolhash = _context.Role.Where(p => p.id.Equals(request.role)).FirstOrDefault();//id

                Users user = new Users()
                {
                    Email = request.Email,
                    first_name = request.FirstName,
                    last_name = request.LastName,
                    Password = hashedPassword,
                    Role = rolhash,
                    IsActive = request.isactive,
                    InsertTime = DateTime.Now,
                    // Token=
                };

                //List<UserInRole> userInRoles = new List<UserInRole>();
                //foreach (var item in request.roles)
                //{
                //    var roles = _context.Roles.Find(item.Id);
                //    userInRoles.Add(new UserInRole
                //    {
                //        Role = roles,
                //        RoleId = roles.Id,
                //        User = user,
                //        UserId = user.Id,
                //    });
                //}
                //user.UserInRoles = userInRoles;

                _context.Users.Add(user);


                _context.SaveChanges();

                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = user.id,
                    },
                    IsSuccess = true,
                    Message = "ثبت نام کاربر انجام شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد !"
                };
            }
        }

        public ResultDto<ResultRegisterUserDto> Execute2(RequestRegisterUserDto2 request)
        {
            try
            {
                //foreach(var n in ...)
                //{
                    if (string.IsNullOrWhiteSpace(request.Mobile)|| 
                    string.IsNullOrWhiteSpace(request.Password)|| 
                    string.IsNullOrWhiteSpace(request.RePasword))
                    {
                        return new ResultDto<ResultRegisterUserDto>()
                        {
                            Data = new ResultRegisterUserDto()
                            {
                                UserId = 0,
                            },
                            IsSuccess = false,
                            Message = "همه مقادیر را وارد نمایید به درستی."
                        };
                    }
                //}
                try { int r = Convert.ToInt32(request.role); }
                catch
                {
                    request.role = _getItemsService.getonevalueofitem(new Filterrequestonefild { fildgeted = "id", DbName = request.DB, DTname = "Role", filters = new Infrastructure.Enum.Equation { Compare = new List<string>() { "=" }, DBname = request.DB, Tablename = "Role", Filname = new List<string>() { "Rolename" }, Value = new List<string>() { request.role }, Addcon = new List<string>() } });
                }
                //
                var user = _createItem.Execute(new RequestCreateItemdDto { DbName = request.DB, TableName = "Users", I = new List<string>() { request.Mobile, request.Password ,request.role}, S = new List<string>() { "UserName", "Password","Roleid" } });

                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                       UserId = user.Data.ItemId,
                    },
                    IsSuccess = true,
                    Message = "ثبت نام کاربر انجام شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultRegisterUserDto>()
                {
                    Data = new ResultRegisterUserDto()
                    {
                        UserId = 0,
                    },
                    IsSuccess = false,
                    Message = "ثبت نام انجام نشد !"
                };
            }
        }
    }
    public class RequestRegisterUserDto2
    {
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        public string Mobile { get; set; }//==username
        public string Password { get; set; }
        public string RePasword { get; set; }
        public string role { get; set; }
        public bool isactive { get; set; }
        public string DB { get; set; }

    }
    public class RequestRegisterUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePasword { get; set; }
        public string role { get; set; }
        public bool isactive { get; set; }


        ///////1400-11-23 - change and dynamic user Table in any DB for Auth
        public string DbName { get; set; }
        public string Tname { get; set; }
    }

    public class RolesInRegisterUserDto
    {
        public long Id { get; set; }
    }

    public class ResultRegisterUserDto
    {
        public long UserId { get; set; }
    }
}
