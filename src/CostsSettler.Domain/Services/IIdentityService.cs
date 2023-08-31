namespace CostsSettler.Domain.Services;

/// <summary>
/// Interface of service that checks logged user permissions.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Checks if logged user's id is equal to 'userId' argument.
    /// </summary>
    /// <param name="userId">Id to compare with logged user id.</param>
    void CheckEqualityWithLoggedUserId(Guid userId);
    
    /// <summary>
    /// Checks if logged user's id is equal to on if ids in 'ids' argument.
    /// </summary>
    /// <param name="ids">Ids to compare with logged user id.</param>
    void CheckIfLoggedUserIsOneOf(IEnumerable<Guid> ids);
}
