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
    /// Interaction logic for QuantitySelector.xaml
    /// </summary>
    public partial class QuantitySelector : UserControl
    {

        private int quantity = 1;
        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                QuantityText.Text = value.ToString();
            }
        }

        public delegate void QuantityChangedEventHandler(int newQuantity);
        public event QuantityChangedEventHandler QuantityChanged;

        public QuantitySelector()
        {
            InitializeComponent();
        }

        private void DecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity > 1)
            {
                Quantity--;
                QuantityChanged?.Invoke(Quantity);
            }
        }

        private void IncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            Quantity++;
            QuantityChanged?.Invoke(Quantity);
        }
    }
}
