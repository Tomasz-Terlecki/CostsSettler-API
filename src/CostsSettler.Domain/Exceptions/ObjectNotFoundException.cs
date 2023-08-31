namespace CostsSettler.Domain.Exceptions;

/// <summary>
/// Exception that informs object is not found.
/// </summary>
public class ObjectNotFoundException : CostsSettlerExceptionBase
{
    /// <summary>
    /// Creates new default ObjectNotFoundException instance.
    /// </summary>
    public ObjectNotFoundException(Type type, Guid id) : base($"Could not find object of type {nameof(type)} with Id {id}")
    {
    }
}
