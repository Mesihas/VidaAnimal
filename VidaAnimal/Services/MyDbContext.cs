using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using VidaAnimal.Models;

namespace VidaAnimal.Services
{
    public class MyDbContext : IdentityDbContext
    {
        public DbSet<Product> TblProduct { get; set; }
        public DbSet<Sales> TblSales { get; set; }
        public DbSet<SalesDetail> TblSalesDetail { get; set; }
        public DbSet<Purchasing> TblPurchasing { get; set; }
        public DbSet<Supplier> TblSupplier { get; set; }
        public DbSet<Stock> TblStock { get; set; }
        public DbSet<Profile> TblProfile { get; set; }
        public DbSet<Category> TblCategory { get; set; }
        public DbSet<Client> TblClient { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
        //    option.UseSqlServer("Data Source = 10.0.1.225,1433; Initial Catalog = VidaAnimal;  User ID = sa; Password = macanudo22 ;");
            option.UseSqlServer("Data Source = DESKTOP-ALE; Initial Catalog = VidaAnimal; Integrated Security=True; MultipleActiveResultSets=true"); 
        }
    }
}
