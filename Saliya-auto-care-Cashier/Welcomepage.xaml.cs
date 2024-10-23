using Saliya_auto_care_Cashier.MVVM.View;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Saliya_auto_care_Cashier
{
    public partial class Welcomepage : Window
    {
        public Welcomepage()
        {
            InitializeComponent();
        }
        private void btn_registor(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/Register_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the register view: {ex.Message}");
            }
        }

        private void btn_customer(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/Customers_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the Customers view: {ex.Message}");
            }
        }

        private void btn_inventory(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/Inventory_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the Inventory view: {ex.Message}");
            }
        }

        private void btn_paintjobs(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/PaintJobs_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the Paint Jobs view: {ex.Message}");
            }
        }

        private void btn_vehicleservice(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/VehicleService_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the Vehicle Service view: {ex.Message}");
            }
        }

        private void btn_vehiclerepairs(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/VehicleRepairs_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the Vehicle Repairs view: {ex.Message}");
            }
        }

        private void btn_spareparts(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/SpareParts_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the Spare Parts view: {ex.Message}");
            }
        }


        private void btn_delivaryservice(object sender, RoutedEventArgs e)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri("MVC/View/DelivaryService_View.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the Delivary Service view: {ex.Message}");
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += FadeOutAnimation_Completed;
            this.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {

            try
            {
                Loginpage lg = new Loginpage();
                lg.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
