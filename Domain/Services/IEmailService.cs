using System.Threading.Tasks;
using PRY20220278.Domain.Models;
using PRY20220278.Domain.Services.Communications;

namespace PRY20220278.Domain.Services
{
    public interface IEmailService 
    {
        Task<EmailResponse> SendPlainText(Email email, string fromName, string fromEmail);
        Task<EmailResponse> SendHtml(Email email, string fromName, string fromEmail);
    }
}