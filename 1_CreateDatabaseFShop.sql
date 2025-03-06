USE [master]
GO

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT Name FROM master.dbo.sysdatabases WHERE Name = N'FShop')
BEGIN
	ALTER DATABASE FShop SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE FShop SET ONLINE;
	DROP DATABASE FShop;
END

GO

CREATE DATABASE FShop
GO

USE FShop
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
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL
);

CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(255) NOT NULL,
    Birthday DATE,
    [Password] VARCHAR(500) NOT NULL,
    PhoneNumber VARCHAR(15),
    Email VARCHAR(254),
    Gender CHAR(6),
    CreatedDate DATE,
	[Status] BIT,
    Avatar TEXT,
    RoleID INT,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(255) NOT NULL,
    Birthday DATE,
    [Password] VARCHAR(500) NOT NULL,
    PhoneNumber VARCHAR(15),
    Email VARCHAR(254),
    Gender CHAR(6),
    CreatedDate DATETIME,
    IsBlock BIT,
    IsDeleted BIT,
    Avatar TEXT
);

CREATE TABLE Addresses (
    AddressID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    AddressDetails NTEXT,
	IsDefault BIT,	
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    TaxID VARCHAR(20) UNIQUE,
    [Name] NVARCHAR(255) UNIQUE NOT NULL,
    Email VARCHAR(254) UNIQUE,
    PhoneNumber VARCHAR(15) UNIQUE,
    [Address] VARCHAR(255) UNIQUE,
    CreatedDate DATETIME,
    LastModify DATETIME,
    IsDeleted BIT,
	IsActivate BIT
);

CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL
);

CREATE TABLE Brands (
    BrandID INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    BrandID INT,
    CategoryID INT,
    Model NVARCHAR(50),
    FullName VARCHAR(255),
    [Description] TEXT,
	IsDeleted BIT,
	Price BIGINT,
    [Image] TEXT,
	[Image1] TEXT,
	[Image2] TEXT,
	[Image3] TEXT,
    Quantity INT,
	Stock INT
    FOREIGN KEY (BrandID) REFERENCES Brands(BrandID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

CREATE TABLE Attributes (
    AttributeID INT PRIMARY KEY IDENTITY(1,1),
	CategoryID INT,
	FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    [Name] NVARCHAR(100) NOT NULL
);

CREATE TABLE AttributeDetails (
    AttributeID INT,
    ProductID INT,
    AttributeInfor VARCHAR(100),
    PRIMARY KEY (AttributeID, ProductID),
    FOREIGN KEY (AttributeID) REFERENCES Attributes(AttributeID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE OrderStatus (
    ID INT PRIMARY KEY,
    [Status]NVARCHAR(50) NOT NULL
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT,
    FullName VARCHAR(100) NOT NULL,
    [Address] NTEXT NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    OrderedDate DATETIME NOT NULL,
    DeliveredDate DATETIME,
    [Status]INT,
    TotalAmount BIGINT,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
	FOREIGN KEY ([Status]) REFERENCES OrderStatus(ID),
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


CREATE TABLE ImportOrders (
    IOID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT,
    SupplierID INT,
    ImportDate DATETIME,
    TotalCost BIGINT,
    Completed BIT,
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

CREATE TABLE ProductRatings (
    RateID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT,
    ProductID INT,
	OrderID INT,
    CreatedDate DATETIME,
    Star INT,
    Comment NVARCHAR(300),
    IsDeleted BIT,
    IsRead BIT,
	FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

CREATE TABLE RatingReplies (
    ReplyID INT IDENTITY (1,1) PRIMARY KEY,
    EmployeeID INT,
    RateID INT,
    Answer NVARCHAR(300),
    IsRead BIT,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID),
    FOREIGN KEY (RateID) REFERENCES ProductRatings(RateID)
);

CREATE TABLE Carts (
    CustomerID INT,
    ProductID INT,
    Quantity INT,
    PRIMARY KEY (CustomerID, ProductID),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

/*******************************************************************************
   Schema for UI/UX Testing
********************************************************************************/