using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    internal class Invoice
    {
        private readonly DatabaseStringModel conn;

        public Invoice()
        {
            conn = new DatabaseStringModel();
        }

        public void SaveInvoice(string invoiceID, string customerName, string customerAddress, 
            string vehicleType, string vehicleNumber, DateTime date, decimal subtotal, 
            decimal salesTax, decimal discount, decimal totalAmount, decimal Charges, 
            decimal paidAmount, decimal balance, 
            List<(string Description, int Quantity, decimal Price, decimal Tax, decimal Amount)> items)
        {
            string connectionString = conn.ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Save Invoice header (unchanged)
                    string invoiceQuery = @"INSERT INTO Invoice (InvoiceID, CustomerName, CustomerAddress, 
                        VehicleType, VehicleNumber, Date, Subtotal, SalesTax, Discount, TotalAmount, 
                        ServiceCharges, PaidAmount, Balance)
                        VALUES (@InvoiceID, @CustomerName, @CustomerAddress, @VehicleType, 
                        @VehicleNumber, @Date, @Subtotal, @SalesTax, @Discount, @TotalAmount, 
                        @ServiceCharges, @PaidAmount, @Balance)";

                    using (MySqlCommand invoiceCommand = new MySqlCommand(invoiceQuery, connection, transaction))
                    {
                        invoiceCommand.Parameters.AddWithValue("@InvoiceID", invoiceID);
                        invoiceCommand.Parameters.AddWithValue("@CustomerName", customerName);
                        invoiceCommand.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                        invoiceCommand.Parameters.AddWithValue("@VehicleType", vehicleType);
                        invoiceCommand.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                        invoiceCommand.Parameters.AddWithValue("@Date", date);
                        invoiceCommand.Parameters.AddWithValue("@Subtotal", subtotal);
                        invoiceCommand.Parameters.AddWithValue("@SalesTax", salesTax);
                        invoiceCommand.Parameters.AddWithValue("@Discount", discount);
                        invoiceCommand.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        invoiceCommand.Parameters.AddWithValue("@ServiceCharges", Charges);
                        invoiceCommand.Parameters.AddWithValue("@PaidAmount", paidAmount);
                        invoiceCommand.Parameters.AddWithValue("@Balance", balance);
                        invoiceCommand.ExecuteNonQuery();
                    }

                    // Save Invoice Details with Tax as string
                    string detailsQuery = @"INSERT INTO InvoiceDetails (InvoiceID, Description, 
                        Quantity, Price, Tax, Amount)
                        VALUES (@InvoiceID, @Description, @Quantity, @Price, @Tax, @Amount)";

                    foreach (var item in items)
                    {
                        using (MySqlCommand detailsCommand = new MySqlCommand(detailsQuery, connection, transaction))
                        {
                            detailsCommand.Parameters.AddWithValue("@InvoiceID", invoiceID);
                            detailsCommand.Parameters.AddWithValue("@Description", item.Description);
                            detailsCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                            detailsCommand.Parameters.AddWithValue("@Price", item.Price);
                            detailsCommand.Parameters.AddWithValue("@Tax", item.Tax); // Keep tax as string
                            detailsCommand.Parameters.AddWithValue("@Amount", item.Amount);
                            detailsCommand.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Error saving invoice: {ex.Message}", ex);
                }
            }
        }
    }
}

