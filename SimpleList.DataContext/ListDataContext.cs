using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SimpleList.DataContext
{
    public class ListDataContext : IdentityDbContext
    {
        public ListDataContext(DbContextOptions<ListDataContext> options) : base(options)
        {

        }

        public DbSet<ListItem> Items { get; set; }
    }
}
