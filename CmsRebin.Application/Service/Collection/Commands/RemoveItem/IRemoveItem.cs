using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Commands.RemoveItem
{
    public interface IRemoveItem
    {
        ResultDto Execute(itemDto req);
    }
    public class RemoveItem : IRemoveItem
    {
        private readonly IDatabaseContext _context;

        public RemoveItem(IDatabaseContext context)
        {
            _context = context;
        }


        public ResultDto Execute(itemDto req)
        { 

            _context.DeleteItem(req.dbname, req.tname, req.iditem.ToString());
       

            _context.SaveChanges();
            //////
            ///
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ایتم با موفقیت حذف شد"
            };
        }


    }

    public class itemDto
    {
        public string tname { get; set; }
        public string dbname { get; set; }
        public long iditem { get; set; }

    }
}
