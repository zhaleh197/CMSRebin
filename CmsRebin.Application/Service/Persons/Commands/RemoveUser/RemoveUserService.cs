using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;

namespace CmsRebin.Application.Service.Persons.Commands.RemoveUser
{
    public class RemoveUserService : IRemoveUserService
    {
        private readonly IDatabaseContext _context;

        public RemoveUserService(IDatabaseContext context)
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
            user.RemoveTime = DateTime.Now;
            user.IsRemoved = true;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "کاربر با موفقیت حذف شد"
            };
        }
    }
}
