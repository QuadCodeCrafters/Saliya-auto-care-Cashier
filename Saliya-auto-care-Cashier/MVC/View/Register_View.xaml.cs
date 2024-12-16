using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Saliya_auto_care_Cashier.MVC.Controller; 

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class Register_View : UserControl
    {
        private List<Control> requiredFields;
        private VehicleRegistrationController controller;

        public Register_View()
        {
            InitializeComponent();
            InitializeRequiredFields();
            controller = new VehicleRegistrationController(this);
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

            translateTransform.BeginAnimation(TranslateTransform.XProperty, Saliya_auto_care_Cashier.Animations.ErrorAnimation.animation); //imported from ErrorAnimation.cs
        }

        private void ResetToDefaultAppearance(Control control)
        {
            control.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD");
            control.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
        }

        public void RegisterVehicle()
        {
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

            controller.RegisterVehicle(vehicleNumber, vehicleType, vehicleModel, customerName, customerAddress, customerNIC, customerEmail, customerPhone, emergencyContact, specialNotes);
        }
        public void ClearAllFields()
        {
            foreach (var field in requiredFields)
            {
                if (field is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                }
                else if (field is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1;
                }
            }
        }
    }
}
