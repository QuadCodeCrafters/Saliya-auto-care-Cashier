using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    internal class CategoryViewModel
    {
        private readonly string connectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";

        private readonly DatabaseStringModel conn; //DatabaseStringModel
        private object vehicleType = "Mercedes_Benz";

        public CategoryViewModel()
        {

            conn = new DatabaseStringModel(); // conn
        }

        public void sendvehicleno(string vehicleNumber)
        {
            MessageBox.Show($"Got the Vehicle Number: {vehicleNumber}", "Vehicle Information", MessageBoxButton.OK, MessageBoxImage.Information);

            string connectionString1 = conn.ConnectionString; // Use the DatabaseStringModel connection string

            using (var connection = new MySqlConnection(connectionString1))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT VehicleType FROM vehicleregister WHERE VehicleNumber = @VehicleNumber";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);

                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            string VehicleType = result.ToString();
                            MessageBox.Show($"Vehicle Brand: {VehicleType}", "Vehicle Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            GetCategories();
                        }
                        else
                        {
                            MessageBox.Show("No vehicle found .", "Vehicle Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                        MessageBox.Show("The connection has been successfully closed.", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT `{vehicleType}` FROM paintservices";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading categories: {ex.Message}");
            }

            return categories;
        }

    }
}

