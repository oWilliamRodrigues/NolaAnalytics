using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts
{
    public class NolaDbContext : DbContext
    {
        public NolaDbContext(DbContextOptions<NolaDbContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<SubBrand> SubBrands { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Domain.Models.Channel> Channels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OptionGroup> OptionGroups { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Sale> Sales { get; set; }
        public DbSet<ProductSale> ProductSales { get; set; }
        public DbSet<ItemProductSale> ItemProductSales { get; set; }
        public DbSet<ItemItemProductSale> ItemItemProductSales { get; set; }

        public DbSet<DeliverySale> DeliverySales { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponSale> CouponSales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map to existing table names exactly as in SQL schema
            modelBuilder.Entity<Brand>().ToTable("brands");
            modelBuilder.Entity<SubBrand>().ToTable("sub_brands");
            modelBuilder.Entity<Store>().ToTable("stores");
            modelBuilder.Entity<Domain.Models.Channel>().ToTable("channels");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<OptionGroup>().ToTable("option_groups");
            modelBuilder.Entity<Item>().ToTable("items");
            modelBuilder.Entity<Customer>().ToTable("customers");

            modelBuilder.Entity<Sale>().ToTable("sales");
            modelBuilder.Entity<ProductSale>().ToTable("product_sales");
            modelBuilder.Entity<ItemProductSale>().ToTable("item_product_sales");
            modelBuilder.Entity<ItemItemProductSale>().ToTable("item_item_product_sales");

            modelBuilder.Entity<DeliverySale>().ToTable("delivery_sales");
            modelBuilder.Entity<DeliveryAddress>().ToTable("delivery_addresses");

            modelBuilder.Entity<PaymentType>().ToTable("payment_types");
            modelBuilder.Entity<Payment>().ToTable("payments");

            modelBuilder.Entity<Coupon>().ToTable("coupons");
            modelBuilder.Entity<CouponSale>().ToTable("coupon_sales");

            modelBuilder.Entity<Sale>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).HasColumnName("id");
                b.HasMany(x => x.ProductSales).WithOne(ps => ps.Sale).HasForeignKey(ps => ps.SaleId);
                b.HasMany(x => x.Payments).WithOne(p => p.Sale).HasForeignKey(p => p.SaleId);
            });

            modelBuilder.Entity<ProductSale>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Id).HasColumnName("id");
                b.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
                b.HasMany(x => x.ItemProductSales).WithOne(ips => ips.ProductSale).HasForeignKey(ips => ips.ProductSaleId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
