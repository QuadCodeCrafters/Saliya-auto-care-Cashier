using Saliya_auto_care_Cashier.MVVM.View;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Saliya_auto_care_Cashier.Notifications
{
    public partial class Notificationbox : Window
    {
        Rect ScreenArea = SystemParameters.WorkArea;

        public string Header { get; set; }
        public string Message { get; set; }
        public string ImagePath { get; set; }
        public SolidColorBrush RecFill { get; set; }

        public Notificationbox()
        {
            InitializeComponent();
            this.DataContext = this;
            Border.MouseEnter += Border_MouseEnter;
            Border.MouseLeave += Border_MouseLeave;
        }

        public Notificationbox(string header, string message, string imagePath, SolidColorBrush recFill)
            : this()
        {
            Header = header;
            Message = message;
            ImagePath = imagePath;
            RecFill = recFill;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard fadeOut = (Storyboard)this.Resources["CloseButtonFadeOutAnimation"];
            fadeOut.Begin();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard fadeIn = (Storyboard)this.Resources["CloseButtonFadeInAnimation"];
            fadeIn.Begin();
        }

        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = ScreenArea.Right - this.Width;
            this.Top = ScreenArea.Top + 50;
            Storyboard slidein = (Storyboard)this.Resources["WindowSlideInAnimation"];
            slidein.Begin();
        }

        private void WindowSlideInAnimation_Completed(object sender, EventArgs e)
        {
            this.Left = ScreenArea.Right - this.Width - 10;
            Storyboard decreaseWidth = (Storyboard)this.Resources["RectangleWidthDecreaseAnimation"];
            decreaseWidth.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            Storyboard SlideOut = (Storyboard)this.Resources["WindowSlideOutAnimation"];
            this.Left = ScreenArea.Right - this.Width;
            SlideOut.Begin();
        }

        private void WindowSlideOutAnimation_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void ShowError()
        {
            Notificationbox error = new Notificationbox(
                "Error !!",
                "You entered wrong credentials.",
                "/Images/Error_Icon.gif",
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5A5A"))
            );
            error.Show();
        }

        public static void ShowInfo()
        {
            Notificationbox info = new Notificationbox(
                "Warning !!",
                "Please review your input and try again.",
                "/Images/info.gif",
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCC00"))
            );
            info.Show();
        }

        public static void ShowSuccess()
        {
            Notificationbox success = new Notificationbox(
                "Success !!",
                "Operation was completed successfully!",
                "/Images/success1.gif",
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"))
            );
            success.Show();
        }

        public static void carrierservice()
        {
            Notificationbox delivery = new Notificationbox(
                "Carrier Service",
                "Carrier Service Requested !!",
                "/Images/emergency.png",
                new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCC00"))
            );

            delivery.MouseDown += async (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (Application.Current.MainWindow is Dashboard dashboard)
                    {
                        try
                        {
                            // Close the notification first
                            delivery.Close();

                            // Navigate to the delivery service view
                            dashboard.fContainer.Navigate(new System.Uri("MVC/View/DelivaryService_View.xaml", UriKind.RelativeOrAbsolute));

                            // Wait for the navigation to complete
                            await Task.Delay(100);

                            // Get the navigated page content
                            if (dashboard.fContainer.Content is DelivaryService_View deliveryServiceView)
                            {
                                // Now we can safely call the method
                                deliveryServiceView.MessageButton_Click(deliveryServiceView.OverviewButton, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error navigating to page: {ex.Message}");
                        }
                    }
                }
            };

            delivery.Show();
        }
    }
}