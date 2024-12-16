using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        //fist check if there is any users currently in DB
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        //to avoid casing difference in json file
        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

        //deserailize the json data into c#endregion
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData,options);
        
        if(users == null) return;

        foreach (var user in    users)
        {
            using var hmac = new HMACSHA512();
            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;
            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }


}
