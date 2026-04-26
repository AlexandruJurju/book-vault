using BookShop.Users.Domain.Users.Events;
using BuildingBlocks.Common.Helpers;
using BuildingBlocks.Domain;

namespace BookShop.Users.Domain.Users;

public sealed class User : Entity, IAggregateRoot
{
    // For EF Core
    private User()
    {
    }

    private User(Guid id, string userName, string email)
        : base(id)
    {
        UserName = userName;
        Email = email;
    }

    public string UserName { get; private set; }
    public string Email { get; private set; }

    private readonly List<Role> _roles = new();
    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public static User Create(string userName, string email)
    {
        var user = new User(GuidHelper.NewGuid(), userName, email);

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        user._roles.Add(Role.Registered);

        return user;
    }
}
