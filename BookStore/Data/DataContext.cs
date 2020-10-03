using BookStore.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookStore.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Item> Items { get; set; }


        public DbSet<Country> Countries { get; set; }


        public DbSet<Order> Orders { get; set; }


        public DbSet<OrderDetail> OrderDetails { get; set; }


        public DbSet<OrderDetailTemp> OrderDetailsTemp { get; set; }


        public DbSet<Category> Categories { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");




            modelBuilder.Entity<OrderDetailTemp>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");



            modelBuilder.Entity<OrderDetail>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Ignore<SelectListItem>();
            modelBuilder.Ignore<SelectListGroup>();


            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
