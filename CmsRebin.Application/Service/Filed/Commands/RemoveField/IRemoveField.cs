using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Filed.Commands.RemoveField
{
    public interface IRemoveField
    {
        ResultDto Execute(long FId);
        ResultDto ExecuteAdmin(long FId);
    }
    public class RemoveField : IRemoveField
    {
        private readonly IDatabaseContext _context;

        public RemoveField(IDatabaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(long FId)
        {

            var filed = _context.FieldsofTable.Find(FId);
            if (filed == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "فیلد یافت نشد"
                };
            }


            ////chek if this table has relation - 1400-11-17
            var rel = _context.RelationsofTable.Where(r => r.many_field == FId).FirstOrDefault();
            if (rel != null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این فیلد در ارتباط با فیلدهای دییگر است و نمیتوان ان را حذف کرد. برای حذف این فیلد، ابتدا فیلدهای وابسته را حذف کنید."
                };
            }
            //////////////////
            ///


            filed.RemoveTime = DateTime.Now;
            filed.IsRemoved = true;
            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "فیلد با موفقیت حذف شد"
            };
        }

        public ResultDto ExecuteAdmin(long FId)
        {

            var filed = _context.FieldsofTable.Find(FId);
            if (filed == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "فیلد یافت نشد"
                };
            }
            ////chek if this table has relation - 1400-11-17
            var rel = _context.RelationsofTable.Where(r => r.many_field == FId).FirstOrDefault();
            if (rel != null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "این فیلد در ارتباط با فیلدهای دییگر است و نمیتوان ان را حذف کرد. برای حذف این فیلد، ابتدا فیلدهای وابسته را حذف کنید."
                };
            }
            //////////////////
            ///
            filed.RemoveTime = DateTime.Now;
            filed.IsRemoved = true;

            ///delet Table raastrasti- Admin
            ///
            var Table = _context.Tables.Where(i => i.id.Equals(filed.IdTable)&&(i.IsRemoved==false)).FirstOrDefault();
            var dbname = _context.DatabaseLists.Where(i => i.id.Equals(Table.IdDBase) && (i.IsRemoved == false)).FirstOrDefault();
             
            _context.DeleteFiled(dbname.DBName, Table.collection, filed.fieldname);
            //_context.SaveChanges();
            //
            ////delete this table from Tables
            ///1400-11-16
            _context.FieldsofTable.Remove(filed);
            _context.SaveChanges();
            //////
            ///
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "فیلد با موفقیت حذف شد"
            };
        }

         
    }
}
