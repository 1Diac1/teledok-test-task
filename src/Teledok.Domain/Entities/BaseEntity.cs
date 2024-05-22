namespace Teledok.Domain.Entities;

public abstract class BaseEntity<TKey>
    where TKey : struct
{
    public virtual TKey Id { get; set; }

    public virtual DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual DateTime UpdatedAt { get; set; }
}