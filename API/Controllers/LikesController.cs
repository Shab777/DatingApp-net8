using API.DTOs;
using API.Entities;
using API.Extenstions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(ILikesRepository likesRepository) : BaseApiController
{
    //add end point- user is liking other users
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        // get the current user id by calims priciple
        var sourceUserId = User.GetUserId();

        if(sourceUserId == targetUserId) return BadRequest("You cannot like yourself");

        //get current user & other user's likes
        var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);

        if(existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId
            };
            likesRepository.AddLike(like);
        } 
        else
        {
            likesRepository.DeleteLike(existingLike);
        }

        if(await likesRepository.SaveChanges()) return Ok();

        return BadRequest("Failed to update like");
    }

    //get the list of current user liked other users
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }
    

    //get user liked by others or other users liked by others
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await likesRepository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}
