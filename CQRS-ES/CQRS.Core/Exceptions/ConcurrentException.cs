namespace CQRS.Core.Exceptions
{
    public class ConcurrentException : Exception
    {
        public ConcurrentException(string message) : base(message)
        {

        }
    }
}
