using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Entity;

namespace UrlShortener.Data;

public static class SeedData
{
    public static async Task SeedIdentityAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (await roleManager.RoleExistsAsync(RoleConstant.Administrator) == false)
        {
            var adminRole = new IdentityRole<Guid>()
            {
                Id = new Guid("44862E93-2906-405C-8C21-6F96A8C7D3F1"),
                Name = RoleConstant.Administrator
            };
            await roleManager.CreateAsync(adminRole);
        }

        if (await roleManager.RoleExistsAsync(RoleConstant.Moderator) == false)
        {
            var moderatorRole = new IdentityRole<Guid>()
            {
                Id = new Guid("E36184B0-12F3-420C-AB8E-2BADFCC7C344"),
                Name = RoleConstant.Moderator
            };
            await roleManager.CreateAsync(moderatorRole);
        }

        if (await roleManager.RoleExistsAsync(RoleConstant.User) == false)
        {
            var userRole = new IdentityRole<Guid>()
            {
                Id = new Guid("C4D3E0E5-E4DF-4F88-BE5D-2B9FDC4C732E"),
                Name = RoleConstant.User
            };
            await roleManager.CreateAsync(userRole);
        }
    }
}