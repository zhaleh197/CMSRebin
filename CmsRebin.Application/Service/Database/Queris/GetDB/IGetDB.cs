
using CmsRebin.Application.Interface.Context;
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using CmsRebin.Domain.Entities.Database;
using CmsRebin.Domain.Entities.Persons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
//using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Database.Queris.GetDB
{
   public interface IGetDB
    {
        public ReslutGetDBDto Executeall(RequesDBDto request);
        public ReslutGetDBDto Execute(RequesDBDto request);
        //Task<ReslutGetDBDto> GetDbbyIdAsync(int id);
        public ReslutGetDBDto GetDbbyIdAsync2(int id);
        public long GetDbbyIdAsync3(string DB);
        public bool ISDBExist(int id);
        public ResultDto downloadDB (int id,string filename);
    }
    public class GetDBService : IGetDB
    {
        IMemoryCache _memoryCache;
        private readonly IDatabaseContext _context;
        public GetDBService(IDatabaseContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public ReslutGetDBDto Executeall(RequesDBDto request)
        {
            //List< DatabaseList>databaselist = _context.DatabaseLists.ToList();
            var databaseLists = _context.DatabaseLists.Include(i => i.User).Include(i => i.User.Role).ToList();
            //var databaseLists = _context.DatabaseLists.Include(i=>i.User).IgnoreQueryFilters().ToList(); 

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                databaseLists = databaseLists.Where(p => p.DBName.Contains(request.SearchKey) && p.User.first_name.Equals(request.SearchKey)).ToList();
            }
            int rowsCount = 0;
            var dbsList =
                databaseLists.ToPaged(request.Page, 20, out rowsCount).Select
                (
                    p => new GetDBDto
                {
                    
                    Id = p.id,
                    DB = p.DBName,
                    Owner = p.User,
                    //Owner = _context.Users.Where(t => t.first_name.Equals(request.OwnerUser)).FirstOrDefault()
                    IsRemoved=p.IsRemoved,
                }
                ).ToList();

            return new ReslutGetDBDto
            {
                Rows = rowsCount,
                DbsDtos = dbsList,
            };
        }
        public ReslutGetDBDto Execute(RequesDBDto request)
        {
            //List< DatabaseList>databaselist = _context.DatabaseLists.ToList();
            var databaseLists = _context.DatabaseLists.Include(i => i.User).Include(i => i.User.Role).ToList();
            //var databaseLists = _context.DatabaseLists.Include(i=>i.User).IgnoreQueryFilters().ToList(); 

            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                databaseLists = databaseLists.Where(p => p.DBName.Contains(request.SearchKey) && p.User.first_name.Equals(request.SearchKey)).ToList();

            }
            databaseLists= databaseLists.Where(p => p.User.id.Equals(request.OwnerUser)).ToList();

            int rowsCount = 0;
            var dbsList =
                databaseLists.ToPaged(request.Page, 20, out rowsCount).Select
                (
                    p => new GetDBDto
                    {

                        Id = p.id,
                        DB = p.DBName,
                        Owner = p.User,
                        IsRemoved=p.IsRemoved,
                        //Owner = _context.Users.Where(t => t.first_name.Equals(request.OwnerUser)).FirstOrDefault()

                    }
                ).ToList();

            return new ReslutGetDBDto
            {
                Rows = rowsCount,
                DbsDtos = dbsList,
            };
        }
        public  ReslutGetDBDto GetDbbyIdAsync2(int id)
        {
            var dbs = new DatabaseList();

            //Example of Cashing 
            var chasdbs = _memoryCache.Get<DatabaseList>(id);
            if (chasdbs != null)
            {
                dbs = chasdbs;
            }
            else
            {
                dbs = _context.DatabaseLists.Include(f => f.User).FirstOrDefault(p => p.id.Equals(id));
                var chashOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _memoryCache.Set(dbs.id, dbs, chashOption);
            }
            List<GetDBDto> temp = new List<GetDBDto>();
            temp.Add(new GetDBDto
            {
                Id = dbs.id,
                DB = dbs.DBName,
                Owner = dbs.User,
                IsRemoved=dbs.IsRemoved,
            }); ;
            return (new ReslutGetDBDto
            {
                Rows = 1,
                //Users = usersList,
                DbsDtos = temp,
            });
        }
        public long GetDbbyIdAsync3(string DB)
        {
            var dbs = new DatabaseList();
            dbs = _context.DatabaseLists.Include(f => f.User).FirstOrDefault(p => p.DBName.Equals(DB)&&p.IsRemoved==false);
            return (dbs.id);
        }

        //public Task<ReslutGetDBDto> GetDbbyIdAsync(int id)
        //{
        //    var dbs = new DatabaseList();

        //    //Example of Cashing 
        //    var chasdbs = _memoryCache.Get<DatabaseList>(id);
        //    if (chasdbs != null)
        //    {
        //        dbs = chasdbs;
        //    }
        //    else
        //    {
        //        dbs = _context.DatabaseLists.Include(f => f.User).FirstOrDefault(p => p.id .Equals( id));
        //        var chashOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        //        _memoryCache.Set(dbs.id, dbs, chashOption);
        //    }
        //    List<GetDBDto> temp = new List<GetDBDto>();
        //    temp.Add(new GetDBDto
        //    {
        //        Id=dbs.id,
        //        DB = dbs.DBName,
        //        Owner=dbs.User,
        //    }); ;
        //    return Task.FromResult(new ReslutGetDBDto
        //    {
        //        Rows = 1,
        //        //Users = usersList,
        //        DbsDtos = temp,
        //    });
        //}
        public bool ISDBExist(int id)
        {
            return _context.DatabaseLists.Any(u => u.id == id);

        }
        public ResultDto downloadDB(int id,string filename)
        {
            var dbs = _context.DatabaseLists.Include(f => f.User).FirstOrDefault(p => p.id.Equals(id));
           // var db = _context.DatabaseLists.Find(id);
            if (dbs== null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "db یافت نشد"
                };
            }
;

            int re=_context.DownloadDB(dbs.DBName,filename);
           
            _context.SaveChanges();
            if (re > 0)
                return new ResultDto()
            {
                IsSuccess = true,
                Message = "bachup با موفقیت  انجام شد"
            };
            else
                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "bachup  انجام نشد"
                };

        }
        
    }

    public class GetDBDto
    {
        public long Id { get; set; }
        public string DB { get; set; }
        //public long Owner { get; set; }
        public Users Owner { get; set; }
        public bool IsRemoved { get; set; }
        //public string DbName { get; set; }

    }
    public class RequesDBDto
    {
        public string SearchKey { get; set; }
        public int Page { get; set; }
        public long OwnerUser { get; set; }
    }
    public class ReslutGetDBDto
    {
        public List<GetDBDto> DbsDtos { get; set; }
        public int Rows { get; set; }

    }
}
