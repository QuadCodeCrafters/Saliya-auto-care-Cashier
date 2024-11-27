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
            Loadnames();
        }

        private void Loadnames()
        {
            List<string> buttonNames = GetButtonNamesFromDatabase();

            foreach (var name in buttonNames)
            {
                Button button = new Button
                {
                    Content = name,
                    Style = (Style)FindResource("Category"),
                    Tag = "Unselected" // Initial state
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

                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
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
                selectedButton.Tag = "Unselected";
            }

            clickedButton.Tag = "Selected";
            selectedButton = clickedButton;
        }
    }
}