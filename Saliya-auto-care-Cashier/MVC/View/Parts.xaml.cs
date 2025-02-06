using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Saliya_auto_care_Cashier.MVC.Model;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Parts : UserControl
    {
        private DatabaseStringModel _dbModel;

        public Parts()
        {
            InitializeComponent();
            _dbModel = new DatabaseStringModel();
            cmbQty.SelectedIndex = 0; // Set default quantity to 1
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                SearchResultsItemsControl.ItemsSource = null;
                return;
            }

            List<Part> searchResults = SearchParts(searchText);
            SearchResultsItemsControl.ItemsSource = searchResults;
        }

        private List<Part> SearchParts(string searchText)
        {
            List<Part> results = new List<Part>();

            using (MySqlConnection connection = new MySqlConnection(_dbModel.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id, name, price FROM parts WHERE name LIKE @searchText OR id LIKE @searchText";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchText", $"%{searchText}%");
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                results.Add(new Part
                                {
                                    Id = reader.GetInt32("id"),
                                    Name = reader.GetString("name"),
                                    Price = reader.GetDecimal("price")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching parts: {ex.Message}");
                }
            }

            return results;
        }

        private void PartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Part selectedPart)
            {
                txtproduct.Text = selectedPart.Name;
                txtprice.Text = FormatCurrency(selectedPart.Price);
                cmbQty.SelectedIndex = 0; // Reset quantity to 1
                UpdateAmount();
            }
        }


        private void CmbQty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAmount();
        }

        private void UpdateAmount()
        {
            if (decimal.TryParse(txtprice.Text.TrimStart('R', 's', ' '), out decimal price) &&
                int.TryParse(cmbQty.SelectedItem as string, out int quantity))
            {
                decimal amount = price * quantity;
                txtamount.Text = FormatCurrency(amount);
            }
            else
            {
                txtamount.Text = "";
            }
        }

        private string FormatCurrency(decimal amount)
        {
            return string.Format("Rs {0:N2}", amount);
        }


        private void Barcode_Click(object sender, RoutedEventArgs e)
        {
            // Barcode functionality
        }


        private void btn_add(object sender, RoutedEventArgs e)
        {



        }
    }

    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

