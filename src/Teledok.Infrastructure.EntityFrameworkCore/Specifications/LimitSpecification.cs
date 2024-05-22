using Ardalis.Specification;

namespace Teledok.Infrastructure.EntityFrameworkCore.Specifications;

public class LimitSpecification<TEntity> : Specification<TEntity>
{
    public LimitSpecification(int limit)
    {
        Query.Take(limit);
    }
}