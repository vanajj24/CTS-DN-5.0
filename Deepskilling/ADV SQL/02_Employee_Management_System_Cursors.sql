/*
================================================================================
Employee Management System - SQL Server Cursor Exercises
File Name: 02_Employee_Management_System_Cursors.sql
================================================================================

This SQL script implements the database schema, seeds sample data, and
provides solutions for Cursor Exercises 1 and 2, including declarations
and descriptions of the different cursor types available in SQL Server.
*/

-- =============================================================================
-- DATABASE SCHEMA SETUP & SEEDING
-- =============================================================================

-- Drop tables if they already exist to ensure a clean slate
IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL DROP TABLE dbo.Employees;
IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL DROP TABLE dbo.Departments;
GO

-- 1. Create Departments Table
CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL
);

-- 2. Create Employees Table
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DepartmentID INT FOREIGN KEY REFERENCES Departments(DepartmentID) NOT NULL,
    Salary DECIMAL(10,2) NOT NULL,
    JoinDate DATE NOT NULL
);
GO

-- Seed Departments
INSERT INTO Departments (DepartmentID, DepartmentName) VALUES
(1, 'HR'),
(2, 'IT'),
(3, 'Finance');

-- Seed Employees
INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES
(1, 'John', 'Doe', 1, 5000.00, '2020-01-15'),
(2, 'Jane', 'Smith', 2, 6000.00, '2019-03-22'),
(3, 'Bob', 'Johnson', 3, 5500.00, '2021-07-30');
GO


-- =============================================================================
-- Exercise 1: Create a Cursor
-- Goal: Create a cursor to iterate over all employees and print their details.
-- =============================================================================

PRINT '--- EXERCISE 1: CREATE A BASIC CURSOR ---';

-- Declare local variables to hold fetched row values
DECLARE @EmployeeID INT;
DECLARE @FirstName VARCHAR(50);
DECLARE @LastName VARCHAR(50);
DECLARE @DepartmentID INT;
DECLARE @Salary DECIMAL(10,2);
DECLARE @JoinDate DATE;

-- Step 1: Declare the cursor to select all columns
DECLARE emp_cursor CURSOR FOR
SELECT EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate
FROM Employees;

-- Step 2: Open the cursor
OPEN emp_cursor;

-- Step 3: Fetch the first row
FETCH NEXT FROM emp_cursor
INTO @EmployeeID, @FirstName, @LastName, @DepartmentID, @Salary, @JoinDate;

-- Loop through all rows in the cursor
WHILE @@FETCH_STATUS = 0
BEGIN
    -- Step 4: Print details of the current employee
    PRINT 'Employee: ' + CAST(@EmployeeID AS VARCHAR(5)) + ' | ' + 
          @FirstName + ' ' + @LastName + ' | Dept ID: ' + 
          CAST(@DepartmentID AS VARCHAR(5)) + ' | Salary: $' + 
          CAST(@Salary AS VARCHAR(10)) + ' | Joined: ' + 
          CONVERT(VARCHAR(10), @JoinDate, 120);

    -- Fetch the next row
    FETCH NEXT FROM emp_cursor
    INTO @EmployeeID, @FirstName, @LastName, @DepartmentID, @Salary, @JoinDate;
END;

-- Step 5: Close and deallocate the cursor
CLOSE emp_cursor;
DEALLOCATE emp_cursor;
GO


-- =============================================================================
-- Exercise 2: Types of Cursors
-- Goal: Understand the different types of cursors in SQL Server.
-- =============================================================================

PRINT '--- EXERCISE 2: DEMONSTRATING AND EXPLAINING CURSOR TYPES ---';

/*
--------------------------------------------------------------------------------
1. STATIC CURSOR
   - Description: A static cursor makes a copy of the results in tempdb. 
   - Behavior: Changes made to the database after the cursor is opened are 
     NOT visible. Scrollability is fully supported.
--------------------------------------------------------------------------------
*/
PRINT '1. Declaring a STATIC cursor...';
DECLARE static_cursor CURSOR STATIC FOR
SELECT EmployeeID, FirstName, LastName, Salary FROM Employees;

OPEN static_cursor;
-- Fills tempdb. Modifying underlying data now will not change cursor data.
CLOSE static_cursor;
DEALLOCATE static_cursor;
GO


/*
--------------------------------------------------------------------------------
2. DYNAMIC CURSOR
   - Description: A dynamic cursor detects all changes made to the rows 
     in the result set as you scroll.
   - Behavior: Data inserts, updates, and deletes by any user are visible 
     in the cursor during fetch.
--------------------------------------------------------------------------------
*/
PRINT '2. Declaring a DYNAMIC cursor...';
DECLARE dynamic_cursor CURSOR DYNAMIC FOR
SELECT EmployeeID, FirstName, LastName, Salary FROM Employees;

OPEN dynamic_cursor;
-- Can scroll in any direction and updates are immediately visible.
CLOSE dynamic_cursor;
DEALLOCATE dynamic_cursor;
GO


/*
--------------------------------------------------------------------------------
3. FORWARD-ONLY CURSOR
   - Description: A forward-only cursor is the default cursor type in SQL Server.
   - Behavior: Can only be fetched sequentially from first to last row. 
     Provides high performance (often referred to as a "firehose cursor").
--------------------------------------------------------------------------------
*/
PRINT '3. Declaring a FORWARD-ONLY cursor...';
DECLARE forward_only_cursor CURSOR FORWARD_ONLY FOR
SELECT EmployeeID, FirstName, LastName, Salary FROM Employees;

OPEN forward_only_cursor;
-- Only FETCH NEXT is valid. Cannot fetch PRIOR, FIRST, LAST, or RELATIVE.
CLOSE forward_only_cursor;
DEALLOCATE forward_only_cursor;
GO


/*
--------------------------------------------------------------------------------
4. KEYSET-DRIVEN CURSOR
   - Description: Keyset cursors create a unique set of keys (keyset) in tempdb.
   - Behavior: The membership and order of rows are fixed when the cursor opens.
     Data updates to existing rows are visible, but new inserts are NOT visible.
--------------------------------------------------------------------------------
*/
PRINT '4. Declaring a KEYSET-driven cursor...';
DECLARE keyset_cursor CURSOR KEYSET FOR
SELECT EmployeeID, FirstName, LastName, Salary FROM Employees;

OPEN keyset_cursor;
-- Keyset stored in tempdb. Data updates are visible by joining keys.
CLOSE keyset_cursor;
DEALLOCATE keyset_cursor;
GO


-- =============================================================================
-- Comparison Summary of Cursor Types in SQL Server
-- =============================================================================
/*
CURSOR TYPE    | SCROLLABLE | TEMPDB STORAGE | VISIBILITY OF UPDATES | PERFORMANCE
--------------------------------------------------------------------------------
STATIC         | Yes        | High (Full copy) | No                    | Slowest setup
DYNAMIC        | Yes        | Low            | Yes (Inserts/Updates) | Moderate
FORWARD_ONLY   | No         | None           | Yes (If dynamic)      | Fastest (Default)
KEYSET         | Yes        | Medium (Keys)  | Yes (Updates only)    | Moderate
*/
PRINT '--- Cursor types declared and documented successfully ---';
GO
