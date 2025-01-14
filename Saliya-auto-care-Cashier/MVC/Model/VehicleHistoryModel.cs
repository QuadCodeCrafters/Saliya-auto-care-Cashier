using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    public class VehicleHistoryModel
    {
        private readonly DatabaseStringModel conn;  // DatabaseStringModel

        public VehicleHistoryModel()
        {
            conn = new DatabaseStringModel();
        }

        public class VehicleInfo
        {
            public string OwnerName { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string VehicleModel { get; set; }
            public string VehicleType { get; set; }
            public string RegistrationDate { get; set; }
            public string LastServiceDate { get; set; }
        }

        public class ServiceHistory
        {
            public string Date { get; set; }
            public string InvoiceID { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Tax { get; set; }
            public decimal Amount { get; set; }
        }

        public VehicleInfo GetVehicleInfo(string vehicleNumber)
        {
            string connectionString = conn.ConnectionString;  // Use the DatabaseStringModel connection string
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT * FROM vehicleregister WHERE VehicleNumber = @VehicleNumber", connection);
                command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new VehicleInfo
                        {
                            OwnerName = reader["CustomerName"].ToString(),
                            PhoneNumber = reader["CustomerPhone"].ToString(),
                            Email = reader["CustomerEmail"].ToString(),
                            Address = reader["CustomerAddress"].ToString(),
                            VehicleModel = reader["VehicleModel"].ToString(),
                            VehicleType = reader["VehicleType"].ToString(),
                            RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]).ToString("yyyy-MM-dd"),
                            LastServiceDate = GetLastServiceDate(vehicleNumber)
                        };
                    }
                }
            }
            return null;
        }

        private string GetLastServiceDate(string vehicleNumber)
        {
            string connectionString = conn.ConnectionString;  // Use the DatabaseStringModel connection string
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT MAX(Date) FROM Invoice WHERE VehicleNumber = @VehicleNumber", connection);
                command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);

                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDateTime(result).ToString("yyyy-MM-dd") : "N/A";
            }
        }

        public List<ServiceHistory> GetServiceHistory(string vehicleNumber, string date = null)
        {
            string connectionString = conn.ConnectionString;  // Use the DatabaseStringModel connection string
            var history = new List<ServiceHistory>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT i.Date, i.InvoiceID, id.Description, id.Quantity, id.Price, id.Tax, id.Amount
                    FROM Invoice i
                    JOIN InvoiceDetails id ON i.InvoiceID = id.InvoiceID
                    WHERE i.VehicleNumber = @VehicleNumber";

                if (!string.IsNullOrEmpty(date))
                {
                    query += " AND i.Date = @Date";
                }

                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                if (!string.IsNullOrEmpty(date))
                {
                    command.Parameters.AddWithValue("@Date", date);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        history.Add(new ServiceHistory
                        {
                            Date = Convert.ToDateTime(reader["Date"]).ToString("yyyy-MM-dd"),
                            InvoiceID = reader["InvoiceID"].ToString(),
                            Description = reader["Description"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Tax = Convert.ToDecimal(reader["Tax"]) * 100,
                            Amount = Convert.ToDecimal(reader["Amount"])
                        });
                    }
                }
            }
            return history;
        }

        public List<string> GetServiceDates(string vehicleNumber)
        {
            string connectionString = conn.ConnectionString;  // Use the DatabaseStringModel connection string
            var dates = new List<string>();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("SELECT DISTINCT Date FROM Invoice WHERE VehicleNumber = @VehicleNumber ORDER BY Date DESC", connection);
                command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dates.Add(Convert.ToDateTime(reader["Date"]).ToString("yyyy-MM-dd"));
                    }
                }
            }
            return dates;
        }

        public Dictionary<string, decimal> GetTotals(string vehicleNumber, string date = null)
        {
            string connectionString = conn.ConnectionString;  // Use the DatabaseStringModel connection string
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT Subtotal, SalesTax, Discount, TotalAmount, ServiceCharges, PaidAmount, Balance
                    FROM Invoice
                    WHERE VehicleNumber = @VehicleNumber";

                if (!string.IsNullOrEmpty(date))
                {
                    query += " AND Date = @Date";
                }
                query += " ORDER BY Date DESC LIMIT 1";

                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                if (!string.IsNullOrEmpty(date))
                {
                    command.Parameters.AddWithValue("@Date", date);
                }

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Dictionary<string, decimal>
                        {
                            {"Subtotal", Convert.ToDecimal(reader["Subtotal"])},
                            {"SalesTax", Convert.ToDecimal(reader["SalesTax"])},
                            {"Discount", Convert.ToDecimal(reader["Discount"])},
                            {"TotalAmount", Convert.ToDecimal(reader["TotalAmount"])},
                            {"ServiceCharges", Convert.ToDecimal(reader["ServiceCharges"])},
                            {"PaidAmount", Convert.ToDecimal(reader["PaidAmount"])},
                            {"Balance", Convert.ToDecimal(reader["Balance"])}
                        };
                    }
                }
            }
            return null;
        }
    }
}

