using Ardalis.Specification;
using Teledok.Domain.Entities;

namespace Teledok.Application.Specifications;

public class ClientUniqueSpecification : Specification<Client>
{
    public ClientUniqueSpecification(string inn, string name)
    {
        Query.Where(f => f.INN == inn || f.Name == name);
    }
}