using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleList.Shared;

namespace SimpleList.Api.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }

        protected override void AddDataContext(IServiceCollection services)
        {
            services.AddTransient<IDbInitializer, TestDbInitializer>();
            services.AddDbContext<DataContext.ListDataContext>(options =>
                    options.UseInMemoryDatabase("Testing"));
        }
    }
}
