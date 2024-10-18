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
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += FadeOutAnimation_Completed;  
            this.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {
      
            Welcomepage welcomePage = new Welcomepage();
            welcomePage.Show();
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
