using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PRY20220278.Domain.Models;
using PRY20220278.Domain.Services.Communications;
using PRY20220278.Extentions;
using PRY20220278.Resources;
using SendGrid;
using SendGrid.Helpers.Mail;
using Swashbuckle.AspNetCore.Annotations;

namespace PRY20220278.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Produces("application/json")]
    public class EmailController :ControllerBase
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EmailController(ISendGridClient sendGridClient, IConfiguration iConfiguration, IMapper mapper)
        {
            _sendGridClient = sendGridClient;
            _configuration = iConfiguration;
            _mapper = mapper;
        }
        
        [HttpPost("send-email")]
        [SwaggerOperation(Summary = "Send an Email to an User")]
        [ProducesResponseType(typeof(AcceptedResult), 202)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(UnauthorizedResult), 401)]
        public async Task<IActionResult> SendEmailTo([FromBody] SaveEmailResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            
            var email = _mapper.Map<SaveEmailResource, Email>(resource);
            
            string fromEmail = _configuration
                .GetSection("SendGridEmailSettings")
                .GetValue<string>("FromEmail");
            string fromName = _configuration
                .GetSection("SendGridEmailSettings")
                .GetValue<string>("FromName");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = email.Subject,
                PlainTextContent = email.PlainTextContent,
            };
            
            msg.AddTo(email.EmailRecipient);
            
            var response = await _sendGridClient.SendEmailAsync(msg);
            //Debug.Print("Codigo de estado " + response.StatusCode);

            switch (response.StatusCode)
            {
                
                case HttpStatusCode.Accepted:
                {
                    return Accepted(new EmailResponse(email));
                }; break;
                case HttpStatusCode.BadRequest:
                {
                    return BadRequest(new EmailResponse("Email not sent, an error occurred"));
                }; break;
                case HttpStatusCode.Unauthorized:
                {
                    return Unauthorized(new EmailResponse("Email not sent, API Key is incorrect"));
                }; break;
                case HttpStatusCode.TooManyRequests:
                {
                    return BadRequest(new EmailResponse("Email not sent, Too many requests/Rate limit exceeded"));
                }; break;
                case HttpStatusCode.InternalServerError:
                {
                    return BadRequest(new EmailResponse("Email not sent, Internal server error"));
                }; break;
                case HttpStatusCode.Forbidden:
                {
                    return BadRequest(new EmailResponse("Email not sent, From address doesn't match Verified Sender Identity. " +
                                                        "To learn how to resolve this error, see our Sender Identity requirements."));
                }; break;
                default:
                    return BadRequest(new EmailResponse("Email not sent, an error occurred"));
            }
            
           
        }
    }
}