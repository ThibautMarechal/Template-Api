using System.ComponentModel.DataAnnotations;

namespace Contract
{
    public class MailIn
    {
        [Required]
        public string Content { set; get; }
        [EmailAddress]
        public string From { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}