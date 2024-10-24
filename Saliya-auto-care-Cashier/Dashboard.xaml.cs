using System;
using System.Windows;

namespace Saliya_auto_care_Cashier
{
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
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

        private void btn_customer(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/Customers_View.xaml", UriKind.RelativeOrAbsolute));
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

        private void btn_Paint_Jobs(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/PaintJobs_View.xaml", UriKind.RelativeOrAbsolute));
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
        }

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
    }
}
