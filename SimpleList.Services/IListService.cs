using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleList.Models;

namespace SimpleList.Services
{
    public interface IListService
    {
        Task<ListItem> Add(ListItem item);
        Task<ListItem> Get(int id);
        Task<IList<ListItem>> All();
        Task Remove(int id);
        Task Update(ListItem item);
    }
}
