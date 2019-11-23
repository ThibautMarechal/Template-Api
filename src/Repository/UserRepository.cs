using System.Data.Entity;
using System.Threading.Tasks;
using Repository.Model;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TemplateContext _context;

        public UserRepository(TemplateContext context)
        {
            _context = context;
        }
        
        public Task<User> GetById(string userId)
        {
            return _context.Users.FirstOrDefaultAsync(u => userId == u.Id);
        }

        public Task<User> GetByUserName(string userName)
        {
            return _context.Users.FirstOrDefaultAsync(u => userName == u.Username);
        }

        public async Task<User> Create(User user)
        {
            if (await _context.Users.AnyAsync(u => user.Id == u.Id || user.Username == u.Username))
            {
                return null;
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return user;
        }
    }
}