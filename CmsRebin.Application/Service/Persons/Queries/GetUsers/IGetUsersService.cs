using System.Text;
using System.Threading.Tasks;
using CmsRebin.Common;

namespace CmsRebin.Application.Service.Persons.Queries.GetUsers
{
    public interface IGetUsersService
    {
        ReslutGetUserDto Execute(RequestGetUserDto request);
        Task<ReslutGetUserDto> GetuserbyIdAsync(int id);
        public int numerUser();
        public bool IsUserExist(int id);
        public bool IsUserExistbyemail(string email);

    }
}
