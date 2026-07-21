/*
================================================================================
Employee Management System - SQL Exercises (User-Defined Functions)
File Name: 03_Employee_Management_System_Functions.sql
================================================================================

This SQL script implements the database schema, seeds sample data, and
provides solutions for Exercises 1 to 10 for User-Defined Functions (UDFs).
*/

-- =============================================================================
-- DATABASE SCHEMA SETUP & SEEDING
-- =============================================================================

-- Drop functions first to avoid dependency issues when dropping tables
IF OBJECT_ID('dbo.fn_CalculateTotalCompensation', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateTotalCompensation;
IF OBJECT_ID('dbo.fn_CalculateBonus', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateBonus;
IF OBJECT_ID('dbo.fn_GetEmployeesByDepartment', 'IF') IS NOT NULL DROP FUNCTION dbo.fn_GetEmployeesByDepartment;
IF OBJECT_ID('dbo.fn_CalculateAnnualSalary', 'FN') IS NOT NULL DROP FUNCTION dbo.fn_CalculateAnnualSalary;

-- Drop tables if they already exist
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
(3, 'Bob', 'Johnson', 3, 5500.00, '2021-07-01');
GO


-- =============================================================================
-- Exercise 1: Create a Scalar Function
-- Goal: Create a scalar function to calculate the annual salary of an employee.
-- =============================================================================

PRINT '--- EXERCISE 1: CREATE SCALAR FUNCTION (fn_CalculateAnnualSalary) ---';
GO

CREATE FUNCTION fn_CalculateAnnualSalary (@Salary DECIMAL(10,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    RETURN @Salary * 12;
END;
GO

-- Test function
SELECT EmployeeID, FirstName, LastName, Salary, 
       dbo.fn_CalculateAnnualSalary(Salary) AS AnnualSalary
FROM Employees;
GO


-- =============================================================================
-- Exercise 2: Create a Table-Valued Function
-- Goal: Create a table-valued function to return employees in a specific department.
-- =============================================================================

PRINT '--- EXERCISE 2: CREATE TABLE-VALUED FUNCTION (fn_GetEmployeesByDepartment) ---';
GO

CREATE FUNCTION fn_GetEmployeesByDepartment (@DepartmentID INT)
RETURNS TABLE
AS
RETURN (
    SELECT EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate
    FROM Employees
    WHERE DepartmentID = @DepartmentID
);
GO

-- Test function by selecting from the IT department (DepartmentID = 2)
SELECT * FROM dbo.fn_GetEmployeesByDepartment(2);
GO


-- =============================================================================
-- Exercise 3: Create a User-Defined Function
-- Goal: Create a user-defined function to calculate the bonus (10%).
-- =============================================================================

PRINT '--- EXERCISE 3: CREATE BONUS FUNCTION (fn_CalculateBonus) ---';
GO

CREATE FUNCTION fn_CalculateBonus (@Salary DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @Salary * 0.10;
END;
GO

-- Test function
SELECT EmployeeID, FirstName, LastName, Salary, 
       dbo.fn_CalculateBonus(Salary) AS Bonus
FROM Employees;
GO


-- =============================================================================
-- Exercise 4: Modify a User-Defined Function
-- Goal: Modify the fn_CalculateBonus function to return Salary * 0.15 (15%).
-- =============================================================================

PRINT '--- EXERCISE 4: MODIFY BONUS FUNCTION (fn_CalculateBonus to 15%) ---';
GO

ALTER FUNCTION fn_CalculateBonus (@Salary DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @Salary * 0.15;
END;
GO

-- Test modified function
SELECT EmployeeID, FirstName, LastName, Salary, 
       dbo.fn_CalculateBonus(Salary) AS ModifiedBonus
FROM Employees;
GO


-- =============================================================================
-- Exercise 5: Delete a User-Defined Function
-- Goal: Delete the fn_CalculateBonus function.
-- =============================================================================

PRINT '--- EXERCISE 5: DELETE BONUS FUNCTION (fn_CalculateBonus) ---';
GO

DROP FUNCTION fn_CalculateBonus;
GO

-- Verify deletion (Should return NULL/not exist)
IF OBJECT_ID('dbo.fn_CalculateBonus', 'FN') IS NULL
    PRINT 'Verification: fn_CalculateBonus has been successfully deleted.';
ELSE
    PRINT 'Verification: fn_CalculateBonus still exists.';
GO


-- =============================================================================
-- Exercise 6: Execute a User-Defined Function
-- Goal: Execute fn_CalculateAnnualSalary and verify results.
-- =============================================================================

PRINT '--- EXERCISE 6: EXECUTE fn_CalculateAnnualSalary FOR ALL EMPLOYEES ---';
GO

SELECT 
    EmployeeID, 
    FirstName, 
    LastName, 
    Salary, 
    dbo.fn_CalculateAnnualSalary(Salary) AS AnnualSalary,
    -- Verification check (Manual verification assertion check)
    CASE 
        WHEN dbo.fn_CalculateAnnualSalary(Salary) = Salary * 12 THEN 'Passed'
        ELSE 'Failed'
    END AS VerificationStatus
FROM Employees;
GO


-- =============================================================================
-- Exercise 7: Return Data from a Scalar Function
-- Goal: Return the annual salary for a specific employee (EmployeeID = 1).
-- =============================================================================

PRINT '--- EXERCISE 7: SCALAR FUNCTION FOR A SPECIFIC EMPLOYEE (ID = 1) ---';
GO

DECLARE @AnnualSalaryResult DECIMAL(12,2);

SELECT @AnnualSalaryResult = dbo.fn_CalculateAnnualSalary(Salary)
FROM Employees
WHERE EmployeeID = 1;

PRINT 'Annual Salary for Employee ID 1 (John Doe): $' + CAST(@AnnualSalaryResult AS VARCHAR(20));
GO


-- =============================================================================
-- Exercise 8: Return Data from a Table-Valued Function
-- Goal: Return employees from the Finance department (ID = 3) using fn_GetEmployeesByDepartment.
-- =============================================================================

PRINT '--- EXERCISE 8: TABLE FUNCTION FOR FINANCE DEPARTMENT (ID = 3) ---';
GO

SELECT * FROM dbo.fn_GetEmployeesByDepartment(3);
GO


-- =============================================================================
-- Exercise 9: Create a Nested User-Defined Function
-- Goal: Create fn_CalculateTotalCompensation using fn_CalculateAnnualSalary 
-- and fn_CalculateBonus.
-- =============================================================================

PRINT '--- EXERCISE 9: CREATE NESTED FUNCTION (fn_CalculateTotalCompensation) ---';
GO

-- Step 1: Recreate fn_CalculateBonus since it was dropped in Exercise 5 (set to 10% initially)
CREATE FUNCTION fn_CalculateBonus (@Salary DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @Salary * 0.10;
END;
GO

-- Step 2: Create fn_CalculateTotalCompensation which nests both calls
CREATE FUNCTION fn_CalculateTotalCompensation (@Salary DECIMAL(10,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    -- Total compensation = Annual Salary + Annualized Bonus (Monthly Bonus * 12)
    -- Or simply sum the scalar outputs directly depending on definition of bonus.
    -- Here we sum the annual salary + annual bonus (calculated as monthly bonus * 12).
    RETURN dbo.fn_CalculateAnnualSalary(@Salary) + (dbo.fn_CalculateBonus(@Salary) * 12);
END;
GO

-- Test function
SELECT EmployeeID, FirstName, LastName, Salary,
       dbo.fn_CalculateAnnualSalary(Salary) AS AnnualSalary,
       dbo.fn_CalculateBonus(Salary) AS MonthlyBonus,
       dbo.fn_CalculateTotalCompensation(Salary) AS TotalAnnualCompensation
FROM Employees;
GO


-- =============================================================================
-- Exercise 10: Modify a Nested User-Defined Function
-- Goal: Modify fn_CalculateTotalCompensation to include new bonus calculation.
-- =============================================================================

PRINT '--- EXERCISE 10: MODIFY NESTED FUNCTION (Altered fn_CalculateBonus to 15%) ---';
GO

-- Step 1: Alter fn_CalculateBonus to 15% rate
ALTER FUNCTION fn_CalculateBonus (@Salary DECIMAL(10,2))
RETURNS DECIMAL(10,2)
AS
BEGIN
    RETURN @Salary * 0.15;
END;
GO

-- Step 2: Alter fn_CalculateTotalCompensation to ensure it recalculates with modified logic
ALTER FUNCTION fn_CalculateTotalCompensation (@Salary DECIMAL(10,2))
RETURNS DECIMAL(12,2)
AS
BEGIN
    -- Utilizing altered fn_CalculateBonus (15%)
    RETURN dbo.fn_CalculateAnnualSalary(@Salary) + (dbo.fn_CalculateBonus(@Salary) * 12);
END;
GO

-- Test modified nested function
SELECT EmployeeID, FirstName, LastName, Salary,
       dbo.fn_CalculateAnnualSalary(Salary) AS AnnualSalary,
       dbo.fn_CalculateBonus(Salary) AS MonthlyBonus_15Percent,
       dbo.fn_CalculateTotalCompensation(Salary) AS TotalAnnualCompensation_Modified
FROM Employees;
GO
