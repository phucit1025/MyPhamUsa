using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyPhamUsa.Client.Models.Entities;

namespace MyPhamUsa.Client.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext() : base(new DbContextOptionsBuilder()
            .UseLazyLoadingProxies()
            .UseSqlServer(@"Data Source=45.119.212.145;Initial Catalog=MyPhamUsa_Client;persist security info=True;Integrated Security=False;TrustServerCertificate=False;uid=sa;password=zaq@123;Trusted_Connection=False;MultipleActiveResultSets=true;")
            .Options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }
}
