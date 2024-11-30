using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    //private SymmetricSecurityKey _key;

    public string CreateToken(AppUser user)
    {
       var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsetting.");
       if(tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");

       var _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

       //user claims several things
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.UserName)
        };  
        //describe the token's element which will return
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
