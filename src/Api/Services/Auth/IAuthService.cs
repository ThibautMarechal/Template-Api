using System.Threading.Tasks;

namespace Api.Services.Auth
{
    public interface IAuthService
    {
        Task<Contract.User> AuthenticateAsync(string userName, string password);
        Task<Contract.User> RefreshTokenAsync(string userName);
    }
}