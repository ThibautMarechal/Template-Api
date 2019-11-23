using System.Threading.Tasks;
using Contract;

namespace Api.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(string userId);
        Task<User> GetUserByUserNameAndPassword(string userName, string password);
    }
}