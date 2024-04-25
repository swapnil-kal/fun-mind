using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using User.Api.Constants;
using User.Api.Entities;

namespace User.Api.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName)
             .IsRequired(true);

            builder.Property(e => e.LastName)
             .IsRequired(true);

            builder.Property(e => e.Email)
              .IsRequired(true);

            builder.Property(e => e.Password)
             .IsRequired(true);

            builder.Property(e => e.LoginProvider)
             .IsRequired(true);

            builder
               .HasOne(e => e.Role)
               .WithMany(c => c.Users)
               .HasForeignKey(c => c.RoleId)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Cascade);             

            builder.HasOne(u => u.UserProfile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfileEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.Email).IsUnique();
        }
    }
}
