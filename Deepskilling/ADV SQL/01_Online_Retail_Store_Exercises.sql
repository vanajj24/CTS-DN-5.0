/*
================================================================================
Advanced SQL Exercises for Online Retail Store
File Name: 01_Online_Retail_Store_Exercises.sql
================================================================================

This SQL script implements the database schema, seeds sample data, and 
provides solutions for Exercises 1 to 5 as detailed in the Advanced SQL 
Exercises for Online Retail Store workbook.
*/

-- =============================================================================
-- DATABASE SCHEMA SETUP & SEEDING
-- =============================================================================

-- Drop tables if they already exist to ensure a clean slate
IF OBJECT_ID('dbo.OrderDetails', 'U') IS NOT NULL DROP TABLE dbo.OrderDetails;
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.StagingProducts', 'U') IS NOT NULL DROP TABLE dbo.StagingProducts;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
GO

-- 1. Create Customers Table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Region VARCHAR(50) NOT NULL
);

-- 2. Create Products Table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);

-- 3. Create Orders Table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID) NOT NULL,
    OrderDate DATE NOT NULL
);

-- 4. Create OrderDetails Table
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY,
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID) NOT NULL,
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID) NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10, 2) NOT NULL
);

-- 5. Create StagingProducts Table (Used in Exercise 3 MERGE)
CREATE TABLE StagingProducts (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL
);
GO

-- Seed Customers
INSERT INTO Customers (CustomerID, Name, Region) VALUES
(1, 'Alice Smith', 'North'),
(2, 'Bob Jones', 'South'),
(3, 'Charlie Brown', 'East'),
(4, 'David Wilson', 'West'),
(5, 'Eva Green', 'North');

-- Seed Products (Includes duplicate/tied prices in categories to compare window functions)
INSERT INTO Products (ProductID, ProductName, Category, Price) VALUES
(101, 'Laptop Elite', 'Electronics', 1500.00),
(102, 'Smart Phone X', 'Electronics', 999.00),
(103, 'Tablet Air', 'Electronics', 999.00),   -- Tied with Smart Phone X
(104, 'Wireless Buds', 'Electronics', 150.00),
(105, 'Ergonomic Chair', 'Furniture', 350.00),
(106, 'Standing Desk', 'Furniture', 600.00),
(107, 'LED Desk Lamp', 'Furniture', 45.00),
(108, 'Leather Sofa', 'Furniture', 1500.00),
(109, 'Coffee Table', 'Furniture', 350.00),    -- Tied with Ergonomic Chair
(110, 'Running Shoes', 'Apparel', 120.00),
(111, 'Hoodie Jacket', 'Apparel', 80.00),
(112, 'Designer Jeans', 'Apparel', 120.00),    -- Tied with Running Shoes
(113, 'Leather Belt', 'Apparel', 40.00);

-- Seed Orders
-- Customer 1 has 4 orders to satisfy Exercise 5 (Count of orders > 3)
INSERT INTO Orders (OrderID, CustomerID, OrderDate) VALUES
(1001, 1, '2025-01-05'),
(1002, 1, '2025-01-12'),
(1003, 1, '2025-02-14'),
(1004, 1, '2025-03-20'),
(1005, 2, '2025-01-08'),
(1006, 2, '2025-02-18'),
(1007, 3, '2025-01-15'),
(1008, 4, '2025-03-22'),
(1009, 5, '2025-02-25');

-- Seed OrderDetails (spread across months: Jan, Feb, Mar)
INSERT INTO OrderDetails (OrderDetailID, OrderID, ProductID, Quantity, UnitPrice) VALUES
(1, 1001, 101, 1, 1500.00),
(2, 1001, 104, 2, 150.00),
(3, 1002, 102, 1, 999.00),
(4, 1003, 108, 1, 1500.00),
(5, 1004, 106, 1, 600.00),
(6, 1005, 110, 2, 120.00),
(7, 1006, 103, 1, 999.00),
(8, 1007, 105, 3, 350.00),
(9, 1008, 111, 2, 80.00),
(10, 1009, 112, 1, 120.00);
GO


-- =============================================================================
-- Exercise 1: Ranking and Window Functions
-- Goal: Use ROW_NUMBER(), RANK(), DENSE_RANK(), OVER(), and PARTITION BY.
-- Scenario: Find the top 3 most expensive products in each category.
-- =============================================================================

PRINT '--- EXERCISE 1: RANKING AND WINDOW FUNCTIONS ---';

WITH RankedProducts AS (
    SELECT 
        Category,
        ProductName,
        Price,
        ROW_NUMBER() OVER (PARTITION BY Category ORDER BY Price DESC) AS RowNum,
        RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS Rnk,
        DENSE_RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS DenseRnk
    FROM Products
)
SELECT 
    Category,
    ProductName,
    Price,
    RowNum AS [ROW_NUMBER],
    Rnk AS [RANK],
    DenseRnk AS [DENSE_RANK]
FROM RankedProducts
WHERE RowNum <= 3 OR Rnk <= 3 OR DenseRnk <= 3;
GO


-- =============================================================================
-- Exercise 2: Aggregation with GROUPING SETS, CUBE, and ROLLUP
-- Goal: Analyze sales data across multiple dimensions (Region and Category).
-- =============================================================================

PRINT '--- EXERCISE 2: AGGREGATIONS ---';

-- 1. GROUPING SETS: Totals by Region, Category, and both Region + Category
PRINT '1. Totals using GROUPING SETS:';
SELECT 
    c.Region,
    p.Category,
    SUM(od.Quantity) AS TotalQuantitySold
FROM OrderDetails od
JOIN Orders o ON od.OrderID = o.OrderID
JOIN Customers c ON o.CustomerID = c.CustomerID
JOIN Products p ON od.ProductID = p.ProductID
GROUP BY GROUPING SETS (
    (c.Region, p.Category),
    (c.Region),
    (p.Category)
);
GO

-- 2. ROLLUP: Subtotals and grand totals based on hierarchical order (Region -> Category)
PRINT '2. Totals using ROLLUP:';
SELECT 
    c.Region,
    p.Category,
    SUM(od.Quantity) AS TotalQuantitySold
FROM OrderDetails od
JOIN Orders o ON od.OrderID = o.OrderID
JOIN Customers c ON o.CustomerID = c.CustomerID
JOIN Products p ON od.ProductID = p.ProductID
GROUP BY ROLLUP (c.Region, p.Category);
GO

-- 3. CUBE: Get all possible cross-tabulation combinations of Region and Category
PRINT '3. Totals using CUBE:';
SELECT 
    c.Region,
    p.Category,
    SUM(od.Quantity) AS TotalQuantitySold
FROM OrderDetails od
JOIN Orders o ON od.OrderID = o.OrderID
JOIN Customers c ON o.CustomerID = c.CustomerID
JOIN Products p ON od.ProductID = p.ProductID
GROUP BY CUBE (c.Region, p.Category);
GO


-- =============================================================================
-- Exercise 3: CTEs and MERGE
-- Goal: Use WITH, CTEs, Recursive CTEs, and MERGE.
-- =============================================================================

PRINT '--- EXERCISE 3: CTEs AND MERGE ---';

-- a) Recursive CTE to generate a calendar table from '2025-01-01' to '2025-01-31'
PRINT 'a) Recursive CTE Calendar Output:';
WITH CalendarCTE AS (
    SELECT CAST('2025-01-01' AS DATE) AS CalendarDate
    UNION ALL
    SELECT DATEADD(day, 1, CalendarDate)
    FROM CalendarCTE
    WHERE CalendarDate < '2025-01-31'
)
SELECT CalendarDate
FROM CalendarCTE
OPTION (MAXRECURSION 100);
GO

-- b) Seed StagingProducts for MERGE demonstration
INSERT INTO StagingProducts (ProductID, ProductName, Category, Price) VALUES
(101, 'Laptop Elite v2', 'Electronics', 1450.00), -- Price decrease and name update
(102, 'Smart Phone X', 'Electronics', 999.00),     -- Unchanged
(105, 'Ergonomic Chair Pro', 'Furniture', 380.00),   -- Price and name update
(114, 'Wireless Charger', 'Electronics', 49.99);    -- Completely new product
GO

-- MERGE statement to update existing products or insert new ones from staging
PRINT 'b) Performing MERGE operation:';
MERGE Products AS target
USING StagingProducts AS source
ON (target.ProductID = source.ProductID)
WHEN MATCHED THEN
    UPDATE SET 
        target.ProductName = source.ProductName,
        target.Category = source.Category,
        target.Price = source.Price
WHEN NOT MATCHED THEN
    INSERT (ProductID, ProductName, Category, Price)
    VALUES (source.ProductID, source.ProductName, source.Category, source.Price);

-- Verify the MERGE results
SELECT * FROM Products WHERE ProductID IN (101, 102, 105, 114);
GO


-- =============================================================================
-- Exercise 4: PIVOT and UNPIVOT
-- Goal: Transform monthly sales quantity per product.
-- =============================================================================

PRINT '--- EXERCISE 4: PIVOT AND UNPIVOT ---';

-- 1 & 2. Aggregate sales and use PIVOT to convert Month rows into separate columns
PRINT '1 & 2. Pivoted monthly sales quantity per product:';
WITH MonthlySales AS (
    SELECT 
        p.ProductName,
        DATENAME(month, o.OrderDate) AS OrderMonth,
        od.Quantity
    FROM OrderDetails od
    JOIN Orders o ON od.OrderID = o.OrderID
    JOIN Products p ON od.ProductID = p.ProductID
),
PivotedSales AS (
    SELECT ProductName, 
           ISNULL([January], 0) AS January, 
           ISNULL([February], 0) AS February, 
           ISNULL([March], 0) AS March
    FROM MonthlySales
    PIVOT (
        SUM(Quantity)
        FOR OrderMonth IN ([January], [February], [March])
    ) AS PivotTable
)
SELECT * FROM PivotedSales;
GO

-- 3. UNPIVOT to convert the pivoted columns back into row format
PRINT '3. Unpivoted data back into row format:';
WITH MonthlySales AS (
    SELECT 
        p.ProductName,
        DATENAME(month, o.OrderDate) AS OrderMonth,
        od.Quantity
    FROM OrderDetails od
    JOIN Orders o ON od.OrderID = o.OrderID
    JOIN Products p ON od.ProductID = p.ProductID
),
PivotedSales AS (
    SELECT ProductName, 
           ISNULL([January], 0) AS January, 
           ISNULL([February], 0) AS February, 
           ISNULL([March], 0) AS March
    FROM MonthlySales
    PIVOT (
        SUM(Quantity)
        FOR OrderMonth IN ([January], [February], [March])
    ) AS PivotTable
)
SELECT ProductName, OrderMonth, Quantity
FROM PivotedSales
UNPIVOT (
    Quantity FOR OrderMonth IN (January, February, March)
) AS UnpivotTable;
GO


-- =============================================================================
-- Exercise 5: Using CTE to Simplify a Query
-- Goal: Find all customers who have placed more than 3 orders in total.
-- =============================================================================

PRINT '--- EXERCISE 5: CTE QUERY SIMPLIFICATION ---';

WITH CustomerOrderCounts AS (
    SELECT 
        o.CustomerID,
        COUNT(o.OrderID) AS OrderCount
    FROM Orders o
    GROUP BY o.CustomerID
)
SELECT 
    c.CustomerID,
    c.Name,
    coc.OrderCount
FROM CustomerOrderCounts coc
JOIN Customers c ON c.CustomerID = coc.CustomerID
WHERE coc.OrderCount > 3;
GO
