using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Saliya_auto_care_Cashier.Notifications;
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
    /// Interaction logic for Cal_View.xaml
    /// </summary>
    public partial class Cal_View : UserControl
    {
        public Cal_View()
        {
            InitializeComponent();
        }
        private void btn_member(object sender, RoutedEventArgs e)
        {
            // Find the Dashboard  and show the dialog
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("MemberDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }

        private void btn_error(object sender, RoutedEventArgs e)
        {
            Notificationbox.ShowError();
        }

        private void btn_info(object sender, RoutedEventArgs e)
        {
            Notificationbox.ShowInfo();
        }
        private void btn_success(object sender, RoutedEventArgs e)
        {
            Notificationbox.ShowSuccess();
        }
    }
}
