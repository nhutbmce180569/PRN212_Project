using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinalProject.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WPFLab.Helper;
using FinalProject.Views.ShopManager.Customer;
using System.Windows;

namespace FinalProject.ViewModels.ShopManager
{
    internal class CustomerViewModel
    {
        public ObservableCollection<Customer> CustomerList { get; set; }

        public ICommand OpenPopupCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public CustomerViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            OpenPopupCommand = new RelayCommand(OpenPopup);
        }
        private void OpenPopup(object obj)
        {
            var popup = new AddCustomer
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            // Dùng Dispatcher để tránh lỗi "Window is closing"
            popup.Deactivated += (s, e) =>
            {
                if (popup.IsLoaded)
                {

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!popup.IsActive) // khi click chuột ra ngoài không trong popup
                        {
                            popup.Topmost = true;

                            //// Lấy lại focus cho MainWindow
                            //Application.Current.MainWindow.Activate();
                            //Application.Current.MainWindow.Opacity = 1;

                        }
                    }));
                }
            };
            Application.Current.MainWindow.Opacity = 0.5;
            popup.ShowDialog();
        }


        public void Load()
        {
            using (var context = new FstoreContext())
            {
                var list = context.Customers.ToList();
                CustomerList = new ObservableCollection<Customer>(list);
            }
        }
        private void Search(object obj)
        {
            throw new NotImplementedException();
        }

        private void Delete(object obj)
        {
            throw new NotImplementedException();
        }

        private void Add(object obj)
        {
            throw new NotImplementedException();

        }

        private void Update(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
