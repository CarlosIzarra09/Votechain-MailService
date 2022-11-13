using AutoMapper;
using PRY20220278.Domain.Models;
using PRY20220278.Resources;

namespace PRY20220278.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Email,EmailResource>();
        }
    }
}