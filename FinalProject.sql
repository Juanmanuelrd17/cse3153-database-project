-- CSE 3153 Final Project - Auto Service Shop Database

-- #1 Switch to master
USE master;
GO

-- #2 Drop database if it already exists
IF DB_ID('AutoServiceShopDB') IS NOT NULL
BEGIN
    ALTER DATABASE AutoServiceShopDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AutoServiceShopDB;
END;
GO

-- #3 Create database
CREATE DATABASE AutoServiceShopDB;
GO

-- #4 Switch to AutoServiceShopDB
USE AutoServiceShopDB;
GO

-- #5 Create garage schema
CREATE SCHEMA garage;
GO

-- #6 Create Customers table
CREATE TABLE garage.Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    CreatedDate DATE NOT NULL DEFAULT GETDATE()
);

-- #7 Create Vehicles table
CREATE TABLE garage.Vehicles (
    VehicleID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT NOT NULL,
    Make VARCHAR(40) NOT NULL,
    Model VARCHAR(40) NOT NULL,
    ModelYear INT NOT NULL CHECK (ModelYear BETWEEN 1900 AND 2030),
    VIN VARCHAR(17) NOT NULL UNIQUE,
    LicensePlate VARCHAR(15) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES garage.Customers(CustomerID)
);

-- #8 Create Mechanics table
CREATE TABLE garage.Mechanics (
    MechanicID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Specialty VARCHAR(60) NOT NULL,
    HourlyRate DECIMAL(8,2) NOT NULL,
    HireDate DATE NOT NULL
);

-- #9 Create Services table
CREATE TABLE garage.Services (
    ServiceID INT IDENTITY(1,1) PRIMARY KEY,
    ServiceName VARCHAR(80) NOT NULL UNIQUE,
    Description VARCHAR(200) NULL,
    StandardPrice DECIMAL(10,2) NOT NULL,
    EstimatedHours DECIMAL(4,2) NOT NULL
);

-- #10 Create WorkOrders table
CREATE TABLE garage.WorkOrders (
    WorkOrderID INT IDENTITY(1,1) PRIMARY KEY,
    VehicleID INT NOT NULL,
    MechanicID INT NOT NULL,
    ServiceID INT NOT NULL,
    ServiceDate DATE NOT NULL DEFAULT GETDATE(),
    Status VARCHAR(20) NOT NULL DEFAULT 'Scheduled'
        CHECK (Status IN ('Scheduled', 'In Progress', 'Completed', 'Cancelled')),
    TotalCost DECIMAL(10,2) NOT NULL,
    Notes VARCHAR(250) NULL,
    FOREIGN KEY (VehicleID) REFERENCES garage.Vehicles(VehicleID),
    FOREIGN KEY (MechanicID) REFERENCES garage.Mechanics(MechanicID),
    FOREIGN KEY (ServiceID) REFERENCES garage.Services(ServiceID)
);
GO

-- #11 Insert sample customers
INSERT INTO garage.Customers (FirstName, LastName, Phone, Email)
VALUES
    ('Carlos', 'Mendez', '404-219-8340', 'cmendez@gmail.com'),
    ('Ashley', 'Brooks', '678-412-9055', 'abrooks@yahoo.com'),
    ('Kevin', 'Tran', '770-308-6122', 'ktran@gmail.com'),
    ('Jordan', 'Miller', '404-731-2890', 'jmiller@outlook.com');

-- #12 Insert sample vehicles
INSERT INTO garage.Vehicles (CustomerID, Make, Model, ModelYear, VIN, LicensePlate)
VALUES
    (1, 'Honda', 'Civic', 2020, '2HGFC2F59LH512301', 'RHT4821'),
    (2, 'Toyota', 'RAV4', 2022, '2T3P1RFV8NW204512', 'KLM7749'),
    (3, 'Ford', 'F-150', 2021, '1FTEW1EP4MF309823', 'TRK9012'),
    (4, 'Mazda', 'CX-5', 2019, 'JM3KFBCM5K0412034', 'GA3156B');

-- #13 Insert sample mechanics
INSERT INTO garage.Mechanics (FirstName, LastName, Specialty, HourlyRate, HireDate)
VALUES
    ('Mike', 'Torres', 'Engine & Diagnostics', 95.00, '2022-03-15'),
    ('Rachel', 'Kim', 'Brakes & Suspension', 88.50, '2023-06-01'),
    ('Devon', 'Carter', 'Electrical', 98.00, '2021-11-10'),
    ('Sam', 'Ortiz', 'General Maintenance', 76.00, '2025-02-18');

-- #14 Insert sample services
INSERT INTO garage.Services (ServiceName, Description, StandardPrice, EstimatedHours)
VALUES
    ('Oil Change', 'Synthetic oil and filter change', 79.99, 0.75),
    ('Brake Pad Replacement', 'Front brake pads and rotor check', 249.50, 1.50),
    ('Engine Diagnostic', 'Check engine light scan', 120.00, 1.00),
    ('Battery Replacement', 'Battery test and replacement', 165.00, 0.50);

-- #15 Insert sample work orders
INSERT INTO garage.WorkOrders (VehicleID, MechanicID, ServiceID, ServiceDate, Status, TotalCost, Notes)
VALUES
    (1, 1, 1, '2026-10-12', 'Completed', 79.99, 'Oil change done'),
    (2, 2, 2, '2026-10-18', 'Completed', 264.50, 'Replaced front brake pads'),
    (3, 1, 3, '2026-11-01', 'Completed', 145.00, 'Check engine light'),
    (4, 3, 4, '2026-11-08', 'Completed', 165.00, 'New battery'),
    (1, 2, 3, '2026-11-15', 'In Progress', 120.00, 'Check engine light follow up');
GO
