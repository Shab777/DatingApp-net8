using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        //fist check if there is any users currently in DB
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        //to avoid casing difference in json file
        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        //deserailize the json data into c#endregion
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData,options);
        
        if(users == null) return;

        var roles = new List<AppRole>
        {
            new AppRole{Name = "Member"},
            new AppRole{Name = "Admin"},
            new AppRole{Name = "Moderator"}

        };

        foreach(var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
        
        
        foreach (var user in    users)
        {
            //using var hmac = new HMACSHA512();
            //user.UserName = user.UserName.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            // user.PasswordSalt = hmac.Key;
            //context.Users.Add(user);
            user.UserName = user.UserName!.ToLower();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");
        }

        //await context.SaveChangesAsync();
        var admin = new AppUser
        {
            UserName = "admin",
            knownAs = "Admin",
            Gender = "",
            City =" ",
            Country = ""
            
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
    }
    


}
