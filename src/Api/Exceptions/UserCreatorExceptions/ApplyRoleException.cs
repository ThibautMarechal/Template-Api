using System;

namespace Api.Exceptions.UserCreatorExceptions
{
    public class ApplyRoleException : Exception
    {
        public ApplyRoleException(): base("failed to bind role admin to user admin")
        {}
    }
}