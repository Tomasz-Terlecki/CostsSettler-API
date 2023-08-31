namespace CostsSettler.Domain.Exceptions;

/// <summary>
/// Exception that informs domain logic exception occured.
/// </summary>
public class DomainLogicException : CostsSettlerExceptionBase
{
    /// <summary>
    /// Creates new default DomainLogicException instance.
    /// </summary>
    public DomainLogicException(string text) : base(text)
    {   
    }
}
