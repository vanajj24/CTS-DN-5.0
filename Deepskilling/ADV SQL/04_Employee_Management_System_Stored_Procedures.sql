/*
================================================================================
Employee Management System SQL Exercises (Stored Procedures)
File Name: 04_Employee_Management_System_Stored_Procedures.sql
================================================================================

This SQL script implements the database schema, seeds sample data, and
provides solutions for Exercises 1 to 11 for Stored Procedures (SPs).
*/

-- =============================================================================
-- DATABASE SCHEMA SETUP & SEEDING
-- =============================================================================

-- Drop stored procedures first to avoid dependency conflicts
IF OBJECT_ID('dbo.sp_UpdateEmployeeSalaryWithErrorHandling', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_UpdateEmployeeSalaryWithErrorHandling;
IF OBJECT_ID('dbo.sp_GetEmployeesDynamicFilter', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetEmployeesDynamicFilter;
IF OBJECT_ID('dbo.sp_UpdateEmployeeSalaryTx', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_UpdateEmployeeSalaryTx;
IF OBJECT_ID('dbo.sp_GiveBonus', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GiveBonus;
IF OBJECT_ID('dbo.sp_UpdateEmployeeSalary', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_UpdateEmployeeSalary;
IF OBJECT_ID('dbo.sp_GetTotalSalaryByDepartment', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetTotalSalaryByDepartment;
IF OBJECT_ID('dbo.sp_GetEmployeeCountByDepartment', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetEmployeeCountByDepartment;
IF OBJECT_ID('dbo.sp_GetEmployeesByDepartment', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_GetEmployeesByDepartment;
IF OBJECT_ID('dbo.sp_InsertEmployee', 'P') IS NOT NULL DROP PROCEDURE dbo.sp_InsertEmployee;

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
(2, 'Finance'),
(3, 'IT'),
(4, 'Marketing');

-- Seed Employees
INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate) VALUES
(1, 'John', 'Doe', 1, 5000.00, '2020-01-15'),
(2, 'Jane', 'Smith', 2, 6000.00, '2019-03-22'),
(3, 'Michael', 'Johnson', 3, 7000.00, '2018-07-30'),
(4, 'Emily', 'Davis', 4, 5500.00, '2021-11-05');
GO


-- =============================================================================
-- Exercise 1: Create a Stored Procedure
-- Goal: Create stored procedures to retrieve details by department and insert.
-- =============================================================================

PRINT '--- EXERCISE 1: CREATE STORED PROCEDURES ---';
GO

-- a) Create procedure to retrieve employee details by department (Initially without Salary to show Exercise 2 ALTER)
CREATE PROCEDURE sp_GetEmployeesByDepartment
    @DepartmentID INT
AS
BEGIN
    SELECT EmployeeID, FirstName, LastName, DepartmentID, JoinDate
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- b) Create procedure named sp_InsertEmployee as specified in step 3
CREATE PROCEDURE sp_InsertEmployee
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @DepartmentID INT,
    @Salary DECIMAL(10,2),
    @JoinDate DATE
AS
BEGIN
    -- Auto-generate unique EmployeeID since it is NOT an IDENTITY column
    DECLARE @NewEmployeeID INT;
    SELECT @NewEmployeeID = ISNULL(MAX(EmployeeID), 0) + 1 FROM Employees;

    INSERT INTO Employees (EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate)
    VALUES (@NewEmployeeID, @FirstName, @LastName, @DepartmentID, @Salary, @JoinDate);
END;
GO

-- Test sp_InsertEmployee
EXEC sp_InsertEmployee 'David', 'Miller', 3, 6200.00, '2025-06-15';
SELECT * FROM Employees WHERE FirstName = 'David';
GO


-- =============================================================================
-- Exercise 2: Modify a Stored Procedure
-- Goal: Modify the retrieval procedure to include employee salary.
-- =============================================================================

PRINT '--- EXERCISE 2: MODIFY STORED PROCEDURE (ADD SALARY COLUMN) ---';
GO

ALTER PROCEDURE sp_GetEmployeesByDepartment
    @DepartmentID INT
AS
BEGIN
    SELECT EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO


-- =============================================================================
-- Exercise 3: Delete a Stored Procedure
-- Goal: Delete the stored procedure created in Exercise 1.
-- =============================================================================

PRINT '--- EXERCISE 3: DELETE STORED PROCEDURE ---';
GO

-- Drop sp_GetEmployeesByDepartment
DROP PROCEDURE sp_GetEmployeesByDepartment;
GO

-- Verify drop
IF OBJECT_ID('dbo.sp_GetEmployeesByDepartment', 'P') IS NULL
    PRINT 'Verification: sp_GetEmployeesByDepartment successfully deleted.';
ELSE
    PRINT 'Verification: sp_GetEmployeesByDepartment still exists.';
GO

-- Recreate sp_GetEmployeesByDepartment so it can be used for Exercise 4
CREATE PROCEDURE sp_GetEmployeesByDepartment
    @DepartmentID INT
AS
BEGIN
    SELECT EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO


-- =============================================================================
-- Exercise 4: Execute a Stored Procedure
-- Goal: Execute the stored procedure to retrieve employee details for a specific department.
-- =============================================================================

PRINT '--- EXERCISE 4: EXECUTE STORED PROCEDURE (DEPT ID = 3) ---';
GO

-- Execute the procedure for IT Department (DepartmentID = 3)
EXEC sp_GetEmployeesByDepartment @DepartmentID = 3;
GO


-- =============================================================================
-- Exercise 5: Return Data from a Stored Procedure
-- Goal: Create a stored procedure that returns the total number of employees in a department.
-- =============================================================================

PRINT '--- EXERCISE 5: RETURN DATA (COUNT OF EMPLOYEES BY DEPT) ---';
GO

CREATE PROCEDURE sp_GetEmployeeCountByDepartment
    @DepartmentID INT
AS
BEGIN
    SELECT COUNT(*) AS TotalEmployees
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- Test counting employees in IT (Dept 3)
EXEC sp_GetEmployeeCountByDepartment @DepartmentID = 3;
GO


-- =============================================================================
-- Exercise 6: Use Output Parameters in a Stored Procedure
-- Goal: Create a procedure that returns total salary using an output parameter.
-- =============================================================================

PRINT '--- EXERCISE 6: STORED PROCEDURE WITH OUTPUT PARAMETER ---';
GO

CREATE PROCEDURE sp_GetTotalSalaryByDepartment
    @DepartmentID INT,
    @TotalSalary DECIMAL(10,2) OUTPUT
AS
BEGIN
    SELECT @TotalSalary = ISNULL(SUM(Salary), 0)
    FROM Employees
    WHERE DepartmentID = @DepartmentID;
END;
GO

-- Test output parameter
DECLARE @TotalSalOut DECIMAL(10,2);
EXEC sp_GetTotalSalaryByDepartment @DepartmentID = 3, @TotalSalary = @TotalSalOut OUTPUT;
PRINT 'Total Salary for Department 3: $' + CAST(@TotalSalOut AS VARCHAR(20));
GO


-- =============================================================================
-- Exercise 7: Create a Stored Procedure with Multiple Parameters
-- Goal: Create a stored procedure to update employee salary.
-- =============================================================================

PRINT '--- EXERCISE 7: UPDATE SALARY USING MULTIPLE PARAMETERS ---';
GO

CREATE PROCEDURE sp_UpdateEmployeeSalary
    @EmployeeID INT,
    @Salary DECIMAL(10,2)
AS
BEGIN
    UPDATE Employees
    SET Salary = @Salary
    WHERE EmployeeID = @EmployeeID;
END;
GO

-- Execute the update as requested in steps:
EXEC sp_UpdateEmployeeSalary 1, 5500.00;

-- Verify results
SELECT EmployeeID, FirstName, LastName, Salary FROM Employees WHERE EmployeeID = 1;
GO


-- =============================================================================
-- Exercise 8: Create a Stored Procedure with Conditional Logic
-- Goal: Create a stored procedure to give a bonus to employees.
-- =============================================================================

PRINT '--- EXERCISE 8: CONDITIONAL LOGIC IN STORED PROCEDURE ---';
GO

CREATE PROCEDURE sp_GiveBonus
    @EmployeeID INT,
    @BonusAmount DECIMAL(10,2)
AS
BEGIN
    DECLARE @DeptID INT;
    SELECT @DeptID = DepartmentID FROM Employees WHERE EmployeeID = @EmployeeID;

    IF @DeptID IS NOT NULL
    BEGIN
        -- Conditional Logic: If IT (Dept 3) give extra $100, otherwise normal bonus
        IF @DeptID = 3
        BEGIN
            UPDATE Employees
            SET Salary = Salary + @BonusAmount + 100.00
            WHERE EmployeeID = @EmployeeID;
            PRINT 'IT Department employee identified. Bonus + $100 extra applied.';
        END
        ELSE
        BEGIN
            UPDATE Employees
            SET Salary = Salary + @BonusAmount
            WHERE EmployeeID = @EmployeeID;
            PRINT 'Normal bonus applied.';
        END
    END
    ELSE
    BEGIN
        PRINT 'Error: Employee ID not found.';
    END
END;
GO

-- Execute the bonus update as requested in steps:
EXEC sp_GiveBonus 1, 500.00;

-- Verify results (Employee 1 should now be $5500.00 + $500.00 = $6000.00)
SELECT EmployeeID, FirstName, LastName, Salary FROM Employees WHERE EmployeeID = 1;
GO


-- =============================================================================
-- Exercise 9: Use Transactions in a Stored Procedure
-- Goal: Create sp_UpdateEmployeeSalaryTx with a transaction to ensure integrity.
-- =============================================================================

PRINT '--- EXERCISE 9: EXPLICIT TRANSACTIONS IN STORED PROCEDURE ---';
GO

CREATE PROCEDURE sp_UpdateEmployeeSalaryTx
    @EmployeeID INT,
    @Salary DECIMAL(10,2)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Update statement
        UPDATE Employees
        SET Salary = @Salary
        WHERE EmployeeID = @EmployeeID;

        -- If no employee matched, abort transaction
        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50001, 'Employee not found. Rollback transaction.', 1;
        END

        COMMIT TRANSACTION;
        PRINT 'Transaction committed successfully.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        PRINT 'Error encountered, transaction rolled back. Details: ' + ERROR_MESSAGE();
    END CATCH;
END;
GO

-- Test transaction with valid ID
EXEC sp_UpdateEmployeeSalaryTx 2, 6500.00;
-- Test transaction with invalid ID (triggers custom throw/rollback)
EXEC sp_UpdateEmployeeSalaryTx 99, 8000.00;
GO


-- =============================================================================
-- Exercise 10: Use Dynamic SQL in a Stored Procedure
-- Goal: Retrieve employee details based on a flexible filter.
-- =============================================================================

PRINT '--- EXERCISE 10: DYNAMIC SQL IN STORED PROCEDURE ---';
GO

CREATE PROCEDURE sp_GetEmployeesDynamicFilter
    @FilterColumn VARCHAR(50),
    @FilterValue VARCHAR(100)
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @ParmDefinition NVARCHAR(500);

    -- Safety Check: Whitelist column names to block SQL injection
    IF @FilterColumn NOT IN ('EmployeeID', 'FirstName', 'LastName', 'DepartmentID')
    BEGIN
        RAISERROR('Error: Invalid filter column.', 16, 1);
        RETURN;
    END

    -- Build Query
    SET @SQL = N'SELECT EmployeeID, FirstName, LastName, DepartmentID, Salary, JoinDate 
                 FROM Employees 
                 WHERE ' + QUOTENAME(@FilterColumn) + N' = @Val';
                 
    SET @ParmDefinition = N'@Val VARCHAR(100)';

    -- Execute dynamically
    EXEC sp_executesql @SQL, @ParmDefinition, @Val = @FilterValue;
END;
GO

-- Test dynamic SQL procedure
EXEC sp_GetEmployeesDynamicFilter 'LastName', 'Smith';
EXEC sp_GetEmployeesDynamicFilter 'DepartmentID', '3';
GO


-- =============================================================================
-- Exercise 11: Handle Errors in a Stored Procedure
-- Goal: Create a stored procedure that handles errors with TRY...CATCH.
-- =============================================================================

PRINT '--- EXERCISE 11: ERROR HANDLING (TRY...CATCH) ---';
GO

CREATE PROCEDURE sp_UpdateEmployeeSalaryWithErrorHandling
    @EmployeeID INT,
    @Salary DECIMAL(10,2)
AS
BEGIN
    BEGIN TRY
        -- Validate Business Rule
        IF @Salary < 0
        BEGIN
            RAISERROR('Error: Salary cannot be negative.', 16, 1);
        END

        UPDATE Employees
        SET Salary = @Salary
        WHERE EmployeeID = @EmployeeID;

        -- Verify Rowcount
        IF @@ROWCOUNT = 0
        BEGIN
            THROW 51000, 'Error: The specified EmployeeID does not exist.', 1;
        END

        PRINT 'Salary updated successfully.';
    END TRY
    BEGIN CATCH
        -- Capture details and re-raise/custom print
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR('Custom Error Handled: %s', @ErrorSeverity, @ErrorState, @ErrorMessage);
    END CATCH;
END;
GO

-- Test with valid data
EXEC sp_UpdateEmployeeSalaryWithErrorHandling 3, 7200.00;
-- Test with invalid negative salary (triggers custom error handler)
EXEC sp_UpdateEmployeeSalaryWithErrorHandling 3, -1000.00;
-- Test with non-existent employee ID (triggers custom error handler)
EXEC sp_UpdateEmployeeSalaryWithErrorHandling 999, 5000.00;
GO
