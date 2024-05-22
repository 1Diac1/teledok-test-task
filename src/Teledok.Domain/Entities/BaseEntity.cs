namespace Teledok.Domain.Entities;

public abstract class BaseEntity<TKey>
    where TKey : struct
{
    public virtual TKey Id { get; set; }

    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}