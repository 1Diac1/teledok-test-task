using Teledok.Domain.Enums;

namespace Teledok.Domain.Entities;

public class Client : BaseEntity<int>
{
    public string INN { get; set; }
    public string Name { get; set; }
    public ClientType ClientType { get; set; }

    public IList<Founder> Founders { get; set; }
}