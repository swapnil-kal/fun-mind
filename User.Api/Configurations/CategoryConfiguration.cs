using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Api.Entities;

namespace User.Api.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
              .IsRequired(true);

            builder.HasIndex(p => p.Title).IsUnique();
        }
    }
}
