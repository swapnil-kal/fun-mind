using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Api.Entities;

namespace User.Api.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfileEntity>
    {
        public void Configure(EntityTypeBuilder<UserProfileEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AgeGroupId).IsRequired(true);

            builder.Property(x => x.CategoryIds).IsRequired(true);

            builder
             .HasOne(e => e.AgeGroup)
             .WithMany()
             .HasForeignKey(c => c.AgeGroupId)
             .IsRequired(true);
        }
    }
}
