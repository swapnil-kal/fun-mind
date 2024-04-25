using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Api.Entities;

namespace User.Api.Configurations
{
    public class AgeGroupConfiguration : IEntityTypeConfiguration<AgeGroupEntity>
    {
        public void Configure(EntityTypeBuilder<AgeGroupEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
              .IsRequired(true);

            builder.HasIndex(p => p.Title).IsUnique();
        }
    }
}
