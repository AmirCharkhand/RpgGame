namespace RPG.Domain.Core;

public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
}