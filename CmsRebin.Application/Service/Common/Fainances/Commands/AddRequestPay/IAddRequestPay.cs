using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Common.Fainances.Commands.AddRequestPay
{
    public interface IAddRequestPay
    {
        //ResultDto<ResultRequestPayDto> Execute(int Amount, long UserId);
    }


    public class AddRequestPayService : IAddRequestPay
    {
        private readonly IDatabaseContext _context;
        public AddRequestPayService(IDatabaseContext context)
        {
            _context = context;
        }
        //public ResultDto<ResultRequestPayDto> Execute(int Amount, long UserId)
        //{
        //    var user = _context.Users.Find(UserId);

        //    RequestPay requestPay = new RequestPay()
        //    {
        //        Amount = Amount,
        //        Guid = Guid.NewGuid(),
        //        IsPay = false,
        //        User = user,
        //    };

        //    _context.RequestPays.Add(requestPay);

        //    _context.SaveChanges();

        //    return new ResultDto<ResultRequestPayDto>()
        //    {
        //        Data = new ResultRequestPayDto
        //        {
        //            guid = requestPay.Guid,
        //            Amount = requestPay.Amount,
        //            Email = user.Email,
        //            RequestPayId = requestPay.Id,
        //        },
        //        IsSuccess = true,
        //    };
        //}
    }

    public class ResultRequestPayDto
    {
        public Guid guid { get; set; }
        public int Amount { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public long RequestPayId { get; set; }

    }

    public class RequestCreatPayDto
    {
        public string DBName { get; set; }

        public long IdUserOwner { get; set; }
    }
    public class RequestPayDto
    {
        public string DBName { get; set; }
        //public long IdUserOwner { get; set; }
        //public bool SendSMS { get; set; }
        //public bool SendEmail { get; set; }
        //public bool SendLinkPay { get; set; }

        /// <summary>
        public string Mobile { get; set; }
        /// </summary>
        //fildssss
        public string Guid { get; set; }
        public int IdUser { get; set; }
        public int Amount { get; set; }
        public bool IsPay { get; set; }
        public DateTime PayDate { get; set; }
        public int RefId { get; set; }

    }

}