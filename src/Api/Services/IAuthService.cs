using System.Threading.Tasks;
using Contract;

namespace Api.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(string userName, string password);
        Task<User> RefreshToken(string userName);
    }
}