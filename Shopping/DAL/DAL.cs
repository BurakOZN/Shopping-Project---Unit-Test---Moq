using Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<CampaignCategory> CampaignCategories { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartProduct> CartProducts { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            mb.Entity<Category>().HasKey(x => x.Id);
            mb.Entity<Cart>().HasKey(x => x.Id);
            mb.Entity<Product>().HasKey(x => x.Id);
            mb.Entity<Campaign>().HasKey(x => x.Id);
            mb.Entity<Coupon>().HasKey(x => x.Id);
            mb.Entity<CartProduct>().HasKey(x => new { x.CartId, x.ProductId });
            mb.Entity<CampaignCategory>().HasKey(x => new { x.CampaignId, x.CategoryId });

            mb.Entity<CartProduct>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.ProductId);

            mb.Entity<CartProduct>()
                .HasOne(x => x.Cart)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CartId);

            mb.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId);

            mb.Entity<CampaignCategory>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Campaigns)
                .HasForeignKey(x => x.CategoryId);

            mb.Entity<CampaignCategory>()
                .HasOne(x => x.Campaign)
                .WithMany(x => x.Categories)
                .HasForeignKey(x => x.CampaignId);

            base.OnModelCreating(mb);
        }
    }
}

