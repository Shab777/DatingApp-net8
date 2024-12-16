using System;
using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task<AppUser?> GetUserByIdAsync(int id);

    Task<AppUser?> GetuserByUsernameAsync(string username);

    Task<IEnumerable<MemberDto>>GetMembersAsyn();

    Task<MemberDto?> GetMemberAsyn(string username);
}
