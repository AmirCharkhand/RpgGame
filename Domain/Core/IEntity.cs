namespace RPG.Domain.Core;

public interface IEntity<TKey>
{
    public TKey Id { get; set; }
}