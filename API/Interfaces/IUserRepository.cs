using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<AppUser?> GetUserByIdAsync(int id);

    Task<AppUser?> GetuserByUsernameAsync(string username);

    Task<PagedList<MemberDto>>GetMembersAsyn(UserParams userParams);

    Task<MemberDto?> GetMemberAsyn(string username);
}
