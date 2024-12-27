using Saliya_auto_care_Cashier.MVC.Model;
using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Saliya_auto_care_Cashier.Notifications;
using Saliya_auto_care_Cashier.MVC.View;
using static Saliya_auto_care_Cashier.MVC.View.Bill_VIew;
using static Saliya_auto_care_Cashier.MVC.View.Categories_View;
using System.Collections.Generic;
using Saliya_auto_care_Cashier.MVVM.View;

namespace Saliya_auto_care_Cashier
{
    public partial class Dashboard : Window
    {
        private Bill_VIew billView;
        private List<Control> requiredFields;
        public Bill_VIew LoadedBillView { get; set; }
        public Categories_View LoadedCategoriesView { get; set; }
        public static object SharedInstance { get; internal set; }

        private Sharedname sharename;
        private Sharedaddress sharecustomeraddress;
        private Sharedtype sharevehicletype;
        private Sharednumber sharevehiclenumber;

        private readonly DatabaseStringModel conn; //DatabaseStringModel
        public Dashboard()
        {
            InitializeComponent();
            RequiredFields();

            sharename = new Sharedname();
            Bill_VIew.name = sharename;

            sharecustomeraddress = new Sharedaddress();
            Bill_VIew.address = sharecustomeraddress;

            sharevehicletype = new Sharedtype();
            Bill_VIew.type = sharevehicletype;

            sharevehiclenumber = new Sharednumber();
            Bill_VIew.number = sharevehiclenumber;

            conn = new DatabaseStringModel(); // conn

        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {

            Welcomepage w2 = new Welcomepage();
            w2.Show();
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                fadeOutAnimation.Completed += FadeOutAnimation_Completed;
                this.BeginAnimation(OpacityProperty, fadeOutAnimation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Login Out: {ex.Message}");
            }
        }

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/Register_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_nenu(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/Menu_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_VehicleHistory(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/VehicleHistory_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Inventory(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/Inventory_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

      /*  private void btn_Paint_Jobs(object sender, RoutedEventArgs e)
        {
            try
            {
                var billView = new Bill_VIew();
                LoadedBillView = billView;


                var categoriesView = new Categories_View();
                LoadedCategoriesView = categoriesView;


                fContainer.Navigate(categoriesView);


                fContainer.Navigate(new System.Uri("MVC/View/PaintJobs_View.xaml", UriKind.RelativeOrAbsolute));

                // MessageBox.Show($"LoadedBillView: {LoadedBillView != null}, LoadedCategoriesView: {LoadedCategoriesView != null}"); // Debugging
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }



        private void btn_Vehicle_Services(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/VehicleService_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Vehicle_Repairs(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/VehicleRepairs_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Spare_Parts(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/SpareParts_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }*/

        private void btn_Delivary_Service(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/DelivaryService_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void addbtn_cancel(object sender, RoutedEventArgs e)
        {
            txtloyalid.Text = "";
            Cusname.Text = "";
        }
        private void addbtn_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtloyalid.Text))
            {
                ErrorAnimation();
                IDError.Text = "Please Enter the ID";
                return;
            }

            string VHNUM = txtloyalid.Text;
            string connectionString = conn.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"SELECT 
                                    vr.CustomerName, 
                                    vr.CustomerAddress, 
                                    vr.VehicleType, 
                                    vr.VehicleNumber, 
                                    csc.price, 
                                    csc.tax,
                                    csc.vehiclePlateNumber,
                                    csc.billedStatus 
                                    FROM vehicleregister vr
                                    INNER JOIN carrierServiceCustomers csc ON vr.VehicleNumber = csc.vehiclePlateNumber
                                    WHERE vr.VehicleNumber = @VehicleNumber"
                    ;

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@VehicleNumber", VHNUM);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Data from vehicleregister
                                string Name = reader.GetString("CustomerName");
                                string CustomerAddress = reader.GetString("CustomerAddress");
                                string VehicleType = reader.GetString("VehicleType");
                                string VehicleNumber = reader.GetString("VehicleNumber");

                                // Data from carrierServiceCustomers
                                decimal Price = reader.GetDecimal("price");
                                decimal Tax = reader.GetDecimal("tax");
                                string BilledStatus = reader.GetString("billedStatus");
                                string PlateNumber = reader.GetString("vehiclePlateNumber");

                                decimal Total = Price + Tax;
                                decimal Qty = 1;

                                // Assign vehicleregister data to shared properties
                                sharename.CustomerName = Name;
                                sharecustomeraddress.CustomerAddress = CustomerAddress;
                                sharevehicletype.VehicleType = VehicleType;
                                sharevehiclenumber.VehicleNumber = VehicleNumber;



                                // Check BilledStatus
                                if (BilledStatus == "Billed")
                                {
                                    MessageBox.Show("This customer has already been billed for Carrier Service.", "Billed Status", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return;
                                }
                                else
                                {
                                    // Display Price and Tax for Debugging
                                    MessageBox.Show(
                                        $"Price: {Price.ToString("C")}\n Tax: {Tax.ToString("C")}\n PlateNumber: {PlateNumber}\n The Total:{Total}\n The Qty:{Qty}",
                                        "Price and Tax Details",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information
                                    );

                                    Bill_VIew.sharedPrice.Price = (double)Price; // Update the price
                                    Bill_VIew.sharedTax.Tax = (double)Tax;       // Update the tax
                                    Bill_VIew.sharedProduct.Description = ("Carrier Service: "+PlateNumber); // Update the plate number
                                    Bill_VIew.sharedTotal.Amount = (double)Total; // Update the total
                                    Bill_VIew.sharedQty.Quantity = (double)Qty; // Update the Qty


                                    Cusname.Text = "Owner: " + Name;
                                    Notificationbox.ShowSuccess();
                                }


                            }
                            else
                            {
                                IDError.Text = "No Vehicle Found";
                                ErrorAnimation();
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }



        private void txtloyalid_TextChanged(object sender, TextChangedEventArgs e)
        {
            IDError.Text = ""; // Clear the error message  
        }

        private void ErrorAnimation()
        {
            txtloyalid.BorderBrush = Brushes.Red;
            txtloyalid.Foreground = new SolidColorBrush(Colors.Red);

            TranslateTransform translateTransform = new TranslateTransform();
            txtloyalid.RenderTransform = translateTransform;

            translateTransform.BeginAnimation(TranslateTransform.XProperty, Saliya_auto_care_Cashier.Animations.ErrorAnimation.animation); //imported from ErrorAnimation.cs

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                txtloyalid.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD"); // Reset to default border color
                txtloyalid.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
                timer.Stop();
            };
            timer.Start();
        }

        private void ErrorAppearance(Control control)
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

        private void ShowError()
        {
            foreach (var field in requiredFields)
            {
                bool hasError = (field is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text)) ||
                                (field is ComboBox comboBox && comboBox.SelectedItem == null);

                if (hasError)
                {
                    ErrorAppearance(field);
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
                    DefaultAppearance(field);
                }
                timer.Stop();
            };
            timer.Start();
        }

        private void DefaultAppearance(Control control)
        {
            control.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD");
            control.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
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

        private void RequiredFields()
        {
            requiredFields = new List<Control>
            {
               txtcusmobile,txtcusname ,cmbcarriername,cmbdrivername
            };
        }

        private void ButtonSchedule_click(object sender, RoutedEventArgs e)
        {
            if (IsAnyFieldEmpty())
            {
                ShowError();
            }

            else
            {
                InsertData();
            }
        }

        private void goback(object sender, RoutedEventArgs e)
        {
            cmbcarriername.Items.Clear();
            cmbdrivername.Items.Clear();
            txtcusmobile.Text = "";
            txtcusname.Text = "";
        }


        private void ComboBoxtext()
        {
            string connectionString = conn.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string driverQuery = "SELECT Name FROM Employee WHERE Position = 'Driver'";
                    using (MySqlCommand cmd = new MySqlCommand(driverQuery, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbdrivername.Items.Add(reader["Name"].ToString());
                            }
                        }
                    }


                    string carrierQuery = "SELECT Model FROM CarrierVehicle"; // need to add the Available status
                    using (MySqlCommand cmd = new MySqlCommand(carrierQuery, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbcarriername.Items.Add(reader["Model"].ToString());
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }

                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                        if (connection.State == System.Data.ConnectionState.Closed)
                        {
                            //MessageBox.Show("The connection has been successfully closed.");  //for Debugging
                        }
                    }
                }

            }
        }

        private void btn_availability(object sender, RoutedEventArgs e)
        {
            ComboBoxtext();
        }

        public void InsertData()
        {
            string connectionString = conn.ConnectionString;

            // Get the data 
            string carrierName = cmbcarriername.SelectedItem?.ToString();
            string driverName = cmbdrivername.SelectedItem?.ToString();
            string customerMobile = txtcusmobile.Text.ToString();
            string customerName = txtcusname.Text.ToString();

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO SchedulePickup (CarrierName, DriverName, CustomerMobile, CustomerName) "+" VALUES (@CarrierName, @DriverName, @CustomerMobile, @CustomerName)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {

                        cmd.Parameters.AddWithValue("@CarrierName", carrierName);
                        cmd.Parameters.AddWithValue("@DriverName", driverName);
                        cmd.Parameters.AddWithValue("@CustomerMobile", customerMobile);
                        cmd.Parameters.AddWithValue("@CustomerName", customerName);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                           Notificationbox.ShowSuccess();

                            // Clear the form
                            cmbcarriername.SelectedIndex = -1;//to clear the selected item
                            cmbdrivername.SelectedIndex = -1;
                            txtcusmobile.Clear();
                            txtcusname.Clear();

                            MessageBox.Show("In here when new column added to the DB new notification need to go to the Mobile Appp ");
                        }
                        else
                        {
                            Notificationbox.ShowError();
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        private void Buttondashboardclear_Click(object sender, RoutedEventArgs e)
        {
            // Clear the text the function is located in Menu_View
            if (fContainer.Content is PaintJobs_View paintJobsView)
            {
                paintJobsView.ClearAll();
            }
            else
            {
                MessageBox.Show("PaintJobs_View is not currently loaded.");
            }
        }

    }
}
