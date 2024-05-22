﻿using System.Linq.Expressions;
using Ardalis.Specification;
using Teledok.Domain.Entities;

namespace Teledok.Infrastructure.Abstractions.Repositories;

public interface IReadEntityRepository<TKey, TEntity> 
    where TEntity : BaseEntity<TKey>
    where TKey : struct
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(ISpecification<TEntity> spec, bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(IList<ISpecification<TEntity>> specs, bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, CancellationToken  cancellationToken = default);
    Task<TEntity> GetAsync(ISpecification<TEntity> spec, bool disableTracking = true, CancellationToken  cancellationToken = default);
    Task<TEntity> GetByIdAsync(TKey id, bool disableTracking = true, CancellationToken cancellationToken = default);
}