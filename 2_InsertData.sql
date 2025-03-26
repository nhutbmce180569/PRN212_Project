USE FStore

-- Roles
INSERT INTO Roles (RoleID, Name) VALUES
(1, N'Admin'),
(2, N'Shop Manager'),
(3, N'Warehouse Manager'),
(4, N'Order Manager');

-- Employees
INSERT INTO Employees (FullName, Birthday, Password, PhoneNumber, Email, Gender, CreatedDate, [Status], RoleID) VALUES
(N'Pham Vu Thanh Nguyen', '1990-05-14', '36fdba5968850579c0a89444f4ca4772', '0987654321', 'nguyenvana@example.com', 'Male', '2024-03-01', 'Active', 1),
( N'Ly Thi Kieu Thy', '1992-08-21', '36fdba5968850579c0a89444f4ca4772', '0977123456', 'nguyenvanb@example.com', 'Female', '2024-03-01', 'Active', 2),
( N'Bui Minh Nhut', '1992-08-21', '36fdba5968850579c0a89444f4ca4772', '0977123456', 'buiminhnhut@example.com', 'Female', '2024-03-01', 'Active', 2),
( N'Pham Thanh Tu', '1985-12-30', '36fdba5968850579c0a89444f4ca4772', '0909333222', 'nguyenvanc@example.com', 'Male', '2024-03-01', 'Active', 3),
( N'Bui Trung Kien', '1995-07-11', '36fdba5968850579c0a89444f4ca4772', '0911222333', 'nguyenvand@example.com', 'Female', '2024-03-01', 'Active', 4);

-- Customers
INSERT INTO Customers ( FullName, Birthday, Password, PhoneNumber, Email, Gender, CreatedDate, IsBlock, IsDeleted) VALUES
( N'Alex Johnson', '1993-03-10', '36fdba5968850579c0a89444f4ca4772', '0934567890', 'alex@example.com', 'Male', '2024-03-01', 0, 0),
( N'Sophia Carter', '1998-07-22', '36fdba5968850579c0a89444f4ca4772', '0922345678', 'sophia@example.com', 'Female', '2024-03-01', 0, 0),
( N'Liam Wilson', '2000-11-15', '36fdba5968850579c0a89444f4ca4772', '0911122334', 'liam@example.com', 'Male', '2024-03-01', 0, 0),
( N'Nguyễn Văn An', '1990-05-15', '36fdba5968850579c0a89444f4ca4772', '0901234567', 'an.nguyen@example.com', 'Male', '2024-03-02', 0, 0),
( N'Trần Thị Bình', '1995-08-20', '36fdba5968850579c0a89444f4ca4772', '0987654321', 'binh.tran@example.com', 'Female', '2024-03-02', 0, 0),
( N'Lê Minh Cường', '1988-12-01', '36fdba5968850579c0a89444f4ca4772', '0933445566', 'cuong.le@example.com', 'Male', '2024-03-03', 0, 0),
( N'Hoàng Thu Dung', '1992-04-10', '36fdba5968850579c0a89444f4ca4772', '0977889900', 'dung.hoang@example.com', 'Female', '2024-03-03', 0, 0),
( N'Phạm Xuân Đức', '1997-09-25', '36fdba5968850579c0a89444f4ca4772', '0966554433', 'duc.pham@example.com', 'Male', '2024-03-04', 0, 0),
( N'Vũ Ngọc Em', '1999-06-30', '36fdba5968850579c0a89444f4ca4772', '0955443322', 'em.vu@example.com', 'Female', '2024-03-04', 0, 0),
( N'Đỗ Thành Giang', '1991-01-05', '36fdba5968850579c0a89444f4ca4772', '0944332211', 'giang.do@example.com', 'Male', '2024-03-05', 0, 0);

-- Suppliers
SET IDENTITY_INSERT Suppliers ON
INSERT INTO Suppliers (SupplierID, TaxID, Name, Email, PhoneNumber, Address, CreatedDate, LastModify, DeletedDate, IsActivate, IsDeleted) VALUES
(1, '0152345871', N'Ventrue Co.', 'contact@ventrue.com', '0988888888', N'123 Main St', '2024-03-01', NULL, NULL, 1, 0),
(2, '0156329874', N'TechWorld Ltd.', 'info@techworld.com', '0977777777', N'456 Tech Park', '2024-03-01', NULL, NULL, 1, 0),
(3, '0159876543', N'Global Electronics', 'sales@global.com', '0966666666', N'789 Innovation Ave', '2024-03-02', NULL, NULL, 1, 0),
(4, '0151234789', N'Smart Devices Inc.', 'support@smart.com', '0955555555', N'1011 Future Blvd', '2024-03-02', NULL, NULL, 1, 0),
(5, '0154567890', N'Digital Solutions', 'order@digital.com', '0944444444', N'1213 Digital Lane', '2024-03-03', NULL, NULL, 1, 0),
(6, '0157890123', N'Electro World', 'contact@electro.com', '0933333333', N'1415 Power Rd', '2024-03-03', NULL, NULL, 1, 0),
(7, '0150123456', N'Modern Tech', 'info@modern.com', '0922222222', N'1617 Modern St', '2024-03-04', NULL, NULL, 1, 0),
(8, '0153456789', N'Advanced Systems', 'sales@advanced.com', '0911111111', N'1819 Advanced Ave', '2024-03-04', NULL, NULL, 1, 0),
(9, '0156789012', N'Future Gadgets', 'support@future.com', '0999999999', N'2021 Future Blvd', '2024-03-05', NULL, NULL, 1, 0),
(10, '0159012345', N'Innovative Tech', 'order@innovative.com', '0987654321', N'2223 Innovative Lane', '2024-03-05', NULL, NULL, 1, 0);
SET IDENTITY_INSERT Suppliers OFF

-- Categories
INSERT INTO Categories (Name) VALUES
('Smartphones'),
('Laptops'),
('Accessories'),
('Tablets'),
('Smartwatches'),
('Headphones'),
('Cameras'),
('Printers'),
('Routers'),
('Storage Devices');

-- Brands
INSERT INTO Brands (Name) VALUES
('Apple'),
('Samsung'),
('Asus'),
('Dell'),
('HP'),
('Sony'),
('Canon'),
('Logitech'),
('TP-Link'),
('Seagate');

-- Products
SET IDENTITY_INSERT Products ON
INSERT INTO Products (ProductID, BrandID, CategoryID, Model, FullName, Description, IsDeleted, Price, Stock) VALUES
(1, 1, 1, 'iPhone 15', N'Apple iPhone 15', N'Latest Apple smartphone', 0, 25000000, 10),
(2, 2, 1, 'Galaxy S23', N'Samsung Galaxy S23', N'Flagship Samsung smartphone', 0, 20000000, 15),
(3, 3, 2, 'ZenBook 14', N'Asus ZenBook 14', N'Ultrabook with OLED display', 0, 30000000, 5),
(4, 4, 2, 'Inspiron 15', N'Dell Inspiron 15', N'Affordable Dell laptop', 0, 18000000, 20),
(5, 5, 2, 'Pavilion x360', N'HP Pavilion x360', N'Convertible laptop with touchscreen', 0, 22000000, 8),
(6, 6, 3, 'WH-1000XM5', N'Sony WH-1000XM5', N'Noise-cancelling headphones', 0, 8000000, 30),
(7, 7, 7, 'EOS R6 Mark II', N'Canon EOS R6 Mark II', N'Full-frame mirrorless camera', 0, 60000000, 3),
(8, 8, 3, 'MX Master 3S', N'Logitech MX Master 3S', N'Wireless mouse for productivity', 0, 3000000, 25),
(9, 9, 9, 'Archer AX55', N'TP-Link Archer AX55', N'Wi-Fi 6 router', 0, 2500000, 12),
(10, 10, 10, 'Backup Plus', N'Seagate Backup Plus', N'External hard drive', 0, 1500000, 40);
SET IDENTITY_INSERT Products OFF

-- ImportOrders
SET IDENTITY_INSERT ImportOrders ON
INSERT INTO ImportOrders (IOID, EmployeeID, SupplierID, ImportDate, TotalCost, Completed) VALUES
( 1, 3, 1, '2024-03-01', 50000000, 1),
( 2, 3, 2, '2024-03-02', 270000000, 1),
( 3, 3, 2, '2024-03-02', 60000000, 1),
( 4, 3, 3, '2024-03-03', 180000000, 1),
( 5, 3, 4, '2024-03-04', 110000000, 1),
( 6, 3, 5, '2024-03-05', 240000000, 1),
( 7, 3, 6, '2024-03-06', 80000000, 1),
( 8, 3, 7, '2024-03-07', 30000000, 1),
( 9, 3, 8, '2024-03-08', 25000000, 1),
( 10, 3, 9, '2024-03-09', 15000000, 1);
SET IDENTITY_INSERT ImportOrders OFF

-- ImportOrderDetails
INSERT INTO ImportOrderDetails ( IOID, ProductID, Quantity, ImportPrice) VALUES
(1, 1, 10, 23000000),
(2, 2, 15, 18000000),
(3, 3, 5, 12000000),
(4, 4, 10, 16000000),
(5, 5, 5, 20000000),
(6, 6, 30, 7000000),
(7, 7, 3, 55000000),
(8, 8, 25, 2800000),
(9, 9, 12, 2300000),
(10, 10, 40, 1400000);

-- OrderStatus
INSERT INTO OrderStatus (ID, Status) VALUES
(1, N'Pending'),
(2, N'Processing'),
(3, N'Completed'),
(4, N'Cancelled');

-- Orders
INSERT INTO Orders (CustomerID, FullName, Address, PhoneNumber, OrderedDate, DeliveredDate, Status, TotalAmount) VALUES
( 1, N'Alex Johnson', N'123 Elm Street', '0934567890', '2024-03-01', NULL, 1, 25000000),
( 2, N'Sophia Carter', N'456 Oak Avenue', '0922345678', '2024-03-02', NULL, 1, 20000000),
( 3, N'Nguyễn Văn An', N'789 Pine Lane', '0901234567', '2024-03-03', NULL, 2, 30000000),
( 4, N'Trần Thị Bình', N'1011 Maple Drive', '0987654321', '2024-03-04', NULL, 1, 18000000),
( 5, N'Lê Minh Cường', N'1213 Cedar Road', '0933445566', '2024-03-05', NULL, 3, 22000000),
( 6, N'Hoàng Thu Dung', N'1415 Birch Street', '0977889900', '2024-03-06', NULL, 1, 8000000),
( 7, N'Phạm Xuân Đức', N'1617 Willow Avenue', '0966554433', '2024-03-07', NULL, 2, 60000000),
( 8, N'Vũ Ngọc Em', N'1819 Redwood Lane', '0955443322', '2024-03-08', NULL, 3, 3000000),
( 9, N'Đỗ Thành Giang', N'2021 Oak Street', '0944332211', '2024-03-09', NULL, 1, 2500000),
( 10, N'Alex Johnson', N'2223 Elm Avenue', '0934567890', '2024-03-10', NULL, 2, 1500000);

-- OrderDetails
INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price) VALUES
(1, 1, 1, 25000000),
(1, 2, 1, 20000000),
(2, 1, 1, 25000000),
(3, 3, 1, 30000000),
(4, 4, 1, 18000000),
(5, 5, 1, 22000000),
(6, 6, 1, 8000000),
(7, 7, 1, 60000000),
(8, 8, 1, 3000000),
(9, 9, 1, 2500000),
(10, 10, 1, 1500000);
