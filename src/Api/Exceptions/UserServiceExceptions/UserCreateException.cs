using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Exceptions.UserServiceExceptions
{
    public class UserCreateException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }

        public UserCreateException(IEnumerable<IdentityError> errors): base("failed to create user")
        {
            Errors = errors;
        }
    }
}