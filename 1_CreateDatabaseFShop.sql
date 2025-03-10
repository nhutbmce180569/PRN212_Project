USE [master]
GO

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT Name FROM master.dbo.sysdatabases WHERE Name = N'FStore')
BEGIN
	ALTER DATABASE FStore SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE FStore SET ONLINE;
	DROP DATABASE FStore;
END

GO

CREATE DATABASE FStore
GO

USE FStore
GO

/*******************************************************************************
   Use camelCase for Name of attribute. Ex: FullDateOfCreateAccount
   ConsIDer use prefix for attribute Name of each TABLE. Ex: TABLE Accounts have (a_ID, a_FullName, a_email,...)
********************************************************************************/

-- DROP TABLE IF EXISTS Wards;
-- DROP TABLE IF EXISTS Districts;
-- DROP TABLE IF EXISTS Provinces;
-- DROP TABLE IF EXISTS AdministrativeUnits;
-- DROP TABLE IF EXISTS AdministrativeRegions;


-----------------------------------------------------------------------------------

CREATE TABLE Roles (
    RoleID INT PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL
);

CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1, 1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    Birthday DATE,
    Password VARCHAR(500) NOT NULL,
    PhoneNumber VARCHAR(15),
    Email VARCHAR(254),
    Gender CHAR(6),
    CreatedDate DATETIME,
    [Status] VARCHAR(20),
    RoleID INT,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

CREATE TABLE Customers (
    CustomerID INT IDENTITY(1, 1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    Birthday DATE,
    Password VARCHAR(500) NOT NULL,
    PhoneNumber VARCHAR(15),
    Email VARCHAR(254),
    Gender CHAR(6),
    CreatedDate DATETIME,
    IsBlock BIT DEFAULT 0,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY,
    TaxID VARCHAR(20),
    Name NVARCHAR(255) NOT NULL,
    Email VARCHAR(254),
    PhoneNumber VARCHAR(15),
    Address NVARCHAR(255),
    CreatedDate DATETIME,
    LastModify DATETIME,
    DeletedDate DATETIME,
    IsActivate BIT DEFAULT 1,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE Brands (
    BrandID INT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE Products (
    ProductID INT IDENTITY(1, 1) PRIMARY KEY,
    BrandID INT,
    CategoryID INT,
    Model VARCHAR(50),
    FullName NVARCHAR(255),
    Description TEXT,
    IsDeleted BIT DEFAULT 0,
    Price BIGINT,
    Stock INT,
    FOREIGN KEY (BrandID) REFERENCES Brands(BrandID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

CREATE TABLE ImportOrders (
    IOID INT IDENTITY(1, 1) PRIMARY KEY,
    EmployeeID INT,
    SupplierID INT,
    ImportDate DATETIME,
    TotalCost BIGINT,
    Completed BIT DEFAULT 0,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

CREATE TABLE ImportOrderDetails (
    IOID INT,
    ProductID INT,
    Quantity INT,
    ImportPrice BIGINT,
    PRIMARY KEY (IOID, ProductID),
    FOREIGN KEY (IOID) REFERENCES ImportOrders(IOID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


CREATE TABLE OrderStatus (
    ID INT PRIMARY KEY,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE Orders (
    OrderID INT IDENTITY(1, 1) PRIMARY KEY,
    CustomerID INT,
    FullName NVARCHAR(100) NOT NULL,
    Address NTEXT NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    OrderedDate DATETIME NOT NULL,
    DeliveredDate DATETIME,
    Status INT,
    TotalAmount BIGINT,
	FOREIGN KEY (Status) REFERENCES OrderStatus(ID),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE OrderDetails (
    OrderID INT,
    ProductID INT,
    Quantity INT,
    Price BIGINT,
    PRIMARY KEY (OrderID, ProductID),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);


/*******************************************************************************
   Schema for UI/UX Testing
********************************************************************************/