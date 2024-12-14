using Saliya_auto_care_Cashier.MVC.View;
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

namespace Saliya_auto_care_Cashier.MVVM.View
{
    /// <summary>
    /// Interaction logic for VehicleService_View.xaml
    /// </summary>
    public partial class VehicleService_View : UserControl
    {
        private Bill_VIew billView;
        public VehicleService_View()
        {
            InitializeComponent();
            LoadViews();
        }

        private void LoadViews()
        {
            try
            {
                CalContainer.Navigate(new System.Uri("MVC/View/Cal_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }

            try
            {
                billView = new Bill_VIew();
                BillContainer.Navigate(billView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Bill View: {ex.Message}");
            }
        }
    }
}
