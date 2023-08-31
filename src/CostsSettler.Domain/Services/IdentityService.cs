using CostsSettler.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CostsSettler.Domain.Services;

/// <summary>
/// Implementation of IIdentityService interface.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly HttpContext _httpContext;
    private Guid _userId
        => Guid.TryParse(_httpContext.User.FindFirst("appId")?.Value, out Guid id) ? id : throw new AuthorizationException();
    
    /// <summary>
    /// Creates new IdentityService. Requires HttpContext in IHttpContextAccessor to be not null.
    /// </summary>
    /// <param name="httpContextAccessor">Http context accessor.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);

        _httpContext = httpContextAccessor.HttpContext;
    }

    /// <summary>
    /// Checks if logged user's id is equal to 'userId' argument.
    /// </summary>
    /// <param name="userId">Id to compare with logged user id.</param>
    public void CheckEqualityWithLoggedUserId(Guid userId)
    {
        if (userId != _userId)
            throw new AuthorizationException();
    }

    /// <summary>
    /// Checks if logged user's id is equal to on if ids in 'ids' argument.
    /// </summary>
    /// <param name="ids">Ids to compare with logged user id.</param>
    public void CheckIfLoggedUserIsOneOf(IEnumerable<Guid> ids)
    {
        if (!ids.Any(id => id == _userId))
            throw new AuthorizationException();
    }
}
