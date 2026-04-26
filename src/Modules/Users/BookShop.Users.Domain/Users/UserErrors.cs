using BuildingBlocks.Common;

namespace BookShop.Users.Domain.Users;

public static class UserErrors
{
    public static string UserExists(string email) => $"There already is an user with the email {email}";
}
