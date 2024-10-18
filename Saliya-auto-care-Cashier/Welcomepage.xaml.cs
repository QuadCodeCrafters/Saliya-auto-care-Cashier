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
