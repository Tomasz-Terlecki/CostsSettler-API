namespace CostsSettler.Domain.Exceptions
{
    /// <summary>
    /// Base abstract class for application exceptions.
    /// </summary>
    public abstract class CostsSettlerExceptionBase : Exception
    {
        /// <summary>
        /// Creates new default CostsSettlerExceptionBase instance.
        /// </summary>
        public CostsSettlerExceptionBase(string text) : base(text)
        {
        }
    }
}