using Microsoft.AspNetCore.Identity;

namespace NeoForum.Areas.Identity.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<NeoForumUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Наполнение ролей
            await roleManager.CreateAsync(new IdentityRole(Models.Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Models.Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Models.Enums.Roles.VIP.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Models.Enums.Roles.Basic.ToString()));
        }

        public static async Task SeedAdminAsync(UserManager<NeoForumUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Создание аккаунта администратора
            var defaultUser = new NeoForumUser
            {
                FirstName = "Andrey",
                LastName = "Aleshin",
                UserName = "Admin",
                Email = "admin@gmail.com",
                Age = 21,
                Country = 0
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "12Mn34nf");
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.VIP.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Admin.ToString());
                }
            }
        }

        public static async Task SeedTestUserAsync(UserManager<NeoForumUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Создание аккаунта пользователя
            var defaultUser = new NeoForumUser
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                UserName = "TestUser",
                Email = "testuser@gmail.com",
                Age = 33,
                Country = 0
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "12Mn34nf");
                    await userManager.AddToRoleAsync(defaultUser, Models.Enums.Roles.Basic.ToString());
                }
            }
        }
    }
}
