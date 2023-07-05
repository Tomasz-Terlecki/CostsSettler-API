namespace CostsSettler.Domain.Exceptions;

public class ObjectReferenceException : CostsSettlerExceptionBase
{
    public ObjectReferenceException(string text) : base(text)
    {
    }
}
