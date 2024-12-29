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

        private void StartFadeOutAnimation(string view)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += (s, e) => FadeOutAnimation_Completed(view);
            this.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed(string view)
        {
            try
            {
                Dashboard d1 = new Dashboard();
                d1.Show();
                d1.fContainer.Navigate(new Uri(view, UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the view: {ex.Message}");
            }
        }

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/Register_View.xaml");
        }

        private void btn_VehicleHistory(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/VehicleHistory_View.xaml");
        }

        private void btn_inventory(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/Inventory_View.xaml");
        }

        private void btn_paintjobs(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/Menu_View.xaml");
        }

        private void btn_vehicleservice(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/Menu_View.xaml");
        }

        private void btn_vehiclerepairs(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/Menu_View.xaml");
        }

        private void btn_spareparts(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/Menu_View.xaml");
        }

        private void btn_delivaryservice(object sender, RoutedEventArgs e)
        {
            StartFadeOutAnimation("MVC/View/DelivaryService_View.xaml");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += FadeOutAnimation_Completed_Logout;
            this.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed_Logout(object sender, EventArgs e)
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
