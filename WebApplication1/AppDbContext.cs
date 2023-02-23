using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products => Set<Product>();
    }
}
