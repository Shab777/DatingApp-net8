using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
{
    public void AddLike(UserLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
        context.Likes.Remove(like);
    }

// user liked other users- returns their's ids
    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
       return await context.Likes
                    .Where(x => x.SourceUserId == currentUserId)
                    .Select(x => x.TargetUserId)
                    .ToListAsync();
    }

// returns each users likes- individual
    public async Task<UserLike?> GetUserLike(int SourceUserId, int TargetUserId)
    {
        return await context.Likes.FindAsync(SourceUserId, TargetUserId);
    }

//user liked by others, other user liked by others, liked each other
    public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
    {
      var likes = context.Likes.AsQueryable();
      IQueryable<MemberDto> query;

      switch (likesParams.Predicate)
      {
      //user liked others
        case "liked": 
                query = likes
                    .Where( x=> x.SourceUserId == likesParams.UserId)
                    .Select(x => x.TargetUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
        break;            
        // user liked by others
        case "likedBy":
                query = likes
                    .Where( x=> x.TargetUserId == likesParams.UserId)
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
         break;           
        //liked each others- mutual            
        default:
            var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);
                query =  likes
                    .Where( x=> x.TargetUserId == likesParams.UserId&& likeIds.Contains(x.SourceUserId))
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
        break;               

      }

      return await PagedList<MemberDto>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<bool> SaveChanges()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
