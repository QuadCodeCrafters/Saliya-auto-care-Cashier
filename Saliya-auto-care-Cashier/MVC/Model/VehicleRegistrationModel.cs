using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Saliya_auto_care_Cashier.MVC.Model
{
    public class VehicleRegistrationModel
    {
        public string VehicleNumber { get; set; }
        public string VehicleCategory { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerNIC { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string SpecialNotes { get; set; }

        private readonly DatabaseStringModel Model;

        public VehicleRegistrationModel()
        {
            Model = new DatabaseStringModel();
        }

        public void RegisterVehicle()
        {
            using (MySqlConnection con = new MySqlConnection(Model.ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = @"INSERT INTO vehicleregister 
                                   (VehicleNumber, VehicleCategory, VehicleModel, VehicleType, 
                                    CustomerName, CustomerAddress, CustomerNIC, CustomerEmail, 
                                    CustomerPhone, SpecialNotes) 
                                   VALUES 
                                   (@VehicleNumber, @VehicleCategory, @VehicleModel, @VehicleType, 
                                    @CustomerName, @CustomerAddress, @CustomerNIC, @CustomerEmail, 
                                    @CustomerPhone, @SpecialNotes)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@VehicleNumber", VehicleNumber);
                        cmd.Parameters.AddWithValue("@VehicleCategory", VehicleCategory);
                        cmd.Parameters.AddWithValue("@VehicleModel", VehicleModel);
                        cmd.Parameters.AddWithValue("@VehicleType", VehicleType);
                        cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                        cmd.Parameters.AddWithValue("@CustomerAddress", CustomerAddress);
                        cmd.Parameters.AddWithValue("@CustomerNIC", CustomerNIC);
                        cmd.Parameters.AddWithValue("@CustomerEmail", CustomerEmail);
                        cmd.Parameters.AddWithValue("@CustomerPhone", CustomerPhone);
                        cmd.Parameters.AddWithValue("@SpecialNotes", SpecialNotes);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while registering the vehicle: " + ex.Message);
                }
            }
        }
    }
}