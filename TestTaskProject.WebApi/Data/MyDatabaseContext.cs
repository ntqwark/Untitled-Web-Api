using Microsoft.EntityFrameworkCore;
using TestTaskProject.Common.Models;

namespace TestTaskProject.WebApi.Data
{
    public class MyDatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<SalesPoint> SalePoints { get; set; }
        public DbSet<ProvidedProduct> ProvidedProducts { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleData> SalesData { get; set; }

        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
