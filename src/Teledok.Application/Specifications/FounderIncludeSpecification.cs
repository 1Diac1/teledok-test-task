using Ardalis.Specification;
using Teledok.Domain.Entities;

namespace Teledok.Application.Specifications;

public class FounderIncludeSpecification : Specification<Founder>
{
    public FounderIncludeSpecification()
    {
        Query.Include(f => f.Client);
    }
}