USE FStore

-- Roles
INSERT INTO Roles (RoleID, Name) VALUES
(1, N'Admin'),
(2, N'Shop Manager'),
(3, N'Warehouse Manager'),
(4, N'Order Manager');

-- Employees
INSERT INTO Employees (FullName, Birthday, Password, PhoneNumber, Email, Gender, CreatedDate, [Status], RoleID) VALUES
(N'John Doe', '1990-05-14', '36fdba5968850579c0a89444f4ca4772', '0987654321', 'nguyenvana@example.com', 'Male', '2024-03-01', 'Active', 1),
( N'Jane Smith', '1992-08-21', '36fdba5968850579c0a89444f4ca4772', '0977123456', 'nguyenvanb@example.com', 'Female', '2024-03-01', 'Active', 2),
( N'Michael Brown', '1985-12-30', '36fdba5968850579c0a89444f4ca4772', '0909333222', 'nguyenvanc@example.com', 'Male', '2024-03-01', 'Active', 3),
( N'Emily White', '1995-07-11', '36fdba5968850579c0a89444f4ca4772', '0911222333', 'nguyenvand@example.com', 'Female', '2024-03-01', 'Inactive', 4);

-- Customers
INSERT INTO Customers ( FullName, Birthday, Password, PhoneNumber, Email, Gender, CreatedDate, IsBlock, IsDeleted) VALUES
( N'Alex Johnson', '1993-03-10', 'hashed_pwd', '0934567890', 'alex@example.com', 'Male', '2024-03-01', 0, 0),
( N'Sophia Carter', '1998-07-22', 'hashed_pwd', '0922345678', 'sophia@example.com', 'Female', '2024-03-01', 0, 0),
( N'Liam Wilson', '2000-11-15', 'hashed_pwd', '0911122334', 'liam@example.com', 'Male', '2024-03-01', 0, 0);

-- Suppliers
INSERT INTO Suppliers (SupplierID, TaxID, Name, Email, PhoneNumber, Address, CreatedDate, LastModify, DeletedDate, IsActivate, IsDeleted) VALUES
(1, 'TAX12345', N'Ventrue Co.', 'contact@ventrue.com', '0988888888', N'123 Main St', '2024-03-01', NULL, NULL, 1, 0),
(2, 'TAX67890', N'TechWorld Ltd.', 'info@techworld.com', '0977777777', N'456 Tech Park', '2024-03-01', NULL, NULL, 1, 0);

-- Categories
INSERT INTO Categories (Name) VALUES
('Smartphones'),
('Laptops'),
('Accessories');

-- Brands
INSERT INTO Brands (Name) VALUES
('Apple'),
('Samsung'),
('Asus');

-- Products
INSERT INTO Products (BrandID, CategoryID, Model, FullName, Description, IsDeleted, Price, Stock) VALUES
(1, 1, 'iPhone 15', N'Apple iPhone 15', N'Latest Apple smartphone', 0, 25000000, 50),
( 2, 1, 'Galaxy S23', N'Samsung Galaxy S23', N'Flagship Samsung smartphone', 0, 20000000, 40),
( 3, 2, 'ZenBook 14', N'Asus ZenBook 14', N'Ultrabook with OLED display', 0, 30000000, 20);

-- ImportOrders
INSERT INTO ImportOrders ( EmployeeID, SupplierID, ImportDate, TotalCost, Completed) VALUES
( 3, 1, '2024-03-01', 50000000, 1),
( 3, 2, '2024-03-02', 70000000, 1);

-- ImportOrderDetails
INSERT INTO ImportOrderDetails ( IOID, ProductID, Quantity, ImportPrice) VALUES
(1, 1, 10, 23000000),
(1, 2, 15, 18000000),
(1, 3, 8, 28000000);


-- OrderStatus
INSERT INTO OrderStatus (ID, Status) VALUES
(1, N'Pending'),
(2, N'Processing'),
(3, N'Completed'),
(4, N'Cancelled');

-- Orders
INSERT INTO Orders (CustomerID, FullName, Address, PhoneNumber, OrderedDate, DeliveredDate, Status, TotalAmount) VALUES
( 1, N'Alex Johnson', N'123 Elm Street', '0934567890', '2024-03-01', NULL, 1, 25000000),
( 2, N'Sophia Carter', N'456 Oak Avenue', '0922345678', '2024-03-02', NULL, 1, 20000000);

-- OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price) VALUES
(1, 1, 1, 25000000),
(1, 2, 1, 20000000);


