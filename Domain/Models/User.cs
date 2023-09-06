using Microsoft.EntityFrameworkCore.Infrastructure;
using RPG.Domain.Core;

namespace RPG.Domain.Models;

public class User : Entity<int>
{
    private readonly ILazyLoader _loader;
    private List<Character> _characters;

    public User()
    {
        
    }
    public User(ILazyLoader loader)
    {
        _loader = loader;
    }

    public string Username { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;

    public virtual List<Character>? Characters
    {
        get => _loader.Load(this, ref _characters!);
        set => _characters = value!;
    }
}