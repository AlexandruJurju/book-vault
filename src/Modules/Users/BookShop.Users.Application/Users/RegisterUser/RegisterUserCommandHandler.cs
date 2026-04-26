using Ardalis.Result;
using BookShop.Users.Application.Abstractions;
using BookShop.Users.Domain.Users;
using BuildingBlocks.Application.CQRS;
using BuildingBlocks.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Users.Application.Users.RegisterUser;

public sealed class RegisterUserCommandHandler(
    IUsersDbContext usersDbContext,
    IUnitOfWork unitOfWork
) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async ValueTask<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await usersDbContext.Users.AnyAsync(x => x.Email == request.Email, cancellationToken: cancellationToken))
        {
            return Result<Guid>.Error(UserErrors.UserExists(request.Email));
        }

        var user = User.Create(
            request.UserName,
            request.Email
        );

        usersDbContext.Users.Add(user);
        usersDbContext.Roles.Attach(Role.Registered);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
