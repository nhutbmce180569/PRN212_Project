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

namespace FinalProject.ViewModels.ShopManager
{
    internal class CustomerViewModel
    {
        public ObservableCollection<Customer> CustomerList { get; set; }

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
