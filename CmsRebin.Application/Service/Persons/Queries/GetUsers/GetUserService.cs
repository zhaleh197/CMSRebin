using CmsRebin.Application.Interface.Context;
using System;
using System.Linq;
using CmsRebin.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using CmsRebin.Domain.Entities.Persons;

namespace CmsRebin.Application.Service.Persons.Queries.GetUsers
{
    public class GetUserService : IGetUsersService
    {
        private readonly IDatabaseContext _context;
        IMemoryCache _memoryCache;
        public GetUserService(IMemoryCache memoryCache, IDatabaseContext context)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        public bool IsUserExist(int id)
        {
            return _context.Users.Any(u=>u.id==id);
           
        }
        public bool IsUserExistbyemail(string email)
        {
            return _context.Users.Any(u => u.Email == email);

        }
        public int numerUser()
        {
            int n = _context.Users.Count();
            return n;
        }
        public ReslutGetUserDto Execute(RequestGetUserDto request)
        {
            var users = _context.Users.AsQueryable().Include(r=>r.Role).Where(p=>p.IsRemoved==false);
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                users = users.Where(p => p.last_name.Contains(request.SearchKey) && p.first_name.Contains(request.SearchKey));
            }
            int rowsCount = 0;
            var usersList = users.ToPaged(request.Page,20, out rowsCount).Select(p => new GetUsersDto
            {
                Id = p.id,
                first_name = p.first_name,
                last_name = p.last_name,
                //IsRemoved=p.IsRemoved,
                IsActive=p.IsActive,
                role = p.Role.rolename
            }).ToList();

            return new ReslutGetUserDto
            {
                Rows = rowsCount,
                Users = usersList,
            };
        }
        public async Task<ReslutGetUserDto> GetuserbyIdAsync(int id)
        {
            var userss =new  Users();

            //Example of Cashing 
            var chasUsesr = _memoryCache.Get<Users>(id);
            if (chasUsesr != null)
            {
                userss= chasUsesr;
            }
            else
            {
                //var users = _context.Users.AsQueryable();

                userss = _context.Users.Include(f=>f.Role).FirstOrDefault(p => p.id == id);
                //userss = await _context.Users.FirstOrDefaultAsync((p => p.id == id));
                

                //int rowsCount = 0;
                //var usersList = users.ToPaged(1, 20, out rowsCount).Select(p => new GetUsersDto
                //{
                //    first_name = p.first_name,
                //    last_name = p.last_name,
                //    IsRemoved = p.IsRemoved,
                //    IsActive = p.IsActive,
                //    Id = p.id,
                //}).ToList();



                var chashOption = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
                _memoryCache.Set(userss.id, userss, chashOption);
            }
              List<GetUsersDto> temp = new List<GetUsersDto>();
            temp.Add(new GetUsersDto
            {
                first_name = userss.first_name,
                last_name = userss.last_name,
                //IsRemoved = userss.IsRemoved,
                IsActive = userss.IsActive,
                Id = userss.id,
                role = userss.Role.rolename
            }); ;
                return new ReslutGetUserDto
                {
                    Rows = 1,
                    //Users = usersList,
                    Users = temp,
                };
            
        }
    }
}
