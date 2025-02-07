using Saliya_auto_care_Cashier.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Saliya_auto_care_Cashier.MVVM.Model;
using System.Security.Cryptography.X509Certificates;
using Saliya_auto_care_Cashier.MVC.Model;
using Saliya_auto_care_Cashier.MVC.Controller;
using System.Data;
using System.Collections.Generic;

namespace Saliya_auto_care_Cashier
{
    public partial class Loginpage : Window
    {
        private LoginPageController controller;
        private string selectedUsername;

        public Loginpage()
        {
            InitializeComponent();
            controller = new LoginPageController(this);
            LoadUsernames();
        }


        private void LoadUsernames()
        {
            List<string> usernames = controller.GetUsernames();
            UserDisplay.ItemsSource = usernames; // Ensure this is a List<string>
        }


        private void UsernamesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserDisplay.SelectedItem != null)
            {
                selectedUsername = UserDisplay.SelectedItem.ToString();
                // You can perform actions with the selected username here.
                controller.SelectedUsername = UserDisplay.SelectedItem.ToString();
            }
        }

        


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        public void ShowSuccessMessage(string message)
        {
           // MessageBox.Show(message);
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message);
        }

        //public void DisplayData(DataTable dataTable)
        //{
        //    UserDisplay.ItemsSource = dataTable.DefaultView;
        //}


        private const int MaxPasswordLength = 5; // Set your maximum length

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPasswordInput.Password;
            controller.Login(password); // Pass the password to the controller's Login method

            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5));
            fadeOutAnimation.Completed += FadeOutAnimation_Completed;  
            this.BeginAnimation(OpacityProperty, fadeOutAnimation);
        }

        private void FadeOutAnimation_Completed(object sender, EventArgs e)
        {

            // Create an instance of the Viewmodel class
            PassClass pclass = new PassClass();

            // Assuming txtPasswordInput is a TextBox or similar control
            // and has a property called Password (or Text) that you want to set
            pclass.setPassword(txtPasswordInput.Password); // Use .Text if it's a TextBox

            string pass = pclass.getPassword();


          //  MessageBox.Show("Password sent "+pass);

            Welcomepage welcomePage = new Welcomepage();
            welcomePage.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Welcomepage w = new Welcomepage();
            w.Show();
            this.Close();
        }




        private void AppendToPassword(string input)
        {
            // Only append if the current password length is less than the maximum
            if (txtPasswordInput.Password.Length < MaxPasswordLength)
            {
                txtPasswordInput.Password += input;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn1.Content.ToString());
            ResetButtonColor(btn1); // Call the reset method
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn2.Content.ToString());
            ResetButtonColor(btn2);
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn3.Content.ToString());
            ResetButtonColor(btn3);
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn4.Content.ToString());
            ResetButtonColor(btn4);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn5.Content.ToString());
            ResetButtonColor(btn5);
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn6.Content.ToString());
            ResetButtonColor(btn6);
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn7.Content.ToString());
            ResetButtonColor(btn7);
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn8.Content.ToString());
            ResetButtonColor(btn8);
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn9.Content.ToString());
            ResetButtonColor(btn9);
        }

        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            AppendToPassword(btn0.Content.ToString());
            ResetButtonColor(btn0);
        }

        private void ResetButtonColor(Button btn)
        {
            btn.Background = Brushes.White; // or the default color
            btn.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, new Action(() => { }));
        }



        private void btnPwdClear_Click(object sender, RoutedEventArgs e)
        {
            txtPasswordInput.Password = "";
        }

        private void btn12_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnection dbcon = new DatabaseConnection();

            dbcon.Connect();
        }

        
    }
}
