using System;
using System.Collections.Generic;
using System.Data;
using Saliya_auto_care_Cashier.MVC.Model;
using Saliya_auto_care_Cashier.MVVM.View;

namespace Saliya_auto_care_Cashier.MVC.Controller
{
    public class LoginPageController
    {
        private readonly DatabaseConnection loginModel;
        private readonly Loginpage view;

        // Property for selected username
        private string selectedUsername;
        public string SelectedUsername
        {
            get => selectedUsername;
            set
            {
                selectedUsername = value;
                // Notify the view if needed
            }
        }

        // Event for login status
        public event Action<string> OnLoginStatus;

        public LoginPageController(Loginpage view)
        {
            this.view = view;
            this.loginModel = new DatabaseConnection();
        }

        // Method to retrieve usernames
        public List<string> GetUsernames()
        {
            return loginModel.GetUsernames();
        }

        public void Login(string password)
        {
            if (string.IsNullOrEmpty(SelectedUsername) || string.IsNullOrEmpty(password))
            {
                view.ShowErrorMessage("Please select a user and enter a password.");
                return;
            }

            bool isAuthenticated = loginModel.AuthenticateUser(SelectedUsername, password);
            if (isAuthenticated)
            {
                view.ShowSuccessMessage("Login successful!");
                OnLoginStatus?.Invoke("Login successful!");
                NavigateToDashboard();
            }
            else
            {
                view.ShowErrorMessage("Login failed. Please check your credentials.");
                OnLoginStatus?.Invoke("Login failed. Invalid credentials.");
            }
        }
        // Method to navigate to a dashboard or welcome page
        private void NavigateToDashboard()
        {
            // Implement navigation logic to the dashboard or welcome page
            // For example, you could use an interface or directly instantiate a new page.
            // view.NavigateToDashboard(); // Assuming such a method exists in the view
        }
    }
}
