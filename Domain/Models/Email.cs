namespace VotechainMails.Domain.Models
{
    public class Email
    {
        public string EmailRecipient { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }

    }
}