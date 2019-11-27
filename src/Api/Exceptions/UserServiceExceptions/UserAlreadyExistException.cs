using System;

namespace Api.Exceptions.UserServiceExceptions
{
    public class UserAlreadyExistException : Exception
    {
        public string UserName { get; }

        public UserAlreadyExistException(string userName): base($"A user with the userName \"{userName}\" already exist")
        {
            UserName = userName;
        }
    }
}