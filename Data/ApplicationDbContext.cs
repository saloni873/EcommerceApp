using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceApp.Data // !! ENSURE this namespace matches your actual project namespace !!
{
    // -------------------------------------------------------------------
    // Model Classes (Database Tables)
    // FIX: Added '= null!;' to satisfy CS8618 warnings
    // -------------------------------------------------------------------

    /// <summary>
    /// Represents a product in the shop.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; // FIX: Null-forgiving operator
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Represents an item currently in the shopping cart.
    /// </summary>
    public class CartItem
    {
        public int Id { get; set; }

        // Foreign Key to Product
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!; // FIX: Null-forgiving operator (Navigation property)

        public int Quantity { get; set; }
    }

    // -------------------------------------------------------------------
    // DbContext
    // -------------------------------------------------------------------

    /// <summary>
    /// The main Entity Framework Core database context.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define the database tables (DbSets)
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // OPTIONAL: You can explicitly set precision for MySQL if needed, 
            // but the MySQL provider often handles 'decimal' correctly by default.
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);


            // Seed initial product data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 999.99M },
                new Product { Id = 2, Name = "Keyboard", Price = 75.50M },
                new Product { Id = 3, Name = "Mouse", Price = 25.00M },
                new Product { Id = 4, Name = "Monitor", Price = 450.00M }

            );

            // Configure the relationship between CartItem and Product
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);
        }
    }
}