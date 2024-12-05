using Saliya_auto_care_Cashier.MVC.Model;
using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Saliya_auto_care_Cashier.Notifications;
using Saliya_auto_care_Cashier.MVC.View;
using static Saliya_auto_care_Cashier.MVC.View.Bill_VIew;

namespace Saliya_auto_care_Cashier
{
    public partial class Dashboard : Window
    {
        private Shared sharename;
        private Sharedaddress sharecustomeraddress;
        private Shared sharevehicletype;
        private Shared sharevehiclenumber;

        private readonly DatabaseStringModel conn; //DatabaseStringModel
        public Dashboard()
        {
            InitializeComponent();

            sharename = new Shared();
            Bill_VIew.name = sharename;

            sharecustomeraddress = new Sharedaddress();
            Bill_VIew.address = sharecustomeraddress;

            sharevehicletype = new Shared();
            Bill_VIew.type = sharevehicletype;

            sharevehiclenumber = new Shared();
            Bill_VIew.number = sharevehiclenumber;

            conn = new DatabaseStringModel(); // conn
        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {

            Welcomepage w2 = new Welcomepage();
            w2.Show();
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
                fadeOutAnimation.Completed += FadeOutAnimation_Completed;
                this.BeginAnimation(OpacityProperty, fadeOutAnimation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Login Out: {ex.Message}");
            }
        }

        private void btn_registor(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/Register_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_VehicleHistory(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/VehicleHistory_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Inventory(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/Inventory_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Paint_Jobs(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/PaintJobs_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Vehicle_Services(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/VehicleService_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Vehicle_Repairs(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/VehicleRepairs_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Spare_Parts(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/SpareParts_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void btn_Delivary_Service(object sender, RoutedEventArgs e)
        {
            try
            {
                fContainer.Navigate(new System.Uri("MVC/View/DelivaryService_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void addbtn_cancel(object sender, RoutedEventArgs e)
        {
            txtloyalid.Text = "";
            Cusname.Text = "";
        }
        private void addbtn_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtloyalid.Text))
            {
                ErrorAnimation();
                IDError.Text = "Please Enter the ID";
                return;
            }

            string ID = txtloyalid.Text;
            string connectionString = conn.ConnectionString;

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CustomerName, CustomerAddress, VehicleType, VehicleNumber FROM vehicleregister WHERE CustomerNIC = @CustomerNIC";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CustomerNIC", ID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string Name = reader.GetString("CustomerName");
                                string CustomerAddress = reader.GetString("CustomerAddress");
                                string VehicleType = reader.GetString("VehicleType");
                                string VehicleNumber = reader.GetString("VehicleNumber");
                                Cusname.Text = "Customer Found: " + Name;
                                Notificationbox.ShowSuccess();

                                // send the data
                                sharename.CustomerName = Name;
                                sharecustomeraddress.CustomerAddress = CustomerAddress;



                            }
                            else
                            {
                                IDError.Text = "No Customer Found";
                                ErrorAnimation();
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void txtloyalid_TextChanged(object sender, TextChangedEventArgs e)
        {
            IDError.Text = ""; // Clear the error message TextChanged property on xaml
        }

        private void ErrorAnimation()
        {
            txtloyalid.BorderBrush = Brushes.Red;
            txtloyalid.Foreground = new SolidColorBrush(Colors.Red);

            TranslateTransform translateTransform = new TranslateTransform();
            txtloyalid.RenderTransform = translateTransform;

            translateTransform.BeginAnimation(TranslateTransform.XProperty, Saliya_auto_care_Cashier.Animations.ErrorAnimation.animation); //imported from ErrorAnimation.cs

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                txtloyalid.BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFDDDDDD"); // Reset to default border color
                txtloyalid.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6e6e6e"));
                timer.Stop();
            };
            timer.Start();
        }

    }
}
