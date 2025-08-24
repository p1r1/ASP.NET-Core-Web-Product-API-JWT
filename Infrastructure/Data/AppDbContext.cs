using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data {
    public class AppDbContext : IdentityDbContext<User> {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity => {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            builder.Entity<User>(entity => {
                entity.Property(u => u.FirstName).HasMaxLength(50);
                entity.Property(u => u.LastName).HasMaxLength(50);
            });
        }
    }
}