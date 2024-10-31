using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class Inventory_View : UserControl
    {
        public ObservableCollection<InventoryItem> InventoryItems { get; set; }

        public Inventory_View()
        {
            InitializeComponent();
            InventoryItems = new ObservableCollection<InventoryItem>();
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

                        InventoryItems.Add(new InventoryItem
                        {
                            ItemName = itemName,
                            Price = price,
                            ImageSource = image
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory items: " + ex.Message);
            }
        }
    }

    public class InventoryItem
    {
        public string ItemName { get; set; }
        public double Price { get; set; }
        public BitmapImage ImageSource { get; set; }
    }
}
