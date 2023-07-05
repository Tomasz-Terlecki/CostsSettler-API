namespace CostsSettler.Domain.Exceptions;
public class ObjectNotFoundException : CostsSettlerExceptionBase
{
    public ObjectNotFoundException(Type type, Guid id) : base($"Could not find object of type {nameof(type)} with Id {id}")
    {
    }
}
