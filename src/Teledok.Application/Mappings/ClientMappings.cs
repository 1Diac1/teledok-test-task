using AutoMapper;
using Teledok.Application.Dtos;
using Teledok.Domain.Entities;

namespace Teledok.Application.Mappings;

public class ClientMappings : Profile
{
    public ClientMappings()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<Client, ClientDto>().ReverseMap();
    }
}