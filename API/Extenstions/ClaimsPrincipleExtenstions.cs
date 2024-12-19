using System;
using System.Security.Claims;

namespace API.Extenstions;

public static class ClaimsPrincipleExtenstions
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if(username == null) throw new Exception("Cannot get username from the token");

        return username;
    }

}
