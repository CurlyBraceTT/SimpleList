using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleList.DataContext;

namespace SimpleList.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<ListDataContext>(options =>
                    options.UseSqlServer(connectionString));

            return services;
        }
    }
}
