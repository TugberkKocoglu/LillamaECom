using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace MultiShop.Models
{
    public class ShopConnection:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //veri tabanı baglanti ayarları

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration["ConnectionStrings:ShopConnection"]);
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<vw_MyOrders> vw_MyOrders { get; set; }

        public DbSet<Sp_Arama> sp_Aramas { get; set; }
    }
}
