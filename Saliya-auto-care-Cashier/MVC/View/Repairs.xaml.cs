using Saliya_auto_care_Cashier.MVC.Controller;
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
    /// Interaction logic for Repairs.xaml
    /// </summary>
    public partial class Repairs : UserControl
    {

        private readonly RepairsController repairsController;
        private List<Button> selectedButtons = new List<Button>();
        public event EventHandler<List<string>> RepairSelected;


        public Repairs()
        {
            InitializeComponent();
            InitializeComponent();
            repairsController = new RepairsController();
            LoadNames();
        }

        private void LoadNames()
        {
            try
            {
                List<string> buttonNames = repairsController.GetCategoryNames();

                foreach (var name in buttonNames)
                {
                    Button button = new Button
                    {
                        Content = name,
                        Style = (Style)FindResource("Category"),
                        Tag = "Unselected"
                    };

                    button.Click += Button_Click;
                    buttonPanel.Children.Add(button);
                }


                // need to change
                // Check if there are any buttons or not 
                if (buttonPanel.Children.Count == 0)
                {
                    noButtons.Visibility = Visibility.Visible;
                    buttonPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    noButtons.Visibility = Visibility.Collapsed;
                    buttonPanel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading category names: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton.Tag.ToString() == "Unselected")
            {
                clickedButton.Tag = "Selected";
                selectedButtons.Add(clickedButton);
            }

            List<string> currentSelection = new List<string> { clickedButton.Content.ToString() };
            RepairSelected?.Invoke(this, currentSelection); // Event is triggered with the selected category name
        }

        private void ClearSelections_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
        }

        public void ClearSelection()
        {
            foreach (Button button in selectedButtons.ToList())
            {
                button.Tag = "Unselected";
            }

            selectedButtons.Clear();
            RepairSelected?.Invoke(this, new List<string>());
        }

    }
}
