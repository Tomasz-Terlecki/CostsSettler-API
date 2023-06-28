namespace CostsSettler.Domain.Exceptions;
public abstract class DomainLogicException : Exception
{
    public DomainLogicException(string text) : base(text)
    {   
    }
}
