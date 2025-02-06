using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    public class DatabaseConnection
    {

        private readonly string connectionString;

        //the DatabaseStringModel that open the conn
        public DatabaseConnection()
        {
            // Instantiate DatabaseStringModel to retrieve the connection string
            DatabaseStringModel dbconnect = new DatabaseStringModel();
            connectionString = dbconnect.ConnectionString;
        }


        public DataTable GetData(string query)
        {
            var dataTable = new DataTable();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    var adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }

        public void Connect()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Successfully connected to the database.");
                    MessageBox.Show("Success DB connected");
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Connection failed: {ex.Message}");
                    MessageBox.Show(" not connected");
                }
            }
        }

        // Method to retrieve all usernames from the Cashier table
        public List<string> GetUsernames()
        {
            var usernames = new List<string>();
            string query = "SELECT Username FROM Cashier"; // Adjusted table and column name

            DataTable dataTable = GetData(query);

            foreach (DataRow row in dataTable.Rows)
            {
                usernames.Add(row["Username"].ToString()); // Adjusted column name
            }

            return usernames;
        }

        // Method to authenticate the user based on username and password
        public bool AuthenticateUser(string username, string password)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Cashier WHERE Username = @username AND PasswordHash = @password"; // Adjusted table and column names

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password); // Replace with hashed/encrypted password in production

                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    return userCount > 0;
                }
            }
        }

    }
}