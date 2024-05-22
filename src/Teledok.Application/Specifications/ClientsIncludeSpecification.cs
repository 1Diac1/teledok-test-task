using Ardalis.Specification;
using Teledok.Domain.Entities;

namespace Teledok.Application.Specifications;

public class ClientsIncludeSpecification : Specification<Client>
{
    public ClientsIncludeSpecification()
    {       
        Query.Include(cl => cl.Founders);
    }
}