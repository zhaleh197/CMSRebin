using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Persons.Commands.UserSatusChange
{
    public interface IUserSatusChangeService
    {
        ResultDto Execute(long UserId);
    }

    public class UserSatusChangeService : IUserSatusChangeService
    {
        private readonly IDatabaseContext _context;


        public UserSatusChangeService(IDatabaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long UserId)
        {
            var user = _context.Users.Find(UserId);
            if (user == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "کاربر یافت نشد"
                };
            }

            user.IsActive = !user.IsActive;
            user.UpdateTime = DateTime.Now;
            _context.SaveChanges();
            string userstate = user.IsActive == true ? "فعال" : "غیر فعال";
            return new ResultDto()
            {
                IsSuccess = true,
                Message = $"کاربر با موفقیت {userstate} شد!",
            };
        }
    }
}
