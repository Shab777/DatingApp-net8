using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace API.Controllers;

public class AdminController(UserManager<AppUser> userManager): BaseApiController
{
    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await userManager.Users
                         .OrderBy(x=> x.UserName)
                         .Select(x=> new
                            {
                                x.Id,
                                Username= x.UserName,
                                Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
                            }).ToListAsync();

        return Ok(users);
    }

    [Authorize(Policy = "ModeratePhotoRole")]
    [HttpGet("photos-to-moderate")]
    public ActionResult GetPhotosForModeration()
    {
        return Ok("Admins or moderators can see this");
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, string roles)
    {
        if(string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

        var selectedRoles = roles.Split(",").ToArray();

        var user = await userManager.FindByNameAsync(username);

        if(user == null) return BadRequest("User not found");

        // get current role of the user 
        var userRoles = await userManager.GetRolesAsync(user);

        //update the roles- between the existing and passing roles
        var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if(!result.Succeeded) return BadRequest("Failed to add to roles");

        //remove any other roles except the selected roles
        result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if(!result.Succeeded) return BadRequest("Failed to remove from roles");

        return Ok(await userManager.GetRolesAsync(user));
    }


}