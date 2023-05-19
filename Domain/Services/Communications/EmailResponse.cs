using VotechainMails.Domain.Models;

namespace VotechainMails.Domain.Services.Communications
{
    public class EmailResponse :BaseResponse<Email>
    {
        public EmailResponse(Email resource) : base(resource)
        {
        }

        public EmailResponse(string message, int statusCode) : base(message,statusCode)
        {
        }
    }
}