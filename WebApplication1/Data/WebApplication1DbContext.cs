using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class WebApplication1DbContext : DbContext
    {
        public WebApplication1DbContext(DbContextOptions<WebApplication1DbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductPrice)
                .HasColumnType("money");

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderPrice)
                .HasColumnType("money");
        }
    }
}