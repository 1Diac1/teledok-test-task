using Ardalis.Specification;
using Teledok.Domain.Entities;

namespace Teledok.Application.Specifications;

public class FounderUniqueSpecification : Specification<Founder>
{
    public FounderUniqueSpecification(string inn)
    {
        Query.Where(f => f.INN == inn);
    }
}