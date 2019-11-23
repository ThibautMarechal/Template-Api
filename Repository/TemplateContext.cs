using Microsoft.EntityFrameworkCore;
using Repository.Model;

namespace Repository
{
    public class TemplateContext : DbContext
    {

        public TemplateContext(DbContextOptions options): base(options)
        {}

        public System.Data.Entity.DbSet<User> Users { get; set; }
    }
}