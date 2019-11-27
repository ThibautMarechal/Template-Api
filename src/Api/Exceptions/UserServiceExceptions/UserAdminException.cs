using System;

namespace Api.Exceptions.UserServiceExceptions
{
    public class UserAdminException : Exception
    {
        public UserAdminException(): base("Admin user cannot be updated or deleted")
        {}
    }
}