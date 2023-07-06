namespace CostsSettler.Domain.Services;
public interface IIdentityService
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    void CheckEqualityWithLoggedUserId(Guid userId);
}
