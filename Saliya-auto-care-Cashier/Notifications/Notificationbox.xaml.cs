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
        Rect _ScreenArea = SystemParameters.WorkArea;

        public string Header { get; set; }
        public string Message { get; set; }
        public string ImagePath { get; set; }
        public SolidColorBrush RecFill { get; set; }


        public Notificationbox()
        {
            InitializeComponent();
            this.DataContext = this;
            _Border.MouseEnter += _Border_MouseEnter;
            _Border.MouseLeave += _Border_MouseLeave;
        }

        public Notificationbox(string header, string message, string imagePath, SolidColorBrush recFill)
            : this()
        {
            Header = header;
            Message = message;
            ImagePath = imagePath;
            RecFill = recFill;
        }

        private void _Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Storyboard fadeOUt = (Storyboard)this.Resources["CloseButtonFadeOutAnimation"];
            fadeOUt.Begin();
        }

        private void _Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Storyboard fadeIn = (Storyboard)this.Resources["CloseButtonFadeInAnimation"];
            fadeIn.Begin();
        }

        private void _Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = _ScreenArea.Right - this.Width;
            this.Top = _ScreenArea.Top +50;  
            Storyboard slidein = (Storyboard)this.Resources["WindowSlideInAnimation"];
            slidein.Begin();
        }



        private void WindowSlideInAnimation_Completed(object sender, EventArgs e)
        {
            // Slide in Complete then decrease rectangle length
            this.Left = _ScreenArea.Right - this.Width - 10;
            Storyboard decreaseWidth = (Storyboard)this.Resources["RectangleWidthDecreaseAnimation"];
            decreaseWidth.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            // after decrease width of rectangle slide out window
            Storyboard SlideOut = (Storyboard)this.Resources["WindowSlideOutAnimation"];
            this.Left = _ScreenArea.Right - this.Width;
            SlideOut.Begin();
        }

        private void WindowSlideOutAnimation_Completed(object sender, EventArgs e)
        {
            // after slide out close the window
            this.Close();
        }
    }
}

