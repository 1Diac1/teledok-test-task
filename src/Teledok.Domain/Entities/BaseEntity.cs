namespace Teledok.Domain.Entities;

public abstract class BaseEntity<TKey>
    where TKey : struct
{
    public virtual TKey Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}