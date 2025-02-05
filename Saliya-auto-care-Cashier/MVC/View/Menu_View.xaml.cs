using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Saliya_auto_care_Cashier.MVC.View;
using System.Windows.Navigation;
using System.Windows.Input;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class PaintJobs_View : UserControl
    {

        private Categories_View categoriesView;
        private Service servicesView;
        private Repairs repairsView;
        private Parts partsView;        

        private Dashboard dashboard;
        private Bill_VIew billView;

        public PaintJobs_View()
        {
            InitializeComponent();
            LoadViews();
            PaintButton_Click(PaintButton, null);

        }

        //For load all the views
        private void LoadViews()
        {
            try
            {
                categoriesView = new Categories_View();
                servicesView = new Service();
                repairsView = new Repairs();
                partsView = new Parts();


                CatContainer.Navigate(categoriesView);
                categoriesView.CategoriesSelected += OnCategoriesSelected;

                CatContainer.Navigate(servicesView);
                servicesView.ServicesSelected += OnServicesSelected;

                CatContainer.Navigate(repairsView);
                repairsView.RepairSelected += OnRepairSelected;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Categories View: {ex.Message}");
            }

            try
            {
                billView = new Bill_VIew();
                BillContainer.Content = billView;


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Bill View: {ex.Message}");
            }
        }

        private void OnCategoriesSelected(object sender, List<(string Name, decimal Price)> categories)
        {
            if (billView != null)
            {
                billView.Updateitems(categories);
            }
        }

        private void OnServicesSelected(object sender, List<(string Name, decimal Price)> categories)
        {
            if (billView != null)
            {
                billView.Updateitems(categories);
            }
        }

        private void OnRepairSelected(object sender, List<(string Name, decimal Price)> categories)
        {
            if (billView != null)
            {
                billView.Updateitems(categories);
            }
        }





        //for the toggle buttons
        private void PaintButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(PaintButton);

            try
            {
                CatContainer.Content = categoriesView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing Categories View: {ex.Message}");
            }


            //try
            //{
            //    CatContainer.Navigate(new System.Uri("MVC/View/Categories_View.xaml", UriKind.RelativeOrAbsolute));
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error navigating to page: {ex.Message}");
            //}

        }

        public void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(ServiceButton);

            try
            {
                CatContainer.Content = servicesView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        public void RepairButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(RepairButton);

            try
            {
                CatContainer.Content = repairsView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        public void PartsButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(PartsButton);

            try
            {
                CatContainer.Content = partsView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to page: {ex.Message}");
            }
        }

        private void SetSelectedButton(Button selectedButton)
        {
            // Reset all buttons
            PaintButton.Background = Brushes.Transparent;
            ServiceButton.Background = Brushes.Transparent;
            RepairButton.Background = Brushes.Transparent;
            PartsButton.Background = Brushes.Transparent;
            // Set clicked button as selected
            selectedButton.Background = Brushes.White;
        }

        private void Number_Click(object sender, RoutedEventArgs e) // from 1 to 9 and 500 , 1000 , 5000
        {
            var button = sender as Button;
            if (button != null)
            {
                // display number to the Display TextBox
                Display.Text += button.Content.ToString();

            }
        }
        private void Numberminus_Click(object sender, RoutedEventArgs e)  
        {
            var button = sender as Button;
            if (button != null)
            {
  
                Display.Text = "-";

            }
        }


        private void Value_Click(object sender, RoutedEventArgs e) //  500 , 1000 , 5000
        {
            var button = sender as Button;
            if (button != null)
            {
                var buttonContent = button.Content.ToString();//get the value

                var numericValue = buttonContent.Replace("Rs", "").Trim(); //Remove the Rs tag

                // display number to the Display TextBox
                Display.Text += numericValue;

            }
        }

        private void ClearDisplay_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = string.Empty; // Clear the display
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)  // Remove the last character from the right to left 
        {
            if (!string.IsNullOrEmpty(Display.Text))
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Handle numeric keys
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                int number = e.Key - Key.D0;  // Convert Key to it to a number
                Display.Text += number.ToString();
                e.Handled = true;
 
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                int number = e.Key - Key.NumPad0;
                Display.Text += number.ToString();
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                // backspace
                if (!string.IsNullOrEmpty(Display.Text))
                {
                    Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                // Handle clear (Escape key)
                Display.Text = string.Empty;  // Clear Display with Escape
                e.Handled = true;
            }
        }

        private void btn_member(object sender, RoutedEventArgs e)
        {
            // Find the Dashboard  and show the dialog
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("MemberDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("ClearAllDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }

        private void ButtonCharges_Click(object sender, RoutedEventArgs e) // need to change
        {
            if (billView != null && !string.IsNullOrEmpty(Display.Text))
            {
                if (decimal.TryParse(Display.Text, out decimal amount))
                {
                    // Find the TextBox for "Amount Paid" in the Bill_View
                    var amountPaid = billView.FindName("ChargesText") as TextBox;
                    if (amountPaid != null)
                    {
                        // Update the text of the "Amount Paid" TextBox
                        amountPaid.Text = $"Rs {amount:N2}";
                    }

                    // Clear the display
                    Display.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Invalid amount entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnsku(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (billView != null && !string.IsNullOrEmpty(Display.Text))
            {
                if (decimal.TryParse(Display.Text, out decimal amount))
                {
                    // Find the TextBox for "Amount Paid" in the Bill_View
                    var amountPaid = billView.FindName("AmountPaidText") as TextBox;
                    if (amountPaid != null)
                    {
                        // Update the text of the "Amount Paid" TextBox
                        amountPaid.Text = $"Rs {amount:N2}";
                    }

                    // Clear the display
                    Display.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Invalid amount entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void ClearAll()
        {
            // Clear the display text
            if (Display != null)
            {
                Display.Text = string.Empty;
            }

            // Clear categories selection
            if (categoriesView != null)
            {
                categoriesView.ClearSelection();
            }

            // Clear service selection
            if (servicesView != null)
            {
                servicesView.ClearSelection();
            }

            if (repairsView != null)
            {
                repairsView.ClearSelection();
            }

            // Clear bill view
            if (billView != null)
            {
                billView.Billclear_Click(this, new RoutedEventArgs());
            }
        }

        private void btnRefund_Click(object sender, RoutedEventArgs e)
        {
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("RefundButtonDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }

        private void btncal_click(object sender, RoutedEventArgs e)
        {
            // Get the calculatorHost
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("calculatorHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }
    }
}

