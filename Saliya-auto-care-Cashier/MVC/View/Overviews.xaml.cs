using GMap.NET;
using GMap.NET.MapProviders;
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
    /// Interaction logic for Overviews.xaml
    /// </summary>
    public partial class Overviews : UserControl
    {
        private Border selectedBorder;
        private Button selectedButton;

        public Overviews()
        {
            InitializeComponent();
            InitializeMap();
        }

        private void InitializeMap()
        {
        
              MapControl.MapProvider = GMapProviders.OpenStreetMap;
              MapControl.Position = new PointLatLng(7.2906, 80.6337); // Coordinates for Kandy, Sri Lanka
              MapControl.MinZoom = 2;
              MapControl.MaxZoom = 17;
              MapControl.Zoom = 10;
              MapControl.ShowCenter = false;

              // Enable map dragging
              MapControl.DragButton = System.Windows.Input.MouseButton.Left;
              MapControl.CanDragMap = true;
            
        }

        private void Shipment_MouseDown(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BorderBrush = Brushes.White; // Reset previous selection
            }

            if (sender is Button clickedButton)
            {
                clickedButton.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 120, 215)); // Highlight blue
                selectedButton = clickedButton;
            }
        }
    }
}
