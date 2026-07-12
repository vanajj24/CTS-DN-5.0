using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetailInventory.Models;

namespace RetailInventory.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<ProductDetail> ProductDetails { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure localdb SQL Server with lazy loading proxies enabled
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RetailInventoryDb;Trusted_Connection=True;TrustServerCertificate=True;")
                    // Outputs the executed SQL commands directly to the console so we can see real-time queries
                    .LogTo(message => {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(message);
                        Console.ResetColor();
                    }, new[] { DbLoggerCategory.Database.Command.Name }, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Lab 11: Configure One-to-One relationship
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductDetail)
                .WithOne(pd => pd.Product)
                .HasForeignKey<ProductDetail>(pd => pd.ProductId);

            // Lab 9: Seeding data during migrations
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Groceries" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Smartphone", Price = 25000, CategoryId = 1, StockQuantity = 50 },
                new Product { Id = 2, Name = "Wheat Flour", Price = 800, CategoryId = 2, StockQuantity = 100 }
            );
        }

        // Custom change tracking logging to show "in real time how EF Core saves"
        public override int SaveChanges()
        {
            LogTrackerStates("SaveChanges START");
            var result = base.SaveChanges();
            LogTrackerStates("SaveChanges COMPLETE");
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            LogTrackerStates("SaveChangesAsync START");
            var result = await base.SaveChangesAsync(cancellationToken);
            LogTrackerStates("SaveChangesAsync COMPLETE");
            return result;
        }

        private void LogTrackerStates(string stage)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n--- [EF CORE REAL-TIME TRACKER] {stage} ---");
            var entries = ChangeTracker.Entries().ToList();
            if (!entries.Any())
            {
                Console.WriteLine("No tracked entities found in context.");
            }
            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name} | State: {entry.State}");
                if (entry.State == EntityState.Modified)
                {
                    Console.WriteLine("  Modified fields detected:");
                    foreach (var prop in entry.Properties.Where(p => p.IsModified))
                    {
                        Console.WriteLine($"    * {prop.Metadata.Name}: '{prop.OriginalValue}' -> '{prop.CurrentValue}'");
                    }
                }
                else if (entry.State == EntityState.Added)
                {
                    Console.WriteLine("  Properties to insert:");
                    foreach (var prop in entry.Properties.Where(p => p.CurrentValue != null))
                    {
                        Console.WriteLine($"    * {prop.Metadata.Name} = '{prop.CurrentValue}'");
                    }
                }
            }
            Console.WriteLine("--------------------------------------------------\n");
            Console.ResetColor();
        }
    }
}
