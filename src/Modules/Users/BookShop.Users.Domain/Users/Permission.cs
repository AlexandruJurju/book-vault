namespace BookShop.Users.Domain.Users;

public sealed class Permission
{
    public static readonly Permission GetUser = new(1, "users:read");
    public static readonly Permission ModifyUser = new(2, "users:update");

    private Permission(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }
}
