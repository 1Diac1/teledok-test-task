using Teledok.Domain.Enums;

namespace Teledok.Application.Dtos;

public class ClientDto
{
    public string INN { get; set; }
    public string Name { get; set; }
    public ClientType ClientType { get; set; }
    public IList<FounderDto> Founders { get; set; }
}