using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Api.Entities;

namespace User.Api.Configurations
{
    public class AgeGroupCategoryConfiguration : IEntityTypeConfiguration<AgeGroupCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<AgeGroupCategoryEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AgeGroupId).IsRequired(true);

            builder.Property(x => x.CategoryId).IsRequired(true);

            builder
              .HasOne(e => e.AgeGroup)
              .WithMany(c => c.AgeGroupCategories)
              .HasForeignKey(c => c.AgeGroupId)
              .IsRequired(true)
              .OnDelete(DeleteBehavior.Cascade);

            builder
             .HasOne(e => e.Category)
             .WithMany(c => c.AgeGroupCategories)
             .HasForeignKey(c => c.CategoryId)
             .IsRequired(true)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
