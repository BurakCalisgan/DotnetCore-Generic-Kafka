using Microsoft.EntityFrameworkCore;

namespace User.Api.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<Entity.User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("user");
            modelBuilder.Entity<Entity.User>().ToTable("users");
        }
    }
}