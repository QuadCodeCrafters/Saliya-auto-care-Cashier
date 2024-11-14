using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Categories_View : UserControl
    {
        private Button selectedButton;

        public Categories_View()
        {
            InitializeComponent();
            LoadButtonsFromDatabase();
        }

        private void LoadButtonsFromDatabase()
        {
            List<string> buttonNames = GetButtonNamesFromDatabase();

            foreach (var name in buttonNames)
            {
                Button button = new Button
                {
                    Content = name,
                    Style = (Style)FindResource("CategoryButtonStyle"),
                };

                button.Click += Button_Click;
                buttonPanel.Children.Add(button);
            }
        }

        private List<string> GetButtonNamesFromDatabase()
        {
            List<string> names = new List<string>();

            string connectionString = "Server=localhost;Database=POSDB;User ID=root;Password=19216811;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT name FROM Categories";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                {
                    while (reader.Read())
                    {
                        names.Add(reader.GetString(0));
                    }
                }
            }

            return names;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (selectedButton != null)
            {
                selectedButton.BorderBrush = Brushes.Gray;
                selectedButton.BorderThickness = new Thickness(1);
            }

            clickedButton.BorderBrush = Brushes.Green;
            clickedButton.BorderThickness = new Thickness(2);
            selectedButton = clickedButton;
        }
    }
}
