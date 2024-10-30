using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MySql.Data.MySqlClient;
using Saliya_auto_care_Cashier.MVC.Model; // Import the namespace for DatabaseStringModel

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class Register_View : UserControl
    {
        private List<Control> requiredFields;

        public Register_View()
        {
            InitializeComponent();
            InitializeRequiredFields();
        }

        private void InitializeRequiredFields()
        {
            requiredFields = new List<Control>
            {
                txtvehiclenum, txtvehicletype, txtvehiclemodel, txtcusname,
                txtcusaddress, txtcusNIC, txtcusmail, txtcusnumber, txtcusspec, txtemergencycontact
            };
        }

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            if (IsAnyFieldEmpty())
            {
                ShowErrorAnimation();
            }
            else
            {
                RegisterVehicle(); // Call the function to register vehicle data
            }
        }

        private bool IsAnyFieldEmpty()
        {
            foreach (var field in requiredFields)
            {
                if (field is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text) ||
                    field is ComboBox comboBox && comboBox.SelectedItem == null)
                {
                    return true;
                }
            }
            return false;
        }

        private void ShowErrorAnimation()
        {
            foreach (var field in requiredFields)
            {
                bool hasError = (field is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text)) ||
                                (field is ComboBox comboBox && comboBox.SelectedItem == null);

                if (hasError)
                {
                    ApplyErrorAppearance(field);
                    ShakeControl(field);
                }
            }

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            timer.Tick += (s, e) =>
            {
                foreach (var field in requiredFields)
                {
                    ResetToDefaultAppearance(field);
                }
                timer.Stop();
            };
            timer.Start();
        }

        private void ApplyErrorAppearance(Control control)
        {
            control.BorderBrush = Brushes.Red;
            control.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void ShakeControl(Control control)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            control.RenderTransform = translateTransform;

            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 10,
                Duration = TimeSpan.FromMilliseconds(50),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(5) // Shake 5 times
            };

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void ResetToDefaultAppearance(Control control)
        {
            control.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD");
            control.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
        }

        public void RegisterVehicle()
        {
            // Create an instance of DatabaseStringModel to access the connection string
            DatabaseStringModel dbModel = new DatabaseStringModel();
            using (MySqlConnection con = new MySqlConnection(dbModel.ConnectionString))
            {
                try
                {
                    con.Open();

                    // Collect data from form fields
                    string vehicleNumber = txtvehiclenum.Text;
                    string vehicleType = txtvehicletype.Text;
                    string vehicleModel = txtvehiclemodel.Text;
                    string customerName = txtcusname.Text;
                    string customerAddress = txtcusaddress.Text;
                    string customerNIC = txtcusNIC.Text;
                    string customerEmail = txtcusmail.Text;
                    string customerPhone = txtcusnumber.Text;
                    string emergencyContact = txtemergencycontact.Text;
                    string specialNotes = txtcusspec.Text;

                    // SQL Insert query
                    string query = "INSERT INTO vehicleregister (VehicleNumber, VehicleType, VehicleModel, CustomerName, CustomerAddress, CustomerNIC, CustomerEmail, CustomerPhone, EmergencyContact, SpecialNotes) " +
                                   "VALUES (@VehicleNumber, @VehicleType, @VehicleModel, @CustomerName, @CustomerAddress, @CustomerNIC, @CustomerEmail, @CustomerPhone, @EmergencyContact, @SpecialNotes)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                        cmd.Parameters.AddWithValue("@VehicleType", vehicleType);
                        cmd.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);
                        cmd.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                        cmd.Parameters.AddWithValue("@CustomerNIC", customerNIC);
                        cmd.Parameters.AddWithValue("@CustomerEmail", customerEmail);
                        cmd.Parameters.AddWithValue("@CustomerPhone", customerPhone);
                        cmd.Parameters.AddWithValue("@EmergencyContact", emergencyContact);
                        cmd.Parameters.AddWithValue("@SpecialNotes", specialNotes);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Vehicle registered successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
