using AutoMapper;
using Teledok.Application.Dtos;
using Teledok.Domain.Entities;

namespace Teledok.Application.Mappings;

public class FounderMappings : Profile
{
    public FounderMappings()
    {
        CreateMap<Founder, FounderDto>();
        CreateMap<Founder, FounderDto>().ReverseMap();
    }
}