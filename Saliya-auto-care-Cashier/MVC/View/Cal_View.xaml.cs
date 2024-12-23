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
        private Dashboard dashboard;

        public Cal_View()
        {
            InitializeComponent();
        }
        private void Number_Click(object sender, RoutedEventArgs e) // from 1 to 9 
        {
            var button = sender as Button;
            if (button != null)
            {
                // display number to the Display TextBox
                Display.Text += button.Content.ToString();
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = string.Empty; // Clear the display
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)  // Remove the last character from the right to left 
        {
            if (!string.IsNullOrEmpty(Display.Text))
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Handle numeric keys
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                int number = e.Key - Key.D0;  // Convert Key to it to a number
                Display.Text += number.ToString();
                e.Handled = true;
 
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                int number = e.Key - Key.NumPad0;
                Display.Text += number.ToString();
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                // backspace
                if (!string.IsNullOrEmpty(Display.Text))
                {
                    Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                // Handle clear (Escape key)
                Display.Text = string.Empty;  // Clear Display with Escape
                e.Handled = true;
            }
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

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = string.Empty;

            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();

            if (dashboardWindow?.LoadedBillView != null)
            {
                dashboardWindow.LoadedBillView?.Billclear_Click(sender, e);
            }

            if (dashboardWindow?.LoadedCategoriesView != null)
            {
                dashboardWindow.LoadedCategoriesView.ClearSelections(sender, e);
            }
        }

        private void ButtonLock_Click(object sender, RoutedEventArgs e) // need to change
        {
            Notifications.Notificationbox.carrierservice();
        }

        private void btnsku(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
