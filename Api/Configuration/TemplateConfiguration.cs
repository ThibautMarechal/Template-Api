namespace Api.Configuration
{
    public class TemplateConfiguration
    {
        public AuthConfiguration Auth { get; set; }
        public DatabaseConfiguration Database { get; set; }
        public AdminConfiguration Admin { get; set; }
    }
}