namespace CostsSettler.Domain.Exceptions;
public class AuthorizationException : CostsSettlerExceptionBase
{
    public AuthorizationException() : base("You are not authorized to do this operation")
    {   
    }
}
