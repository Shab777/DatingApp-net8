using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController] //annotation gives extra power to controllers
[Route("api/[controller]")] // EF can direct http request to the appropriate controller & endpoin /api/users
public class UsersController(DataContext context) : ControllerBase
{
    // Api endpoints
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id}")] // /api/users/1,2,3...
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return user;

    }
}
