using CaravanApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CaravanApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary> All users we have in DB so far. </summary>
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products {  get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Product>().ToTable("product");
        }
    }
}
