using AutoMapper;
using VotechainMails.Domain.Models;
using VotechainMails.Resources;

namespace VotechainMails.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Email,EmailResource>();
        }
    }
}