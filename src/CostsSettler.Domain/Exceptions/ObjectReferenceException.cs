namespace CostsSettler.Domain.Exceptions;

/// <summary>
/// Exception that informs object reference failed.
/// </summary>
public class ObjectReferenceException : CostsSettlerExceptionBase
{
    /// <summary>
    /// Creates new default ObjectReferenceException instance.
    /// </summary>
    public ObjectReferenceException(string text) : base(text)
    {
    }
}
