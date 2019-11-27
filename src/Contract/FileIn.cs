using System.ComponentModel.DataAnnotations;

namespace Contract
{
    public class FileIn
    {
        [Required]
        public string Name { get; set; }
        
        public byte[] Image { get; set; }
    }
}