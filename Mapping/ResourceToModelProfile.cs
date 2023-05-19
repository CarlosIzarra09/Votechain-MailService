using AutoMapper;
using VotechainMails.Domain.Models;
using VotechainMails.Resources;

namespace VotechainMails.Mapping
{
    public class ResourceToModelProfile :Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveEmailResource, Email>();
        }
    }
}