using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    /// <summary>
    /// Interaction logic for Register_View.xaml
    /// </summary>
    public partial class Register_View : UserControl
    {
        private List<Control> requiredFields;

        public Register_View()
        {
            InitializeComponent();
            InitializeRequiredFields();
        }

        private void InitializeRequiredFields()
        {
            requiredFields = new List<Control>
            {
                txtvehiclenum, txtvehicletype, txtvehiclemodel, txtcusname,
                txtcusaddress, txtcusNIC, txtcusmail, txtcusnumber, txtcusspec
            };
        }

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            if (IsAnyFieldEmpty())
            {
                ShowErrorAnimation();
            }
            else
            {
                
            }
        }

        private bool IsAnyFieldEmpty()
        {
            foreach (var field in requiredFields)
            {
                if (field is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text) ||
                    field is ComboBox comboBox && comboBox.SelectedItem == null)
                {
                    return true;
                }
            }
            return false;
        }

        private void ShowErrorAnimation()
        {
            foreach (var field in requiredFields)
            {
                bool hasError = (field is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text)) ||
                                (field is ComboBox comboBox && comboBox.SelectedItem == null);

                if (hasError)
                {
                    ApplyErrorAppearance(field);
                    ShakeControl(field);
                }
            }

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)//timer
            };
            timer.Tick += (s, e) =>
            {
                foreach (var field in requiredFields)
                {
                    ResetToDefaultAppearance(field);
                }
                timer.Stop();
            };
            timer.Start();
        }

        private void ApplyErrorAppearance(Control control)
        {
            control.BorderBrush = Brushes.Red;
            control.Foreground = new SolidColorBrush(Colors.Red);
        }

        private void ShakeControl(Control control)
        {
            TranslateTransform translateTransform = new TranslateTransform();
            control.RenderTransform = translateTransform;

            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 10,
                Duration = TimeSpan.FromMilliseconds(50),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(5) // Shake 5 times
            };

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void ResetToDefaultAppearance(Control control)
        {
            control.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD");
            control.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
        }
    }
}
