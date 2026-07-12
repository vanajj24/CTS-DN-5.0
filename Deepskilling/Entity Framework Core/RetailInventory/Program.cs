using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using RetailInventory.Data;
using RetailInventory.Models;
using RetailInventory.DTOs;

namespace RetailInventory
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=========================================================");
            Console.WriteLine("     EF Core 8.0 Retail Inventory Management System");
            Console.WriteLine("=========================================================");

            using var context = new AppDbContext();

            // Try to set up the database. If LocalDB isn't running or available, explain gracefully.
            try
            {
                Console.WriteLine("Initializing database (ensuring it is deleted and recreated)...");
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("Database setup successful!\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERROR] Could not connect to SQL Server LocalDB.");
                Console.WriteLine($"Error Details: {ex.Message}");
                Console.WriteLine("Please ensure SQL Server LocalDB is installed and running.");
                Console.WriteLine("You can still view and review the implementation code in the workspace.\n");
                Console.ResetColor();
            }

            bool exit = false;
            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select a Lab to execute (1-15) or 0 to Exit:");
                Console.ResetColor();
                Console.WriteLine("1. What is ORM? (Concept)");
                Console.WriteLine("2. Setting Up DB Context (Concept)");
                Console.WriteLine("3. EF Core CLI & Migrations (Concept)");
                Console.WriteLine("4. Inserting Initial Data (AddAsync & SaveChangesAsync)");
                Console.WriteLine("5. Retrieving Data (Find, FirstOrDefault, ToListAsync)");
                Console.WriteLine("6. Updating and Deleting Records");
                Console.WriteLine("7. Writing Queries with LINQ");
                Console.WriteLine("8. Managing Schema Changes (Concept)");
                Console.WriteLine("9. Seeding Data During Migrations");
                Console.WriteLine("10. Eager, Lazy, and Explicit Loading");
                Console.WriteLine("11. Configuring One-to-One and Many-to-Many Relationships");
                Console.WriteLine("12. Navigating Circular References (DTO Projection)");
                Console.WriteLine("13. Query Caching and Tracking Behavior (AsNoTracking)");
                Console.WriteLine("14. Batch Processing and Bulk Operations (BulkUpdateAsync)");
                Console.WriteLine("15. Handling Concurrency with RowVersion (DbUpdateConcurrencyException)");
                Console.Write("\nEnter choice: ");

                string? choice = Console.ReadLine();
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "0":
                            exit = true;
                            break;
                        case "1":
                            RunLab1();
                            break;
                        case "2":
                            RunLab2();
                            break;
                        case "3":
                            RunLab3();
                            break;
                        case "4":
                            await RunLab4(context);
                            break;
                        case "5":
                            await RunLab5(context);
                            break;
                        case "6":
                            await RunLab6(context);
                            break;
                        case "7":
                            await RunLab7(context);
                            break;
                        case "8":
                            RunLab8();
                            break;
                        case "9":
                            await RunLab9(context);
                            break;
                        case "10":
                            await RunLab10(context);
                            break;
                        case "11":
                            await RunLab11(context);
                            break;
                        case "12":
                            await RunLab12(context);
                            break;
                        case "13":
                            await RunLab13(context);
                            break;
                        case "14":
                            await RunLab14(context);
                            break;
                        case "15":
                            await RunLab15(context);
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Exception while running lab: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    Console.ResetColor();
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress ENTER to return to menu...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        static void RunLab1()
        {
            Console.WriteLine("=== Lab 1: Understanding ORM ===");
            Console.WriteLine("ORM (Object-Relational Mapping) allows C# developers to work with database tables using C# classes.");
            Console.WriteLine("Benefits:");
            Console.WriteLine(" - Productivity: No need to write raw SQL strings inside your C# application code.");
            Console.WriteLine(" - Maintainability: Database structure changes map directly to C# compiler errors rather than runtime SQL exceptions.");
            Console.WriteLine(" - Abstraction: Simplifies query creation by compiling LINQ expressions to SQL tailored for specific database providers.");
        }

        static void RunLab2()
        {
            Console.WriteLine("=== Lab 2: Setting Up the Database Context ===");
            Console.WriteLine("In EF Core, DbContext is the coordinator that links entity classes with database tables.");
            Console.WriteLine("You configure the DbContext subclass by specifying: ");
            Console.WriteLine(" 1. DbSet properties representing tables (e.g. DbSet<Product> Products).");
            Console.WriteLine(" 2. Configuring connection properties and behaviors in OnConfiguring.");
            Console.WriteLine(" 3. Explicit schema relations or data seedings in OnModelCreating.");
        }

        static void RunLab3()
        {
            Console.WriteLine("=== Lab 3: Using EF Core CLI to Create and Apply Migrations ===");
            Console.WriteLine("EF Core CLI helps track modifications made to model structures over time.");
            Console.WriteLine("Commands used:");
            Console.WriteLine(" - dotnet tool install --global dotnet-ef (Installs the CLI tools)");
            Console.WriteLine(" - dotnet ef migrations add InitialCreate (Creates a migrations directory containing state tracking code)");
            Console.WriteLine(" - dotnet ef database update (Applies pending migrations directly to the SQL database)");
        }

        static async Task RunLab4(AppDbContext context)
        {
            Console.WriteLine("=== Lab 4: Inserting Initial Data ===");
            Console.WriteLine("Demonstrating real-time state tracking when calling AddRangeAsync and SaveChangesAsync...\n");

            // Clear database first to isolate the lab
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);
            await context.SaveChangesAsync();

            var electronics = new Category { Name = "Electronics" };
            var groceries = new Category { Name = "Groceries" };

            Console.WriteLine("Adding categories to Context tracking...");
            await context.Categories.AddRangeAsync(electronics, groceries);

            var product1 = new Product { Name = "Laptop", Price = 75000, Category = electronics, StockQuantity = 10 };
            var product2 = new Product { Name = "Rice Bag", Price = 1200, Category = groceries, StockQuantity = 20 };

            Console.WriteLine("Adding products to Context tracking...");
            await context.Products.AddRangeAsync(product1, product2);

            Console.WriteLine("Calling SaveChangesAsync() to persist items in real time:");
            await context.SaveChangesAsync();

            Console.WriteLine("Data inserted successfully!");
        }

        static async Task RunLab5(AppDbContext context)
        {
            Console.WriteLine("=== Lab 5: Retrieving Data ===");
            
            Console.WriteLine("\n1. Retrieving all products using ToListAsync():");
            var products = await context.Products.ToListAsync();
            foreach (var p in products)
            {
                Console.WriteLine($" - ID: {p.Id} | Name: {p.Name} | Price: ₹{p.Price} | Stock: {p.StockQuantity}");
            }

            Console.WriteLine("\n2. Finding a product by primary key using FindAsync(1):");
            var product = await context.Products.FindAsync(1);
            Console.WriteLine($"Found: {product?.Name ?? "Not Found"}");

            Console.WriteLine("\n3. FirstOrDefaultAsync with a price filter (Price > 50000):");
            var expensive = await context.Products.FirstOrDefaultAsync(p => p.Price > 50000);
            Console.WriteLine($"Expensive: {expensive?.Name ?? "None"}");
        }

        static async Task RunLab6(AppDbContext context)
        {
            Console.WriteLine("=== Lab 6: Updating and Deleting Records ===");

            Console.WriteLine("\n1. Updating Laptop price to 70000:");
            var product = await context.Products.FirstOrDefaultAsync(p => p.Name == "Laptop");
            if (product != null)
            {
                product.Price = 70000;
                Console.WriteLine("Price modified in code. Saving changes...");
                await context.SaveChangesAsync();
                Console.WriteLine("Price updated!");
            }
            else
            {
                Console.WriteLine("Laptop not found in database. Run Lab 4 first.");
            }

            Console.WriteLine("\n2. Deleting Rice Bag product:");
            var toDelete = await context.Products.FirstOrDefaultAsync(p => p.Name == "Rice Bag");
            if (toDelete != null)
            {
                context.Products.Remove(toDelete);
                Console.WriteLine("Entity removed from context. Saving changes...");
                await context.SaveChangesAsync();
                Console.WriteLine("Rice Bag deleted!");
            }
            else
            {
                Console.WriteLine("Rice Bag not found in database.");
            }
        }

        static async Task RunLab7(AppDbContext context)
        {
            Console.WriteLine("=== Lab 7: Writing Queries with LINQ ===");

            Console.WriteLine("\n1. Filter and Sort (Price > 1000, ordered by Price descending):");
            var filtered = await context.Products
                .Where(p => p.Price > 1000)
                .OrderByDescending(p => p.Price)
                .ToListAsync();

            foreach (var p in filtered)
            {
                Console.WriteLine($" - {p.Name}: ₹{p.Price}");
            }

            Console.WriteLine("\n2. Projecting columns into an anonymous DTO class:");
            var productDTOs = await context.Products
                .Select(p => new { p.Name, p.Price })
                .ToListAsync();

            foreach (var dto in productDTOs)
            {
                Console.WriteLine($" - DTO Item -> Name: {dto.Name}, Price: ₹{dto.Price}");
            }
        }

        static void RunLab8()
        {
            Console.WriteLine("=== Lab 8: Managing Schema Changes ===");
            Console.WriteLine("To add a column, modify the model class by adding a property (e.g. StockQuantity).");
            Console.WriteLine("Run migrations to update the database schema:");
            Console.WriteLine(" - dotnet ef migrations add AddStockQuantity");
            Console.WriteLine(" - dotnet ef database update");
        }

        static async Task RunLab9(AppDbContext context)
        {
            Console.WriteLine("=== Lab 9: Seeding Data During Migrations ===");
            Console.WriteLine("Data seeding has been defined in AppDbContext.cs via HasData() within OnModelCreating.");
            Console.WriteLine("Let's fetch the seeded records to verify:");
            
            var electronics = await context.Categories.FindAsync(1);
            var groceries = await context.Categories.FindAsync(2);
            Console.WriteLine($"Category 1: {electronics?.Name ?? "Null"}");
            Console.WriteLine($"Category 2: {groceries?.Name ?? "Null"}");

            var smartphone = await context.Products.FindAsync(1);
            var wheatFlour = await context.Products.FindAsync(2);
            Console.WriteLine($"Seeded Product 1: {smartphone?.Name ?? "Null"} (Price: ₹{smartphone?.Price})");
            Console.WriteLine($"Seeded Product 2: {wheatFlour?.Name ?? "Null"} (Price: ₹{wheatFlour?.Price})");
        }

        static async Task RunLab10(AppDbContext context)
        {
            Console.WriteLine("=== Lab 10: Eager, Lazy, and Explicit Loading ===");

            // Detach tracked items to force loading queries
            context.ChangeTracker.Clear();

            Console.WriteLine("\n1. Eager Loading (Using .Include() to load category inside SQL JOIN):");
            var productsEager = await context.Products
                .Include(p => p.Category)
                .ToListAsync();
            foreach (var p in productsEager)
            {
                Console.WriteLine($"Product: {p.Name} | Loaded Category Name: {p.Category.Name}");
            }

            context.ChangeTracker.Clear();

            Console.WriteLine("\n2. Explicit Loading (Querying product first, then explicitly loading relation):");
            var product = await context.Products.FirstAsync();
            Console.WriteLine($"Fetched Product: {product.Name}");
            Console.WriteLine("Loading related category reference explicitly...");
            await context.Entry(product).Reference(p => p.Category).LoadAsync();
            Console.WriteLine($"Loaded Category: {product.Category.Name}");

            context.ChangeTracker.Clear();

            Console.WriteLine("\n3. Lazy Loading (Triggered automatically when accessing the navigation property):");
            // AppDbContext has UseLazyLoadingProxies() enabled, and navigation properties are virtual
            var productLazy = await context.Products.FirstAsync();
            Console.WriteLine($"Fetched Product (Proxied): {productLazy.Name}");
            Console.WriteLine("Accessing productLazy.Category.Name will trigger a lazy database query now...");
            string catName = productLazy.Category.Name; 
            Console.WriteLine($"Loaded Category Name: {catName}");
        }

        static async Task RunLab11(AppDbContext context)
        {
            Console.WriteLine("=== Lab 11: Configuring Relationships ===");

            // 1. One-to-One
            Console.WriteLine("\n1. Setting up One-to-One ProductDetail:");
            var product = await context.Products.FirstAsync();
            
            // Delete existing detail if it exists
            if (product.ProductDetail != null)
            {
                context.ProductDetails.Remove(product.ProductDetail);
                await context.SaveChangesAsync();
            }

            product.ProductDetail = new ProductDetail { WarrantyInfo = "2 Year Comprehensive Warranty" };
            Console.WriteLine("Linking ProductDetail entity. Saving changes...");
            await context.SaveChangesAsync();
            Console.WriteLine($"One-to-One mapping verified: {product.Name} has warranty: '{product.ProductDetail.WarrantyInfo}'");

            // 2. Many-to-Many
            Console.WriteLine("\n2. Setting up Many-to-Many Tags:");
            var tag1 = await context.Tags.FirstOrDefaultAsync(t => t.Name == "On Sale") ?? new Tag { Name = "On Sale" };
            var tag2 = await context.Tags.FirstOrDefaultAsync(t => t.Name == "New Arrival") ?? new Tag { Name = "New Arrival" };

            product.Tags.Clear();
            product.Tags.Add(tag1);
            product.Tags.Add(tag2);

            Console.WriteLine("Saving Many-to-Many relations...");
            await context.SaveChangesAsync();
            Console.WriteLine($"Product '{product.Name}' now has tags: {string.Join(", ", product.Tags.Select(t => t.Name))}");
        }

        static async Task RunLab12(AppDbContext context)
        {
            Console.WriteLine("=== Lab 12: Navigating Circular References ===");
            Console.WriteLine("Returning database-tracked models directly in API serialization leads to Infinite loops because Category contains Products list and Product contains Category.");
            Console.WriteLine("Solution: Project database query models directly into DTOs:");

            var productDTOs = await context.Products
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();

            Console.WriteLine("Projected DTO Output (No circular loops):");
            foreach (var dto in productDTOs)
            {
                Console.WriteLine($" - DTO -> Name: {dto.Name} | Category: {dto.CategoryName}");
            }
        }

        static async Task RunLab13(AppDbContext context)
        {
            Console.WriteLine("=== Lab 13: Query Caching and Tracking Behavior ===");
            Console.WriteLine("For read-only operations, we can bypass the Change Tracker using .AsNoTracking() to save CPU/Memory and speed up performance.");

            Console.WriteLine("\nQuerying products with .AsNoTracking():");
            var products = await context.Products
                .AsNoTracking()
                .ToListAsync();

            foreach (var p in products)
            {
                Console.WriteLine($" - {p.Name}");
            }

            Console.WriteLine($"Total tracking entries in Context Change Tracker: {context.ChangeTracker.Entries().Count()}");
            Console.WriteLine("Because AsNoTracking was used, these entities are not being monitored.");
        }

        static async Task RunLab14(AppDbContext context)
        {
            Console.WriteLine("=== Lab 14: Batch Processing and Bulk Operations ===");
            Console.WriteLine("Regular EF updates compile individual statements. BulkExtensions updates thousands of records in a single query.");

            var productList = await context.Products.ToListAsync();
            foreach (var p in productList)
            {
                p.StockQuantity += 5; // Increment stock
            }

            Console.WriteLine("Performing BulkUpdateAsync directly to SQL database:");
            await context.BulkUpdateAsync(productList);
            Console.WriteLine("Bulk update complete!");

            // Verify
            var updatedProduct = await context.Products.FirstAsync();
            Console.WriteLine($"Verified First Product Stock after Bulk Update: {updatedProduct.StockQuantity}");
        }

        static async Task RunLab15(AppDbContext context)
        {
            Console.WriteLine("=== Lab 15: Handling Concurrency with RowVersion ===");
            Console.WriteLine("Testing optimistic concurrency conflicts...");

            // Fetch a product first
            var productUser1 = await context.Products.FirstAsync();
            var productUser2 = await context.Products.FirstAsync();

            // Simulate User 1 changing and saving the product price
            productUser1.Price = 28000;
            Console.WriteLine($"User 1 updating price to: {productUser1.Price}. Saving changes...");
            await context.SaveChangesAsync();

            // Simulate User 2 (holding stale state) trying to modify the same product's price
            productUser2.Price = 26500;
            Console.WriteLine($"User 2 (with stale RowVersion) updating price to: {productUser2.Price}. Saving changes...");
            
            try
            {
                await context.SaveChangesAsync();
                Console.WriteLine("Update succeeded (Concurrency failure - did not throw exception).");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[CONCURRENCY CONFLICT DETECTED!]");
                Console.WriteLine("EF Core detected that the RowVersion value in database has updated since User 2 read the row.");
                Console.WriteLine($"Details: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
