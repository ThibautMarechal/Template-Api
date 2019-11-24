using Api.Configuration;
using Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            
            //Configurations
            var appSettingsSection = Configuration.GetSection("Template");
            services.Configure<TemplateConfiguration>(appSettingsSection);
            var templateConfiguration = appSettingsSection.Get<TemplateConfiguration>();
            
            services.AddSingleton(templateConfiguration.Auth);
            services.AddSingleton(templateConfiguration.Admin);
            
            //Authentications & Authorizations
            services.AddAuth(templateConfiguration.Auth);
            
            //Services
            services.AddScoped<IAuthService, AuthService>();

            //Database
            services.AddDbContext<TemplateContext>(options =>
                options.UseMySql(templateConfiguration.Database.ConnectionString));
            
            //Repositories
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}