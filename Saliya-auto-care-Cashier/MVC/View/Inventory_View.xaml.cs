using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
        public ObservableCollection<InventoryItem> InventoryItems { get; set; }

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
            InventoryItems = new ObservableCollection<InventoryItem>();
            FilteredInventoryItems = new ObservableCollection<InventoryItem>();
            allInventoryItems = new ObservableCollection<InventoryItem>();

            ClearSearchCommand = new RelayCommand(ClearSearch);
            LoadInventoryItems();
            DataContext = this;
        }

        private void LoadInventoryItems()
        {
            string connectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";
            string query = "SELECT ItemName, Price, ImagePath FROM Inventory";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string itemName = reader["ItemName"].ToString();
                        double price = Convert.ToDouble(reader["Price"]);
                        string imagePath = reader["ImagePath"].ToString();

                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                        image.EndInit();

                        var item = new InventoryItem
                        {
                            ItemName = itemName,
                            Price = price,
                            ImageSource = image
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
                string connectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";
                string query = "SELECT * FROM Inventory WHERE ItemName = @ItemName";

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ItemName", clickedItem.ItemName);
                        connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            SelectedItemDetails = new InventoryItem
                            {
                                ItemName = reader["ItemName"].ToString(),
                                Price = Convert.ToDouble(reader["Price"]),
                                ImageSource = new BitmapImage(new Uri(reader["ImagePath"].ToString(), UriKind.RelativeOrAbsolute))
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving item details: " + ex.Message);
                }
            }
        }

        private void FilterItems()
        {
            FilteredInventoryItems.Clear();
            var filteredItems = allInventoryItems.Where(item =>
                                item.ItemName.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0);
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
    public class InventoryItem
    {
        public string ItemName { get; set; }
        public double Price { get; set; }
        public BitmapImage ImageSource { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action executes;
        private readonly Func<bool> canExecutes;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            executes = execute;
            canExecutes = canExecute;
        }

        public bool CanExecute(object parameter) => canExecutes == null || canExecutes();

        public void Execute(object parameter) => executes();

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
