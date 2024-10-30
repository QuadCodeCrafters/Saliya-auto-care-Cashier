using Saliya_auto_care_Cashier.MVC.Model;
using System;
using System.Windows;

namespace Saliya_auto_care_Cashier.MVC.Controller
{
    public class VehicleRegistrationController
    {
        private readonly VehicleRegistrationModel _model;

        public VehicleRegistrationController()
        {
            _model = new VehicleRegistrationModel();
        }

        public void RegisterVehicle(string vehicleNumber, string vehicleType, string vehicleModel, string customerName, string customerAddress, string customerNIC, string customerEmail, string customerPhone, string emergencyContact, string specialNotes)
        {
            try
            {
                // Set model properties
                _model.VehicleNumber = vehicleNumber;
                _model.VehicleType = vehicleType;
                _model.VehicleModel = vehicleModel;
                _model.CustomerName = customerName;
                _model.CustomerAddress = customerAddress;
                _model.CustomerNIC = customerNIC;
                _model.CustomerEmail = customerEmail;
                _model.CustomerPhone = customerPhone;
                _model.EmergencyContact = emergencyContact;
                _model.SpecialNotes = specialNotes;

                _model.RegisterVehicle();
                MessageBox.Show("Vehicle registered successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
