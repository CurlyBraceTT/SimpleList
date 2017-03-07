using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SimpleList.DataContext;

namespace SimpleList.Shared
{
    public static class IndentityBuilderExtensions
    {
        public static IdentityBuilder AddIdentityEntityFrameworkStores(this IdentityBuilder builder)
        {
            builder.AddEntityFrameworkStores<ListDataContext>();
            return builder;
        }
    }
}
