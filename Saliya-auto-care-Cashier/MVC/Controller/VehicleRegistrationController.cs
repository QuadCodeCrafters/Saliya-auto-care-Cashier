using Saliya_auto_care_Cashier.MVC.Model;
using System;
using System.Windows;

namespace Saliya_auto_care_Cashier.MVC.Controller
{
    public class VehicleRegistrationController
    {
        private readonly VehicleRegistrationModel model;

        public VehicleRegistrationController()
        {
            model = new VehicleRegistrationModel();
        }

        public void RegisterVehicle(string vehicleNumber, string vehicleType, string vehicleModel, string customerName, string customerAddress, string customerNIC, string customerEmail, string customerPhone, string emergencyContact, string specialNotes)
        {
            try
            {
                // Set model properties
                model.VehicleNumber = vehicleNumber;
                model.VehicleType = vehicleType;
                model.VehicleModel = vehicleModel;
                model.CustomerName = customerName;
                model.CustomerAddress = customerAddress;
                model.CustomerNIC = customerNIC;
                model.CustomerEmail = customerEmail;
                model.CustomerPhone = customerPhone;
                model.EmergencyContact = emergencyContact;
                model.SpecialNotes = specialNotes;

                model.RegisterVehicle();
                MessageBox.Show("Vehicle registered successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
