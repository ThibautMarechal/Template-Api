using Microsoft.EntityFrameworkCore;
using Repository.Model;

namespace Repository
{
    public class BaseContext : DbContext
    {

        public BaseContext(DbContextOptions options): base(options)
        {}

        public System.Data.Entity.DbSet<User> Users { get; set; }
    }
}