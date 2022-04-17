using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Filed.Commands.AddField;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Filed.Commands.EditField
{
   public interface IEditedField
    {

        ResultDto Execute(FiledDto request);
    }
    public class EditedField : IEditedField
    {
        private readonly IDatabaseContext _context;

        public EditedField(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(FiledDto request)
        {
            var filed = _context.FieldsofTable.Find(request.id);
            if (filed == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "Field یافت نشد"
                };
            }
            

            filed.fieldname = request.name;
            filed.relation = request.Relation;
            filed.interfaces = request.type;
            filed.UpdateTime = DateTime.Now;

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "ویرایش Field انجام شد"
            };

        }
    }
}
