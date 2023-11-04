using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domian.Entities;
using Pharmacy.Domian.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Infrastructure.Data
{
    public class StoreContext : IdentityDbContext<IdentityUser>
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        public DbSet<Product> products { get; set; }
        public DbSet <ProductType> productTypes { get; set; }
        public DbSet<Basket> baskets { get; set; }
        public DbSet <BasketItem> basketItems { get; set; }

        public DbSet <Order> orders { get; set; }
        public DbSet <OrderItem> orderItems { get; set; }
        public DbSet <DeliveryMethod> deliveryMethods { get; set; }

     
        public DbSet<WishList> WishLists { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasNoKey();
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();



        }
    }
}
