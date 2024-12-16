using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class DelivaryService_View : UserControl
    {
        public DelivaryService_View()
        {
            InitializeComponent();
            OverviewButton_Click(OverviewButton,null);
            OverviewButton.Background = Brushes.White;
            MessageButton.Background = Brushes.Transparent;


        }
        private void OverviewButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(OverviewButton);

            try
            {
                Container.Navigate(new System.Uri("MVC/View/Overviews.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Overviews View: {ex.Message}");
            }
        }

        public void MessageButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(MessageButton);

            try
            {
                Container.Navigate(new System.Uri("MVC/View/Newcarrier.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Newcarrier View: {ex.Message}");
            }
        }

        private void SetSelectedButton(Button selectedButton)
        {
            // Reset all buttons
            OverviewButton.Background = Brushes.Transparent;
            MessageButton.Background = Brushes.Transparent;

            // Set clicked button as selected
            selectedButton.Background = Brushes.White;
        }

        private void btn_pickupclick(object sender, RoutedEventArgs e)
        {
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("SchedulePickupDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }
    }
}

