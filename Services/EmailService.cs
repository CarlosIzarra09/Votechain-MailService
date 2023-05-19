using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using VotechainMails.Domain.Models;
using VotechainMails.Domain.Services;
using VotechainMails.Domain.Services.Communications;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace VotechainMails.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
       

        public EmailService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task<EmailResponse> SendPlainText(Email email, string fromName, string fromEmail)
        {
            
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = email.Subject,
                PlainTextContent = email.PlainTextContent,
            };
            
            msg.AddTo(email.EmailRecipient);
            
            var response = await _sendGridClient.SendEmailAsync(msg);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return new EmailResponse(email);
            }

            return new EmailResponse("", (int)response.StatusCode);
        }

        public async Task<EmailResponse> SendHtml(Email email, string fromName, string fromEmail)
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Assets/EmailTemplates/EmailWithOTP.html");
            string htmlContent = await File.ReadAllTextAsync(path);
            int otpCode = 789456;
            
            htmlContent = htmlContent.Replace("{{name}}", email.PlainTextContent)
                .Replace("{{code}}",otpCode.ToString());
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = email.Subject,
                HtmlContent = htmlContent
            };
            
            msg.AddTo(email.EmailRecipient);

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (response.StatusCode == HttpStatusCode.Accepted)
            {
                return new EmailResponse(email);
            }

            return new EmailResponse("", (int)response.StatusCode);
        }
    }
}