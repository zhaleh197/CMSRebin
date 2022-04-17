using CmsRebin.Application.Service.Collection.Commands.PostTable;
using CmsRebin.Common.Dto;
using System.Text;
using System.Threading.Tasks;

namespace CmsRebin.Application.Service.Collection.Commands.PostTable
{
    public interface IPostTable
    {
        ReslutPostTableDto Execute(RequestDto request);
        ReslutPostTableDto Execute2(int dbid);
        bool ISTableExist(int id);
        ReslutPostTableDto ExecuteIDs(RequestDtoIDs request);
        ReslutPostTableDto GetCollectionbyId(int Tid);
        public ReslutPostTableDto Executeall();
    }
}
