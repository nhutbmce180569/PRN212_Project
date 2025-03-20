using FinalProject.Models;
using MaterialDesignColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFLab.Helper;
using WPFLab.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinalProject.ViewModels.WarehouseManager
{
    class ImportOrderViewModel : BaseViewModel
    {
        private ObservableCollection<ImportOrder> _importList;
        public ObservableCollection<ImportOrder> ImportList
        {
            get => _importList;
            set
            {
                _importList = value;
                OnPropertyChanged(nameof(ImportList)); // Kích hoạt UI update
            }
        }
        public ObservableCollection<ImportOrder> AllImportList { set; get; }
        public ICommand SearchCommand { get; }
        public ImportOrderViewModel()
        {
            Load();
            SearchCommand = new RelayCommand(Search);
        }
        private void Load()
        {
            using var context = new FstoreContext();
            var list = context.ImportOrders.Include(i => i.ImportOrderDetails).ThenInclude(p => p.Product).Include(s => s.Supplier).ToList();
            AllImportList = new ObservableCollection<ImportOrder>(list);
            ImportList = new ObservableCollection<ImportOrder>(AllImportList);
        }
        private String _searchBoxItem;
        public String SearchBoxItem
        {
            get { return _searchBoxItem; }
            set
            {
                _searchBoxItem = value;
                OnPropertyChanged(nameof(SearchBoxItem));
            }
        }
        private void Search(object obj)
        {
            if (!_searchBoxItem.Equals(""))
            {
                var list = from item in AllImportList
                           where item.Supplier != null // Đảm bảo Supplier không null
                           && item.Supplier.Name != null // Đảm bảo Name không null
                           && item.Supplier.Name.ToLower().Contains(_searchBoxItem?.ToLower() ?? "")
                           select item;

                if (list.Any())
                {
                    Debug.WriteLine("helo");
                    ImportList = new ObservableCollection<ImportOrder>(list);
                }
                else
                {
                    MessageBox.Show("There is no supplier", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (_searchBoxItem.Equals(""))
            {
                ImportList = new ObservableCollection<ImportOrder>(AllImportList);
                OnPropertyChanged(nameof(ImportList));
            }
        }
    }
}
