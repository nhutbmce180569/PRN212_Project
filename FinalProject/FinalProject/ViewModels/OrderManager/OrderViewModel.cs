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
                    OnPropertyChanged(nameof(TextBoxItem));
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
                if (_selectedProduct != null) {
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


        //private Customer _selectedCustomer;
        //public Customer SelectedCustomer
        //{
        //    get => _selectedCustomer;
        //    set
        //    {
        //        _selectedCustomer = value;
        //        OnPropertyChanged(nameof(SelectedCustomer));
        //    }
        //}
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



        private OrderDetail textBoxDetail= new OrderDetail();
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
        public OrderViewModel()
        {
            Load();

            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
             OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);

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


        private void OpenUpdatePopup(object obj)
        {
            if (_selectedItem != null)
            {
                
                var action = new OrderViewModel(_selectedItem);
                action.OnOrderAdded += Load;
                var popup = new UpdateOrder(_selectedItem)
                {
                    DataContext = action,
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
                };

                Application.Current.Windows[0].Opacity = 0.5;
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

            using (var Context = new FstoreContext())
            {
                var orderToCancel = Context.Orders.FirstOrDefault(o => o.OrderId == textBoxItem.OrderId);
                if (orderToCancel != null)
                {
                    orderToCancel.Status = 4; 
                    Context.SaveChanges();
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

           
            OnOrderAdded?.Invoke();
        }


        private void Update(object obj)
        {
           
            if (textBoxItem.FullName.IsNullOrEmpty())
            {
                MessageBox.Show("Please input all required information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                using (var Context = new FstoreContext())
                {
                  
                    var existingOrder = Context.Orders.FirstOrDefault(o => o.OrderId == textBoxItem.OrderId);

                    if (existingOrder != null)
                    {
                        
                        existingOrder.FullName = textBoxItem.FullName;
                        existingOrder.Address = textBoxItem.Address;
                        existingOrder.PhoneNumber = textBoxItem.PhoneNumber;
                        existingOrder.Status = textBoxItem.Status;
                        existingOrder.TotalAmount = textBoxProduct.Price;

                        
                        var existingOrderDetail = Context.OrderDetails
                            .FirstOrDefault(od => od.OrderId == textBoxItem.OrderId && od.ProductId == textBoxProduct.ProductId);

                        if (existingOrderDetail != null)
                        {
                            existingOrderDetail.Quantity = textBoxDetail.Quantity;
                            existingOrderDetail.Price = textBoxProduct.Price; 
                        }
                        else
                        {
                            
                            var newOrderDetail = new OrderDetail
                            {
                                OrderId = existingOrder.OrderId,
                                ProductId = textBoxProduct.ProductId,
                                Quantity = textBoxDetail.Quantity,
                                Price = textBoxProduct.Price
                            };
                            Context.OrderDetails.Add(newOrderDetail);
                        }

                        
                        Context.SaveChanges();
                        MessageBox.Show("Order updated successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Order not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                OnOrderAdded?.Invoke();
            }
        }


        private void Add(object obj)
        {
            if (textBoxItem == null) return;

            var newitem = new Order
            {
                CustomerId = textBoxItem.CustomerId,
                FullName = textBoxItem.FullName,
                Address = textBoxItem.Address,
                PhoneNumber = textBoxItem.PhoneNumber,
                OrderedDate = DateTime.Now,
                DeliveredDate = DateTime.Now.AddDays(3),
                Status = textBoxItem.Status,    
                TotalAmount = textBoxProduct.Price,

            };
            
            using (var Context = new FstoreContext()) {
                Context.Orders.Add(newitem);
                Context.SaveChanges();
                
            }
            AllOrderList= new ObservableCollection<Order>(OrderList);


            var newitemOrderDatail = new OrderDetail
            {
                OrderId = OrderList.Any() ? OrderList.Max(x => x.OrderId) + 1 : 1,
                ProductId = textBoxProduct.ProductId,
                Price = textBoxProduct.Price,
                Quantity = textBoxDetail.Quantity,

            };
            using (var Context = new FstoreContext())
            {
                Context.OrderDetails.Add(newitemOrderDatail);
                Context.SaveChanges();

            }

            using (var Context = new FstoreContext())
            {
                var product = Context.Products.FirstOrDefault(p => p.ProductId == textBoxProduct.ProductId);
                if (product != null)
                {
                    product.Stock = product.Stock - textBoxDetail.Quantity;
                    // Hoặc: product.Stock -= textBoxDetail.Quantity;
                    Context.SaveChanges();
                }
            }

            OnOrderAdded?.Invoke();
            textBoxItem = new Order();


            OnPropertyChanged(nameof(textBoxItem));
            Application.Current.Windows[1]?.Close();
            Application.Current.Windows[0].Opacity = 1;
            Application.Current.Windows[0].Focus();
        }


        private void Load()
        {
            using (var Context = new FstoreContext())
            {
                // Lấy các đơn hàng mà không có trạng thái cancel (status != 4)
                var list = Context.Orders.Where(o => o.Status != 4).ToList();
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

                // Tương tự với các collection khác
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


    }
}
