using System;

namespace Api.Exceptions.UserServiceExceptions
{
    public class UserNotFoundException: Exception
    {
        public UserNotFoundException(string userName): base($"user \"{userName}\" not found.")
        {}
    }
}