using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : BaseApiController
{
    //Create a http endpoint to register a user//
    [HttpPost("register")] //api/account/register

    public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
    {
        //Condition- if the user namapie is exist
        if (await UserExists(registerDto.UserName)) return BadRequest ("Username is already exists.");      
       
        var user = mapper.Map<AppUser> (registerDto);

        user.UserName = registerDto.UserName.ToLower();

         var result = await userManager.CreateAsync(user, registerDto.Password);

         if(!result.Succeeded) return BadRequest(result.Errors);

        //using var hmac = new HMACSHA512();
        // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        // user.PasswordSalt =  hmac.Key;
        
        //save a new user in DB
        // context.Users.Add(user);
        // await context.SaveChangesAsync();
        
        return new UserDto
        {   
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
            KnownAs = user.knownAs,
            Gender = user.Gender
        };

       

        
    }
    //Create a Http endpoint for user to login
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>>Login(LoginDto loginDto)
    {
        //first compare the user name from provided to DB
        var user = await     userManager.Users
                            .Include(p => p.Photos)
                            .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.Username.ToUpper());

        if(user == null || user.UserName == null) return Unauthorized("Invalid username.");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if(!result) return Unauthorized();

        // //compare the provided pw with DB

        // //create an object of DB pw salt
        // using var hmac = new HMACSHA512(user.PasswordSalt);

        // // calculate the provided pw# length
        // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        // //compare the given pw# length with DB pw#
        // for (int i = 0; i < computedHash.Length; i++)
        // {
        //     if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        // }
        
        return new UserDto
        { 
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
            KnownAs = user.knownAs,
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            
        };
     }
     
    //method to check if the user name is already exists
    private async Task<bool> UserExists(string username)
    {
        //return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower()); // Bob != bob
        return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper()); // Bob != bob
    }


}

