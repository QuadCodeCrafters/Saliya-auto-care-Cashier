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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Saliya_auto_care_Cashier.Animations;

namespace Saliya_auto_care_Cashier.MVC.View
{
    /// <summary>
    /// Interaction logic for VehicleHistory_View.xaml
    /// </summary>
    public partial class VehicleHistory_View : UserControl
    {
        public VehicleHistory_View()
        {
            InitializeComponent();
        }

        private void ErrorAnimation()
        {
            txtvehiclenum.BorderBrush = Brushes.Red;
            txtvehiclenum.Foreground = new SolidColorBrush(Colors.Red);

            TranslateTransform translateTransform = new TranslateTransform();
            txtvehiclenum.RenderTransform = translateTransform;

            translateTransform.BeginAnimation(TranslateTransform.XProperty, Saliya_auto_care_Cashier.Animations.ErrorAnimation.animation); //imported from ErrorAnimation.cs

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                txtvehiclenum.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD"); // Reset to default border color
                txtvehiclenum.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
                timer.Stop();
            };
            timer.Start();
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtvehiclenum.Text))
            {
                ErrorAnimation();
                
            }

            else
            {

            }
        }
    }

 
}
