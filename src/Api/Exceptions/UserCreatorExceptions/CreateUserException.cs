using System;

namespace Api.Exceptions.UserCreatorExceptions
{
    public class CreateUserException : Exception
    {
        public CreateUserException(): base("Failed to create user admin")
        {}
    }
}