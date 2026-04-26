namespace BookShop.Users.Application.Users;

public sealed record UserResponse(
    Guid Id,
    string Username,
    string Email
);
