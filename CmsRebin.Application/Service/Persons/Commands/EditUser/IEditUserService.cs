using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Persons.Queries.GetUsers;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Persons.Commands.EditUser
{
    public interface IEditUserService
    {
        ResultDto Execute(RequestEdituserDto request);
        ResultDto Execute2(GetUsersDto request);
    }
    public class EditUserService : IEditUserService
    {
        private readonly IDatabaseContext _context;

        public EditUserService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEdituserDto request)
        {
            //var user = _context.Users.Find(request.UserId);
            var user= _context.Users.Include(f => f.Role).FirstOrDefault(p => p.id == request.UserId);

            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            user.first_name = request.FirstName;
            user.last_name = request.LasttName;
            //////new
            //user.Role.rolename = request.Role;
            user.UpdateTime = DateTime.Now;

            //////

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش کاربر انجام شد"
            };

        }

        public ResultDto Execute2(GetUsersDto request)
        {
            var user = _context.Users.Include(f => f.Role).FirstOrDefault(p => p.id == request.Id);

            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            user.first_name = request.first_name;
            user.last_name = request.last_name;
            //////new
            user.Role=_context.Role.Where(r=>r.rolename.Equals(request.role)).FirstOrDefault();
            user.IsActive = request.IsActive;

            //////

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش کاربر انجام شد"
            };
        }
    }


    public class RequestEdituserDto
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LasttName { get; set; }
        /// new
        //public string Role { get; set; }
        
        ////
    }
}
