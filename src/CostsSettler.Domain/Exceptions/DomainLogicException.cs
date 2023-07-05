namespace CostsSettler.Domain.Exceptions;
public class DomainLogicException : CostsSettlerExceptionBase
{
    public DomainLogicException(string text) : base(text)
    {   
    }
}
