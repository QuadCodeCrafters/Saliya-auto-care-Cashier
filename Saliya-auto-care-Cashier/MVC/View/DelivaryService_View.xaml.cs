using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class DelivaryService_View : UserControl
    {
        private const string ConnectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";
        private const double PricePerKm = 50; 
        private static readonly (double Lat, double Lon) AutoCareCenter = (7.2906, 80.6337); 

        public DelivaryService_View()
        {
            InitializeComponent();
            LoadServiceRequests();
        }

        private async void LoadServiceRequests()
        {
            List<CarrierServiceRequest> requests = await GetServiceRequestsFromDatabase();
            ServiceRequestsDataGrid.ItemsSource = requests;
        }

        private async Task<List<CarrierServiceRequest>> GetServiceRequestsFromDatabase()
        {
            List<CarrierServiceRequest> requests = new List<CarrierServiceRequest>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM carrierServiceCustomers WHERE approvalStatus = 0";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            CarrierServiceRequest request = new CarrierServiceRequest
                            {
                                serviceID = reader.GetInt32(0),
                                firstName = reader.GetString(1),
                                lastName = reader.GetString(2),
                                NIC = reader.GetString(3),
                                Date = reader.GetDateTime(4),
                                address = reader.GetString(5),
                                phone = reader.GetString(6),
                                mail = reader.GetString(7),
                                brand = reader.GetString(8),
                                vehiclePlateNumber = reader.GetString(9),
                                problem = reader.IsDBNull(10) ? null : reader.GetString(10),
                                accidentLocationAddress = reader.GetString(11),
                                approvalStatus = reader.GetBoolean(12),
                                emID = reader.GetString(13),
                                billedStatus = reader.GetBoolean(14),
                                Latitude = reader.GetDouble(15),
                                Longitude = reader.GetDouble(16)
                            };

                            request.Distance = CalculateDistance(AutoCareCenter.Lat, AutoCareCenter.Lon, request.Latitude, request.Longitude);
                            request.TotalBill = CalculateTotalBill(request.Distance);

                            requests.Add(request);
                        }
                    }
                }
            }

            return requests;
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; 

            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private double CalculateTotalBill(double distance)
        {
            return distance * PricePerKm;
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            CarrierServiceRequest request = (CarrierServiceRequest)((Button)sender).DataContext;
            UpdateApprovalStatus(request.serviceID, true);
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            CarrierServiceRequest request = (CarrierServiceRequest)((Button)sender).DataContext;
            UpdateApprovalStatus(request.serviceID, false);
        }

        private async void UpdateApprovalStatus(int serviceID, bool approved)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE carrierServiceCustomers SET approvalStatus = @ApprovalStatus WHERE serviceID = @ServiceID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApprovalStatus", approved);
                    command.Parameters.AddWithValue("@ServiceID", serviceID);
                    await command.ExecuteNonQueryAsync();
                }
            }

            LoadServiceRequests();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadServiceRequests();
        }

        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            CarrierServiceRequest selectedRequest = (CarrierServiceRequest)ServiceRequestsDataGrid.SelectedItem;
            if (selectedRequest != null)
            {
                ShowRequestDetails(selectedRequest);
            }
            else
            {
                MessageBox.Show("Please select a request to view details.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowRequestDetails(CarrierServiceRequest request)
        {
            MessageBox.Show($"Service ID: {request.serviceID}\n" +
                            $"Name: {request.FullName}\n" +
                            $"NIC: {request.NIC}\n" +
                            $"Date: {request.Date}\n" +
                            $"Address: {request.address}\n" +
                            $"Phone: {request.phone}\n" +
                            $"Email: {request.mail}\n" +
                            $"Brand: {request.brand}\n" +
                            $"Vehicle Plate Number: {request.vehiclePlateNumber}\n" +
                            $"Problem: {request.problem}\n" +
                            $"Accident Location: {request.accidentLocationAddress}\n" +
                            $"Latitude: {request.Latitude}\n" +
                            $"Longitude: {request.Longitude}\n" +
                            $"Distance: {request.Distance:F2} km\n" +
                            $"Total Bill: {request.TotalBill:C}",
                            "Request Details",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }

    public class CarrierServiceRequest
    {
        public int serviceID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string FullName => $"{firstName} {lastName}";
        public string NIC { get; set; }
        public DateTime Date { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string brand { get; set; }
        public string vehiclePlateNumber { get; set; }
        public string problem { get; set; }
        public string accidentLocationAddress { get; set; }
        public bool approvalStatus { get; set; }
        public string emID { get; set; }
        public bool billedStatus { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }
        public double TotalBill { get; set; }
    }
}