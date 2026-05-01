using Ardalis.Result;

namespace BookShop.Users.Application.Abstractions.Identity;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterAsync(UserModel user, string password, CancellationToken cancellationToken = default);
}
