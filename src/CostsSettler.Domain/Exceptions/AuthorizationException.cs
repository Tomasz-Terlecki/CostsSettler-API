namespace CostsSettler.Domain.Exceptions;

/// <summary>
/// Exception that informs user is unauthorized.
/// </summary>
public class AuthorizationException : CostsSettlerExceptionBase
{
    /// <summary>
    /// Creates new default AuthorizationException instance.
    /// </summary>
    public AuthorizationException() : base("You are not authorized to do this operation")
    {   
    }
}
