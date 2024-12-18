using System;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    // Api endpoints
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsyn();
        return Ok(users);
    }

    [HttpGet("{username}")] // /api/users/1,2,3...
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsyn(username);
        if (user == null) return NotFound();
        return user;

    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
        //get the username from client's side bearer thing
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(username == null) return BadRequest("No username found in Token");

        var user = await userRepository.GetuserByUsernameAsync(username);

        if(user == null) return BadRequest("Could not fin user");
         
        mapper.Map(memberUpdateDto, user);

        if(await userRepository.SaveAllAsync())  return NoContent();

        return BadRequest("Failed to update user");
    }
}
