using System;
using System.ComponentModel.DataAnnotations;

namespace Contract
{
    public class FileIn
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTimeOffset ShootingDate { get; set; }
        
        public byte[] ImageData { get; set; }
    }
}