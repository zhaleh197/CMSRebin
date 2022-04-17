using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using CmsRebin.Domain.Entities.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Database.Commands
{
    public interface ICreatDB
    {
        ResultDto<ResultCreateDBDto> Execute(RequestCreateDBDto request);
    }


    public class CreatDBService : ICreatDB
    {
        private readonly IDatabaseContext _context;
        public CreatDBService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultCreateDBDto> Execute(RequestCreateDBDto request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.DBName))
                {
                    return new ResultDto<ResultCreateDBDto>()
                    {
                        Data = new ResultCreateDBDto()
                        {
                            DBname = request.DBName
                        },
                        IsSuccess = false,
                        Message = "نام DB را وارد نمایید"
                    };
                }
                
                if(_context.DatabaseLists.Where(db=>db.DBName.Equals(request.DBName)).Count()>0)
                {
                    return new ResultDto<ResultCreateDBDto>()
                    {
                        Data = new ResultCreateDBDto()
                        {
                            DBname = request.DBName
                        },
                        IsSuccess = false,
                        Message = "نام DB تکراری ست"
                    };
                }
                int res=_context.CreatBD(request.DBName);
                if(res==1)
                _context.DatabaseLists.Add(
                     new DatabaseList { 
                         DBName = request.DBName,
                         User =_context.Users.FirstOrDefault(i=>i.id.Equals( request.IdUserOwner)),
                         InsertTime = DateTime.Now,
                         IsRemoved = false 
                     }
                    //new DatabaseList{ DBName = request.DBName, IdUser = request.IdUserOwner,InsertTime=DateTime.Now,IsRemoved=false }
                    );

                _context.SaveChanges();

                return new ResultDto<ResultCreateDBDto>()
                {
                    Data = new ResultCreateDBDto()
                    {
                        DBname = request.DBName
                    },
                    IsSuccess = true,
                    Message = " DB اضافه شد",
                };
            }
            catch (Exception)
            {
                return new ResultDto<ResultCreateDBDto>()
                {
                    Data = new ResultCreateDBDto()
                    {
                        DBname = request.DBName
                    },
                    IsSuccess = false,
                    Message = " DB اضافه نشد !!!!"
                };
            }
        }

    }
    public class RequestCreateDBDto
    {

        public string DBName { get; set; }
        public long IdUserOwner { get; set; }
    }

    public class ResultCreateDBDto
    {
        public string DBname { get; set; }
    }


}
