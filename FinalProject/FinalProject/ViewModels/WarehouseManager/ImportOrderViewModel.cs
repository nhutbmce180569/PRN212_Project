using FinalProject.Models;
using MaterialDesignColors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
                OnPropertyChanged(nameof(ImportList));
            }
        }

        public ObservableCollection<ImportOrder> AllImportList { set; get; }
        public ICommand SearchCommand { get; }
        public ICommand ExportCommand { get; }

        public ImportOrderViewModel()
        {
            Load();
            SearchCommand = new RelayCommand(Search);
            ExportCommand = new RelayCommand(Export);
        }

        private void Export(object obj)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = "import_order.xlsx",
                    Title = "Save Exported Data"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var workbook = new ClosedXML.Excel.XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("ImportOrder");

                    var headerRange = worksheet.Range(1, 1, 1, 9);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    worksheet.Columns().AdjustToContents();

                    // Header row
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Product";
                    worksheet.Cell(1, 3).Value = "Supplier";
                    worksheet.Cell(1, 4).Value = "Quantity";
                    worksheet.Cell(1, 5).Value = "Import Price";
                    worksheet.Cell(1, 6).Value = "Total Amount";
                    worksheet.Cell(1, 7).Value = "CreatedDate";

                    // Data rows
                    for (int i = 0; i < AllImportList.Count; i++)
                    {
                        var p = AllImportList[i];
                        worksheet.Cell(i + 2, 1).Value = p.Ioid;
                        worksheet.Cell(i + 2, 2).Value = p.ImportOrderDetails?.FirstOrDefault()?.Product.Model;
                        worksheet.Cell(i + 2, 3).Value = p.Supplier?.Name;
                        worksheet.Cell(i + 2, 4).Value = p.ImportOrderDetails?.FirstOrDefault()?.Quantity;
                        worksheet.Cell(i + 2, 5).Value = p.ImportOrderDetails?.FirstOrDefault()?.ImportPrice;
                        worksheet.Cell(i + 2, 6).Value = p.TotalCost;
                        worksheet.Cell(i + 2, 7).Value = p.ImportDate;


                    }

                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Export to Excel successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Load()
        {
            using var context = new FstoreContext();
            var list = context.ImportOrders.Include(i => i.ImportOrderDetails).ThenInclude(p => p.Product).Include(s => s.Supplier).Include(e => e.Employee).ToList();
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
