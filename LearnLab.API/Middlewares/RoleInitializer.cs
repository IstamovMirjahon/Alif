using Microsoft.AspNetCore.Identity;
using LearnLab.Core.Constants;
using LearnLab.Identity.Models;
using LearnLab.Core.Enum.GenderTypes;

namespace LearnLab.API.Middleware;
public class RoleInitializer
{
    public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        string SuperAdminEmail = "admin@learnlab.com";
        string SuperAdminPhoneNumber = "998901234567";
        string SuperAdminPassword = "SecurePassword123";

        var Roles = new Dictionary<string, string>()
        {
            { RoleNames.SuperAdmin, "SuperAdmin" },
            { RoleNames.Teacher, "Teacher" },
            { RoleNames.Student, "Student" },
            { RoleNames.Guest, "Guest" },
        };

        foreach (var role in Roles)
            if (await roleManager.FindByNameAsync(role.Key) == null)
                await roleManager.CreateAsync(new Role(role.Key, role.Value));

        User user = await userManager.FindByNameAsync(SuperAdminEmail);

        if (user != null)
        {
            await userManager.AddToRolesAsync(user, new string[] {
                RoleNames.Student,
                RoleNames.Teacher,
                RoleNames.SuperAdmin
            });
        }
        else
        {
            user = new User(
                "John", "Doe", SuperAdminPhoneNumber, SuperAdminEmail, GenderEnum.Male, new DateTime(1990, 01, 01)
            );
            user.PhoneNumberConfirmed = true;
            user.EmailConfirmed = true;

            IdentityResult result = await userManager.CreateAsync(user, SuperAdminPassword);

            if (result.Succeeded)
                await userManager.AddToRolesAsync(user, new string[] {
                    RoleNames.Student,
                    RoleNames.Teacher,
                    RoleNames.SuperAdmin
                });
        }
    }
}