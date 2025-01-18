using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Saliya_auto_care_Cashier.MVC.Controller;
using Saliya_auto_care_Cashier.Notifications;

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
                txtcusaddress, txtcusNIC, txtcusmail, txtcusnumber, txtcusspec, txtvehiclecategory
            };
        }

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            if (IsAnyFieldEmpty())
            {
                ShowErrorAnimation();
                return;
            }

            try
            {
                controller.RegisterVehicle(
                    txtvehiclenum.Text,
                    txtvehiclecategory.Text,
                    txtvehiclemodel.Text,
                    txtvehicletype.Text,
                    txtcusname.Text,
                    txtcusaddress.Text,
                    txtcusNIC.Text,
                    txtcusmail.Text,
                    txtcusnumber.Text,
                    txtcusspec.Text
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Notificationbox.ShowError();
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
            translateTransform.BeginAnimation(TranslateTransform.XProperty,
                Saliya_auto_care_Cashier.Animations.ErrorAnimation.animation);
        }

        private void ResetToDefaultAppearance(Control control)
        {
            control.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD");
            control.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#6e6e6e"));
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
        private void btn_clear(object sender, RoutedEventArgs e)
        {
            // Find the Dashboard  and show the dialog
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("ClearRegisterDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }
    }
}