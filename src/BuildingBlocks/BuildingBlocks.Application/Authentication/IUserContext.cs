namespace BuildingBlocks.Application.Authentication;

public interface IUserContext
{
    Guid UserId { get; }
    string IdentityId { get; }
}
