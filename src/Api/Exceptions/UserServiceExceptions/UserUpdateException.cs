using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Api.Exceptions.UserServiceExceptions
{
    public class UserUpdateException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }

        public UserUpdateException(string userName, IEnumerable<IdentityError> errors): base($"Failed to update user \"{userName}\"")
        {
            Errors = errors;
        }
    }
}