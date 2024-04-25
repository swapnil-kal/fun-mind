using Microsoft.EntityFrameworkCore;
using User.Api.Configurations;
using User.Api.Entities;

namespace User.Api
{
    public class UserDbContext : DbContext
    {
        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<AgeGroupEntity> AgeGroups { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<AgeGroupCategoryEntity> AgeGroupCategories { get; set; }

        public DbSet<UserProfileEntity> UserProfiles { get; set; }


        public UserDbContext(DbContextOptions options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AgeGroupConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new AgeGroupCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileConfiguration());
        }
    }
}
