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

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVVM/View/registorview.xaml", UriKind.RelativeOrAbsolute));
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
                fContainer.Navigate(new System.Uri("MVVM/View/customersview.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }
    }
}
