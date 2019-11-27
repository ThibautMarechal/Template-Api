namespace Api.Configuration
{
    public class AdminConfiguration {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string AdminRole { get; } = "admin";
    }
}