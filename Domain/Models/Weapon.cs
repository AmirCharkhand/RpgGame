using Microsoft.EntityFrameworkCore.Infrastructure;
using RPG.Domain.Core;

namespace RPG.Domain.Models;

public class Weapon : Entity<int>
{
    private readonly ILazyLoader _loader;
    private Character _character;

    public Weapon()
    {
        
    }
    public Weapon(ILazyLoader loader)
    {
        _loader = loader;
    }

    public string Name { get; set; } = string.Empty;
    public float Damage { get; set; }
    public int CharacterRef { get; set; }

    public virtual Character? Character
    {
        get => _loader.Load(this, ref _character!);
        set => _character = value!;
    }
}