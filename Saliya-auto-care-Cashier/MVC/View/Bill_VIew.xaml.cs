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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Saliya_auto_care_Cashier.MVC.View
{
    /// <summary>
    /// Interaction logic for Bill_VIew.xaml
    /// </summary>
    public partial class Bill_VIew : UserControl
    {
        public Bill_VIew()
        {
            InitializeComponent();
            date.Text = DateTime.Now.ToString("MMMM dd, yyyy");
        }

    }
}
