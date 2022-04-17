using CmsRebin.Application.Interface.Context;

using CmsRebin.Application.Service.Common.Queries;
using CmsRebin.Common;
using CmsRebin.Domain.Entities.Collections;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Filed.Queries.Get
{
    public interface IGetFiledsService
    {
        ReslutGetFiledDto Execute(RequestGetFiledDto request);
        List<string> Executejustfildname(RequestGetFiledDto request);
        ReslutGetFiledDto ExecutebyTid(RequestGetFiledDtobyIdT request);
        ReslutGetFiledDto GetFiledbyIdAsync2(int Fid);
        public ReslutGetFiledDto ExecuteaafildinallTable();
    }
    public class GetFiledsService : IGetFiledsService
    {
        IMemoryCache _memoryCache;
        private readonly IDatabaseContext _context;

        public GetFiledsService(IDatabaseContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;

        }


        public ReslutGetFiledDto ExecuteaafildinallTable()
        {
            var fileds = _context.FieldsofTable.AsQueryable();
            int rowsCount = 0;
            var filedsList = fileds.ToPaged(1, 20, out rowsCount).Select(p => new GetFiledsDto
            {

                fieldname = p.fieldname,
                tablename = _context.Tables.Where(i => i.id.Equals(p.IdTable) && i.IdDBase != 0).FirstOrDefault().collection,
                BdName = _context.DatabaseLists.Where(d => d.id == (_context.Tables.Where(i => i.id.Equals(p.IdTable) && i.IdDBase != 0).FirstOrDefault().IdDBase)).FirstOrDefault().DBName,
                IsRemoved = p.IsRemoved,
                typeReleaton = p.relation,
                Id = p.id,
                interfacefild=p.interfaces
                

            }).ToList();

            return new ReslutGetFiledDto
            {
                Rows = rowsCount,
                Fileds = filedsList,
            };
        }

        public ReslutGetFiledDto GetFiledbyIdAsync2(int Fid)
        {
            var dbs = new FieldsofTables();

            //Example of Cashing 
            var chasdbs = _memoryCache.Get<FieldsofTables>(Fid);
            if (chasdbs != null)
            {
                dbs = chasdbs;
            }
            else
            {
                dbs = _context.FieldsofTable.FirstOrDefault(p => p.id.Equals(Fid));
                var chashOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _memoryCache.Set(dbs.id, dbs, chashOption);
            }
            List<GetFiledsDto> temp = new List<GetFiledsDto>();
            var t = _context.Tables.FirstOrDefault(p => p.id.Equals(dbs.IdTable));
            temp.Add(new GetFiledsDto
            {
                fieldname = dbs.fieldname,
                tablename = t.collection,
                BdName = _context.DatabaseLists.FirstOrDefault(b => b.id.Equals(t.IdDBase)).DBName,
                Id = dbs.id,
                IsRemoved = dbs.IsRemoved,
                typeReleaton = dbs.relation,
                interfacefild=dbs.interfaces


            });
            return (new ReslutGetFiledDto
            {
                Rows = 1,
                //Users = usersList,
                Fileds = temp,
            });
        }
        public List<string> Executejustfildname(RequestGetFiledDto request)
        {
            var fileds = _context.FieldsofTable.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchKey) && request.SearchKey != "string")
            {
                fileds = fileds.Where(p => p.fieldname.Equals(request.SearchKey));
            }


            //1401-01-09
            //fileds = fileds.Where(p=> p.Tables.collection.Equals(request.nametable));
            var iddb = _context.DatabaseLists.Where(d => d.DBName.Equals(request.DbName) && d.IsRemoved == false).FirstOrDefault().id;
            var t = _context.Tables.Where(i => i.collection.Equals(request.nametable) && i.IdDBase == iddb).FirstOrDefault().id;
            fileds = fileds.Where(p => p.IdTable.Equals(t));

            int rowsCount = 0;
            var filedsList = fileds.ToPaged(request.Page, 20, out rowsCount).Select(p => new GetFiledsDto
            {
                /* first_name = p.first_name,
                 last_name = p.last_name,
                 IsRemoved = p.IsRemoved,
                 IsActive = p.IsActive,
                 Id = p.id,*/
                fieldname = p.fieldname,
                tablename = request.nametable,
                BdName = request.DbName,
                //tablename = p.Tables.collection,
                //BdName = p.Tables.DatabaseList.DBName,
                IsRemoved = p.IsRemoved,
                typeReleaton = p.relation,
                Id = p.id,
                interfacefild=p.interfaces


            }).ToList();

            var re= new ReslutGetFiledDto
            {
                Rows = filedsList.Count,
                Fileds = filedsList,
            };
            List<string> S = new List<string>();
            for (int f = 0; f < re.Fileds.Count; f++)
                S.Add(re.Fileds[f].fieldname);
            return S;
        }

        
        public ReslutGetFiledDto Execute(RequestGetFiledDto request)
        {
            var fileds = _context.FieldsofTable.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchKey) && request.SearchKey != "string")
            {
                fileds = fileds.Where(p => p.fieldname.Equals(request.SearchKey));
            }


            //1401-01-09
            //fileds = fileds.Where(p=> p.Tables.collection.Equals(request.nametable));
            var iddb = _context.DatabaseLists.Where(d => d.DBName.Equals(request.DbName) && d.IsRemoved == false).FirstOrDefault().id;
            var t = _context.Tables.Where(i => i.collection.Equals(request.nametable) && i.IdDBase == iddb).FirstOrDefault().id;
            fileds = fileds.Where(p => p.IdTable.Equals(t));

            int rowsCount = 0;
            var filedsList = fileds.ToPaged(request.Page, 20, out rowsCount).Select(p => new GetFiledsDto
            {
                /* first_name = p.first_name,
                 last_name = p.last_name,
                 IsRemoved = p.IsRemoved,
                 IsActive = p.IsActive,
                 Id = p.id,*/
                fieldname = p.fieldname,
                tablename = request.nametable,
                BdName = request.DbName,
                //tablename = p.Tables.collection,
                //BdName = p.Tables.DatabaseList.DBName,
                IsRemoved = p.IsRemoved,
                typeReleaton = p.relation,
                Id = p.id,
                interfacefild=p.interfaces


            }).ToList();

            return new ReslutGetFiledDto
            {
                Rows = rowsCount,
                Fileds = filedsList,
            };
        }
        public ReslutGetFiledDto ExecutebyTid(RequestGetFiledDtobyIdT request)
        {
            var fileds = _context.FieldsofTable.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                fileds = fileds.Where(p => p.fieldname.Equals(request.SearchKey));
            }

            //fileds = fileds.Where(p=> p.Tables.collection.Equals(request.nametable));
            //var t = _context.Tables.Where(i => i.id.Equals(request.Tableid)&&i.IdDBase.Equals(request.Dbid)).FirstOrDefault();
            var t = _context.Tables.Where(i => i.id.Equals(request.Tableid)).FirstOrDefault();
            fileds = fileds.Where(p => p.IdTable.Equals(request.Tableid));

            int rowsCount = 0;
            var filedsList = fileds.ToPaged(request.Page, 20, out rowsCount).Select(p => new GetFiledsDto
            {
                /* first_name = p.first_name,
                 last_name = p.last_name,
                 IsRemoved = p.IsRemoved,
                 IsActive = p.IsActive,
                 Id = p.id,*/
                fieldname = p.fieldname,
                tablename = t.collection,
                BdName = _context.DatabaseLists.FirstOrDefault(f => f.id.Equals(t.IdDBase)).DBName,
                //tablename = p.Tables.collection,
                //BdName = p.Tables.DatabaseList.DBName,
                IsRemoved = p.IsRemoved,
                typeReleaton = p.relation,
                Id = p.id,
                interfacefild=p.interfaces


            }).ToList();

            return new ReslutGetFiledDto
            {
                Rows = rowsCount,
                Fileds = filedsList,
            };
        }
    }
    public class GetFiledsDto
    {
        public long Id { get; set; }
        public string BdName { get; set; }
        public string tablename { get; set; }
        public string fieldname { get; set; }
        public string typeReleaton { get; set; }
        public bool IsRemoved { get; set; }
        public string interfacefild { get; set; }
    }
    public class RequestGetFiledDto
    {
        public string DbName { get; set; }
        public string nametable { get; set; }
        public string SearchKey { get; set; }
        public int Page { get; set; }
    }
    public class RequestGetFiledDtobyIdT
    {
        //public int Dbid { get; set; }
        public long Tableid { get; set; }
        public string SearchKey { get; set; }
        public int Page { get; set; }
    }
    public class ReslutGetFiledDto
    {
        public List<GetFiledsDto> Fileds { get; set; }
        public int Rows { get; set; }


    }

}
