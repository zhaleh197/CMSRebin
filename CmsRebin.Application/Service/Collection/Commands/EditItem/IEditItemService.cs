using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Collection.Commands.CreateItem;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Commands.EditItem
{
    public interface IEditItemService
    {
        ResultDto Execute(ItemdDto request);
        ResultDto Executeone(ItemdDto request);
    }
    public class EditItemService : IEditItemService
    {
        private readonly IDatabaseContext _context;

        public EditItemService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(ItemdDto request)
        {
            _context.EditItem(request);

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش آتیم انجام شد"
            };

        }
        public ResultDto Executeone(ItemdDto request)
        {
            _context.EditoneItem(request);

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش آتیم انجام شد"
            };

        }

    }

}
