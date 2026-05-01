namespace BookShop.Users.Infrastructure.IdentityProvider;

internal sealed record UserRepresentation(
    string Username,
    string Email,
    bool EmailVerified,
    bool Enabled,
    CredentialRepresentation[] Credentials
);
