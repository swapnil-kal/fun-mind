namespace Content.Api.Exceptions
{
    public class UnAuthenticateException : Exception
    {
        public UnAuthenticateException(string message) : base(message) { }
        public UnAuthenticateException(string message, Exception ex) : base(message, ex) { }
    }
}
