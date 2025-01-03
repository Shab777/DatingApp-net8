using System.Runtime.CompilerServices;
using API.Interfaces;

namespace API.Data;

public class UnitOfWork(DataContext context, ILikesRepository likesRepository,
             IMessageRepository messageRepository, IUserRepository userRepository) : IUnitOfWork
{
    public IUserRepository UserRepository => userRepository;

    public IMessageRepository MessageRepository => messageRepository;

    public ILikesRepository LikesRepository => likesRepository;

    public async  Task<bool> Complete()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}
