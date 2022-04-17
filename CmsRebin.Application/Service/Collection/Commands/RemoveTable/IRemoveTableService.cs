using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Filed.Commands.RemoveField;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Commands.RemoveTable
{
  public interface IRemoveTableService
    {
        ResultDto Execute(long TableId);
        ResultDto ExecuteAdmin(long TableId);
    }
    public class RemoveTableService : IRemoveTableService
    {
        private readonly IDatabaseContext _context;
        private readonly IRemoveField _removeField;


        public RemoveTableService(IDatabaseContext context, IRemoveField removeField)
        {
            _context = context;
            _removeField=removeField;
        }


        public ResultDto Execute(long TableId)
        {
            //var table = _context.Tables.Where(t=>t.id==TableId&&t.IsRemoved==false).FirstOrDefault();
            var table = _context.Tables.Find(TableId);//== balayy
            if (table == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "جدول یافت نشد"
                };
            }

            ////chek if this table has relation - 1400-11-12
            var rel = _context.RelationsofTable.Where(r => r.many_collection == table.id).FirstOrDefault();
            if (rel != null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این جدول در ارتباط با جداول دییگر است و نمیتوان ان را حذف کرد. برای حذف این جدول، ابتدا جداول وابسته را حذف کنید."
                };
            }
            //////////////////
            table.RemoveTime = DateTime.Now;
            table.IsRemoved = true;



            /////delet Table raastrasti
            /////
            //var dbname = _context.DatabaseLists.Where(i => i.id.Equals(table.IdDBase)).FirstOrDefault().DBName;
            //_context.DeleteTable(dbname, table.collection);



            ///delete all filed of this table
            ///

            var fildsthistable = _context.FieldsofTable.Where(t => t.IdTable == TableId);
            foreach (var f in fildsthistable)
                f.IsRemoved = true;
            /////
            ///

            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "جدول با موفقیت حذف شد"
            };
        }
        public ResultDto ExecuteAdmin(long TableId)
        {

            //var table = _context.Tables.Where(t=>t.id==TableId&&t.IsRemoved==false).FirstOrDefault();
            var table = _context.Tables.Find(TableId);//== balayy
            if (table == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "جدول یافت نشد"
                };
            }

            ////chek if this table has relation - 1400-11-12
            var rel = _context.RelationsofTable.Where(r => r.many_collection == table.id).FirstOrDefault();
            if (rel != null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این جدول در ارتباط با جداول دییگر است و نمیتوان ان را حذف کرد. برای حذف این جدول، ابتدا جداول وابسته را حذف کنید."
                };
            }
            //////////////////

            table.RemoveTime = DateTime.Now;
            table.IsRemoved = true;



            

            ///delete all filed of this table
            ///

            var fildsthistable = _context.FieldsofTable.Where(t => t.IdTable == TableId);
            foreach (var f in fildsthistable)
                _removeField.ExecuteAdmin(f.id);
            /////


            ///delet Table raastrasti- Admin
            ///
            var dbname = _context.DatabaseLists.Where(i => i.id.Equals(table.IdDBase)).FirstOrDefault().DBName;
            _context.DeleteTable(dbname, table.collection);




            _context.SaveChanges();


            //
            ////delete this table from Tables
            ///1400-11-16
            _context.Tables.Remove(table);
            _context.SaveChanges();
            //////
            ///
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "جدول با موفقیت حذف شد"
            };
        }


    }
}
