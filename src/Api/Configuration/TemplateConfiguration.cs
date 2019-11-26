namespace Api.Configuration
{
    public class TemplateConfiguration
    {
        public AuthConfiguration Auth { get; set; } = new AuthConfiguration();
        public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();
        public AdminConfiguration Admin { get; set; } = new AdminConfiguration();
        public FileConfiguration Files { get; set; } = new FileConfiguration();
    }
}