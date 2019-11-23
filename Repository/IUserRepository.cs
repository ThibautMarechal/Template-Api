using System.Threading.Tasks;
using Repository.Model;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User> GetById(string userId);
        Task<User> GetByUserName(string userName);
        Task<User> Create(User user);
    }
}