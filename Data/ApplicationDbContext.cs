using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestToken.Models;

namespace TestToken.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WishList> Wishlists { get; set; }
        public DbSet<WishListItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().Property(p=>p.FirstName).IsRequired();
            builder.Entity<ApplicationUser>().Property(p=>p.LastName).IsRequired();
            builder.Entity<ApplicationUser>().Property(p => p.Address).HasMaxLength(200);


            builder.Entity<Product>()
                .HasOne(b => b.Brand)
                .WithMany(p => p.Products)
                .HasForeignKey(b => b.BrandId);

            builder.Entity<Product>()
                .HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(c => c.CategoryId);

            builder.Entity<Cart>()
                .HasMany(t=>t.CartItems) //one Cart has many CartItems, and each CartItem belongs to one Cart
                .WithOne(c=>c.Cart)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Cart>()
                .HasOne(c=>c.Customer)
                .WithMany()
                .HasForeignKey(c=>c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CartItem>()
                .HasOne(p=>p.Product)
                .WithMany()
                .HasForeignKey(p=>p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            

        }
    }
}
