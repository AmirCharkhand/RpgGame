using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Domain.Models;

namespace RPG.Infrastructure.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(u => u.Username)
                .IsRequired()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            builder
                .Property(u => u.PasswordHash)
                .IsRequired();
        }
    }
}
