using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Saliya_auto_care_Cashier.Animations;
using Saliya_auto_care_Cashier.MVC.Controller;
using Saliya_auto_care_Cashier.MVC.Model;
using Saliya_auto_care_Cashier.Notifications;

namespace Saliya_auto_care_Cashier.MVC.View
{
    /// <summary>
    /// Interaction logic for VehicleHistory_View.xaml
    /// </summary>
    public partial class VehicleHistory_View : UserControl
    {
        private VehicleHistoryController controller;

        public VehicleHistory_View()
        {
            InitializeComponent();
            controller = new VehicleHistoryController();
        }

        private void ErrorAnimation()
        {
            txtvehiclenum.BorderBrush = Brushes.Red;
            txtvehiclenum.Foreground = new SolidColorBrush(Colors.Red);

            TranslateTransform translateTransform = new TranslateTransform();
            txtvehiclenum.RenderTransform = translateTransform;

            translateTransform.BeginAnimation(TranslateTransform.XProperty, Saliya_auto_care_Cashier.Animations.ErrorAnimation.animation); //imported from ErrorAnimation.cs

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                txtvehiclenum.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD"); // Reset to default border color
                txtvehiclenum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
                timer.Stop();
            };
            timer.Start();
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtvehiclenum.Text))
            {
                ErrorAnimation();

            }

            else
            {
                LoadVehicleInfo(txtvehiclenum.Text);
            }
        }

        private void LoadVehicleInfo(string vehicleNumber)
        {
            var vehicleInfo = controller.GetVehicleInfo(vehicleNumber);
            if (vehicleInfo != null)
            {
                txtownername.Text = vehicleInfo.OwnerName;
                txtownerphone.Text = vehicleInfo.PhoneNumber;
                txtemail.Text = vehicleInfo.Email;
                txtaddress.Text = vehicleInfo.Address;
                txtmodel.Text = vehicleInfo.VehicleModel;
                txtphone.Text = vehicleInfo.VehicleType;
                txtregistrationdate.Text = vehicleInfo.RegistrationDate;
                txtlastservicedate.Text = vehicleInfo.LastServiceDate;

                LoadServiceDates(vehicleNumber);
                LoadServiceHistory(vehicleNumber);
                LoadTotals(vehicleNumber);

                Notificationbox.ShowSuccess();
            }
            else
            {
                Notificationbox.ShowInfo();

                ErrorAnimation();
                //MessageBox.Show("Vehicle not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadServiceDates(string vehicleNumber)
        {
            var dates = controller.GetServiceDates(vehicleNumber);
            cmbdate.ItemsSource = dates;
            cmbdate.SelectedIndex = 0;
        }

        private void LoadServiceHistory(string vehicleNumber, string date = null)
        {
            var history = controller.GetServiceHistory(vehicleNumber, date);
            ServiceHistoryDataGrid.ItemsSource = history;
        }

        private void LoadTotals(string vehicleNumber, string date = null)
        {
            var totals = controller.GetTotals(vehicleNumber, date);
            if (totals != null)
            {
                txtsub.Text = string.Format("Rs {0:N2}", totals["Subtotal"]);
                txttax.Text = string.Format("Rs {0:N2}", totals["SalesTax"]);
                txtdiscount.Text = string.Format("Rs {0:N2}", totals["Discount"]);
                txttotal.Text = string.Format("Rs {0:N2}", totals["TotalAmount"]);
                txtcharges.Text = string.Format("Rs {0:N2}", totals["ServiceCharges"]);
                txtpaidamount.Text = string.Format("Rs {0:N2}", totals["PaidAmount"]);
                txtbalance.Text = string.Format("Rs {0:N2}", totals["Balance"]);

                //if (ServiceHistoryDataGrid.ItemsSource == null)
                //{
                //    txtsub.Text = string.Empty;
                //    txttax.Text = string.Empty;
                //    txtdiscount.Text = string.Empty;
                //    txttotal.Text = string.Empty;
                //    txtcharges.Text = string.Empty;
                //    txtpaidamount.Text = string.Empty;
                //    txtbalance.Text = string.Empty;
                //}
                //else
                //{

                //}

            }
        }

        private void cmbdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbdate.SelectedItem != null)
            {
                string selectedDate = cmbdate.SelectedItem.ToString();
                LoadServiceHistory(txtvehiclenum.Text, selectedDate);
                LoadTotals(txtvehiclenum.Text, selectedDate);
            }
        }


        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAllFields();
        }

        private void ClearAllFields()
        {
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("ClearAllDialogHostforhistory") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }

        public void ClearAll()
        {

            // Clear input fields
            txtvehiclenum.Text = string.Empty;

            // Clear vehicle info fields
            txtownername.Text = string.Empty;
            txtownerphone.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtmodel.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtregistrationdate.Text = string.Empty;
            txtlastservicedate.Text = string.Empty;

            // Clear service history
            ServiceHistoryDataGrid.ItemsSource = null;

            // Clear totals
            txtsub.Text = string.Empty;
            txttax.Text = string.Empty;
            txtdiscount.Text = string.Empty;
            txttotal.Text = string.Empty;
            txtcharges.Text = string.Empty;
            txtpaidamount.Text = string.Empty;
            txtbalance.Text = string.Empty;

            // Clear date filter
            cmbdate.ItemsSource = null;
            cmbdate.SelectedIndex = -1;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Find the Dashboard  and show the dialog
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("EditButtonDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }
    }

}

