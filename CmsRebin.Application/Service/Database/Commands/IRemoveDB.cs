using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Database.Commands
{
   public interface IRemoveDB
    {
        ResultDto Execute(long dbId);
        ResultDto ExecuteADmin(long dbId);
    }
    public class RemoveDBService : IRemoveDB
    {
        private readonly IDatabaseContext _context;

        public RemoveDBService(IDatabaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(long dbId)
        {

            var db = _context.DatabaseLists.Find(dbId);
            if (db == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "db یافت نشد"
                };
            }
            db.RemoveTime = DateTime.Now;
            db.IsRemoved = true;

            ///delet db raastrasti
           // _context.DeleteDb(db.DBName);// delet1 not need delet totaly


            ///delet all table of this DB //mahz etminan. 
            ///
            var tablelist = _context.Tables.Where(t => t.IdDBase == dbId);
            foreach (var tabl in tablelist)
            {
                tabl.IsRemoved = true;
                ///delete all filed of this table
                var fildsthistable = _context.FieldsofTable.Where(t => t.IdTable == tabl.id);
                foreach (var f in fildsthistable)
                    f.IsRemoved = true;
                /////
            }
            ///



            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "db با موفقیت حذف شد"
            };
        }
        public ResultDto ExecuteADmin(long dbId)
        {

            var db = _context.DatabaseLists.Find(dbId);
            if (db == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "db یافت نشد"
                };
            }
            db.RemoveTime = DateTime.Now;
            db.IsRemoved = true;
            _context.SaveChanges();
            ///delet db raastrasti- Paiiin neveshtam- Exeption midad. migofy is used alredy.
            //_context.DeleteDb(db.DBName);//delet2. delet Totaly.


            ///delet all table of this DB
            ///
            var tablelist = _context.Tables.Where(t => t.IdDBase == dbId);
            foreach (var tabl in tablelist)
            {
                tabl.IsRemoved = true;


                ///
                /// 1401-1-07
                  ///////////////////////////////
                _context.DeleteTable(db.DBName, tabl.collection);
                ///////////////////////////////

                ///delete all filed of this table
                var fildsthistable = _context.FieldsofTable.Where(t => t.IdTable == tabl.id);

                foreach (var f in fildsthistable)
                {
                    f.IsRemoved = true;

                    /// 1401-1-07
                     ///////////////////////////////
                    _context.DeleteFiled(db.DBName, tabl.collection, f.fieldname);
                    ///////////////////////////////
                }
                /////
            }




            /////////////////////////////////
            _context.DeleteDb(db.DBName);//delet2. delet Totaly.
            _context.SaveChanges();
             //////////////////////////////////////


            return new ResultDto()
            {
                IsSuccess = true,
                Message = "db با موفقیت حذف شد"
            };
        }
    }
}
