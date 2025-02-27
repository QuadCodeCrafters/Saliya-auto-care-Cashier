using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class Inventory_View : UserControl, INotifyPropertyChanged
    {
        private string searchText;
        private InventoryItem selectedItemDetails;
        private ObservableCollection<InventoryItem> allInventoryItems;

        public ObservableCollection<InventoryItem> FilteredInventoryItems { get; set; }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterItems();
            }
        }

        public InventoryItem SelectedItemDetails
        {
            get => selectedItemDetails;
            set
            {
                selectedItemDetails = value;
                OnPropertyChanged(nameof(SelectedItemDetails));
            }
        }

        public ICommand ClearSearchCommand { get; }

        public Inventory_View()
        {
            InitializeComponent();
            FilteredInventoryItems = new ObservableCollection<InventoryItem>();
            allInventoryItems = new ObservableCollection<InventoryItem>();

            ClearSearchCommand = new RelayCommand(ClearSearch);
            LoadInventoryItems();
            DataContext = this;
        }

        private void LoadInventoryItems()
        {
            string connectionString = "Your Connection string";

            string connectionString = "Your Connection string";

            string query = "SELECT * FROM inventory";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var item = new InventoryItem
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            ItemName = reader["ItemName"].ToString(),
                            SKU = reader["SKU"].ToString(),
                            ItemPrice = Convert.ToDecimal(reader["ItemPrice"]),
                            Category = reader["Category"].ToString(),
                            Description = reader["Description"].ToString(),
                            Quantity = Convert.ToDecimal(reader["Quantity"]),
                            Manufacturer = reader["Manufacturer"].ToString(),
                            ModelNumber = reader["ModelNumber"].ToString(),
                            Warranty = reader["Warranty"].ToString(),
                            StorageLocation = reader["StorageLocation"].ToString(),
                            ImageSource = new BitmapImage(new Uri(reader["PicLocation"].ToString(), UriKind.RelativeOrAbsolute))
                        };
                        allInventoryItems.Add(item);
                        FilteredInventoryItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory items: " + ex.Message);
            }
        }

        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is InventoryItem clickedItem)
            {
                SelectedItemDetails = clickedItem;
            }
        }

        private void FilterItems()
        {
            FilteredInventoryItems.Clear();
            var filteredItems = allInventoryItems.Where(item =>
                item.ItemName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                item.Category.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
            foreach (var item in filteredItems)
            {
                FilteredInventoryItems.Add(item);
            }
        }

        private void ClearSearch()
        {
            SearchText = string.Empty;
            FilteredInventoryItems.Clear();
            foreach (var item in allInventoryItems)
            {
                FilteredInventoryItems.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class InventoryItem : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string SKU { get; set; }
        public decimal ItemPrice { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string Manufacturer { get; set; }
        public string ModelNumber { get; set; }
        public string Warranty { get; set; }
        public string StorageLocation { get; set; }
        public BitmapImage ImageSource { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        public void Execute(object parameter) => execute();

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
