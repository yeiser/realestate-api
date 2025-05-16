using Application.Commands.Properties;
using Application.Commands.PropertyTraces;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Owner, OwnerDto>();
            CreateMap<Property, PropertyDto>();
            CreateMap<CreatePropertyWithImageCommand, Property>();
            CreateMap<CreatePropertyTraceCommand, PropertyTrace>();
        }
    }
}
