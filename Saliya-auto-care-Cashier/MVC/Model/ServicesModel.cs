using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    internal class ServicesModel
    {
        private readonly DatabaseStringModel conn; // DatabaseStringModel

        public ServicesModel()
        {
            conn = new DatabaseStringModel();
        }

        public List<(string Name, decimal Price)> GetCategories()
        {
            string connectionString = conn.ConnectionString; // Use the DatabaseStringModel connection string

            List<(string Name, decimal Price)> categories = new List<(string Name, decimal Price)>();

            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = "SELECT name, price FROM services";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add((reader.GetString(0) , reader.GetDecimal(1)));
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Paint Services : {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // connection is closed  
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            return categories;
        }
    }
}
