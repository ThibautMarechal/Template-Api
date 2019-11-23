using System.Threading.Tasks;
using Contract;

namespace Api.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> RefreshToken(string userId);
    }
}