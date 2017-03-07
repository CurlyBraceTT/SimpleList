using System.Linq;
using SimpleList.DataContext;
using SimpleList.Shared;

namespace SimpleList.Api.Tests
{
    public class TestDbInitializer : IDbInitializer
    {
        public static readonly ListItem RemoveItem = new ListItem
        {
            Id = 5,
            Description = "Remove test data",
            Done = false
        };

        public static readonly ListItem UpdateItem = new ListItem
        {
            Id = 3,
            Description = "Update test data",
            Done = false
        };

        private ListDataContext _context;

        public TestDbInitializer(ListDataContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
            _context.Items.Add(RemoveItem);
            _context.Items.Add(UpdateItem);
            _context.SaveChanges();
        }
    }
}
