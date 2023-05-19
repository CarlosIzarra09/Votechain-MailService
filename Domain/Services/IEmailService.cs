using System.Threading.Tasks;
using VotechainMails.Domain.Models;
using VotechainMails.Domain.Services.Communications;

namespace VotechainMails.Domain.Services
{
    public interface IEmailService 
    {
        Task<EmailResponse> SendPlainText(Email email, string fromName, string fromEmail);
        Task<EmailResponse> SendHtml(Email email, string fromName, string fromEmail);
    }
}