using Saliya_auto_care_Cashier.Mails;
using Saliya_auto_care_Cashier.MVC.Model;
using Saliya_auto_care_Cashier.MVVM.View;
using Saliya_auto_care_Cashier.Notifications;
using System;
using System.Windows;

namespace Saliya_auto_care_Cashier.MVC.Controller
{
    public class VehicleRegistrationController
    {
        private readonly VehicleRegistrationModel model;
        private readonly Register_View view;

        public VehicleRegistrationController(Register_View view)
        {
            model = new VehicleRegistrationModel();
            this.view = view;
        }

        public void RegisterVehicle(string vehicleNumber, string vehicleCategory, string vehicleModel,
            string vehicleType, string customerName, string customerAddress, string customerNIC,
            string customerEmail, string customerPhone, string specialNotes)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(vehicleNumber) ||
                    string.IsNullOrWhiteSpace(customerName) ||
                    string.IsNullOrWhiteSpace(customerNIC))
                {
                    MessageBox.Show("Required fields cannot be empty!", "Validation Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Set model properties
                model.VehicleNumber = vehicleNumber;
                model.VehicleCategory = vehicleCategory;
                model.VehicleModel = vehicleModel;
                model.VehicleType = vehicleType;
                model.CustomerName = customerName;
                model.CustomerAddress = customerAddress;
                model.CustomerNIC = customerNIC;
                model.CustomerEmail = customerEmail;
                model.CustomerPhone = customerPhone;
                model.SpecialNotes = specialNotes;

                // Register the vehicle
                model.RegisterVehicle();

                // Show success notification
                Notificationbox.ShowSuccess();

                // Send registration email if email is provided
                if (!string.IsNullOrWhiteSpace(customerEmail))
                {
                    RegisterMail();
                }

                // Clear all fields after successful registration
                view.ClearAllFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void RegisterMail()
        {
            try
            {
                EmailService emailService = new EmailService();
                string registrationContent = emailService.RegistrationContent(model.CustomerName);

                bool emailSent = await emailService.SendEmailAsync(
                    model.CustomerEmail,
                    model.CustomerName,
                    "Welcome to Saliya Auto Care!",
                    registrationContent
                );

                if (!emailSent)
                {
                    MessageBox.Show(
                        $"Failed to send registration email to {model.CustomerEmail}. " +
                        "Please check your Email Address formatting or Network Connection.",
                        "Email Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the email: {ex.Message}",
                    "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}