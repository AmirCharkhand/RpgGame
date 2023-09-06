using Microsoft.EntityFrameworkCore.Infrastructure;
using RPG.Domain.Core;

namespace RPG.Domain.Models;

public class Skill : Entity<int>
{
    private readonly ILazyLoader _loader;
    private List<Character> _characters;

    public Skill()
    {
        
    }
    public Skill(ILazyLoader loader)
    {
        _loader = loader;
    }

    public string Name { get; set; } = string.Empty;
    public float Damage { get; set; }
    public virtual List<Character>? Characters
    {
        get => _loader.Load(this, ref _characters!);
        set => _characters = value!;
    }
}