using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SimpleList.Shared
{
    public class DbIndentityInitializer
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;

        public DbIndentityInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            var user = await _userManager.FindByNameAsync("testuser");

            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "testuser"
                };

                var userResult = await _userManager.CreateAsync(user, "P@ssw0rd!");

                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }
    }
}
