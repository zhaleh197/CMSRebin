using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Collection.Queris.GetItems;
using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using CmsRebin.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Persons.Commands.LoginUser
{
    public interface IUserLoginService
    {
        ResultDto<ResultUserloginDto> Execute(string Username, string Password);
        ResultDto<ResultUserloginDto2> Execute2(string Username, string Password, string DB);
        ResultDto AddToken(string Token, long userid);
        string GetToken(long userid);
        string GetToken2(string userid,string DB);
        Users GetUsertbyToken(string tok);
    }

    public class UserLoginService : IUserLoginService
    {
        private readonly IDatabaseContext _context;
        private readonly IGetEverythings _getEverythings;
        private readonly IGetItemsService _getItemsService;
        
        public UserLoginService(IDatabaseContext context,
             IGetEverythings getEverything , IGetItemsService getItemsService)
        {
            _getEverythings = getEverything;
            _context = context;
            _getItemsService = getItemsService;
        }

        public ResultDto AddToken(string token, long userid)
        {
            _context.Tokens.Add(new Tokens { id = userid, Token = token });
            _context.SaveChanges();
            return (new ResultDto { IsSuccess = true, Message = "Token Added." });
        }


        public ResultDto<ResultUserloginDto> Execute(string Username, string Password)
        {

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return new ResultDto<ResultUserloginDto>()
                {
                    Data = new ResultUserloginDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "نام کاربری و رمز عبور را وارد نمایید",
                };
            }
            var user = _context.Users
                .Include(p => p.Role)
                .Where(p => p.Email.Equals(Username)
                && p.IsActive == true)
                .FirstOrDefault();
            if (user == null)
            {
                return new ResultDto<ResultUserloginDto>()
                {
                    Data = new ResultUserloginDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "کاربری با این ایمیل ثبت نام نکرده است",
                };
            }
            var passwordHasher = new PasswordHasher();
            bool resultVerifyPassword = passwordHasher.VerifyPassword(user.Password, Password) || (user.Password == Password);
            if (resultVerifyPassword == false)
            {
                return new ResultDto<ResultUserloginDto>()
                {
                    Data = new ResultUserloginDto()
                    {

                    },
                    IsSuccess = false,
                    Message = "رمز وارد شده اشتباه است!",
                };
            }

            //List<string> roles = new List<string>();
            //foreach (var item in user.Role)
            //{
            //    roles.Add(item.Role);
            //}
            //
            return new ResultDto<ResultUserloginDto>()
            {
                Data = new ResultUserloginDto()
                {
                    ///new 14001107
                    Role = user.Role.rolename,
                    Email = user.Email,
                    //
                    UserId = user.id,
                    Name = user.first_name,
                    LastName = user.last_name,
                    //Gettoken and return Tokens by id
                    Token = GetToken(user.id)
                },
                IsSuccess = true,
                Message = "ورود به سایت با موفقیت انجام شد",
            };
        }

        public ResultDto<ResultUserloginDto2> Execute2(string Username, string Password, string DB)
        {

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                return new ResultDto<ResultUserloginDto2>()
                {
                    Data = new ResultUserloginDto2()
                    {

                    },
                    IsSuccess = false,
                    Message = "نام کاربری و رمز عبور را وارد نمایید",
                };
            }
            var user = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "UserName", "Password" }, Addcon = new List<string>() { "and" }, Compare = new List<string>() { "=", "=" }, Value = new List<string>() { Username, Password }, DBname = DB, Tablename = "Users" }, nametable = "Users", DbName = DB });

            if (user == null)
            {
                return new ResultDto<ResultUserloginDto2>()
                {
                    Data = new ResultUserloginDto2()
                    {

                    },
                    IsSuccess = false,
                    Message = "کاربری با این مشخصات ثبت نام نکرده است",
                };
            }
            //var passwordHasher = new PasswordHasher();
            //bool resultVerifyPassword = passwordHasher.VerifyPassword(user.Password, Password) || (user.Password == Password);
            //if (resultVerifyPassword == false)
            //{
            //    return new ResultDto<ResultUserloginDto>()
            //    {
            //        Data = new ResultUserloginDto()
            //        {

            //        },
            //        IsSuccess = false,
            //        Message = "رمز وارد شده اشتباه است!",
            //    };
            //}
            List<string> others = new List<string>();
            for (int i = 8; i < user.ITM[0].fieldnamelist.Count; i++)
            {
                others.Add(user.ITM[0].valuefiledlistList[i][0]);
            }
            return new ResultDto<ResultUserloginDto2>()
            {

                Data = new ResultUserloginDto2()
                {
                    ///new 14001107
                    //Role = "user",
                    Role = _getItemsService.getonevalueofitem(new Filterrequestonefild { fildgeted = "Rolename", DbName = DB, DTname = "Role", filters = new Infrastructure.Enum.Equation { Compare = new List<string>() { "=" }, DBname = DB, Tablename = "Role", Filname = new List<string>() { "id" }, Value = new List<string>() { user.ITM[0].valuefiledlistList[7][0] }, Addcon = new List<string>() } }),
                    Mobile = user.ITM[0].valuefiledlistList[5][0],
                    UserName = user.ITM[0].valuefiledlistList[5][0],
                    Token = GetToken2(user.ITM[0].valuefiledlistList[0][0], DB),
                    UserId = user.ITM[0].valuefiledlistList[0][0],
                    OtherFilds = new List<string>(others)
                },
                IsSuccess = true,
                Message = "ورود به سایت با موفقیت انجام شد",
            };
        }

        public string GetToken(long userid)
        {
            return _context.Tokens.Include(P => P.Users).Where(t => t.Users.id.Equals(userid)).FirstOrDefault().Token;
        }

        public string GetToken2(string userid, string DB)
        {

            var Token = _getEverythings.Execute2(new RequestGetDto { filters = new CmsRebin.Infrastructure.Enum.Equation { Filname = new List<string>() { "id" }, Compare = new List<string>() { "=" }, Value = new List<string>() { userid }, }, nametable = "Tokens", DbName = DB });

            return Token.ITM[0].valuefiledlistList[5][0];
        }
        public Users GetUsertbyToken(string tok)
        {
            return _context.Tokens.Where(t => t.Token.Equals(tok)).Include(P => P.Users).FirstOrDefault().Users;
        }

      
        //public Users GetTokenbyCode(long userid)
        //{
        //    var user = _context.Users
        //        .Include(p => p.Token)
        //        .Where(p => p.Equals(Username)
        //        && p.IsActive == true)
        //        .FirstOrDefault();
        //    return _context.Users.Include(t=>t.Token).Where(t => t.id.Equals(userid)).FirstOrDefault().Token;
        //}

    }
    public class ResultUserloginDto
    {
        public long UserId { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class ResultUserloginDto2
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Mobile { get; set; }
        public string Token { get; set; }
        //public string Firstname { get; set; }
        //public string Lastname { get; set; }
        public List<string> OtherFilds { get; set; }
        
    }


}

