using System;

namespace Api.Exceptions.UserCreatorExceptions
{
    public class CreateRoleException : Exception
    {
        public CreateRoleException(): base("Failed to create role admin")
        {}
    }
}