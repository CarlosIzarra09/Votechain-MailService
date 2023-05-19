using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VotechainMails.Domain.Models;
using VotechainMails.Domain.Services;
using VotechainMails.Domain.Services.Communications;
using VotechainMails.Extentions;
using VotechainMails.Resources;
using SendGrid;
using SendGrid.Helpers.Mail;
using Swashbuckle.AspNetCore.Annotations;

namespace VotechainMails.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Produces("application/json")]
    public class EmailController :ControllerBase
    {
        //private readonly ISendGridClient _sendGridClient;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        private readonly string fromEmail;
        private readonly string fromName;

        public EmailController(ISendGridClient sendGridClient, IConfiguration iConfiguration, IEmailService emailService ,IMapper mapper)
        {
            //_sendGridClient = sendGridClient;
            _configuration = iConfiguration;
            _emailService = emailService;
            _mapper = mapper;
            
            fromEmail =  _configuration.GetSection("SendGridEmailSettings")
                .GetValue<string>("FromEmail");
            fromName = _configuration
                .GetSection("SendGridEmailSettings")
                .GetValue<string>("FromName");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private IActionResult HandlerResponse(EmailResponse emailResponse)
        {
            switch (emailResponse.StatusCode)
            {
                case 202:
                {
                    return Accepted(emailResponse);
                }
                case 400:
                {
                    emailResponse.Message = "Email not sent, an error occurred";
                    return BadRequest(emailResponse);
                }
                case 401:
                {
                    emailResponse.Message = "Email not sent, API Key is incorrect";
                    return Unauthorized(emailResponse);
                }
                case 429:
                {
                    emailResponse.Message = "Email not sent, Too many requests/Rate limit exceeded";
                    return BadRequest(emailResponse);
                }
                case 500:
                {
                    emailResponse.Message = "Email not sent, Internal server error";
                    return BadRequest(emailResponse);
                }
                case 403:
                {
                    emailResponse.Message = "Email not sent, From address doesn't match Verified Sender Identity. " +
                                            "To learn how to resolve this error, see our Sender Identity requirements.";
                    return BadRequest(emailResponse);
                }
                default:
                {
                    emailResponse.Message = "Email not sent, an error occurred";
                    return BadRequest(emailResponse);
                }

            }
        }


        [HttpPost("send-email-onlyText")]
        [SwaggerOperation(Summary = "Send an Email to an User")]
        [ProducesResponseType(typeof(EmailResponse), 202)]
        [ProducesResponseType(typeof(EmailResponse), 400)]
        [ProducesResponseType(typeof(EmailResponse), 401)]
        public async Task<IActionResult> SendEmailPlainTextTo([FromBody] SaveEmailResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            
            var email = _mapper.Map<SaveEmailResource, Email>(resource);

            var response = await _emailService.SendPlainText(email, fromName, fromEmail);

            return HandlerResponse(response);
        }
        
        [HttpPost("send-email")]
        [SwaggerOperation(Summary = "Send an Email to an User")]
        [ProducesResponseType(typeof(EmailResponse), 202)]
        [ProducesResponseType(typeof(EmailResponse), 400)]
        [ProducesResponseType(typeof(EmailResponse), 401)]
        public async Task<IActionResult> SendEmailHtmlTo([FromBody] SaveEmailResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            
            var email = _mapper.Map<SaveEmailResource, Email>(resource);

            var response = await _emailService.SendHtml(email, fromName, fromEmail);

            return HandlerResponse(response);
        }
    }

    
}