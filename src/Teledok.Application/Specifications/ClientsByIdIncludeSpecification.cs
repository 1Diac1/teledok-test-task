using Ardalis.Specification;
using Teledok.Domain.Entities;

namespace Teledok.Application.Specifications;

public class ClientsByIdIncludeSpecification : Specification<Client>
{
    public ClientsByIdIncludeSpecification(int clientId)
    {
        Query.Where(cl => cl.Id == clientId)
            .Include(cl => cl.Founders);
    }
}