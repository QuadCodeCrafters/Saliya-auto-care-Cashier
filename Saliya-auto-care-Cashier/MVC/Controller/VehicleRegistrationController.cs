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

                // Register the vehicle
                model.RegisterVehicle();
                Notificationbox.ShowSuccess();

                // Send registration email
                RegisterMail();

                // Clear all fields after successful registration
                view.ClearAllFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public async void RegisterMail()
        {
            try
            {
                EmailService emailService = new EmailService();
                string registrationContent = emailService.GenerateRegistrationContent(model.CustomerName);

                bool emailSent = await emailService.SendEmailAsync(
                    model.CustomerEmail,
                    model.CustomerName,
                    "Welcome to Saliya Auto Care!",
                    registrationContent
                );

                if (emailSent)
                {
                    MessageBox.Show("Registration email sent successfully!");
                }
                else
                {
                    MessageBox.Show(
                        $"Failed to send registration email to {model.CustomerEmail}. Please check the email address or network connection.",
                        "Email Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the email: {ex.Message}");
            }
        }
    }
}
