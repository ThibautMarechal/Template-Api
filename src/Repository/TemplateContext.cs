using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TemplateContext : IdentityDbContext
    {

        public TemplateContext(DbContextOptions options): base(options)
        {}
    }
}