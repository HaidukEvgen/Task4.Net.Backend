namespace Task4.Backend.Exceptions
{
    public class UserDeletedException : Exception
    {
        public UserDeletedException() { }
        public UserDeletedException(string message) : base(message) { }
    }
}
