using System.Collections.Generic;
using System.Threading.Tasks;
using Contract;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>
        {
            new User
            {
                Id = "AAAA-AAAA",
                Username = "testA",
                Password = "testA"
            },
            new User
            {
                Id = "BBBB-BBBB",
                Username = "testB",
                Password = "testB"
            }
        };
        public Task<User> GetUserById(string userId)
        {
            return Task.FromResult(_users.Find(u => userId == u.Id));
        }

        public Task<User> GetUserByUserNameAndPassword(string userName, string password)
        {
            return Task.FromResult(_users.Find(u => userName == u.Username && password == u.Password));
        }
    }
}