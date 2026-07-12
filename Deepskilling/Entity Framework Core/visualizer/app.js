// EF Core 8.0 Simulation Engine

// 1. Initial Seed Database States
const initialDatabase = {
    categories: [
        { Id: 1, Name: "Electronics" },
        { Id: 2, Name: "Groceries" }
    ],
    products: [
        { Id: 1, Name: "Smartphone", Price: 25000, CategoryId: 1, StockQuantity: 50, RowVersion: "0x00000000000007D1" },
        { Id: 2, Name: "Wheat Flour", Price: 800, CategoryId: 2, StockQuantity: 100, RowVersion: "0x00000000000007D2" }
    ],
    details: [
        { ProductDetailId: 101, ProductId: 1, WarrantyInfo: "1 Year Manufacturer Warranty" }
    ],
    tags: [
        { Id: 1, Name: "On Sale", ProductIds: [1] },
        { Id: 2, Name: "New Arrival", ProductIds: [1, 2] }
    ]
};

// Application State
let activeDb = JSON.parse(JSON.stringify(initialDatabase));
let activeLab = 1;
let currentStepIndex = 0;
let changeTracker = []; // Array of { entityType, entityKey, state, originalData, currentData }
let highlightedLine = -1;
let logHistory = [];

// Lab Definitions with Code, Steps, and Behavior Simulation
const labs = {
    1: {
        category: "Core Architecture",
        title: "Lab 1: Understanding ORM",
        desc: "ORM maps C# classes to database tables. Let's see the conceptual mapping between memory objects and SQL Server tables.",
        code: [
            "// C# Class representing a database row",
            "public class Product {",
            "    public int Id { get; set; }",
            "    public string Name { get; set; }",
            "    public decimal Price { get; set; }",
            "}",
            "",
            "// ORM translates operations dynamically:",
            "var product = new Product { Name = \"Laptop\", Price = 75000 };",
            "await context.Products.AddAsync(product);",
            "await context.SaveChangesAsync();"
        ],
        steps: [
            {
                line: 0,
                desc: "An ORM (like EF Core) maps relational database concepts to Object-Oriented C# concepts.",
                action: () => {
                    logInfo("ORM Maps: Class Product ➜ Table Products");
                    logInfo("ORM Maps: Property Name ➜ Column [Name] NVARCHAR(MAX)");
                    logInfo("ORM Maps: Property Price ➜ Column [Price] DECIMAL(18,2)");
                }
            },
            {
                line: 8,
                desc: "Creating a new C# Product instance. At this stage, it exists only in system memory (Detached state).",
                action: () => {
                    trackObject("Product", "Temp-1", "Detached", { Name: "Laptop", Price: 75000 });
                }
            },
            {
                line: 9,
                desc: "Calling AddAsync() registers the object with the DbContext, transition state to 'Added'.",
                action: () => {
                    updateTrackedObject("Product", "Temp-1", "Added");
                }
            },
            {
                line: 10,
                desc: "Calling SaveChangesAsync() tells EF Core to scan the tracking graph, generate SQL, and save it to the DB.",
                action: () => {
                    logSQL("INSERT", "INSERT INTO [Products] ([Name], [Price], [CategoryId], [StockQuantity]) VALUES ('Laptop', 75000, 1, 0);\nSELECT [Id], [RowVersion] FROM [Products] WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();");
                    const newId = activeDb.products.length + 1;
                    activeDb.products.push({ Id: newId, Name: "Laptop", Price: 75000, CategoryId: 1, StockQuantity: 0, RowVersion: "0x00000000000008AA" });
                    updateTrackedObject("Product", "Temp-1", "Unchanged", newId);
                    renderDbTables();
                    logSuccess("Successfully saved 1 entity. State transitions from Added ➜ Unchanged. Temp ID resolved to Key " + newId);
                }
            }
        ]
    },
    2: {
        category: "Core Architecture",
        title: "Lab 2: Setting Up the DbContext",
        desc: "The DbContext is the heart of Entity Framework Core. It connects C# models to SQL Server using a connection string.",
        code: [
            "public class AppDbContext : DbContext {",
            "    public DbSet<Product> Products { get; set; }",
            "    public DbSet<Category> Categories { get; set; }",
            "",
            "    protected override void OnConfiguring(DbContextOptionsBuilder options) {",
            "        options.UseSqlServer(\"Server=(localdb)\\mssqllocaldb;Database=RetailInventoryDb;\");",
            "    }",
            "}",
            "using var context = new AppDbContext();"
        ],
        steps: [
            {
                line: 0,
                desc: "Creating the custom DbContext class. DbSet properties specify which database tables EF Core should manage.",
                action: () => {
                    logInfo("AppDbContext registered mapping definitions.");
                }
            },
            {
                line: 5,
                desc: "OnConfiguring is invoked when initialized. It points the application to the Microsoft SQL Server LocalDB database.",
                action: () => {
                    logInfo("DB Provider configured: Microsoft.EntityFrameworkCore.SqlServer");
                }
            },
            {
                line: 8,
                desc: "Initializing context using 'using var' pattern. It opens a database connection scope.",
                action: () => {
                    logSuccess("AppDbContext instance created and database connection successfully initialized.");
                }
            }
        ]
    },
    3: {
        category: "Core Architecture",
        title: "Lab 3: Migrations & Schema Setup",
        desc: "Create and apply migrations using CLI tools to build your database schema from C# models.",
        code: [
            "# Install Global EF tools command",
            "dotnet tool install --global dotnet-ef",
            "",
            "# Scaffold C# models into a migration file",
            "dotnet ef migrations add InitialCreate",
            "",
            "# Execute script to build tables in SQL Server",
            "dotnet ef database update"
        ],
        steps: [
            {
                line: 1,
                desc: "Installs the command-line tool to compile migrations.",
                action: () => logInfo("dotnet-ef command line tool ready.")
            },
            {
                line: 4,
                desc: "Scans C# model classes to create code files in the 'Migrations' folder.",
                action: () => {
                    logInfo("Analyzing Category and Product entities...");
                    logSuccess("Created Migration file: [timestamp]_InitialCreate.cs");
                }
            },
            {
                line: 7,
                desc: "Executes sql scripts to build the Categories and Products tables in SQL Server.",
                action: () => {
                    logSQL("INFO", "CREATE TABLE [Categories] ([Id] INT IDENTITY, [Name] NVARCHAR(MAX));\nCREATE TABLE [Products] ([Id] INT IDENTITY, [Name] NVARCHAR(MAX), [Price] DECIMAL, [CategoryId] INT);");
                    logSuccess("SQL Server database schemas updated successfully! Tables 'Products' and 'Categories' created.");
                }
            }
        ]
    },
    4: {
        category: "Data Persistence",
        title: "Lab 4: Inserting Initial Data",
        desc: "Using AddRangeAsync and SaveChangesAsync to write multiple records into the database in real time.",
        code: [
            "using var context = new AppDbContext();",
            "var electronics = new Category { Name = \"Electronics\" };",
            "var groceries = new Category { Name = \"Groceries\" };",
            "await context.Categories.AddRangeAsync(electronics, groceries);",
            "",
            "var product1 = new Product { Name = \"Laptop\", Price = 75000, Category = electronics };",
            "var product2 = new Product { Name = \"Rice Bag\", Price = 1200, Category = groceries };",
            "await context.Products.AddRangeAsync(product1, product2);",
            "await context.SaveChangesAsync();"
        ],
        steps: [
            {
                line: 0,
                desc: "Starting the database transaction context.",
                action: () => {
                    activeDb.products = [];
                    activeDb.categories = [];
                    renderDbTables();
                    logInfo("Context tracking graph is clear.");
                }
            },
            {
                line: 1,
                desc: "Creating Category 'Electronics' in C#.",
                action: () => trackObject("Category", "Temp-C1", "Detached", { Name: "Electronics" })
            },
            {
                line: 2,
                desc: "Creating Category 'Groceries' in C#.",
                action: () => trackObject("Category", "Temp-C2", "Detached", { Name: "Groceries" })
            },
            {
                line: 3,
                desc: "Registering categories into EF Change Tracker. Both transitions to 'Added'.",
                action: () => {
                    updateTrackedObject("Category", "Temp-C1", "Added");
                    updateTrackedObject("Category", "Temp-C2", "Added");
                }
            },
            {
                line: 5,
                desc: "Creating Product 'Laptop' in C# (linked to electronics).",
                action: () => trackObject("Product", "Temp-P1", "Detached", { Name: "Laptop", Price: 75000, Category: "Electronics" })
            },
            {
                line: 6,
                desc: "Creating Product 'Rice Bag' in C# (linked to groceries).",
                action: () => trackObject("Product", "Temp-P2", "Detached", { Name: "Rice Bag", Price: 1200, Category: "Groceries" })
            },
            {
                line: 7,
                desc: "Registering products into EF Change Tracker. Both transitions to 'Added'.",
                action: () => {
                    updateTrackedObject("Product", "Temp-P1", "Added");
                    updateTrackedObject("Product", "Temp-P2", "Added");
                }
            },
            {
                line: 8,
                desc: "Calling SaveChangesAsync() inserts records into Categories first, retrieves generated IDs, then inserts Products using those IDs.",
                action: () => {
                    logSQL("INSERT", "INSERT INTO [Categories] ([Name]) VALUES ('Electronics'), ('Groceries');");
                    activeDb.categories.push({ Id: 1, Name: "Electronics" });
                    activeDb.categories.push({ Id: 2, Name: "Groceries" });

                    logSQL("INSERT", "INSERT INTO [Products] ([Name], [Price], [CategoryId]) VALUES ('Laptop', 75000, 1), ('Rice Bag', 1200, 2);");
                    activeDb.products.push({ Id: 1, Name: "Laptop", Price: 75000, CategoryId: 1, StockQuantity: 0, RowVersion: "0x00000000000009A1" });
                    activeDb.products.push({ Id: 2, Name: "Rice Bag", Price: 1200, CategoryId: 2, StockQuantity: 0, RowVersion: "0x00000000000009A2" });

                    updateTrackedObject("Category", "Temp-C1", "Unchanged", 1);
                    updateTrackedObject("Category", "Temp-C2", "Unchanged", 2);
                    updateTrackedObject("Product", "Temp-P1", "Unchanged", 1);
                    updateTrackedObject("Product", "Temp-P2", "Unchanged", 2);
                    
                    renderDbTables();
                    highlightTableRow("products", 1);
                    highlightTableRow("products", 2);
                    logSuccess("SaveChangesAsync processed successfully. All 4 entities updated to 'Unchanged' state.");
                }
            }
        ]
    },
    5: {
        category: "Data Persistence",
        title: "Lab 5: Retrieving Data",
        desc: "Different ways of loading data: ToListAsync retrieves a collection, FindAsync checks memory state before hitting SQL, FirstOrDefault matches conditions.",
        code: [
            "// 1. Retrieve all items",
            "var products = await context.Products.ToListAsync();",
            "",
            "// 2. Fetch specific row by Key (id: 1)",
            "var product = await context.Products.FindAsync(1);",
            "",
            "// 3. Conditional match query",
            "var expensive = await context.Products.FirstOrDefaultAsync(p => p.Price > 50000);"
        ],
        steps: [
            {
                line: 1,
                desc: "Retrieves all products. EF Core issues a SELECT command, pulls rows, builds C# entities, and tracks them as 'Unchanged'.",
                action: () => {
                    logSQL("SELECT", "SELECT p.[Id], p.[Name], p.[Price], p.[CategoryId], p.[StockQuantity] FROM [Products] AS p");
                    changeTracker = [];
                    activeDb.products.forEach(p => {
                        trackObject("Product", p.Id, "Unchanged", { Name: p.Name, Price: p.Price });
                    });
                    logInfo(`Loaded ${activeDb.products.length} products into Change Tracker.`);
                }
            },
            {
                line: 4,
                desc: "FindAsync(1) looks inside the DbContext tracker first. Since Product ID 1 is already tracked, it returns it instantly without database SQL query!",
                action: () => {
                    logInfo("FindAsync(1): Entry found in Change Tracker cache. Skipping SQL Command execution!");
                }
            },
            {
                line: 7,
                desc: "FirstOrDefaultAsync evaluates condition. Translates to SELECT TOP(1) with a WHERE filter in SQL Server.",
                action: () => {
                    logSQL("SELECT", "SELECT TOP(1) p.[Id], p.[Name], p.[Price] FROM [Products] AS p WHERE p.[Price] > 50000");
                    logSuccess("Found Product: Smartphone (Price: ₹25000)");
                }
            }
        ]
    },
    6: {
        category: "Data Persistence",
        title: "Lab 6: Updating & Deleting",
        desc: "EF Core tracks changes on class fields. Setting properties flags the state as Modified. SaveChanges compiles the update.",
        code: [
            "// 1. Update Laptop Price",
            "var product = await context.Products.FirstOrDefaultAsync(p => p.Name == \"Laptop\");",
            "if (product != null) {",
            "    product.Price = 70000;",
            "    await context.SaveChangesAsync();",
            "}",
            "",
            "// 2. Delete Rice Bag",
            "var toDelete = await context.Products.FirstOrDefaultAsync(p => p.Name == \"Rice Bag\");",
            "if (toDelete != null) {",
            "    context.Products.Remove(toDelete);",
            "    await context.SaveChangesAsync();",
            "}"
        ],
        steps: [
            {
                line: 1,
                desc: "Querying Laptop. Entity is loaded into the tracker as 'Unchanged'.",
                action: () => {
                    logSQL("SELECT", "SELECT TOP(1) p.[Id], p.[Name], p.[Price] FROM [Products] AS p WHERE p.[Name] = 'Laptop'");
                    trackObject("Product", 1, "Unchanged", { Name: "Laptop", Price: 75000 });
                }
            },
            {
                line: 3,
                desc: "Modifying the Price in C# code. The Change Tracker intercepts this property set, flagging it as 'Modified'.",
                action: () => {
                    const item = changeTracker.find(t => t.entityType === "Product" && t.entityKey === 1);
                    if (item) {
                        item.state = "Modified";
                        item.currentData.Price = 70000;
                        renderTracker();
                    }
                    logInfo("Product ID 1 state changed to 'Modified'. Modified field: Price (75000 ➜ 70000)");
                }
            },
            {
                line: 4,
                desc: "Saving changes compiles UPDATE statement, executing it in database.",
                action: () => {
                    logSQL("UPDATE", "UPDATE [Products] SET [Price] = 70000 WHERE [Id] = 1;\nSELECT [RowVersion] FROM [Products] WHERE @@ROWCOUNT = 1 AND [Id] = 1;");
                    const dbProd = activeDb.products.find(p => p.Id === 1);
                    if (dbProd) {
                        dbProd.Price = 70000;
                        dbProd.RowVersion = "0x0000000000000B2B";
                    }
                    const item = changeTracker.find(t => t.entityType === "Product" && t.entityKey === 1);
                    if (item) item.state = "Unchanged";
                    renderDbTables();
                    highlightTableRow("products", 1);
                    logSuccess("Update executed. Laptop price changed to ₹70000.");
                }
            },
            {
                line: 8,
                desc: "Querying Rice Bag product to delete. Tracked as 'Unchanged'.",
                action: () => {
                    logSQL("SELECT", "SELECT TOP(1) p.[Id], p.[Name] FROM [Products] AS p WHERE p.[Name] = 'Rice Bag'");
                    trackObject("Product", 2, "Unchanged", { Name: "Rice Bag", Price: 1200 });
                }
            },
            {
                line: 10,
                desc: "Calling Remove() updates the entity state to 'Deleted' in the Change Tracker graph.",
                action: () => {
                    const item = changeTracker.find(t => t.entityType === "Product" && t.entityKey === 2);
                    if (item) {
                        item.state = "Deleted";
                        renderTracker();
                    }
                    logInfo("Product ID 2 marked as 'Deleted'. It is still in database until SaveChanges is executed.");
                }
            },
            {
                line: 11,
                desc: "Saving changes compiles DELETE SQL query and detaches entity.",
                action: () => {
                    logSQL("DELETE", "DELETE FROM [Products] WHERE [Id] = 2;");
                    activeDb.products = activeDb.products.filter(p => p.Id !== 2);
                    changeTracker = changeTracker.filter(t => !(t.entityType === "Product" && t.entityKey === 2));
                    renderDbTables();
                    renderTracker();
                    logSuccess("Deleted Product ID 2. Entity detached from Context.");
                }
            }
        ]
    },
    7: {
        category: "Data Persistence",
        title: "Lab 7: LINQ Queries & DTOs",
        desc: "Use LINQ to write readable filters and projections. Projecting with Select returns specific columns rather than whole objects.",
        code: [
            "// 1. Where filter & sort",
            "var filtered = await context.Products",
            "    .Where(p => p.Price > 1000)",
            "    .OrderByDescending(p => p.Price)",
            "    .ToListAsync();",
            "",
            "// 2. Select column projection DTO",
            "var productDTOs = await context.Products",
            "    .Select(p => new { p.Name, p.Price })",
            "    .ToListAsync();"
        ],
        steps: [
            {
                line: 1,
                desc: "Executing query. EF Core compiles the Where and OrderBy filters directly into a SQL statement containing WHERE and ORDER BY DESC clauses.",
                action: () => {
                    logSQL("SELECT", "SELECT p.[Id], p.[Name], p.[Price] FROM [Products] AS p WHERE p.[Price] > 1000 ORDER BY p.[Price] DESC");
                    logInfo("Retrieved and tracked products costing > ₹1000 sorted by price.");
                }
            },
            {
                line: 7,
                desc: "Running anonymous projection. The SQL SELECT clause now requests ONLY [Name] and [Price] columns, optimizing network transmission size.",
                action: () => {
                    logSQL("SELECT", "SELECT p.[Name], p.[Price] FROM [Products] AS p");
                    logSuccess("Anonymous DTOs projected successfully. Entities are read-only and NOT tracked.");
                }
            }
        ]
    },
    8: {
        category: "Relations & Schemas",
        title: "Lab 8: Managing Schema Changes",
        desc: "Adding a new column like StockQuantity. Generate migrations to build updates in SQL server tables.",
        code: [
            "// 1. Update C# Model first:",
            "public class Product {",
            "    public int StockQuantity { get; set; }",
            "}",
            "",
            "// 2. CLI migrations & execution commands:",
            "dotnet ef migrations add AddStockQuantity",
            "dotnet ef database update"
        ],
        steps: [
            {
                line: 1,
                desc: "We write property 'StockQuantity' in C# Product class.",
                action: () => logInfo("Property added in class definition.")
            },
            {
                line: 6,
                desc: "Scaffolds a migration describing column modification.",
                action: () => {
                    logSuccess("Created Migration: [timestamp]_AddStockQuantity.cs");
                }
            },
            {
                line: 7,
                desc: "Alters the database table Products in SQL Server directly.",
                action: () => {
                    logSQL("UPDATE", "ALTER TABLE [Products] ADD [StockQuantity] INT DEFAULT 0 NOT NULL;");
                    logSuccess("Successfully updated schema: Column [StockQuantity] added to Products table!");
                }
            }
        ]
    },
    9: {
        category: "Relations & Schemas",
        title: "Lab 9: Seeding Data",
        desc: "Use HasData() in OnModelCreating to pre-populate database lookup values during migrations.",
        code: [
            "protected override void OnModelCreating(ModelBuilder modelBuilder) {",
            "    modelBuilder.Entity<Category>().HasData(",
            "        new Category { Id = 1, Name = \"Electronics\" },",
            "        new Category { Id = 2, Name = \"Groceries\" }",
            "    );",
            "}"
        ],
        steps: [
            {
                line: 0,
                desc: "When applying migrations, EF Core checks if seeding data matches existing table rows.",
                action: () => logInfo("ModelBuilder checking HasData lookup collections...")
            },
            {
                line: 1,
                desc: "If seeded keys do not exist in the database, EF Core generates INSERT commands during database update.",
                action: () => {
                    logSQL("INSERT", "IF NOT EXISTS (SELECT * FROM [Categories] WHERE [Id] = 1)\nINSERT INTO [Categories] ([Id], [Name]) VALUES (1, 'Electronics');\nIF NOT EXISTS (SELECT * FROM [Categories] WHERE [Id] = 2)\nINSERT INTO [Categories] ([Id], [Name]) VALUES (2, 'Groceries');");
                    logSuccess("Categories table initialized with seed data.");
                }
            }
        ]
    },
    10: {
        category: "Relations & Schemas",
        title: "Lab 10: Loading Strategies",
        desc: "Compare Eager Loading (SQL Join), Explicit Loading (Separate load command), and Lazy Loading (Triggered automatically on access).",
        code: [
            "// 1. Eager Loading (Using Include - Single JOIN query)",
            "var products = await context.Products.Include(p => p.Category).ToListAsync();",
            "",
            "// 2. Explicit Loading (Querying product first, then loading category)",
            "var product = await context.Products.FirstAsync();",
            "await context.Entry(product).Reference(p => p.Category).LoadAsync();",
            "",
            "// 3. Lazy Loading (Triggered automatically on access)",
            "var lazyProduct = await context.Products.FirstAsync();",
            "string catName = lazyProduct.Category.Name; // Queries DB here"
        ],
        steps: [
            {
                line: 1,
                desc: "Eager Loading writes a single SQL JOIN query, loading products and categories at the same time.",
                action: () => {
                    logSQL("SELECT", "SELECT p.[Id], p.[Name], p.[Price], c.[Id], c.[Name] \nFROM [Products] AS p \nINNER JOIN [Categories] AS c ON p.[CategoryId] = c.[Id]");
                    logSuccess("Loaded 2 products along with their Category object structures.");
                }
            },
            {
                line: 4,
                desc: "First, queries the product alone.",
                action: () => {
                    logSQL("SELECT", "SELECT TOP(1) p.[Id], p.[Name], p.[CategoryId] FROM [Products] AS p");
                    trackObject("Product", 1, "Unchanged", { Name: "Smartphone", Price: 25000 });
                }
            },
            {
                line: 5,
                desc: "Explicitly loads category. Issues a second SELECT command matching Product's CategoryId.",
                action: () => {
                    logSQL("SELECT", "SELECT c.[Id], c.[Name] FROM [Categories] AS c WHERE c.[Id] = 1");
                    logSuccess("Category 'Electronics' explicitly loaded into Product Smartphone.");
                }
            },
            {
                line: 8,
                desc: "Queries the product alone using lazy proxy.",
                action: () => {
                    changeTracker = [];
                    logSQL("SELECT", "SELECT TOP(1) p.[Id], p.[Name], p.[CategoryId] FROM [Products] AS p");
                    trackObject("ProductProxy", 1, "Unchanged", { Name: "Smartphone" });
                }
            },
            {
                line: 9,
                desc: "Accessing navigation property 'Category' intercepts the call and runs SQL query automatically in background!",
                action: () => {
                    logInfo("Lazy Loading Proxy triggered! Intercepting property read...");
                    logSQL("SELECT", "SELECT c.[Id], c.[Name] FROM [Categories] AS c WHERE c.[Id] = 1");
                    logSuccess("Category 'Electronics' loaded on-demand.");
                }
            }
        ]
    },
    11: {
        category: "Relations & Schemas",
        title: "Lab 11: 1:1 and M:N Relationships",
        desc: "Configure One-to-One and Many-to-Many configurations using ModelBuilder and navigations.",
        code: [
            "// 1. One-to-One ProductDetail assignment",
            "product.ProductDetail = new ProductDetail { WarrantyInfo = \"2 Years\" };",
            "await context.SaveChangesAsync();",
            "",
            "// 2. Many-to-Many Tags adding",
            "product.Tags.Add(new Tag { Name = \"On Sale\" });",
            "await context.SaveChangesAsync();"
        ],
        steps: [
            {
                line: 1,
                desc: "Creating ProductDetail instance and linking to a product in memory.",
                action: () => {
                    trackObject("ProductDetail", "Temp-D1", "Added", { WarrantyInfo: "2 Years", ProductId: 1 });
                    logInfo("ProductDetail marked as 'Added' and linked to Product Key 1.");
                }
            },
            {
                line: 2,
                desc: "Saving compiles INSERT statement with ProductId foreign key.",
                action: () => {
                    logSQL("INSERT", "INSERT INTO [ProductDetails] ([WarrantyInfo], [ProductId]) VALUES ('2 Years', 1);\nSELECT [ProductDetailId] FROM [ProductDetails] WHERE @@ROWCOUNT = 1 AND [ProductDetailId] = scope_identity();");
                    activeDb.details.push({ ProductDetailId: 102, ProductId: 1, WarrantyInfo: "2 Years" });
                    updateTrackedObject("ProductDetail", "Temp-D1", "Unchanged", 102);
                    renderDbTables();
                    highlightTableRow("details", 102);
                    logSuccess("One-to-One details added successfully.");
                }
            },
            {
                line: 5,
                desc: "Assigning Tag to a Product. In EF Core 8, many-to-many intermediate join tables are updated automatically.",
                action: () => {
                    trackObject("Tag", "Temp-T1", "Added", { Name: "On Sale" });
                    logInfo("Tag added to tracked collection.");
                }
            },
            {
                line: 6,
                desc: "Saving compiles INSERT command into Tag master table AND the hidden Join table.",
                action: () => {
                    logSQL("INSERT", "INSERT INTO [Tags] ([Name]) VALUES ('On Sale');\nINSERT INTO [ProductTag] ([ProductsId], [TagsId]) VALUES (1, 3);");
                    activeDb.tags.push({ Id: 3, Name: "On Sale", ProductIds: [1] });
                    updateTrackedObject("Tag", "Temp-T1", "Unchanged", 3);
                    renderDbTables();
                    highlightTableRow("tags", 3);
                    logSuccess("Many-to-Many relation persisted successfully!");
                }
            }
        ]
    },
    12: {
        category: "Advanced Topics",
        title: "Lab 12: Circular References",
        desc: "Returning tracked entity models in APIs results in endless loop errors. Use DTO projections to solve this.",
        code: [
            "// Problem: Category references Products, and Product references Category.",
            "// Solution: Project query using Select into a flat DTO structure:",
            "var productDTOs = await context.Products",
            "    .Select(p => new ProductDTO {",
            "        Name = p.Name,",
            "        CategoryName = p.Category.Name",
            "    }).ToListAsync();"
        ],
        steps: [
            {
                line: 0,
                desc: "Serialization converts objects to JSON. Product contains Category, which contains List<Product>, repeating forever.",
                action: () => logInfo("Circular loops detected during default JSON Serialization.")
            },
            {
                line: 2,
                desc: "Creating projection query. EF Core generates a JOIN query extracting only the selected properties.",
                action: () => {
                    logSQL("SELECT", "SELECT p.[Name], c.[Name] AS [CategoryName]\nFROM [Products] AS p\nINNER JOIN [Categories] AS c ON p.[CategoryId] = c.[Id]");
                }
            },
            {
                line: 6,
                desc: "Result is a flat list of DTO objects, containing no circular properties, ready for API responses.",
                action: () => {
                    logSuccess("Flat DTO List:\n[ { Name: 'Smartphone', CategoryName: 'Electronics' } ]");
                }
            }
        ]
    },
    13: {
        category: "Advanced Topics",
        title: "Lab 13: Tracking & Caching Behavior",
        desc: "Bypass tracker cache using .AsNoTracking() to save processing time and memory for read-only dashboards.",
        code: [
            "// Querying read-only list with AsNoTracking()",
            "var products = await context.Products",
            "    .AsNoTracking()",
            "    .ToListAsync();"
        ],
        steps: [
            {
                line: 1,
                desc: "Executing query. The tracker will not allocate tracking blocks or scan fields when read.",
                action: () => {
                    changeTracker = [];
                    logSQL("SELECT", "SELECT p.[Id], p.[Name], p.[Price] FROM [Products] AS p");
                    logInfo("Loaded entities directly into memory. Tracker is empty!");
                    renderTracker();
                }
            },
            {
                line: 3,
                desc: "This speeds up select execution time by avoiding allocation costs.",
                action: () => {
                    logSuccess("Retrieved entities. Total items tracked in context: 0");
                }
            }
        ]
    },
    14: {
        category: "Advanced Topics",
        title: "Lab 14: Batch Processing & Bulk Updates",
        desc: "BulkExtensions executes updates in a single raw query, bypassing the slow step of loading entities into memory first.",
        code: [
            "// Increments stock by 5 inside a list",
            "foreach(var p in productList) { p.StockQuantity += 5; }",
            "",
            "// Execute raw batch update in SQL Server",
            "await context.BulkUpdateAsync(productList);"
        ],
        steps: [
            {
                line: 1,
                desc: "Updating stock values inside standard list objects in code.",
                action: () => {
                    logInfo("Updated stock property locally.");
                }
            },
            {
                line: 4,
                desc: "BulkUpdateAsync creates temporary tables or compiled bulk scripts to write data directly in a single SQL operation.",
                action: () => {
                    logSQL("UPDATE", "MERGE [Products] USING #TempProductsTable ON [Products].[Id] = #TempProductsTable.[Id] WHEN MATCHED THEN UPDATE SET [StockQuantity] = #TempProductsTable.[StockQuantity];");
                    activeDb.products.forEach(p => p.StockQuantity += 5);
                    renderDbTables();
                    logSuccess("BulkUpdate finished! All rows updated directly on Server.");
                }
            }
        ]
    },
    15: {
        category: "Advanced Topics",
        title: "Lab 15: Concurrency conflicts",
        desc: "Simulating two users trying to edit the same row simultaneously. RowVersion triggers concurrency exceptions.",
        code: [
            "// User 1 reads row, User 2 reads same row",
            "var user1Product = await context.Products.FindAsync(1);",
            "var user2Product = await context.Products.FindAsync(1);",
            "",
            "// User 1 edits and saves first",
            "user1Product.Price = 28000; await context.SaveChangesAsync();",
            "",
            "// User 2 tries to save stale row details later",
            "user2Product.Price = 26500; await context.SaveChangesAsync();"
        ],
        steps: [
            {
                line: 1,
                desc: "User 1 fetches Smartphone. Current RowVersion is 0x07D1.",
                action: () => {
                    trackObject("Product (User 1)", 1, "Unchanged", { Name: "Smartphone", Price: 25000, RowVersion: "0x00000000000007D1" });
                    logSQL("SELECT", "SELECT TOP(1) [Id], [Price], [RowVersion] FROM [Products] WHERE [Id] = 1");
                }
            },
            {
                line: 2,
                desc: "User 2 fetches same Smartphone. Stale state reads same RowVersion (0x07D1).",
                action: () => {
                    trackObject("Product (User 2)", 1, "Unchanged", { Name: "Smartphone", Price: 25000, RowVersion: "0x00000000000007D1" });
                }
            },
            {
                line: 5,
                desc: "User 1 updates price and saves. Database writes modified price and updates RowVersion to 0x07E9.",
                action: () => {
                    updateTrackedObject("Product (User 1)", 1, "Modified");
                    logSQL("UPDATE", "UPDATE [Products] SET [Price] = 28000 WHERE [Id] = 1 AND [RowVersion] = 0x00000000000007D1;\nSELECT [RowVersion] FROM [Products] WHERE @@ROWCOUNT = 1 AND [Id] = 1;");
                    
                    const dbProd = activeDb.products.find(p => p.Id === 1);
                    if (dbProd) {
                        dbProd.Price = 28000;
                        dbProd.RowVersion = "0x00000000000007E9";
                    }
                    updateTrackedObject("Product (User 1)", 1, "Unchanged");
                    renderDbTables();
                    highlightTableRow("products", 1);
                    logSuccess("User 1 save succeeded! Database RowVersion updated to 0x07E9.");
                }
            },
            {
                line: 8,
                desc: "User 2 tries to save their edit. The UPDATE query fails because RowVersion is checked in WHERE clause, and it no longer matches 0x07D1!",
                action: () => {
                    updateTrackedObject("Product (User 2)", 1, "Modified");
                    logSQL("UPDATE", "UPDATE [Products] SET [Price] = 26500 WHERE [Id] = 1 AND [RowVersion] = 0x00000000000007D1;");
                    
                    // Simulation fails (0 rows affected)
                    ConsoleError("DbUpdateConcurrencyException: Database operation expected to affect 1 row(s) but affected 0 row(s). Concurrency conflict detected!");
                    logSuccess("Failed: Entity state remained modified. Conflict caught and handled.");
                }
            }
        ]
    }
};

// Logger helpers
function logInfo(msg) {
    const consoleLogs = document.getElementById("sql-console");
    const line = document.createElement("div");
    line.className = "sql-log-line";
    line.innerHTML = `<span class="sql-time-stamp">[${new Date().toLocaleTimeString()}]</span><span class="sql-query-type info">INFO:</span> ${msg}`;
    consoleLogs.appendChild(line);
    consoleLogs.scrollTop = consoleLogs.scrollHeight;
}

function logSQL(queryType, queryText) {
    const consoleLogs = document.getElementById("sql-console");
    const line = document.createElement("div");
    line.className = "sql-log-line";
    line.innerHTML = `<span class="sql-time-stamp">[${new Date().toLocaleTimeString()}]</span><span class="sql-query-type select">${queryType}:</span> <span class="sql-query-text">${queryText.replace(/\n/g, '<br>')}</span>`;
    consoleLogs.appendChild(line);
    consoleLogs.scrollTop = consoleLogs.scrollHeight;
}

function logSuccess(msg) {
    const consoleLogs = document.getElementById("sql-console");
    const line = document.createElement("div");
    line.className = "sql-log-line";
    line.innerHTML = `<span class="sql-time-stamp">[${new Date().toLocaleTimeString()}]</span><span class="sql-query-type insert">SUCCESS:</span> <span style="color:#56d364">${msg}</span>`;
    consoleLogs.appendChild(line);
    consoleLogs.scrollTop = consoleLogs.scrollHeight;
}

function ConsoleError(msg) {
    const consoleLogs = document.getElementById("sql-console");
    const line = document.createElement("div");
    line.className = "sql-log-line";
    line.innerHTML = `<span class="sql-time-stamp">[${new Date().toLocaleTimeString()}]</span><span class="sql-query-type delete">EXCEPTION:</span> <span style="color:#ff7b72; font-weight:700;">${msg}</span>`;
    consoleLogs.appendChild(line);
    consoleLogs.scrollTop = consoleLogs.scrollHeight;
}

// Change tracker helpers
function trackObject(type, key, state, data) {
    changeTracker.push({
        entityType: type,
        entityKey: key,
        state: state,
        originalData: JSON.parse(JSON.stringify(data)),
        currentData: JSON.parse(JSON.stringify(data))
    });
    renderTracker();
}

function updateTrackedObject(type, oldKey, newState, newKey = null) {
    const item = changeTracker.find(t => t.entityType === type && t.entityKey === oldKey);
    if (item) {
        item.state = newState;
        if (newKey !== null) {
            item.entityKey = newKey;
        }
    }
    renderTracker();
}

// Rendering Tracker Panel
function renderTracker() {
    const trackerContainer = document.getElementById("tracker-list");
    trackerContainer.innerHTML = "";
    
    document.getElementById("active-tracked-entities").innerText = `${changeTracker.length} Tracked`;

    if (changeTracker.length === 0) {
        trackerContainer.innerHTML = `
            <div class="empty-state">
                <i class="fa-solid fa-magnifying-glass"></i>
                <p>Change tracker is currently empty. Run code to track objects.</p>
            </div>`;
        return;
    }

    changeTracker.forEach(entry => {
        const card = document.createElement("div");
        card.className = `entity-card state-${entry.state.toLowerCase()}`;
        
        let propertiesHtml = "";
        for (let prop in entry.currentData) {
            if (prop === "Category" || prop === "Products") continue;
            const originalVal = entry.originalData[prop];
            const currentVal = entry.currentData[prop];
            const isChanged = originalVal !== currentVal;
            
            propertiesHtml += `
                <div class="prop-row">
                    <span class="prop-name">${prop}:</span>
                    <span class="prop-value ${isChanged ? 'changed' : ''}">
                        ${currentVal} ${isChanged ? `(was ${originalVal})` : ''}
                    </span>
                </div>`;
        }

        card.innerHTML = `
            <div class="entity-card-header">
                <div class="entity-title-box">
                    <span class="entity-type">${entry.entityType}</span>
                    <span class="entity-key">[Key: ${entry.entityKey}]</span>
                </div>
                <span class="entity-state-badge">${entry.state}</span>
            </div>
            <div class="entity-card-body">
                ${propertiesHtml}
            </div>`;
        
        trackerContainer.appendChild(card);
    });
}

// Rendering Live DB tables
function renderDbTables() {
    // Products Table
    const productsBody = document.getElementById("db-products-rows");
    productsBody.innerHTML = "";
    activeDb.products.forEach(p => {
        const row = document.createElement("tr");
        row.id = `row-products-${p.Id}`;
        row.innerHTML = `
            <td>${p.Id}</td>
            <td>${p.Name}</td>
            <td>₹${p.Price}</td>
            <td>${p.CategoryId}</td>
            <td>${p.StockQuantity}</td>
            <td><span style="font-size:10px; color:#8b949e;">${p.RowVersion}</span></td>`;
        productsBody.appendChild(row);
    });

    // Categories Table
    const categoriesBody = document.getElementById("db-categories-rows");
    categoriesBody.innerHTML = "";
    activeDb.categories.forEach(c => {
        const row = document.createElement("tr");
        row.id = `row-categories-${c.Id}`;
        row.innerHTML = `<td>${c.Id}</td><td>${c.Name}</td>`;
        categoriesBody.appendChild(row);
    });

    // Details Table
    const detailsBody = document.getElementById("db-details-rows");
    detailsBody.innerHTML = "";
    activeDb.details.forEach(d => {
        const row = document.createElement("tr");
        row.id = `row-details-${d.ProductDetailId}`;
        row.innerHTML = `<td>${d.ProductDetailId}</td><td>${d.ProductId}</td><td>${d.WarrantyInfo}</td>`;
        detailsBody.appendChild(row);
    });

    // Tags Table
    const tagsBody = document.getElementById("db-tags-rows");
    tagsBody.innerHTML = "";
    activeDb.tags.forEach(t => {
        const row = document.createElement("tr");
        row.id = `row-tags-${t.Id}`;
        row.innerHTML = `<td>${t.Id}</td><td>${t.Name}</td><td>[${t.ProductIds.join(", ")}]</td>`;
        tagsBody.appendChild(row);
    });
}

function highlightTableRow(tableName, rowId) {
    const row = document.getElementById(`row-${tableName}-${rowId}`);
    if (row) {
        row.classList.add("highlight-row");
        setTimeout(() => row.classList.remove("highlight-row"), 1500);
    }
}

// C# Code displays with highlighters
function loadLabCode(labId) {
    const lab = labs[labId];
    document.getElementById("lab-title").innerText = lab.title;
    document.getElementById("current-category").innerText = lab.category;
    document.getElementById("lab-explanation").innerText = lab.desc;

    const codeDisplay = document.getElementById("code-display");
    codeDisplay.innerHTML = "";

    lab.code.forEach((line, index) => {
        const lineSpan = document.createElement("span");
        lineSpan.className = "code-line-span";
        lineSpan.id = `line-${index}`;
        lineSpan.innerText = line || " ";
        codeDisplay.appendChild(lineSpan);
    });

    highlightedLine = -1;
    currentStepIndex = 0;
}

function highlightCodeLine(lineIndex) {
    // Remove previous highlights
    document.querySelectorAll(".code-line-span").forEach(span => {
        span.classList.remove("highlighted");
    });
    
    if (lineIndex >= 0) {
        const span = document.getElementById(`line-${lineIndex}`);
        if (span) {
            span.classList.add("highlighted");
            span.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }
    }
}

// Run Step-by-Step Lab Engine
function stepLab() {
    const lab = labs[activeLab];
    if (currentStepIndex >= lab.steps.length) {
        logInfo("Lab execution complete. Resetting debugger pointer.");
        currentStepIndex = 0;
        highlightCodeLine(-1);
        return;
    }

    const step = lab.steps[currentStepIndex];
    highlightCodeLine(step.line);
    
    // Set timer label for SQL
    document.getElementById("sql-time").innerText = Math.floor(Math.random() * 8 + 2) + " ms";

    // Run custom behavior
    step.action();
    
    // Output description to logs
    logInfo(`Step ${currentStepIndex + 1}: ${step.desc}`);

    currentStepIndex++;
}

// Run the full Lab in one click
async function runAllLab() {
    currentStepIndex = 0;
    const lab = labs[activeLab];
    for (let i = 0; i < lab.steps.length; i++) {
        stepLab();
        await new Promise(resolve => setTimeout(resolve, 800));
    }
}

// Reset context state
function resetLab() {
    activeDb = JSON.parse(JSON.stringify(initialDatabase));
    changeTracker = [];
    currentStepIndex = 0;
    document.getElementById("sql-console").innerHTML = `<div class="sql-placeholder">// System reset. Ready to run Lab ${activeLab}...</div>`;
    renderDbTables();
    renderTracker();
    highlightCodeLine(-1);
    logInfo(`Context reset. Ready for ${labs[activeLab].title}`);
}

// Setup Event Listeners
document.addEventListener("DOMContentLoaded", () => {
    // Select Lab button clicks
    document.querySelectorAll(".lab-btn").forEach(btn => {
        btn.addEventListener("click", (e) => {
            document.querySelectorAll(".lab-btn").forEach(b => b.classList.remove("active"));
            const targetBtn = e.currentTarget;
            targetBtn.classList.add("active");
            
            activeLab = parseInt(targetBtn.getAttribute("data-lab"));
            resetLab();
            loadLabCode(activeLab);
        });
    });

    // Control buttons
    document.getElementById("btn-run-all").addEventListener("click", runAllLab);
    document.getElementById("btn-step").addEventListener("click", stepLab);
    document.getElementById("btn-reset").addEventListener("click", resetLab);

    // Database tab buttons
    document.querySelectorAll(".db-tab-btn").forEach(btn => {
        btn.addEventListener("click", (e) => {
            document.querySelectorAll(".db-tab-btn").forEach(b => b.classList.remove("active"));
            e.currentTarget.classList.add("active");

            const tableName = e.currentTarget.getAttribute("data-table");
            document.querySelectorAll(".db-table-view").forEach(view => {
                view.classList.remove("active");
            });
            document.getElementById(`table-${tableName}`).classList.add("active");
        });
    });

    // Initialize UI
    loadLabCode(1);
    renderDbTables();
    renderTracker();
    logInfo("EF Core Real-Time Interactive Suite initialized. Select a lab from the list to start.");
});
