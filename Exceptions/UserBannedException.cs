namespace Task4.Backend.Exceptions
{
    public class UserBannedException : Exception
    {
        public UserBannedException() { }
        public UserBannedException(string message) : base(message) { }
    }
}
