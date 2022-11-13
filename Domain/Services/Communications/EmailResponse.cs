using PRY20220278.Domain.Models;

namespace PRY20220278.Domain.Services.Communications
{
    public class EmailResponse :BaseResponse<Email>
    {
        public EmailResponse(Email resource) : base(resource)
        {
        }

        public EmailResponse(string message) : base(message)
        {
        }
    }
}