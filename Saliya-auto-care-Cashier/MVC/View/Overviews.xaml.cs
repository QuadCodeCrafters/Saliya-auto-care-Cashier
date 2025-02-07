using GMap.NET;
using GMap.NET.MapProviders;
using MySql.Data.MySqlClient;
using Saliya_auto_care_Cashier.MVC.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Saliya_auto_care_Cashier.MVC.View
{
    /// <summary>
    /// Interaction logic for Overviews.xaml
    /// </summary>
    public partial class Overviews : UserControl
    {
        private Border selectedBorder;
        private Button selectedButton;
        private readonly DatabaseConnectionMS connection;

        public Overviews()
        {
            InitializeComponent();
            InitializeMap();
            connection = new DatabaseConnectionMS();
            LoadCarrierServiceHistory();
            LoadDriversAndVehicles();  // Add this line
        }


        private void InitializeMap()
        {
            MapControl.MapProvider = GMapProviders.OpenStreetMap;
            MapControl.Position = new PointLatLng(7.2906, 80.6337); // Coordinates for Kandy, Sri Lanka
            MapControl.MinZoom = 2;
            MapControl.MaxZoom = 17;
            MapControl.Zoom = 10;
            MapControl.ShowCenter = false;

            // Enable map dragging
            MapControl.DragButton = System.Windows.Input.MouseButton.Left;
            MapControl.CanDragMap = true;
        }

        private void Shipment_MouseDown(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BorderBrush = Brushes.White; // Reset previous selection
            }

            if (sender is Button clickedButton)
            {
                clickedButton.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215)); // Highlight blue
                selectedButton = clickedButton;
            }
        }

        private void LoadCarrierServiceHistory()
        {
            try
            {
                using (SqlConnection conn = connection.GetConnection()) // Get connection from DatabaseConnectionMS
                {
                    //for debuging
                   // MessageBox.Show("Connection Successful!", "Database Connection", MessageBoxButton.OK, MessageBoxImage.Information);

                    string query = @"
                                    SELECT serviceID, firstName, lastName, NIC, phone, mail, brand, vehiclePlateNumber, type, emID, billedStatus, price
                                    FROM carrierServiceCustomers
                                    WHERE Date = CAST(GETDATE() AS DATE)";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    CarrierServiceDataGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadDriversAndVehicles()
        {
            string query = @"
                            SELECT e.Name AS DriverName, cv.RegistrationNumber, cv.Model
                            FROM Employee e
                            JOIN CarrierVehicle cv ON e.EmployeeID = cv.DriverID
                            WHERE e.Position = 'Driver' AND e.Status = 'Active'";

            using (MySqlConnection conn = new MySqlConnection(new DatabaseStringModel().ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string driverName = reader["DriverName"].ToString();
                                string vehicleInfo = $"{reader["Model"]} ({reader["RegistrationNumber"]})";
                                CreateDriverButton(driverName, vehicleInfo);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading drivers and vehicles: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateDriverButton(string driverName, string vehicleInfo)
        {
            Button btn = new Button
            {
                Style = (Style)FindResource("ButtonStyle"),
                Height = 80,
                Background = Brushes.White,
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.White,
                Margin = new Thickness(0, 0, 0, 10),
                Tag = new { DriverName = driverName, VehicleInfo = vehicleInfo }
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Image img = new Image
            {
                Source = new BitmapImage(new Uri("/Images/truckcarrier.png", UriKind.Relative)),
                Height = 70,
                Margin = new Thickness(-120, 0, 0, 0)
            };
            Grid.SetColumn(img, 0);

            StackPanel textPanel = new StackPanel();
            textPanel.Children.Add(new TextBlock { Text = $"Driver: {driverName}", FontSize = 17, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 50, 5) });
            textPanel.Children.Add(new TextBlock { Text = $"Vehicle: {vehicleInfo}", FontSize = 17, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 5, 50, 0) });
            Grid.SetColumn(textPanel, 1);

            grid.Children.Add(img);
            grid.Children.Add(textPanel);

            btn.Content = grid;
            btn.Click += DriverButton_Click;

            DriverButtonsPanel.Children.Add(btn);
        }

        private void DriverButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                dynamic tag = clickedButton.Tag;
                string driverName = tag.DriverName;
                string vehicleInfo = tag.VehicleInfo;

                // Update the basic details in the right panel
                txtname.Text = driverName;
                txtplate.Text = vehicleInfo.Split('(', ')')[1].Trim();  // Extract plate number
                txtbrand.Text = vehicleInfo.Split('(')[0].Trim();  // Extract vehicle model

                // Fetch additional details from the database
                using (MySqlConnection conn = new MySqlConnection(new DatabaseStringModel().ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = @"
                                        SELECT e.Phone, e.Mail, cv.Model, cv.Capacity
                                        FROM Employee e
                                        JOIN CarrierVehicle cv ON e.EmployeeID = cv.DriverID
                                        WHERE e.Name = @DriverName AND cv.RegistrationNumber = @PlateNumber";

                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@DriverName", driverName);
                            cmd.Parameters.AddWithValue("@PlateNumber", txtplate.Text);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtphone.Text = reader["Phone"].ToString();
                                    txtemail.Text = reader["Mail"].ToString();
                                    txtpype.Text = reader["Model"].ToString();  // Assuming 'Model' can be used as 'Type'

                                    // For price and tax, we'll use placeholder values
                                    // In a real scenario, you might want to fetch these from a different table
                                    // or calculate them based on some business logic
                                    txtprice.Text = "Rs.6000.00";
                                    txttax.Text = "Rs.500.00";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error fetching driver details: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


    }
}
