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
                d1.fContainer.Navigate(new Uri("MVVM/View/registorview.xaml", UriKind.RelativeOrAbsolute));
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
                d1.fContainer.Navigate(new Uri("MVVM/View/customersview.xaml", UriKind.RelativeOrAbsolute));
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying the register view: {ex.Message}");
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
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
