using System.ComponentModel.DataAnnotations;

namespace VotechainMails.Resources
{
    public class SaveEmailResource
    {
        [Required]
        [MaxLength(40)]
        public string EmailRecipient { get; set; }
        [Required]
        [MaxLength(20)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(200)]
        public string PlainTextContent { get; set; }
    }
}