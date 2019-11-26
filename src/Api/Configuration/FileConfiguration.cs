namespace Api.Configuration
{
    public class FileConfiguration
    {
        public string Path { get; set; } = "files";
        public long MaxSize { get; set; } = 2048;
    }
}