using Saliya_auto_care_Cashier.MVVM.View;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Saliya_auto_care_Cashier.Notifications
{
    /// <summary>
    /// Interaction logic for Notificationbox.xaml
    /// </summary>
    public partial class Notificationbox : Window
    {
        Rect ScreenArea = SystemParameters.WorkArea;

        public string Header { get; set; }
        public string Message { get; set; }
        public string ImagePath { get; set; }
        public Color GradientStart { get; set; }
        public Color GradientEnd { get; set; }


        public Notificationbox()
        {
            InitializeComponent();
            this.DataContext = this;
            Border.MouseEnter += Border_MouseEnter;
            Border.MouseLeave += Border_MouseLeave;
        }

        public Notificationbox(string header, string message, string imagePath, Color gradientStart, Color gradientEnd)
            : this()
        {
            Header = header;
            Message = message;
            ImagePath = imagePath;
            GradientStart = gradientStart;
            GradientEnd = gradientEnd;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard fadeOUt = (Storyboard)this.Resources["CloseButtonFadeOutAnimation"];
            fadeOUt.Begin();
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
            // Slide in Complete then decrease rectangle length
            this.Left = ScreenArea.Right - this.Width - 10;
            Storyboard decreaseWidth = (Storyboard)this.Resources["RectangleWidthDecreaseAnimation"];
            decreaseWidth.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            // after decrease width of rectangle slide out window
            Storyboard SlideOut = (Storyboard)this.Resources["WindowSlideOutAnimation"];
            this.Left = ScreenArea.Right - this.Width;
            SlideOut.Begin();
        }

        private void WindowSlideOutAnimation_Completed(object sender, EventArgs e)
        {
            // after slide out close the window
            this.Close();
        }

        public static void ShowError()
        {
            Notificationbox error = new Notificationbox(
                "Error !!",
                "You entered wrong credentials.",
                "/Images/Error_Icon.gif",
                (Color)ColorConverter.ConvertFromString("#FF3333"),
                (Color)ColorConverter.ConvertFromString("#FF0066")
            );
            error.Show();
        }
        public static void ShowInfo()
        {
            Notificationbox info = new Notificationbox(
                "Warning !!",
                "Please review your input and try again.",
                "/Images/info.gif",
                (Color)ColorConverter.ConvertFromString("#FFB75E"),
                (Color)ColorConverter.ConvertFromString("#ED8F03")
            );
            info.Show();
        }

        public static void ShowSuccess()
        {
            Notificationbox success = new Notificationbox(
                "Success !!",
                "Operation was completed successfully!",
                "/Images/success.gif",
                (Color)ColorConverter.ConvertFromString("#28C76F"),
                (Color)ColorConverter.ConvertFromString("#81FBB8")
            );
            success.Show();
        }

        public static void carrierservice()
        {
            Notificationbox delivery = new Notificationbox(
                "",
                "Carrier Service Requested !!",
                "/Images/emergency.png",
                (Color)ColorConverter.ConvertFromString("#4776E6"),
                (Color)ColorConverter.ConvertFromString("#8E54E9")
            );

            // For click
            delivery.MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // go to the carrier service view
                    if (Application.Current.MainWindow is Dashboard dashboard)
                    {
                        try
                        {
                            var deliveryServiceView = new DelivaryService_View();
                            dashboard.fContainer.Navigate(new System.Uri("MVC/View/DelivaryService_View.xaml", UriKind.RelativeOrAbsolute));
                            deliveryServiceView.MessageButton_Click(deliveryServiceView.OverviewButton, null);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error navigating to page: {ex.Message}");
                        }
                    }
                    delivery.Close(); // Close the notification 
                }
            };

            delivery.Show();
        }

    }
}