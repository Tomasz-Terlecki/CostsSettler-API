using CostsSettler.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CostsSettler.Domain.Services;
public class IdentityService : IIdentityService
{
    private readonly HttpContext _httpContext;

    public bool IsAuthenticated
        => _httpContext.User.Identity?.IsAuthenticated ?? false;
    public Guid UserId
        => Guid.TryParse(_httpContext.User.FindFirst("appId")?.Value, out Guid id) ? id : throw new AuthorizationException();

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext);

        _httpContext = httpContextAccessor.HttpContext;
    }

    public void CheckEqualityWithLoggedUserId(Guid userId)
    {
        if (userId != UserId)
            throw new AuthorizationException();
    }

    public void CheckIfLoggedUserIsOneOf(IEnumerable<Guid> ids)
    {
        if (!ids.Any(id => id == UserId))
            throw new AuthorizationException();
    }
}
