using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Saliya_auto_care_Cashier.MVC.Controller;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Categories_View : UserControl
    {
        private readonly CategoryViewController categoryController;
        private List<Button> selectedButtons = new List<Button>();
        public event EventHandler<List<string>> CategoriesSelected;

        public Categories_View()
        {
            InitializeComponent();
            categoryController = new CategoryViewController();
            LoadNames();
        }

        private void LoadNames()
        {
            try
            {
                List<string> buttonNames = categoryController.GetCategoryNames();

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
            CategoriesSelected?.Invoke(this, currentSelection);
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
            CategoriesSelected?.Invoke(this, new List<string>());
        }
    }
}