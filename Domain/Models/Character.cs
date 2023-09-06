using Microsoft.EntityFrameworkCore.Infrastructure;
using RPG.Application.Models;
using RPG.Domain.Core;

namespace RPG.Domain.Models;

public class Character : Entity<int>
{
    private readonly ILazyLoader _loader;
    private List<Skill> _skills;
    private User _user;
    private Weapon _weapon;

    public Character()
    {
        
    }
    public Character(ILazyLoader loader)
    {
        _loader = loader;
    }
    
    
    public string Name { get; set; } = "Amiroo";
    public float Health { get; set; }
    public float Strength { get; set; }
    public float Attack { get; set; }
    public float Defence { get; set; }
    public CharacterType Type { get; set; }
    public int Fights { get; set; }
    public int Victories { get; set; }
    public int Defeats { get; set; }

    public virtual User? User
    {
        get => _loader.Load(this, ref _user!);
        set => _user = value!;
    }
    public virtual Weapon? Weapon
    {
        get => _loader.Load(this, ref _weapon!);
        set => _weapon = value!;
    }


    public virtual List<Skill>? Skills
    {
        get => _loader.Load(this, ref _skills!);
        set => _skills = value!;
    }
}