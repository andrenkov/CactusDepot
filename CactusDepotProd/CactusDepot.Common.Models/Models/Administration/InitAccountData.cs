using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CactusDepot.Shared;

namespace CactusDepot.Shared.Models.Administration
{
    public class InitAccountData
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public InitAccountData(RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }

        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.Basic.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //var user = new IdentityUser();
            //var hasher = new PasswordHasher<IdentityUser>();
            //var users = new List<IdentityUser>()
            //{ 
            //}

            //Seed Default User
            IdentityUser defaultUser = new()
            {
                UserName = "administrator",//"andrenkov@gmail.com",//use email instead of the Name because the Identity standard Login requires the Email, not name
                //NormalizedUserName = "ADMINISTRATOR",
                Email = "andrenkov@gmail.com",
                //NormalizedEmail = "ANDRENKOV@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                //SecurityStamp = Guid.NewGuid().ToString("D"),
                LockoutEnabled = false
            };
            //defaultUser.PasswordHash = hasher.HashPassword(defaultUser, "C@tal0g2022A");

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                IdentityUser userAdmin = await userManager.FindByEmailAsync(defaultUser.Email);
                if (userAdmin == null)
                {
                    //await userManager.CreateAsync(defaultUser);
                    await userManager.CreateAsync(defaultUser, "C@tal0g2022A");
                    await userManager.AddToRoleAsync(defaultUser, Enums.UserRoles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.UserRoles.User.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.UserRoles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.UserRoles.Administrator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.UserRoles.SuperAdmin.ToString());
                }

            }
        }
        #region Not in use
        //public static void Initialize(IServiceProvider serviceProvider)
        //{
        //    using (SeedsDbContext context = new SeedsDbContext(serviceProvider.GetRequiredService<DbContextOptions<SeedsDbContext>>()))
        //    {
        //        if (context.Users.Any())
        //        {
        //            return;   // DB Users has been seeded
        //        }
        //        if (context.Roles.Any())
        //        {
        //            return;   // DB Roles has been seeded
        //        }
        //        if (context.UserRoles.Any())
        //        {
        //            return;   // DB User Roles has been seeded
        //        }

        //        IdentityUser defaultUser = new IdentityUser
        //        {
        //            UserName = "Administrator",
        //            Email = "andrenkov@gmail.com",
        //            EmailConfirmed = true,
        //            PhoneNumberConfirmed = true
        //        };
        //    }
        #endregion
    }
}
