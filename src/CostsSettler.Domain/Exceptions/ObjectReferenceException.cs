namespace CostsSettler.Domain.Exceptions;

public class ObjectReferenceException : DomainLogicException
{
    public ObjectReferenceException(string text) : base(text)
    {
    }
}
