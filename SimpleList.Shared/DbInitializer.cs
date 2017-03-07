using System.Linq;
using SimpleList.DataContext;

namespace SimpleList.Shared
{
    public interface IDbInitializer
    {
        void Seed();
    }

    public class DbInitializer : IDbInitializer
    {
        private readonly ListDataContext _context;

        public DbInitializer(ListDataContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (_context.Items.Any())
            {
                return;
            }

            var item = new ListItem
            {
                Description = "Create App",
                Done = true
            };

            _context.Items.Add(item);
            _context.SaveChanges();
        }
    }
}
