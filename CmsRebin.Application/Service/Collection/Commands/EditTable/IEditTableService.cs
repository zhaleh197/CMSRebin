using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Commands.EditTable
{
    public interface IEditTableService
    {
        ResultDto Execute(RequestEdittableDto request);
        ResultDto ExecuteAdmin(RequestEdittableAdminDto request);
    }
    public class EditTableService : IEditTableService
    {
        private readonly IDatabaseContext _context;

        public EditTableService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(RequestEdittableDto request)
        {
            var table = _context.Tables.Find(request.TableId);
            if (table == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "جدول یافت نشد"
                };
            }

            _context.EditTable(request.DBname, table.collection,request.collection);
            table.collection = request.collection;
            table.note = request.note;
            table.UpdateTime = DateTime.Now;
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش جدول انجام شد"
            };

        }
        public ResultDto ExecuteAdmin(RequestEdittableAdminDto request)
        {
            var table = _context.Tables.Find(request.TableId);
            if (table == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "جدول یافت نشد"
                };
            }

            _context.EditTable(request.DBname, table.collection, request.collection);
            table.collection = request.collection;
            table.note = request.note;
            table.UpdateTime = DateTime.Now;
            ////
            table.IsRemoved = request.isremove;

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش جدول انجام شد"
            };

        }
    }


    public class RequestEdittableDto
    {
        public long TableId { get; set; }
        public string collection { get; set; }
        public string note { get; set; }
        public string DBname { get; set; }
    }
    public class RequestEdittableAdminDto
    {
        public long TableId { get; set; }
        public string collection { get; set; }
        public string note { get; set; }
        public string DBname { get; set; }
        
        public bool isremove { get; set; }
    }
}