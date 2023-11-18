using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Domain.Models;

namespace RPG.Infrastructure.Data.Config
{
    public class CharacterConfig : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder
                .HasOne(c => c.Weapon)
                .WithOne(w => w.Character)
                .HasForeignKey<Weapon>(w => w.CharacterRef)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(55);
        }
    }
}