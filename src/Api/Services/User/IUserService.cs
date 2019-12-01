using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services.User
{
    public interface IUserService
    {
        Task<IEnumerable<Contract.User>> GetAllAsync();
        Task<Contract.User> GetByUserNameAsync(string userName);
        Task DeleteByUserNameAsync(string userName);
        Task<Contract.User> UpdateUserAsync(Contract.User user, string password);
        Task<Contract.User> CreateUserAsync(Contract.User user, string password);
    }
}