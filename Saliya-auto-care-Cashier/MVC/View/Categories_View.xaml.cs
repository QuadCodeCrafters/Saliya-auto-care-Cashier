using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Categories_View : UserControl
    {
        private List<Button> selectedButtons = new List<Button>();
        private Button lastClickedButton = null; // Track the most recently clicked button
        public event EventHandler<List<string>> CategoriesSelected;

        public Categories_View()
        {
            InitializeComponent();
            LoadNames();
        }

        private void LoadNames()
        {
            List<string> buttonNames = GetButtonNamesFromDatabase();

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
        }

        private List<string> GetButtonNamesFromDatabase()
        {
            List<string> names = new List<string>();
            string connectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT name FROM Categories";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            names.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading category names: {ex.Message}");
            }

            return names;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            lastClickedButton = clickedButton; // Store the last clicked button

            if (clickedButton.Tag.ToString() == "Unselected")
            {
                clickedButton.Tag = "Selected";
                selectedButtons.Add(clickedButton);
            }

            // Send only the current clicked button's content
            List<string> currentSelection = new List<string> { clickedButton.Content.ToString() };
            CategoriesSelected?.Invoke(this, currentSelection);
        }

        private void ClearSelections_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
        }

        public void ClearSelection()
        {
            // Clear all selected buttons
            foreach (Button button in selectedButtons.ToList())
            {
                button.Tag = "Unselected";
            }
            selectedButtons.Clear();
            lastClickedButton = null;

            // Notify that no categories are selected
            CategoriesSelected?.Invoke(this, new List<string>());
        }
    }
}