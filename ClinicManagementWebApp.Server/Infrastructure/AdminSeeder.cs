using System.Security.Claims;
using ClinicManagementWebApp.Server.Features.Users.Entity;
using Microsoft.AspNetCore.Identity;

namespace ClinicManagementWebApp.Server.Infrastructure
{
    public static class AdminSeeder
    {
        public async static Task SeedAdmin(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<UserModel>>();
            var emailStore = (IUserEmailStore<UserModel>)userStore;
            UserModel admin = new();
            if (!userManager.Users.Any(x => x.Email == "admin@admin.com"))
            {
                await userStore.SetUserNameAsync(admin, "admin@admin.com", CancellationToken.None);
                await emailStore.SetEmailAsync(admin, "admin@admin.com", CancellationToken.None);

                var result = await userManager.CreateAsync(admin, "P@ssw0rd");
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                await userManager.AddClaimAsync(admin, new Claim("level", "admin"));
            }
        }
    }
}
