using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SimpleList.Models;

namespace SimpleList.Services
{
    public class ListService : IListService
    {
        private readonly DataContext.ListDataContext _context;
        private readonly IMapper _mapper;

        public ListService(DataContext.ListDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListItem> Add(ListItem item)
        {
            var mapped = _mapper.Map<ListItem, DataContext.ListItem>(item);
            _context.Items.Add(mapped);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<DataContext.ListItem, ListItem>(mapped);
            return result;
        }

        public async Task<IList<ListItem>> All()
        {
            var items = await _context.Items.AsNoTracking().ToListAsync();
            var mapped = _mapper.Map<List<DataContext.ListItem>, List<ListItem>>(items);
            return mapped;
        }

        public async Task<ListItem> Get(int id)
        {
            var item = await _context.Items.FindAsync(id);
            var mapped = _mapper.Map<DataContext.ListItem, ListItem>(item);
            return mapped;
        }

        public async Task Remove(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(ListItem item)
        {
            var dbItem = await _context.Items.FindAsync(item.Id);
            var mapped = _mapper.Map<ListItem, DataContext.ListItem>(item);
            _context.Entry(dbItem).CurrentValues.SetValues(mapped);
            await _context.SaveChangesAsync();
        }
    }
}
