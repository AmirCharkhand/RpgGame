using Microsoft.EntityFrameworkCore;
using RPG.Domain.Models;

namespace RPG.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<Character> Characters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>()
            .HasOne(c => c.Weapon)
            .WithOne(w => w.Character)
            .HasForeignKey<Weapon>(w => w.CharacterRef)
            .OnDelete(DeleteBehavior.Cascade);
    }
}