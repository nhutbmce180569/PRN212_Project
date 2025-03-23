using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinalProject.Models;
using FinalProject.ViewModels.ShopManager;
using FinalProject.Views.ShopManager.Customer;
using Newtonsoft.Json;
using WPFLab.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFLab.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FinalProject.Views.OrderManager.Order;
using Microsoft.IdentityModel.Tokens;
using Avalonia.Controls;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.IO;







namespace FinalProject.ViewModels.OrderManager
{
    public class OrderViewModel : BaseViewModel
    {
        public ObservableCollection<Order> OrderList { get; set; }
        public ObservableCollection<OrderDetail> OrderDetailList { get; set; }
        private ObservableCollection<Order> AllOrderList { get; set; }
        private ObservableCollection<OrderDetail> AllOrderDetailList { get; set; }
        public ObservableCollection<Product> Options { get; set; }
        public ObservableCollection<Product> ProductList { get; set; }

        public ObservableCollection<Customer> OptionsCusID { get; set; }
        public ObservableCollection<Customer> CustomerList { get; set; }

        public ObservableCollection<OrderStatus> OrderStatusList { get; set; }



        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(nameof(SearchText)); }
        }

        public event Action OnOrderAdded;
        private OrderStatus _selectedOrderStatus;
        public OrderStatus SelectedOrderStatus
        {
            get => _selectedOrderStatus;
            set
            {
                _selectedOrderStatus = value;
                OnPropertyChanged(nameof(SelectedOrderStatus));
                if (_selectedOrderStatus != null)
                {
                    TextBoxItem.Status = _selectedOrderStatus.Id;
                    UpdateOrderStatusList();
                    OnPropertyChanged(nameof(TextBoxItem));
                }
            }
        }


        private void IncreaseStock(int productId, int quantity)
        {
            using (var context = new FstoreContext())
            {
                var product = context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    product.Stock += quantity;
                    context.SaveChanges();
                    //MessageBox.Show($"Stock for product {product.FullName} has been increased by {quantity}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DecreaseStock(int productId, int quantity)
        {
            using (var context = new FstoreContext())
            {
                var product = context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    if (product.Stock >= quantity)
                    {
                        product.Stock -= quantity;
                        context.SaveChanges();
                        //MessageBox.Show($"Stock for product {product.FullName} has been decreased by {quantity}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Not enough stock for product {product.FullName}. Available stock: {product.Stock}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Product not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public ObservableCollection<Product> SelectedProducts
        {
            get
            {
                if (SelectedOrderDetails != null && SelectedOrderDetails.Any())
                {

                    var products = (from detail in SelectedOrderDetails
                                    join prod in ProductList on detail.ProductId equals prod.ProductId
                                    select prod).Distinct();

                    return new ObservableCollection<Product>(products);
                }
                return new ObservableCollection<Product>();
            }
        }



        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
                if (_selectedProduct != null)
                {
                    TextBoxProduct.Price = _selectedProduct.Price;
                    TextBoxProduct.ProductId = _selectedProduct.ProductId;
                    OnPropertyChanged(nameof(TextBoxProduct));
                }
            }
        }
        private Product textBoxProduct = new Product();
        public Product TextBoxProduct
        {
            get { return textBoxProduct; }
            set
            {
                textBoxProduct = value;
                OnPropertyChanged(nameof(TextBoxProduct));
            }
        }


        private string _selectedOption;
        public string SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }



        private Order textBoxItem = new Order();
        public Order TextBoxItem
        {
            get { return textBoxItem; }
            set
            {
                textBoxItem = value;
                OnPropertyChanged(nameof(TextBoxItem));
            }
        }

        public ObservableCollection<OrderDetail> NewOrderDetails { get; set; } = new ObservableCollection<OrderDetail>();
        // Số lượng nhập cho sản phẩm mới
        private int? _newOrderDetailQuantity;
        public int? NewOrderDetailQuantity
        {
            get => _newOrderDetailQuantity;
            set { _newOrderDetailQuantity = value; OnPropertyChanged(nameof(NewOrderDetailQuantity)); }
        }

        private OrderDetail textBoxDetail = new OrderDetail();
        public OrderDetail TextBoxDetail
        {
            get { return textBoxDetail; }
            set
            {
                textBoxDetail = value;
                OnPropertyChanged(nameof(TextBoxDetail));
            }
        }


        private Order _selectedItem;
        public Order SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (_selectedItem != null)
                {



                    TextBoxItem = JsonConvert.DeserializeObject<Order>(JsonConvert.SerializeObject(_selectedItem, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));

                    //textBoxItem = JsonConvert.DeserializeObject<Order>(JsonConvert.SerializeObject(_selectedItem));

                    var orderDetail = OrderDetailList.FirstOrDefault(od => od.OrderId == _selectedItem.OrderId);
                    if (orderDetail != null)
                    {
                        TextBoxDetail = new OrderDetail
                        {
                            OrderId = orderDetail.OrderId,
                            ProductId = orderDetail.ProductId,
                            Quantity = orderDetail.Quantity,
                            Price = orderDetail.Price
                        };
                    }
                    else
                    {
                        TextBoxDetail = new OrderDetail();
                    }
                    Console.WriteLine(TextBoxItem.ToString());
                    Console.WriteLine($"SelectedItem Address: {TextBoxItem.Address}");
                    Console.WriteLine($"SelectedItem Quantity: {TextBoxDetail.Quantity}");
                    OnPropertyChanged(nameof(TextBoxDetail));
                    OnPropertyChanged(nameof(TextBoxItem));
                    OnPropertyChanged(nameof(SelectedOrderDetails));
                    OnPropertyChanged(nameof(SelectedProducts));

                }
            }
        }



        private OrderDetail _selectedOrderDetail;
        public OrderDetail SelectedOrderDetail
        {
            get => _selectedOrderDetail;
            set
            {
                _selectedOrderDetail = value;
                OnPropertyChanged(nameof(SelectedCustomer));


                if (_selectedOrderDetail != null)
                {
                    TextBoxDetail.Quantity = _selectedOrderDetail.Quantity;


                    OnPropertyChanged(nameof(TextBoxDetail));
                }
            }
        }

        public ObservableCollection<OrderDetail> SelectedOrderDetails
        {
            get
            {
                if (SelectedItem != null)
                {
                    var details = OrderDetailList.Where(od => od.OrderId == SelectedItem.OrderId);
                    return new ObservableCollection<OrderDetail>(details);
                }
                return new ObservableCollection<OrderDetail>();
            }
        }


        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));


                if (_selectedCustomer != null)
                {
                    TextBoxItem.CustomerId = _selectedCustomer.CustomerId;
                    TextBoxItem.FullName = _selectedCustomer.FullName;

                    TextBoxItem.PhoneNumber = _selectedCustomer.PhoneNumber;
                    TextBoxItem.Status = 1;
                    OnPropertyChanged(nameof(TextBoxItem));
                }
            }
        }


        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand OpenAddPopupCommand { get; }
        public ICommand OpenUpdatePopupCommand { get; }
        public ICommand SearchCommand { get; }             
        public ICommand ExportJsonCommand { get; }
        public ICommand AddProductDetailCommand { get; }
        public bool IsValidPhoneNumber(string phoneNumber)
        {

            return !string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length <= 15 && Regex.IsMatch(phoneNumber, @"^\d+$");
        }


        public OrderViewModel()
        {
            Load();
            AddProductDetailCommand = new RelayCommand(AddProductDetail);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
            SearchCommand = new RelayCommand(ExecuteSearch);
            ExportJsonCommand = new RelayCommand(ExportJson);
        }
        public OrderViewModel(object obj)
        {
            Load();
            if (obj is Order selectedOrder)
            {
                SelectedItem = selectedOrder;

            }
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);

        }
        private void OpenPopup(object obj)
        {
            var action = new OrderViewModel();
            action.OnOrderAdded += Load;
            var popup = new AddOrder
            {
                DataContext = action,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner

            };

            popup.Deactivated += (s, e) =>
            {
                if (popup.IsLoaded)
                {

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!popup.IsActive)
                        {
                            popup.Topmost = true;

                            //// Lấy lại focus cho MainWindow
                            //Application.Current.MainWindow.Activate();
                            //Application.Current.MainWindow.Opacity = 1;

                        }
                    }));
                }
            };
            Application.Current.Windows[0].Opacity = 0.5;
            popup.ShowDialog();
        }
        private void AddProductDetail(object obj)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please choose a product!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!NewOrderDetailQuantity.HasValue || NewOrderDetailQuantity.Value <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newDetail = new OrderDetail
            {
                ProductId = SelectedProduct.ProductId,
                Price = SelectedProduct.Price,
                Quantity = NewOrderDetailQuantity
            };
            NewOrderDetails.Add(newDetail);
          
            NewOrderDetailQuantity = null;
        }

        private void OpenUpdatePopup(object obj)
        {
            if (_selectedItem != null)
            {
                var action = new OrderViewModel(_selectedItem);
                action.OnOrderAdded += Load;
                var popup = new UpdateOrder(_selectedItem)
                {
                    DataContext = action,
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
                };

                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null && mainWindow != popup)
                {
                    popup.Owner = mainWindow;
                }

                if (Application.Current.Windows.Count > 0)
                {
                    var mainWin = Application.Current.Windows[0];
                    if (mainWin != popup)
                    {
                        mainWin.Opacity = 0.5;
                    }
                }

                popup.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an order to update", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void Delete(object obj)
        {
            if (textBoxItem == null || textBoxItem.OrderId == 0)
            {
                MessageBox.Show("Please select an order to cancel", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (SelectedItem.Status == 3 || SelectedItem.Status == 4)
            {
                MessageBox.Show("Cannot delete order with status 'Completed' or 'Cancelled'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var Context = new FstoreContext())
            {
                var orderToCancel = Context.Orders.FirstOrDefault(o => o.OrderId == textBoxItem.OrderId);
                if (orderToCancel != null)
                {
                    orderToCancel.Status = 4;
                    Context.SaveChanges();


                    var orderDetails = Context.OrderDetails.Where(od => od.OrderId == orderToCancel.OrderId).ToList();
                    foreach (var orderDetail in orderDetails)
                    {

                        if (orderDetail.ProductId > 0 && orderDetail.Quantity.HasValue && orderDetail.Quantity.Value > 0)
                        {
                            IncreaseStock(orderDetail.ProductId, orderDetail.Quantity.Value);
                        }
                        else
                        {
                            MessageBox.Show($"Invalid product or quantity for OrderId: {orderToCancel.OrderId}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    MessageBox.Show("Order cancelled successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Order not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (SelectedItem != null)
            {
                OrderList.Remove(SelectedItem);
            }
            Load();
            OnOrderAdded?.Invoke();
        }






        private void Update(object obj)
        {

            if (!IsValidPhoneNumber(TextBoxItem.PhoneNumber))
            {
                MessageBox.Show("Invalid phone number. Ensure that the phone number contains only digits and does not exceed 15 characters.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            else if (string.IsNullOrEmpty(TextBoxItem.Address))
            {
                MessageBox.Show("Address cannot be empty.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            else if (SelectedItem.Status == 3 || SelectedItem.Status == 4)
            {
                MessageBox.Show("Cannot update order with status 'Completed' or 'Cancelled'.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!TextBoxDetail.Quantity.HasValue || TextBoxDetail.Quantity.Value <= 0)
            {
                MessageBox.Show("Quantity must be greater than 0 and cannot be empty.",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            using (var context = new FstoreContext())
            {
                var existingOrder = context.Orders.FirstOrDefault(o => o.OrderId == TextBoxItem.OrderId);
                if (existingOrder != null)
                {
                  
                    var oldOrderDetail = context.OrderDetails.FirstOrDefault(od => od.OrderId == TextBoxItem.OrderId && od.ProductId == TextBoxProduct.ProductId);

                    if (oldOrderDetail != null)
                    {
                        
                        int quantityDifference = oldOrderDetail.Quantity.Value - TextBoxDetail.Quantity.Value;

                        if (quantityDifference > 0)
                        {
                            
                            IncreaseStock(TextBoxProduct.ProductId, quantityDifference);
                        }
                        else if (quantityDifference < 0)
                        {
                            DecreaseStock(TextBoxProduct.ProductId, Math.Abs(quantityDifference));
                        }
                    }

                    
                    existingOrder.FullName = TextBoxItem.FullName;
                    existingOrder.Address = TextBoxItem.Address;
                    existingOrder.PhoneNumber = TextBoxItem.PhoneNumber;
                    existingOrder.Status = TextBoxItem.Status;
                    existingOrder.DeliveredDate = TextBoxItem.DeliveredDate ?? DateTime.Now.AddDays(3); 
                    existingOrder.TotalAmount = TextBoxProduct.Price;

                    var existingOrderDetail = context.OrderDetails
                        .FirstOrDefault(od => od.OrderId == TextBoxItem.OrderId && od.ProductId == TextBoxProduct.ProductId);

                    if (existingOrderDetail != null)
                    {
                        existingOrderDetail.Quantity = TextBoxDetail.Quantity;
                        existingOrderDetail.Price = TextBoxProduct.Price;
                    }
                    else
                    {
                        var newOrderDetail = new OrderDetail
                        {
                            OrderId = existingOrder.OrderId,
                            ProductId = TextBoxProduct.ProductId,
                            Quantity = TextBoxDetail.Quantity,
                            Price = TextBoxProduct.Price
                        };
                        context.OrderDetails.Add(newOrderDetail);
                    }

                    context.SaveChanges();
                    MessageBox.Show("Order updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Order not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Load();
            OnOrderAdded?.Invoke();
        }


        private bool AddOrderWithMultipleProducts(Order order, List<OrderDetail> orderDetails, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                if (order == null)
                {
                    errorMessage = "Order cannot be null.";
                    return false;
                }

                if (orderDetails == null || !orderDetails.Any())
                {
                    errorMessage = "No order details provided.";
                    return false;
                }

                order.TotalAmount = orderDetails.Sum(od => (od.Price ?? 0) * (od.Quantity ?? 0));
                order.OrderedDate = DateTime.Now;
                order.DeliveredDate = order.DeliveredDate ?? DateTime.Now.AddDays(3);

                using (var context = new FstoreContext())
                {
                    Console.WriteLine($"Order Debug: CustomerId={order.CustomerId}, Status={order.Status}, Total={order.TotalAmount}");
                    foreach (var d in orderDetails)
                    {
                        Console.WriteLine($"Detail: ProductId={d.ProductId}, Quantity={d.Quantity}, Price={d.Price}");
                    }

                   
                    context.Orders.Add(order);
                    context.SaveChanges(); 

                   
                    foreach (var detail in orderDetails)
                    {
                        detail.OrderId = order.OrderId;

                        var product = context.Products.FirstOrDefault(p => p.ProductId == detail.ProductId);
                        if (product != null)
                        {
                            int qty = detail.Quantity ?? 0;
                            if (product.Stock >= qty)
                            {
                                product.Stock -= qty;
                            }
                            else
                            {
                                errorMessage = $"Not enough stock for product {product.FullName}. Available: {product.Stock}";
                                return false;
                            }
                        }
                        else
                        {
                            errorMessage = "Product not found.";
                            return false;
                        }
                    }

                    context.OrderDetails.AddRange(orderDetails);
                    context.SaveChanges();
                }

                return true;
            }
            catch (DbUpdateException dbEx)
            {
                errorMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

        }





        private void Add(object obj)
        {
            if (TextBoxItem == null)
                return;
            if (!IsValidPhoneNumber(TextBoxItem.PhoneNumber))
            {
                MessageBox.Show("Invalid phone number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(TextBoxItem.Address))
            {
                MessageBox.Show("Address cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (NewOrderDetails == null || NewOrderDetails.Count == 0)
            {
                MessageBox.Show("Please add at least one product to the order.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string errorMessage;
            bool success = AddOrderWithMultipleProducts(TextBoxItem, NewOrderDetails.ToList(), out errorMessage);
            if (success)
            {
                MessageBox.Show("Order added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                OnOrderAdded?.Invoke();
                TextBoxItem = new Order();
                NewOrderDetails.Clear();
                OnPropertyChanged(nameof(TextBoxItem));
                if (Application.Current.Windows.Count > 1)
                {
                    Application.Current.Windows[1]?.Close();
                    Application.Current.Windows[0].Opacity = 1;
                    Application.Current.Windows[0].Focus();
                }
            }
            else
            {
                MessageBox.Show("Error: " + errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Load()
        {
            using (var Context = new FstoreContext())
            {
               
                var list = Context.Orders.ToList();
                var listpro = Context.Products.ToList();
                var listdetail = Context.OrderDetails.ToList();
                var listCus = Context.Customers.ToList();

                if (OrderList == null)
                    OrderList = new ObservableCollection<Order>(list);
                else
                {
                    OrderList.Clear();
                    foreach (var order in list)
                        OrderList.Add(order);
                }
                OnPropertyChanged(nameof(OrderList));

               
                if (OrderDetailList == null)
                    OrderDetailList = new ObservableCollection<OrderDetail>(listdetail);
                else
                {
                    OrderDetailList.Clear();
                    foreach (var detail in listdetail)
                        OrderDetailList.Add(detail);
                }

                var statusList = Context.OrderStatuses.ToList();
                OrderStatusList = new ObservableCollection<OrderStatus>(statusList);
                OnPropertyChanged(nameof(OrderStatusList));


                OnPropertyChanged(nameof(OrderDetailList));

                Options = new ObservableCollection<Product>(listpro);
                OnPropertyChanged(nameof(Options));

                ProductList = new ObservableCollection<Product>(Context.Products.ToList());
                OnPropertyChanged(nameof(ProductList));

                SelectedProduct = ProductList.FirstOrDefault();

                CustomerList = new ObservableCollection<Customer>(Context.Customers.ToList());
                OnPropertyChanged(nameof(CustomerList));

                SelectedCustomer = CustomerList.FirstOrDefault();

                if (ProductList == null || !ProductList.Any())
                {
                    MessageBox.Show("ProductList is empty!", "Debug", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void UpdateOrderStatusList()
        {
            if (TextBoxItem.Status == 1)
            {
                OrderStatusList = new ObservableCollection<OrderStatus>(OrderStatusList.Where(status => status.Id == 4));
            }
            else if (TextBoxItem.Status == 2)
            {
                OrderStatusList = new ObservableCollection<OrderStatus>(OrderStatusList.Where(status => status.Id != 1));
            }
            else if (TextBoxItem.Status == 3)
            {
                OrderStatusList = new ObservableCollection<OrderStatus>(OrderStatusList.Where(status => status.Id != 1 && status.Id != 2 && status.Id != 4));
            }
            else
            {
                OrderStatusList = new ObservableCollection<OrderStatus>(OrderStatusList);
            }
        }


        private void ExportJson(object obj)
        {
            try
            {
                using (var context = new FstoreContext())
                {
                    var orders = context.Orders
                                        .Include(o => o.OrderDetails)
                                        .ToList();

                    var settings = new Newtonsoft.Json.JsonSerializerSettings
                    {
                        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                        Formatting = Newtonsoft.Json.Formatting.Indented
                    };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(orders, settings);


                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                    var filePath = System.IO.Path.Combine(baseDir, "OrdersExport.json");

                    File.WriteAllText(filePath, json);

                    MessageBox.Show("Export success file JSON at: " + filePath,
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export fail: " + ex.Message,
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExecuteSearch(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {

                OrderList = new ObservableCollection<Order>(AllOrderList);
            }
            else
            {
                var lowerSearch = SearchText.ToLower();
                var filtered = AllOrderList.Where(o =>
                    (!string.IsNullOrEmpty(o.FullName) && o.FullName.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(o.Address) && o.Address.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(o.PhoneNumber) && o.PhoneNumber.ToLower().Contains(lowerSearch)) ||
                    (o.CustomerId.HasValue && o.CustomerId.Value.ToString().Contains(lowerSearch))
                );
                OrderList = new ObservableCollection<Order>(filtered);
            }
            OnPropertyChanged(nameof(OrderList));
        }

    }


}
