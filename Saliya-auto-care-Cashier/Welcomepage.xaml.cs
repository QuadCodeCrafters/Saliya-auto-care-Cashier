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
            this.Loaded += Welcomepage_Loaded; // Event for when the window is loaded
        }

        private void Welcomepage_Loaded(object sender, RoutedEventArgs e)
        {
            // Set initial opacity to 0
            this.Opacity = 0;

            // Fade-in animation
            DoubleAnimation fadeInAnimation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            this.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            Dashboard d1 = new Dashboard();
            d1.Show();
            this.Close();
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
