using CmsRebin.Application.Interface.Context;
using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Common;
using CmsRebin.Common.Dto;
using CmsRebin.Domain.Entities.Collections;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CmsRebin.Application.Service.Collection.Commands.PostTable
{
    public class PostTableService : IPostTable
    {
        IMemoryCache _memoryCache;
        private readonly IDatabaseContext _context;
        public PostTableService(IDatabaseContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public ReslutPostTableDto ExecuteIDs(RequestDtoIDs request)
        {
            var collectins = _context.Tables.AsQueryable();



            if ((request.DbName)>0)
            {
                collectins = collectins.Where(p => p.IdDBase.Equals(request.DbName));
            }
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                collectins = collectins.Where(p => p.collection.Contains(request.SearchKey) && p.note.Contains(request.SearchKey));
            }
            if (request.TableName>0)
            {
                collectins = collectins.Where(p => p.id.Equals(request.TableName));
            }
            int rowsCount = 0;
            var tablesList = collectins.ToPaged(request.Page, 20, out rowsCount).Select(p => new PostTableDto
            {

                collection = p.collection,
                note = p.note,
                IsRemoved = p.IsRemoved,
                Id = p.id,
                //DbName=p.DatabaseList.DBName,
                DbName = _context.DatabaseLists.Where(i => i.id.Equals(p.IdDBase)).FirstOrDefault().DBName.ToString()

            }).ToList();

            return new ReslutPostTableDto
            {
                Rows = rowsCount,
                TableDtos = tablesList,
            };
        }

        public ReslutPostTableDto Execute(RequestDto request)
        {
            var collectins = _context.Tables.AsQueryable();
            


            if (!string.IsNullOrWhiteSpace(request.DbName))
            {
                collectins = collectins.Where(p => p.IdDBase.Equals(request.DbName));
            }
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                collectins = collectins.Where(p => p.collection.Contains(request.SearchKey) && p.note.Contains(request.SearchKey));
            }
            if (!string.IsNullOrWhiteSpace(request.TableName))
            {
                collectins = collectins.Where(p => p.collection.Equals(request.TableName));
            }
            int rowsCount = 0;
            var tablesList = collectins.ToPaged(request.Page, 20, out rowsCount).Select(p => new PostTableDto
            {

                collection = p.collection,
                note = p.note,
                IsRemoved = p.IsRemoved,
                Id = p.id,
                //DbName=p.DatabaseList.DBName,
                DbName = _context.DatabaseLists.Where(i => i.id.Equals(p.IdDBase)).FirstOrDefault().DBName.ToString()

            }).ToList();

            return new ReslutPostTableDto
            {
                Rows = rowsCount,
                TableDtos = tablesList,
            };
        }

        public ReslutPostTableDto Execute2(int dbid)
        {
            var collectins = _context.Tables.AsQueryable();
            int rowsCount = 0;
            collectins = collectins.Where(p => p.IdDBase.Equals(dbid)&&p.IsRemoved==false );

            var tablesList = collectins.ToPaged(1, 20, out rowsCount).Select(
                p => new PostTableDto
                {

                    collection = p.collection,
                    note = p.note,
                    IsRemoved = p.IsRemoved,
                    Id = p.id,
                    //DbName=p.DatabaseList.DBName,
                    DbName = _context.DatabaseLists.Where(i => i.id.Equals(p.IdDBase)).FirstOrDefault().DBName.ToString()

                }).ToList();

            return new ReslutPostTableDto
            {
                Rows = rowsCount,
                TableDtos = tablesList,
            };
        }
        public bool ISTableExist(int id)
        {
            return _context.Tables.Any(u => u.id == id);

        }
        public ReslutPostTableDto GetCollectionbyId(int Tid)
        {
            var dbs = new Tables();

            //Example of Cashing 
            var chasdbs = _memoryCache.Get<Tables>(Tid);
            if (chasdbs != null)
            {
                dbs = chasdbs;
            }
            else
            {
                dbs = _context.Tables.FirstOrDefault(p => p.id.Equals(Tid) && p.IsRemoved==false);
                var chashOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _memoryCache.Set(dbs.id, dbs, chashOption);
            }
            List<PostTableDto> temp = new List<PostTableDto>();
            temp.Add(new PostTableDto
            {
                collection = dbs.collection,
                note=dbs.note,
                DbName= _context.DatabaseLists.FirstOrDefault(b => b.id.Equals(dbs.IdDBase)).DBName,
                Id=dbs.id,
                IsRemoved=dbs.IsRemoved,
            });
            return (new ReslutPostTableDto
            {
                Rows = 1,
                //Users = usersList,
                TableDtos = temp,
            });
        }

        public ReslutPostTableDto Executeall()
        {
            var collectins= _context.Tables.AsQueryable();

            collectins = collectins.Where(p => p.IdDBase !=0);
            int rowsCount = 0; 

            var tablesList = collectins.ToPaged(1, 20, out rowsCount).Select(
                p => new PostTableDto
                {

                    collection = p.collection,
                    note = p.note,
                    IsRemoved = p.IsRemoved,
                    Id = p.id,
                    //DbName=p.DatabaseList.DBName,
                    DbName = _context.DatabaseLists.Where(i => i.id.Equals(p.IdDBase)).FirstOrDefault().DBName.ToString()

                }).ToList();

            return new ReslutPostTableDto
            {
                Rows = rowsCount,
                TableDtos = tablesList,
            };
        }
    }
}
