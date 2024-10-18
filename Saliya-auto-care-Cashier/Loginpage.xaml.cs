using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Saliya_auto_care_Cashier
{
    public partial class Loginpage : Window
    {
        public Loginpage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Fade-out animation for the current window
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += FadeOutAnimation_Completed; // Event for when the animation completes
            this.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {
            // After fade-out, show the welcome page
            Welcomepage welcomePage = new Welcomepage();
            welcomePage.Show();

            // Close the current window
            this.Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Welcomepage w = new Welcomepage();
            w.Show();
            this.Close();
        }
    }
}
