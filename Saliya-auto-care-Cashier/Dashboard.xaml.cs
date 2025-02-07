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
using Windows.Security.Credentials.UI;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Printing;
using System.Windows.Xps;
using Saliya_auto_care_Cashier.Mails;

namespace Saliya_auto_care_Cashier
{
    public partial class Dashboard : Window
    {
        private CategoryViewModel CategoryViewModel;

        private ObservableCollection<InvoiceItem> allBillItems;
        private ObservableCollection<InvoiceItem> refundItems;
        private readonly DatabaseStringModel conn;  // DatabaseStringModel

        private readonly DatabaseConnectionMS connection;  // DatabaseStringModel

        private string currentInvoiceID;

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

        private ObservableCollection<Invoice> invoices;
        private Timer refreshTimer;
        private bool isLoading = false;

        private double lastNumber, result;
        private SelectedOperator selectedOperator;

        public static string SelectedPrinter { get; private set; }


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

            connection = new DatabaseConnectionMS(); // conn

            CategoryViewModel = new CategoryViewModel();

            invoices = new ObservableCollection<Invoice>();
            HistoryDataGrid.ItemsSource = invoices;

            // Initialize timer to refresh every 5 seconds
            refreshTimer = new Timer(RefreshData, null, 0, 5000);

            LoadInvoiceData();

            allBillItems = new ObservableCollection<InvoiceItem>();
            refundItems = new ObservableCollection<InvoiceItem>();
            dgAllBillItems.ItemsSource = allBillItems;
            dgRefundList.ItemsSource = refundItems;

            this.KeyDown += MainWindow_KeyDown;

            // Check database connection on startup
            CheckDatabaseConnection();

            // Check database connection on startup
            CheckDatabaseConnectionMS();


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
                fContainernotification.Navigate(new System.Uri("MVC/View/EmptyView.xaml", UriKind.RelativeOrAbsolute));
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
                fContainernotification.Navigate(new System.Uri("MVC/View/LiveUpdates.xaml", UriKind.RelativeOrAbsolute));
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
                fContainernotification.Navigate(new System.Uri("MVC/View/EmptyView.xaml", UriKind.RelativeOrAbsolute));
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
                fContainernotification.Navigate(new System.Uri("MVC/View/EmptyView.xaml", UriKind.RelativeOrAbsolute));
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
                fContainernotification.Navigate(new System.Uri("MVC/View/EmptyView.xaml", UriKind.RelativeOrAbsolute));
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

            // MySQL Connection for vehicleregister table
            string mysqlConnectionString = conn.ConnectionString; // MySQL connection string

            // SQL Server Connection for carrierServiceCustomers table
            string mssqlConnectionString = connection.connectionString; // MS SQL Server connection string

            // Query for MySQL (vehicleregister table)
            string mysqlQuery = @"SELECT 
                                vr.CustomerName, 
                                vr.CustomerAddress, 
                                vr.VehicleType, 
                                vr.VehicleNumber
                            FROM vehicleregister vr
                            WHERE vr.VehicleNumber = @VehicleNumber";

            // Query for SQL Server (carrierServiceCustomers table)
            string mssqlQuery = @"SELECT 
                                csc.price, 
                                csc.tax,
                                csc.vehiclePlateNumber,
                                csc.billedStatus 
                            FROM carrierServiceCustomers csc
                            WHERE csc.vehiclePlateNumber = @VehiclePlateNumber";

            using (var mysqlConnection = new MySqlConnection(mysqlConnectionString))
            using (var mssqlConnection = new SqlConnection(mssqlConnectionString))
            {
                try
                {
                    // Open MySQL connection and execute query for vehicleregister table
                    mysqlConnection.Open();
                    using (MySqlCommand mysqlCmd = new MySqlCommand(mysqlQuery, mysqlConnection))
                    {
                        mysqlCmd.Parameters.AddWithValue("@VehicleNumber", VHNUM);

                        using (MySqlDataReader mysqlReader = mysqlCmd.ExecuteReader())
                        {
                            if (mysqlReader.Read())
                            {
                                // Data from vehicleregister (MySQL)
                                string Name = mysqlReader.GetString("CustomerName");
                                string CustomerAddress = mysqlReader.GetString("CustomerAddress");
                                string VehicleType = mysqlReader.GetString("VehicleType");
                                string VehicleNumber = mysqlReader.GetString("VehicleNumber");

                                // Assign vehicleregister data to shared properties
                                sharename.CustomerName = Name;
                                sharecustomeraddress.CustomerAddress = CustomerAddress;
                                sharevehicletype.VehicleType = VehicleType;
                                sharevehiclenumber.VehicleNumber = VehicleNumber;
                            }
                            else
                            {
                                IDError.Text = "No Vehicle Found";
                                ErrorAnimation();
                                return;
                            }
                        }
                    }

                    // Open SQL Server connection and execute query for carrierServiceCustomers table
                    mssqlConnection.Open();
                    using (SqlCommand mssqlCmd = new SqlCommand(mssqlQuery, mssqlConnection))
                    {
                        mssqlCmd.Parameters.AddWithValue("@VehiclePlateNumber", VHNUM);

                        using (SqlDataReader mssqlReader = mssqlCmd.ExecuteReader())
                        {
                            if (mssqlReader.Read())
                            {
                                // Data from carrierServiceCustomers (SQL Server)
                                decimal Price = 0;
                                decimal Tax = 0;
                                string BilledStatus = "";
                                string PlateNumber = "";

                                // Ensure correct handling of decimal values from SQL Server
                                if (!mssqlReader.IsDBNull(mssqlReader.GetOrdinal("price")))
                                {
                                    Price = mssqlReader.GetDecimal(mssqlReader.GetOrdinal("price"));
                                }

                                if (!mssqlReader.IsDBNull(mssqlReader.GetOrdinal("tax")))
                                {
                                    Tax = mssqlReader.GetDecimal(mssqlReader.GetOrdinal("tax"));
                                }

                                if (!mssqlReader.IsDBNull(mssqlReader.GetOrdinal("billedStatus")))
                                {
                                    BilledStatus = mssqlReader.GetString(mssqlReader.GetOrdinal("billedStatus"));
                                }

                                if (!mssqlReader.IsDBNull(mssqlReader.GetOrdinal("vehiclePlateNumber")))
                                {
                                    PlateNumber = mssqlReader.GetString(mssqlReader.GetOrdinal("vehiclePlateNumber"));
                                }

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
                                        $"Price: {Price.ToString("C")}\nTax: {Tax.ToString("C")}\nPlateNumber: {PlateNumber}\nThe Total: {Total}\nThe Qty: {Qty}",
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

                                    Cusname.Text = "Owner: " + sharename.CustomerName;
                                    Notificationbox.ShowSuccess();
                                }
                            }
                            else
                            {
                                // No matching carrier service record found
                                MessageBox.Show("No carrier service data found for this vehicle.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL Database error: " + ex.Message);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("SQL Server Database error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    if (mysqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        mysqlConnection.Close();
                    }

                    if (mssqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        mssqlConnection.Close();
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
               SendPickupMailAsync();
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
            string mysqlConnectionString = conn.ConnectionString; // MySQL Connection
            string mssqlConnectionString = connection.connectionString; // MS SQL Connection

            // Fetch drivers from MySQL
            using (var mysqlConnection = new MySqlConnection(mysqlConnectionString))
            {
                try
                {
                    mysqlConnection.Open();
                    string driverQuery = "SELECT Name FROM Employee WHERE Position = 'Driver'";

                    using (MySqlCommand cmd = new MySqlCommand(driverQuery, mysqlConnection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbdrivername.Items.Add(reader["Name"].ToString());
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL error: " + ex.Message);
                }
                finally
                {
                    if (mysqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        mysqlConnection.Close();
                    }
                }
            }

            // Fetch available carrier vehicles from MS SQL Server
            using (var mssqlConnection = new SqlConnection(mssqlConnectionString))
            {
                try
                {
                    mssqlConnection.Open();
                    string carrierQuery = "SELECT Model FROM carrierVehiclesInfo WHERE availabilityStatus = 'Available'"; // Only fetch available vehicles

                    using (SqlCommand cmd = new SqlCommand(carrierQuery, mssqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbcarriername.Items.Add(reader["Model"].ToString());
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("MS SQL Server error: " + ex.Message);
                }
                finally
                {
                    if (mssqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        mssqlConnection.Close();
                    }
                }
            }
        }


        private void btn_availability(object sender, RoutedEventArgs e)
        {
            ComboBoxtext();
        }


        public async Task SendPickupMailAsync()
        {
            try
            {
                EmailService emailService = new EmailService();
                string driverName = cmbdrivername.SelectedItem?.ToString();
                string customerName = txtcusname.Text;
                string customerPhoneNumber = txtcusmobile.Text;

                if (string.IsNullOrEmpty(driverName) || string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(customerPhoneNumber))
                {
                    MessageBox.Show("Please fill in all required fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Fetch the driver's email address from your database
                string driverEmail = await GetDriverEmailAsync(driverName);

                if (string.IsNullOrEmpty(driverEmail))
                {
                    MessageBox.Show("Could not find the driver's email address.", "Email Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string pickupMailContent = emailService.PickupMailContent(driverName, customerName, customerPhoneNumber);

                bool emailSent = await emailService.SendEmailAsync(
                    driverEmail,
                    driverName,
                    "New Pickup Assignment!",
                    pickupMailContent
                );

                if (!emailSent)
                {
                    MessageBox.Show(
                        $"Failed to send pickup assignment email to {driverName}. " +
                        "Please check your network connection and try again.",
                        "Email Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                }
                else
                {
                    MessageBox.Show(
                        $"Pickup assignment email sent successfully to {driverName}.",
                        "Email Sent",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the email: {ex.Message}",
                    "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<string> GetDriverEmailAsync(string driverName)
        {
            string mysqlConnectionString = conn.ConnectionString; // MySQL Connection

            using (var mysqlConnection = new MySqlConnection(mysqlConnectionString))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    string emailQuery = "SELECT Mail FROM Employee WHERE Name = @DriverName AND Position = 'Driver'";

                    using (MySqlCommand cmd = new MySqlCommand(emailQuery, mysqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@DriverName", driverName);
                        object result = await cmd.ExecuteScalarAsync();
                        return result?.ToString();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL error: " + ex.Message);
                    return null;
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

        private async void btn_Poweroff(object sender, RoutedEventArgs e)
        {
            // Step 1: Authenticate using Windows Hello
            var result = await AuthenticateWithWindowsHello();

            if (result)
            {
                // Get the PowerOffDialogHost
                var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
                if (dashboardWindow != null)
                {
                    var dialogHost = dashboardWindow.FindName("PowerOffDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                    if (dialogHost != null)
                    {
                        dialogHost.IsOpen = true;  // Open the dialog
                    }
                }
            }
            else
            {
                // Show failure message
                MessageBox.Show("Authentication failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> AuthenticateWithWindowsHello()
        {
            try
            {
                var availability = await Windows.Security.Credentials.UI.UserConsentVerifier.CheckAvailabilityAsync();

                if (availability == Windows.Security.Credentials.UI.UserConsentVerifierAvailability.Available)
                {
                    var verificationResult = await Windows.Security.Credentials.UI.UserConsentVerifier.RequestVerificationAsync("Authenticate to proceed");
                    return verificationResult == Windows.Security.Credentials.UI.UserConsentVerificationResult.Verified;
                }
                else
                {
                    MessageBox.Show("Windows Hello is not available on this device.",
                                    "Authentication Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during authentication: {ex.Message}",
                                "Authentication Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error
                );
            }

            return false;
        }

        private void Poweroff(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private void CheckDatabaseConnectionMS()
        {
            DatabaseConnectionMS dbConnection = new DatabaseConnectionMS(); // Create a new instance
            if (dbConnection.TestConnection()) // Test the connection
            {
                MessageBox.Show("MS SQL Connection: Successful", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("MS SQL Connection: Failed", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckDatabaseConnection()
        {
            try
            {
                string connectionString = conn.ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                   // MessageBox.Show("Database connection successful!", "Connection Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to the database. Error: {ex.Message}", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentInvoiceID = txtInvoiceNum.Text.Trim();
            if (string.IsNullOrEmpty(currentInvoiceID))
            {
                MessageBox.Show("Please enter an invoice number.");
                return;
            }

            allBillItems.Clear();
            refundItems.Clear();
            UpdateTotalRefundAmount();

            try
            {
                string connectionString = conn.ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT id.Description, id.Quantity, id.Price, id.Amount 
                                     FROM InvoiceDetails id 
                                     INNER JOIN Invoice i ON id.InvoiceID = i.InvoiceID 
                                     WHERE i.InvoiceID = @InvoiceID";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InvoiceID", currentInvoiceID);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                allBillItems.Add(new InvoiceItem
                                {
                                    Description = reader["Description"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Amount = Convert.ToDecimal(reader["Amount"])
                                });
                            }
                        }
                    }
                }

                if (allBillItems.Count == 0)
                {
                    MessageBox.Show("No items found for the given invoice number.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching invoice details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgAllBillItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAllBillItems.SelectedItem is InvoiceItem selectedItem)
            {
                allBillItems.Remove(selectedItem);
                refundItems.Add(selectedItem);
                UpdateTotalRefundAmount();
            }
        }

        private void UpdateTotalRefundAmount()
        {
            decimal totalRefund = refundItems.Sum(item => item.Amount);
            txtTotalRefundAmount.Text = "Rs " + totalRefund.ToString("N2");
        }

        private void BtnRefund_Click(object sender, RoutedEventArgs e)
        {
            if (refundItems.Count == 0)
            {
                MessageBox.Show("Please select items to refund.");
                return;
            }

            try
            {
                string connectionString = conn.ConnectionString;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Insert into Refund table
                            string insertRefundQuery = @"INSERT INTO Refund (InvoiceID, CustomerName, RefundDate, TotalAmount) 
                                                         VALUES (@InvoiceID, (SELECT CustomerName FROM Invoice WHERE InvoiceID = @InvoiceID), @RefundDate, @TotalAmount);
                                                         SELECT LAST_INSERT_ID();";
                            int refundID;
                            decimal totalRefundAmount = refundItems.Sum(item => item.Amount);

                            using (MySqlCommand cmd = new MySqlCommand(insertRefundQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@InvoiceID", currentInvoiceID);
                                cmd.Parameters.AddWithValue("@RefundDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@TotalAmount", totalRefundAmount);
                                refundID = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // Insert into RefundDetails table
                            string insertRefundDetailsQuery = @"INSERT INTO RefundDetails (RefundID, Description, Quantity, Price, Amount) 
                                                                VALUES (@RefundID, @Description, @Quantity, @Price, @Amount)";
                            foreach (var item in refundItems)
                            {
                                using (MySqlCommand cmd = new MySqlCommand(insertRefundDetailsQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@RefundID", refundID);
                                    cmd.Parameters.AddWithValue("@Description", item.Description);
                                    cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                                    cmd.Parameters.AddWithValue("@Price", item.Price);
                                    cmd.Parameters.AddWithValue("@Amount", item.Amount);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // Delete from InvoiceDetails
                            string deleteInvoiceDetailsQuery = "DELETE FROM InvoiceDetails WHERE InvoiceID = @InvoiceID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteInvoiceDetailsQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@InvoiceID", currentInvoiceID);
                                cmd.ExecuteNonQuery();
                            }

                            // Delete from Invoice
                            string deleteInvoiceQuery = "DELETE FROM Invoice WHERE InvoiceID = @InvoiceID";
                            using (MySqlCommand cmd = new MySqlCommand(deleteInvoiceQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@InvoiceID", currentInvoiceID);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Refund processed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearAll();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("An error occurred while processing the refund. The transaction was rolled back.", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing refund: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClearAll()
        {
            txtInvoiceNum.Clear();
            allBillItems.Clear();
            refundItems.Clear();
            UpdateTotalRefundAmount();
            currentInvoiceID = null;
        }

        private void OnNumberClick(object sender, RoutedEventArgs e)
        {
            int selectedValue = int.Parse((sender as Button).Content.ToString());

            if (ResultText.Text == "0")
                ResultText.Text = $"{selectedValue}";
            else
                ResultText.Text += $"{selectedValue}";
        }

        private void OnOperatorClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ResultText.Text, out lastNumber))
            {
                ResultText.Text = "0";
            }

            if (sender == AdditionButton)
                selectedOperator = SelectedOperator.Addition;
            else if (sender == SubtractionButton)
                selectedOperator = SelectedOperator.Subtraction;
            else if (sender == MultiplicationButton)
                selectedOperator = SelectedOperator.Multiplication;
            else if (sender == DivisionButton)
                selectedOperator = SelectedOperator.Division;
        }

        private void OnEqualsClick(object sender, RoutedEventArgs e)
        {
            double newNumber;
            if (double.TryParse(ResultText.Text, out newNumber))
            {
                switch (selectedOperator)
                {
                    case SelectedOperator.Addition:
                        result = SimpleMath.Add(lastNumber, newNumber);
                        break;
                    case SelectedOperator.Subtraction:
                        result = SimpleMath.Subtract(lastNumber, newNumber);
                        break;
                    case SelectedOperator.Multiplication:
                        result = SimpleMath.Multiply(lastNumber, newNumber);
                        break;
                    case SelectedOperator.Division:
                        result = SimpleMath.Divide(lastNumber, newNumber);
                        break;
                }

                ResultText.Text = result.ToString();
            }
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            ResultText.Text = "0";
            lastNumber = 0;
            result = 0;
        }

        private void OnDecimalClick(object sender, RoutedEventArgs e)
        {
            if (!ResultText.Text.Contains("."))
                ResultText.Text += ".";
        }

        private void OnPercentClick(object sender, RoutedEventArgs e)
        {
            double tempNumber;
            if (double.TryParse(ResultText.Text, out tempNumber))
            {
                tempNumber /= 100;
                if (lastNumber != 0)
                    tempNumber *= lastNumber;
                ResultText.Text = tempNumber.ToString();
            }
        }

        private void OnSquareRootClick(object sender, RoutedEventArgs e)
        {
            double tempNumber;
            if (double.TryParse(ResultText.Text, out tempNumber))
            {
                ResultText.Text = Math.Sqrt(tempNumber).ToString();
            }
        }

        private void OnSquareClick(object sender, RoutedEventArgs e)
        {
            double tempNumber;
            if (double.TryParse(ResultText.Text, out tempNumber))
            {
                ResultText.Text = (tempNumber * tempNumber).ToString();
            }
        }

        private void OnReciprocalClick(object sender, RoutedEventArgs e)
        {
            double tempNumber;
            if (double.TryParse(ResultText.Text, out tempNumber))
            {
                if (tempNumber != 0)
                {
                    ResultText.Text = (1 / tempNumber).ToString();
                }
                else
                {
                    MessageBox.Show("Cannot divide by zero", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnNegateClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ResultText.Text, out double tempNumber))
            {
                ResultText.Text = (-tempNumber).ToString();
            }
        }

        private void OnBackspaceClick(object sender, RoutedEventArgs e)
        {
            if (ResultText.Text.Length > 1)
            {
                ResultText.Text = ResultText.Text.Substring(0, ResultText.Text.Length - 1);
                ResultText.Text = "0";
            }
            else
            {
                ResultText.Text = "0";
            }
        }
        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.NumPad0:
                case System.Windows.Input.Key.D0:
                    OnNumberClick(new Button { Content = "0" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad1:
                case System.Windows.Input.Key.D1:
                    OnNumberClick(new Button { Content = "1" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad2:
                case System.Windows.Input.Key.D2:
                    OnNumberClick(new Button { Content = "2" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad3:
                case System.Windows.Input.Key.D3:
                    OnNumberClick(new Button { Content = "3" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad4:
                case System.Windows.Input.Key.D4:
                    OnNumberClick(new Button { Content = "4" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad5:
                case System.Windows.Input.Key.D5:
                    OnNumberClick(new Button { Content = "5" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad6:
                case System.Windows.Input.Key.D6:
                    OnNumberClick(new Button { Content = "6" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad7:
                case System.Windows.Input.Key.D7:
                    OnNumberClick(new Button { Content = "7" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad8:
                case System.Windows.Input.Key.D8:
                    OnNumberClick(new Button { Content = "8" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.NumPad9:
                case System.Windows.Input.Key.D9:
                    OnNumberClick(new Button { Content = "9" }, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Decimal:
                case System.Windows.Input.Key.OemPeriod:
                    OnDecimalClick(new Button(), new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Add:
                    OnOperatorClick(AdditionButton, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Subtract:
                    OnOperatorClick(SubtractionButton, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Multiply:
                    OnOperatorClick(MultiplicationButton, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Divide:
                    OnOperatorClick(DivisionButton, new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Enter:
                    OnEqualsClick(new Button(), new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Back:
                    OnBackspaceClick(new Button(), new RoutedEventArgs());
                    break;
                case System.Windows.Input.Key.Escape:
                    OnClearClick(new Button(), new RoutedEventArgs());
                    break
                ;
            }

        }

        private void BtneditSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string vehicleNumber = txtvehiclenum.Text;
                if (string.IsNullOrWhiteSpace(vehicleNumber))
                {
                    MessageBox.Show("Please enter a vehicle number.");
                    return;
                }
                string connectionString = conn.ConnectionString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM vehicleregister WHERE VehicleNumber = @VehicleNumber";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtvehiclecategory.Text = reader["VehicleCategory"].ToString();
                                txtvehicletype.Text = reader["VehicleType"].ToString();
                                txtvehiclemodel.Text = reader["VehicleModel"].ToString();
                                txtcusnameedit.Text = reader["CustomerName"].ToString();
                                txtcusaddress.Text = reader["CustomerAddress"].ToString();
                                txtcusNIC.Text = reader["CustomerNIC"].ToString();
                                txtcusmail.Text = reader["CustomerEmail"].ToString();
                                txtcusnumber.Text = reader["CustomerPhone"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No vehicle found with the given number.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Btnupdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = conn.ConnectionString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE vehicleregister 
                                     SET VehicleCategory = @VehicleCategory, 
                                         VehicleType = @VehicleType, 
                                         VehicleModel = @VehicleModel, 
                                         CustomerName = @CustomerName, 
                                         CustomerAddress = @CustomerAddress, 
                                         CustomerNIC = @CustomerNIC, 
                                         CustomerEmail = @CustomerEmail, 
                                         CustomerPhone = @CustomerPhone 
                                     WHERE VehicleNumber = @VehicleNumber";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleNumber", txtvehiclenum.Text);
                        command.Parameters.AddWithValue("@VehicleCategory", txtvehiclecategory.Text);
                        command.Parameters.AddWithValue("@VehicleType", txtvehicletype.Text);
                        command.Parameters.AddWithValue("@VehicleModel", txtvehiclemodel.Text);
                        command.Parameters.AddWithValue("@CustomerName", txtcusnameedit.Text);
                        command.Parameters.AddWithValue("@CustomerAddress", txtcusaddress.Text);
                        command.Parameters.AddWithValue("@CustomerNIC", txtcusNIC.Text);
                        command.Parameters.AddWithValue("@CustomerEmail", txtcusmail.Text);
                        command.Parameters.AddWithValue("@CustomerPhone", txtcusnumber.Text);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Vehicle registration updated successfully.");
                            Notificationbox.ShowSuccess();
                            ClearAllFields();
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Please check the vehicle number.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating: {ex.Message}");
            }
        }

        private void ClearAllFields()
        {
            txtvehiclenum.Clear();
            txtvehiclecategory.Text = string.Empty;
            txtvehicletype.Text = string.Empty;
            txtvehiclemodel.Clear();
            txtcusnameedit.Clear();
            txtcusaddress.Clear();
            txtcusNIC.Clear();
            txtcusmail.Clear();
            txtcusnumber.Clear();
        }


        public enum SelectedOperator
        {
            Addition,
            Subtraction,
            Multiplication,
            Division
        }

        public class SimpleMath
        {
            public static double Add(double n1, double n2) => n1 + n2;
            public static double Subtract(double n1, double n2) => n1 - n2;
            public static double Multiply(double n1, double n2) => n1 * n2;
            public static double Divide(double n1, double n2) => n2 == 0 ? throw new DivideByZeroException() : n1 / n2;
        }

        public class InvoiceItem
        {
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal Amount { get; set; }
        }

        private void btnprinter_click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrinterPopupBox.IsPopupOpen = true;
                var printers = System.Drawing.Printing.PrinterSettings.InstalledPrinters;

                var popupBox = FindName("PrinterPopupBox") as MaterialDesignThemes.Wpf.PopupBox;
                if (popupBox != null)
                {
                    var stackPanel = popupBox.PopupContent as StackPanel;
                    if (stackPanel != null)
                    {
                        stackPanel.Children.Clear();

                        foreach (string printer in printers)
                        {
                            var button = new Button
                            {
                                Content = printer,
                                Style = (Style)FindResource("MaterialDesignFlatButton"),
                                Width = 250,
                                Height=40,
                                FontSize=15,
                                HorizontalContentAlignment = HorizontalAlignment.Left
                            };

                            button.Click += (s, args) =>
                            {
                                SelectedPrinter = printer;
                                popupBox.IsPopupOpen = false;
                                //debugging
                                MessageBox.Show($"Selected printer: {printer}", "Printer Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                            };

                            stackPanel.Children.Add(button);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading printers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btninternet_click(object sender, RoutedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

