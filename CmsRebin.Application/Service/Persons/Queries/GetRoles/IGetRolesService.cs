using CmsRebin.Application.Interface.Context;
using CmsRebin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Persons.Queries.GetRoles
{
    public interface IGetRolesService
    {
        ResultDto<List<RolesDto>> Execute();
    }
    public class GetRolesService : IGetRolesService
    {
        private readonly IDatabaseContext _context;

        public GetRolesService(IDatabaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<RolesDto>> Execute()
        {
            var roles = _context.Role.ToList().Select(p => new RolesDto
            {
                Id = p.id,
                Name = p.rolename,
            }).ToList();

            return new ResultDto<List<RolesDto>>()
            {
                Data = roles,
                IsSuccess = true,
                Message = "",
            };
        }
    }
    public class RolesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
