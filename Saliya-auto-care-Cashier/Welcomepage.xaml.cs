using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Saliya_auto_care_Cashier
{
    /// <summary>
    /// Interaction logic for Welcomepage.xaml
    /// </summary>
    public partial class Welcomepage : Window
    {
        public Welcomepage()
        {
            InitializeComponent();
        }

        private void btnlogout(object sender, ContextMenuEventArgs e)
        {
            Loginpage lp=new Loginpage();
            lp.Show();
            this.Close();
      }

    

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            Dashboard d1=new Dashboard();
            d1.Show();
            this.Close();
        }
    }
}
