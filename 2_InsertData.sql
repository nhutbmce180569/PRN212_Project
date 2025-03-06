USE FShop;

-- Insert Roles
INSERT INTO Roles ([Name]) VALUES 
('Admin'),
('Shop Manager'),
('Order Manager'),
('Warehouse Manager');

-- Insert Categories
INSERT INTO Categories ([Name]) 
VALUES 
('Laptop'), ('Smartphone'), ('Mouse'), ('Headphone'), ('Charger'), ('Charging Cable');

-- Insert Brands
INSERT INTO Brands ([Name]) 
VALUES 
('Apple'), ('Samsung'), ('Sony'), ('Huawei'), ('Xiaomi'),
('Dell'), ('HP'), ('Asus'), ('Lenovo'), ('Acer'),
('MSI'), ('Microsoft'), ('Google'), ('LG'), ('Razer');

-- Insert Suppliers
SET IDENTITY_INSERT Suppliers ON;
INSERT INTO Suppliers (SupplierID, TaxID, [Name], Email, PhoneNumber, Address, CreatedDate, LastModify, IsDeleted, IsActivate) 
VALUES 
(1, '0100101234', 'TechGear Solutions', 'contact@techgear.com', '0901234567', '123 Tech Street, District 1, Ho Chi Minh City', GETDATE(), GETDATE(), 0, 1),
(2, '0200101235', 'GadgetWorld', 'info@gadgetworld.vn', '0912345678', '456 Innovation Blvd, District 3, Ho Chi Minh City', GETDATE(), GETDATE(), 0, 1),
(3, '0300101236', 'DigitalZone', 'support@digitalzone.vn', '0923456789', '789 Digital Avenue, District 5, Ho Chi Minh City', GETDATE(), GETDATE(), 0, 1),
(4, '0400101237', 'SmartTech Supplies', 'sales@smarttech.vn', '0934567890', '101 Future Drive, District 7, Ho Chi Minh City', GETDATE(), GETDATE(), 0, 0),
(5, '0500101238', 'NextGen Electronics', 'hello@nextgen.vn', '0945678901', '202 Silicon Valley, District 2, Ho Chi Minh City', GETDATE(), GETDATE(), 0, 0);
SET IDENTITY_INSERT Suppliers OFF;

-- Insert Attributes
-- Insert Attributes
INSERT INTO [Attributes] ([CategoryID], [Name]) VALUES

-- ======== Dùng chung cho Laptop & Điện thoại ========
(1, 'GPU'),                        
(1, 'CPU Speed'),                  
(1, 'Display Technology'),         
(1, 'Screen Resolution'),          
(1, 'Screen Size'),                
(1, 'Touchscreen Glass'),          
(1, 'Battery Capacity'),           
(1, 'Battery Technology'),         
(1, 'Charging Port'),              
(1, 'Bluetooth'),                  
(1, 'Design'),                     
(1, 'Material'),                   
(1, 'Size & Weight'),              

(2, 'GPU'),                        
(2, 'CPU Speed'),                  
(2, 'Display Technology'),         
(2, 'Screen Resolution'),          
(2, 'Screen Size'),                
(2, 'Touchscreen Glass'),          
(2, 'Battery Capacity'),           
(2, 'Battery Technology'),         
(2, 'Charging Port'),              
(2, 'Bluetooth'),                  
(2, 'Design'),                     
(2, 'Material'),                   
(2, 'Size & Weight'),              

-- ======== Laptop ========
(1, 'Keyboard Type'),              
(1, 'LED Backlight'),              
(1, 'Mouse & Keyboard Connection Type'), 

-- ======== Điện thoại ========
(2, 'Mobile Network'),             
(2, 'SIM'),                        
(2, 'Rear Camera Resolution'),     
(2, 'Front Camera Resolution'),    
(2, 'Max Charging Support'),       
(2, 'Advanced Security'),                             

-- ======== Tai nghe ========
(4, 'Headphone Type'),             
(4, 'Audio Technology'),           
(4, 'Connectivity'),               
(4, 'Battery Capacity'),           
(4, 'Usage Time'),                 
(4, 'Charging Time'),              

-- ======== Sạc & Cáp ========
(5, 'Charger Type'),               
(5, 'Charging Power (W)'),         
(5, 'Fast Charging Standard'),     

(6, 'Connector Type'),             
(6, 'Cable Length'),               
(6, 'Cable Material'),             

-- ======== Chuột ========
(3, 'Mouse Type'),                 
(3, 'RGB Lighting');              

-- Insert Products
SET IDENTITY_INSERT Products ON;

INSERT INTO Products (ProductID, BrandID, CategoryID, Model, FullName, [Description], [Image], Price, IsDeleted, Stock) VALUES 
(1, 1, 1, 'Macbook Pro 14', 'Apple Macbook Pro 14 inch M1 Pro chip', 'New model from Apple', '250-7038-macbook-pro-2021-apple-m1-1.jpg', 50000000, 0, 50),
(2, 2, 2, 'Galaxy S23 Ultra', 'Samsung Galaxy S23 Ultra 5G 256GB', 'Latest flagship from Samsung', 'samsung-galaxy-s23-xanh-600x600-1.jpg', 30000000, 1, 50),
(3, 3, 1, 'Sony Vaio Z900', 'Sony Vaio Z900 Core i7 16GB RAM', 'High-performance laptop', 'vaio-z900.jpg', 20000000, 0, 50),
(4, 8, 1, 'ASUS-FA706', 'ASUS TUF Gaming A17', 'Durable gaming laptop with high performance', '250-8562-line-laptop.png', 28909000, 1, 50),
(5, 1, 2, 'iPhone-16-Pro-Max', 'iPhone 16 Pro Max', 'Premium smartphone with advanced camera and powerful performance', 'iphone-16-pro-max-black-thumb-600x600.jpg', 32529000, 0, 50),
(6, 2, 2, 'Galaxy-S23', 'Samsung Galaxy S23', 'Samsung flagship phone with stunning design and excellent camera', 'samsung-galaxy-s23-xanh-600x600-1.jpg', 25899000, 1, 50),
(7, 9, 1, 'Legion-Pro-5', 'Lenovo Legion Pro 5', 'High-end gaming laptop with powerful specs and 240Hz display', '0yp3jx9d-1090-lenovo-legion-pro-5-y9000p-2023-core-i9-13900hx-16gb-1tb-rtx-4050-6gb-16-wqxga-240hz-new.jpg', 38909000, 0, 50),
(8, 8, 1, 'ASUS-TUF-A17', 'ASUS TUF Gaming A17', 'Durable gaming laptop with strong performance and modern design', 'ASUS-TUF-Gaming-A17-FA706-600x600.jpg', 31909000, 1, 50),
(9, 11, 1, 'MSI-Katana-A15', 'MSI Katana Gaming A15', 'Gaming laptop powered by AMD Ryzen 9 and RTX 4060 GPU', '5e0dkkrb-1411-msi-katana-gaming-a15-ai-b8vf-406ca-amd-ryzen-r9-8945hs-32gb-1tb-rtx-4060-8gb-15-6-144hz-fhd-new.jpg', 33909000, 1, 50),
(10, 6, 1, 'Dell-G5511', 'Dell Gaming G5511', 'Gaming laptop with strong design and high performance', '45606_dell_gaming_5511_dark_grey_ha3.jpg', 35909000, 0, 50);


SET IDENTITY_INSERT Products OFF;

-- Insert AttributeDetails
INSERT INTO AttributeDetails (AttributeID, ProductID, AttributeInfor)
VALUES
-- MacBook Pro 14
(1, 1, 'Apple M1 Pro GPU'),
(2, 1, '3.2 GHz'),
(3, 1, 'Liquid Retina XDR'),
(4, 1, '3024 x 1964'),
(5, 1, '14 inch'),
(6, 1, 'No'),
(7, 1, '70Wh'),
(8, 1, 'Li-Po'),
(9, 1, 'USB-C (Thunderbolt 4)'),
(10, 1, '5.0'),
(11, 1, 'Slim, thin bezels'),
(12, 1, 'CNC Aluminum'),
(13, 1, '31.26 x 22.12 x 1.55 cm, 1.6 kg'),
(14, 1, 'Magic Keyboard'),
(15, 1, 'Yes'),
(16, 1, 'Bluetooth'),
-- Sony Vaio Z900
(1, 3, 'Intel Iris Xe'),
(2, 3, '3.1 GHz'),
(3, 3, 'IPS'),
(4, 3, '1920 x 1080'),
(5, 3, '14 inch'),
(6, 3, 'No'),
(7, 3, '53Wh'),
(8, 3, 'Li-ion'),
(9, 3, 'USB-C'),
(10, 3, '5.1'),
(11, 3, 'Ultra-thin, premium'),
(12, 3, 'Carbon fiber'),
(13, 3, '32.0 x 21.5 x 1.6 cm, 1.2 kg'),
(14, 3, 'Chiclet'),
(15, 3, 'Yes'),
(16, 3, 'Bluetooth'),
-- ASUS TUF Gaming A17
(1, 4, 'RTX 3050 4GB'),
(2, 4, '4.2 GHz'),
(3, 4, 'IPS'),
(4, 4, '1920 x 1080'),
(5, 4, '17.3 inch'),
(6, 4, 'No'),
(7, 4, '90Wh'),
(8, 4, 'Li-ion'),
(9, 4, 'USB-C, DC-in'),
(10, 4, '5.2'),
(11, 4, 'Powerful gaming'),
(12, 4, 'Hard plastic & metal'),
(13, 4, '39.9 x 26.8 x 2.5 cm, 2.6 kg'),
(14, 4, 'RGB Backlit'),
(15, 4, 'Yes'),
(16, 4, 'USB, Bluetooth'),
-- Lenovo Legion Pro 5
(1, 7, 'RTX 4050 6GB'),
(2, 7, '4.5 GHz'),
(3, 7, 'IPS'),
(4, 7, '2560 x 1600'),
(5, 7, '16 inch'),
(6, 7, 'No'),
(7, 7, '80Wh'),
(8, 7, 'Li-ion'),
(9, 7, 'USB-C, DC-in'),
(10, 7, '5.2'),
(11, 7, 'Powerful gaming'),
(12, 7, 'Aluminum & hard plastic'),
(13, 7, '35.9 x 26.5 x 2.6 cm, 2.5 kg'),
(14, 7, 'RGB Backlit'),
(15, 7, 'Yes'),
(16, 7, 'USB, Bluetooth'),
-- Samsung Galaxy S23 Ultra
(1, 2, 'Adreno 740'),
(2, 2, '3.36 GHz'),
(3, 2, 'Dynamic AMOLED 2X'),
(4, 2, '3088 x 1440'),
(5, 2, '6.8 inch'),
(6, 2, 'Gorilla Glass Victus 2'),
(7, 2, '5000mAh'),
(8, 2, 'Li-ion'),
(9, 2, 'USB-C'),
(10, 2, '5.3'),
(11, 2, 'Glass back, aluminum frame'),
(12, 2, 'Armor aluminum + tempered glass'),
(13, 2, '163.4 x 78.1 x 8.9 mm, 234g'),
(17, 2, '5G'),
(18, 2, 'Dual SIM (Nano-SIM + eSIM)'),
(19, 2, '200MP + 12MP + 10MP + 10MP'),
(20, 2, '12MP'),
(21, 2, '45W'),
(22, 2, 'Ultrasonic fingerprint'),
-- Galaxy S23
(1, 6, 'Adreno 740'),
(2, 6, '3.36 GHz'),
(3, 6, 'Dynamic AMOLED 2X'),
(4, 6, '2340 x 1080'),
(5, 6, '6.1 inch'),
(7, 6, '3900mAh'),
(8, 6, 'Li-ion'),
(9, 6, 'USB-C'),
(10, 6, '5.3'),
(19, 6, '50MP + 10MP + 12MP'),
-- ASUS TUF A17
(1, 8, 'RTX 4060 8GB'),
(2, 8, '4.2 GHz'),
(3, 8, 'IPS'),
(4, 8, '1920 x 1080'),
(5, 8, '17.3 inch'),
(7, 8, '90Wh'),
(8, 8, 'Li-ion'),
-- MSI Katana A15
(1, 9, 'RTX 4060 8GB'),
(2, 9, '4.5 GHz'),
(3, 9, 'IPS'),
(4, 9, '1920 x 1080'),
(5, 9, '15.6 inch'),
(7, 9, '90Wh'),
(8, 9, 'Li-ion'),
-- Dell G5511
(1, 10, 'RTX 3060 6GB'),
(2, 10, '4.6 GHz'),
(3, 10, 'IPS'),
(4, 10, '1920 x 1080'),
(5, 10, '15.6 inch'),
(7, 10, '86Wh'),
(8, 10, 'Li-ion');

-- Insert Customers
INSERT INTO Customers (FullName, Birthday, [Password], PhoneNumber, Email, Gender, CreatedDate, IsBlock, IsDeleted, Avatar)
VALUES 
('Nguyen Van A', '1995-05-15', '6ad14ba9986e3615423dfca256d04e3f', '0901234567', 'nguyenvana@example.com', 'Male', GETDATE(), 0, 0, 'avatar1.jpg');

-- Insert OrderStatus
INSERT INTO OrderStatus (ID, [Status])
VALUES 
(1, 'Waiting for acceptance'),
(2, 'Packaging'),
(3, 'Waiting for delivery'),
(4, 'Delivered'),
(5, 'Canceled');

-- Insert Orders
INSERT INTO Orders (CustomerID, FullName, [Address], PhoneNumber, OrderedDate, DeliveredDate, Status, TotalAmount)
VALUES 
(1, 'Nguyen Van A', '123 Tech Street, District 1, Ho Chi Minh City', '0901234567', GETDATE(), NULL, 1, 50000000);

INSERT INTO OrderDetails VALUES (1, 1, 3, 50000000);


-- Insert Employees
SET IDENTITY_INSERT Employees ON;

INSERT INTO Employees (EmployeeID, FullName, Birthday, [Password], PhoneNumber, Email, Gender, [Status], CreatedDate, Avatar, RoleID) VALUES
(1, 'Nguyen Van A', '1990-01-01', '36fdba5968850579c0a89444f4ca4772', '0123456789', 'nguyenvana@example.com', 'Male', 1, GETDATE(), '', 1), -- Password là: User123@
(2, 'Nguyen Van B', '1990-01-01', '36fdba5968850579c0a89444f4ca4772', '0123456788', 'nguyenvanb@example.com', 'Male', 1, GETDATE(), '', 2), -- Password là: User123@
(3, 'Nguyen Van C', '1990-01-01', '36fdba5968850579c0a89444f4ca4772', '0123456789', 'nguyenvanc@example.com', 'Male', 1, GETDATE(), '', 3), -- Password là: User123@
(4, 'Nguyen Van D', '1990-01-01', '36fdba5968850579c0a89444f4ca4772', '0123456789', 'nguyenvand@example.com', 'Male', 1, GETDATE(), '', 4); -- Password là: User123@

SET IDENTITY_INSERT Employees OFF;

-- Insert Carts
INSERT INTO Carts (CustomerID, ProductID, Quantity)
VALUES 
(1, 1, 1),
(1, 2, 5);



-- Insert Addresses
INSERT INTO Addresses (CustomerID, AddressDetails, IsDefault)
VALUES 
(1, 'Ap Tra Coi A, My Huong, My Tu, Soc Trang', 1);

-- Insert ImportOrders
INSERT INTO ImportOrders (EmployeeID, SupplierID, ImportDate, TotalCost, Completed)
VALUES
(2, 1, GETDATE(), 100000000, 1),
(2, 2, GETDATE(), 50000000, 1);

-- Insert ImportOrderDetails
INSERT INTO ImportOrderDetails (IOID, ProductID, Quantity, ImportPrice)
VALUES
(1, 1, 10, 45000000),
(2, 2, 20, 28000000),
(2, 3, 15, 18000000);
