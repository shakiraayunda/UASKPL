using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KatalogProduk.Models;

namespace KatalogProduk.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleSeed());
        }
        public DbSet<KatalogProduk.Models.Category> Category { get; set; }
        public DbSet<KatalogProduk.Models.Product> Product { get; set; }
    }
}