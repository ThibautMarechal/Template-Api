using System.Collections.Generic;
using System.Threading.Tasks;
using Contract;

namespace Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByUserNameAsync(string userName);
        Task DeleteByUserNameAsync(string userName);
        Task<User> UpdateUserAsync(User user, string password);
        Task<User> CreateUserAsync(User user, string password);
    }
}