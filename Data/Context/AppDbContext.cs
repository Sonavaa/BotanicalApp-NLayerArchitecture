using Botanical.Models;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<GetInTouch> GetInTouches { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishListItem>()
    .HasKey(w => new { w.AppUserId, w.ProductId });

            modelBuilder.Entity<WishListItem>()
                .HasOne(w => w.Product)
                .WithMany(p => p.WishListItems)
                .HasForeignKey(w => w.ProductId);

            modelBuilder.Entity<WishListItem>()
                .HasOne(w => w.AppUser)
                .WithMany(u => u.WishListItems)
                .HasForeignKey(w => w.AppUserId);


            //        modelBuilder.Entity<Cart>()
            //.HasOne(c => c.AppUser)
            //.WithMany()
            //.HasForeignKey(c => c.AppUserId)
            //.OnDelete(DeleteBehavior.Cascade);

        }
    }
}
