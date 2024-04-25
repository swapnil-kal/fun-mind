using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Api.Entities;

namespace User.Api.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
              .IsRequired(true);

            builder.HasIndex(p => p.Name).IsUnique();
        }
    }
}
