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
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading;

namespace Saliya_auto_care_Cashier
{
    public partial class Dashboard : Window
    {
        private CategoryViewModel CategoryViewModel;

        private Inventory_View Inventory;
        //private PaintJobs_View paintJobs;
        private Register_View Register;
        private VehicleHistory_View History;
        private DelivaryService_View Carrier;

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

        private ObservableCollection<Invoice> invoices;
        private Timer refreshTimer;
        private bool isLoading = false;

        public Dashboard()
        {
            InitializeComponent();
            RequiredFields();
            LoadViews();

            sharename = new Sharedname();
            Bill_VIew.name = sharename;

            sharecustomeraddress = new Sharedaddress();
            Bill_VIew.address = sharecustomeraddress;

            sharevehicletype = new Sharedtype();
            Bill_VIew.type = sharevehicletype;

            sharevehiclenumber = new Sharednumber();
            Bill_VIew.number = sharevehiclenumber;

            conn = new DatabaseStringModel(); // conn

            CategoryViewModel = new CategoryViewModel();

            invoices = new ObservableCollection<Invoice>();
            HistoryDataGrid.ItemsSource = invoices;

            // Initialize timer to refresh every 5 seconds
            refreshTimer = new Timer(RefreshData, null, 0, 5000);

            LoadInvoiceData();
        }

        private void LoadViews()
        {
            try
            {
                Inventory = new Inventory_View();
                //paintJobs = new PaintJobs_View();
                Register = new Register_View();
                History = new VehicleHistory_View();
                Carrier = new DelivaryService_View();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong: {ex.Message}");
            }
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
                fContainer.Content = Register;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_menu(object sender, RoutedEventArgs e)
        {
            //Set the same Navigation method to the Menu_View.xaml.cs
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
                fContainer.Content = History;
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
                fContainer.Content = Inventory;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Delivary_Service(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Content = Carrier;
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
                                    LEFT JOIN carrierServiceCustomers csc ON vr.VehicleNumber = csc.vehiclePlateNumber
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

                                // Assign vehicleregister data to shared properties
                                sharename.CustomerName = Name;
                                sharecustomeraddress.CustomerAddress = CustomerAddress;
                                sharevehicletype.VehicleType = VehicleType;
                                sharevehiclenumber.VehicleNumber = VehicleNumber;

                                //send the VehicleNumber to the CategoryViewModel
                                //CategoryViewModel.sendvehicleno(VehicleNumber);

                                // Data from carrierServiceCustomers
                                if (!reader.IsDBNull(reader.GetOrdinal("vehiclePlateNumber")))
                                {
                                    decimal Price = reader.GetDecimal("price");
                                    decimal Tax = reader.GetDecimal("tax");
                                    string BilledStatus = reader.GetString("billedStatus");
                                    string PlateNumber = reader.GetString("vehiclePlateNumber");

                                    decimal Total = Price + Tax;
                                    decimal Qty = 1;

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


                                        // Update Bill_View shared properties
                                        Bill_VIew.sharedPrice.Price = (double)Price;
                                        Bill_VIew.sharedTax.Tax = (double)Tax;
                                        Bill_VIew.sharedProduct.Description = ("Carrier Service: " + PlateNumber);
                                        Bill_VIew.sharedTotal.Amount = (double)Total;
                                        Bill_VIew.sharedQty.Quantity = (double)Qty;

                                        Cusname.Text = "Owner: " + Name;
                                        Notificationbox.ShowSuccess();
                                    }
                                }
                                else
                                {
                                    // No matching carrier service record found
                                    MessageBox.Show("No carrier service data found for this vehicle.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
              

                            // Clear the form
                            cmbcarriername.SelectedIndex = -1;//to clear the selected item
                            cmbdrivername.SelectedIndex = -1;
                            txtcusmobile.Clear();
                            txtcusname.Clear();

                            MessageBox.Show(" Success! ,In here when new column added to the DB new notification need to go to the Mobile Appp and need to have a ststus");

                            Notificationbox.ShowSuccess();
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
                Notificationbox.ShowError();
            }
        }

        private void Buttonhistoryclear_Click(object sender, RoutedEventArgs e)
        {
            // Clear the text the function is located in History_View
            if (fContainer.Content is VehicleHistory_View historyView)
            {
                historyView.ClearAll();
            }
            else
            {
                MessageBox.Show("History_View is not currently loaded.");
                Notificationbox.ShowError();
            }
        }

        private void ButtonRegisterclear_Click(object sender, RoutedEventArgs e)
        {
            // Clear the text the function is located in Register_View
            if (fContainer.Content is Register_View RegisterView)
            {
                RegisterView.ClearAllFields();
            }
            else
            {
                MessageBox.Show("Register_View is not currently loaded.");
                Notificationbox.ShowError();
            }
        }

        private void RefreshData(object state)
        {
            // Avoid multiple simultaneous refreshes
            if (isLoading) return;

            // Update UI on the UI thread
            Dispatcher.Invoke(() =>
            {
                LoadInvoiceData();
            });
        }

        public class Invoice
        {
            public string InvoiceID { get; set; }
            public string Name { get; set; }
            public string VehicleType { get; set; }
            public string VehicleID { get; set; }
            public decimal TotalAmount { get; set; }
            public decimal PaidAmount { get; set; }
            public decimal Balance { get; set; }
        }

        private void LoadInvoiceData()
        {
            string connectionString = conn.ConnectionString;
            string query = "SELECT InvoiceID, CustomerName, VehicleType, VehicleNumber, TotalAmount, PaidAmount, Balance " + "FROM Invoice WHERE Date = CURDATE()";

            ObservableCollection<Invoice> invoices = new ObservableCollection<Invoice>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                invoices.Add(new Invoice
                                {
                                    InvoiceID = reader["InvoiceID"].ToString(),
                                    Name = reader["CustomerName"].ToString(),
                                    VehicleType = reader["VehicleType"].ToString(),
                                    VehicleID = reader["VehicleNumber"].ToString(),
                                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                    PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                                    Balance = Convert.ToDecimal(reader["Balance"])
                                });
                            }
                        }
                    }

                    HistoryDataGrid.ItemsSource = invoices;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
