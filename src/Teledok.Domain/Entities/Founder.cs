namespace Teledok.Domain.Entities;

public class Founder : BaseEntity<int>
{
    public string? INN { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PatronymicName { get; set; }

    public int ClientId { get; set; }
    public Client? Client { get; set; }
}