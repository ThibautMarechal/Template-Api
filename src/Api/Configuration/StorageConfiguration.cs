namespace Api.Configuration
{
    public class StorageConfiguration
    {
        public string Path { get; set; }
        public long MaxSize { get; set; } = 2048000;
    }
}