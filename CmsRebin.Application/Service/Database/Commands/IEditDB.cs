using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Database.Commands
{
    public interface IEditDB
    {
        ResultDto Execute(RequestEditDBDto request);
        ResultDto ExecuteAdmin(RequestEditDBAdminDto request);
    }
    public class EditDBService : IEditDB
    {
        private readonly IDatabaseContext _context;

        public EditDBService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEditDBDto request)
        {
            //var user = _context.Users.Find(request.UserId);
            var db = _context.DatabaseLists.Include(f => f.User).FirstOrDefault(p => p.id == request.Iddb);

            if (db == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "db یافت نشد"
                };
            }
            _context.EditBD(db.DBName, request.NameDB);
            db.DBName = request.NameDB;
            db.UpdateTime = DateTime.Now;
            //////new
            //user.Role.rolename = request.Role;
           
            //////
            ///change bd name
          //  ALTER DATABASE Test14000826 MODIFY NAME = Test14000826Edited
                
            //

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش db انجام شد"
            };

        }
        public ResultDto ExecuteAdmin(RequestEditDBAdminDto request)
        {
            //var user = _context.Users.Find(request.UserId);
            var db = _context.DatabaseLists.Include(f => f.User).FirstOrDefault(p => p.id == request.Iddb);

            if (db == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "db یافت نشد"
                };
            }
            _context.EditBD(db.DBName, request.NameDB);
            db.DBName = request.NameDB;
            db.UpdateTime = DateTime.Now;
            db.IsRemoved = request.IsRemove;
            //////new
            //user.Role.rolename = request.Role;

            //////
            ///change bd name
            //  ALTER DATABASE Test14000826 MODIFY NAME = Test14000826Edited

            //

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش db انجام شد"
            };

        }

    }

    public class RequestEditDBDto
    {
        public long Iddb { get; set; }
        public string NameDB { get; set; }


    }
    public class RequestEditDBAdminDto
    {
        public long Iddb { get; set; }
        public string NameDB { get; set; }
        public bool IsRemove { get; set; }


    }
}
