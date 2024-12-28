using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    internal class CategoryViewModel
    {
        private readonly string _connectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";

        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT name FROM Categories";

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

