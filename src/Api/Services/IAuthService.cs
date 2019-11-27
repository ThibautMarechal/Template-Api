using System.Threading.Tasks;
using Contract;

namespace Api.Services
{
    public interface IAuthService
    {
        Task<User> AuthenticateAsync(string userName, string password);
        Task<User> RefreshTokenAsync(string userName);
    }
}