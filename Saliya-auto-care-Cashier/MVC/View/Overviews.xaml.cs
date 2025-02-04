using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            LoadCarrierServiceHistory(); // Load data on initialization
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
                    MessageBox.Show("Connection Successful!", "Database Connection", MessageBoxButton.OK, MessageBoxImage.Information);

                    string query = @"
                        SELECT serviceID, firstName, lastName, NIC, phone, mail, brand, vehiclePlateNumber, accidentLocationAddress, emID, billedStatus, price
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
    }
}
