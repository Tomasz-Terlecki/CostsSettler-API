namespace CostsSettler.Domain.Exceptions
{
    public abstract class CostsSettlerExceptionBase : Exception
    {
        public CostsSettlerExceptionBase(string text) : base(text)
        {
        }
    }
}