# 🚀 POS System Cashier (Updated Version)

## Project Overview
This project is the updated version of a Point-of-Sale (POS) system, created by combining two previous individual projects: **Login** and **Dashboard**. The integration of these components provides a seamless user experience, streamlining both authentication and management features into one cohesive system.

## Technology Stack

### Frontend:
- **Language**: XAML , HTML
- **Framework**: Windows Presentation Foundation (WPF)

### Backend:
- **Language**: C#

### Database:
- (Edit the database used here, e.g., SQL Server, MySQL, etc.)

## Features
- **User Login and Authentication**: Secure login system to protect user data.
- **Dashboard**: A comprehensive interface that provides real-time insights into sales, inventory, and other key metrics.
- **Point-of-Sale Interface**: Fast and intuitive checkout process.
- **Inventory Management**: Keep track of products, stock levels, and suppliers.
- **Sales Reports**: Generate detailed reports to analyze performance over time.
- **User Roles and Permissions**: Admins can manage roles and restrict access based on user types.

## Next Steps
- Continue adding features within this unified project structure.
- Refactor and optimize existing code as needed to accommodate additional functionality.

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/your-repository-link.git
    ```
2. Ensure you have the correct development environment:
    - .NET Framework (WPF support)
    - Any required database services (add specifics here)
3. Open the solution file in Visual Studio.
4. Build and run the project.

## Mailjet Installation and Setup
Mailjet is used for email notifications and communications in this POS system.

1. Install the Mailjet NuGet package:
    ```bash
    Install-Package Mailjet.Api -Version latest
    ```
2. Configure Mailjet API keys:
   - Sign up at [Mailjet](https://www.mailjet.com/) and get your API credentials.
   - Add the credentials to your application:
     ```csharp
     using Mailjet.Client;
     using Mailjet.Client.Resources;
     using Newtonsoft.Json.Linq;
     using System;
     using System.Threading.Tasks;
     
     class Program
     {
         static async Task Main(string[] args)
         {
             MailjetClient client = new MailjetClient("your-api-key", "your-secret-key");
             MailjetRequest request = new MailjetRequest
             {
                 Resource = Send.Resource,
             }
             .Property(Send.FromEmail, "your-email@example.com")
             .Property(Send.FromName, "POS System")
             .Property(Send.Subject, "Test Email")
             .Property(Send.TextPart, "Hello, this is a test email from Mailjet!")
             .Property(Send.Recipients, new JArray {
                 new JObject { { "Email", "recipient@example.com" } }
             });
             
             MailjetResponse response = await client.PostAsync(request);
             Console.WriteLine(response.StatusCode);
         }
     }
     ```
3. Use Mailjet to send email notifications for order confirmations, password resets, etc.

## Contribution

If you'd like to contribute, please follow these steps:

1. Fork the repository:
    ```bash
    git fork https://github.com/your-repository-link.git
    ```
2. Create a feature branch:
    ```bash
    git checkout -b feature-branch
    ```
3. Commit your changes:
    ```bash
    git commit -m 'Add new feature'
    ```
4. Push to the branch:
    ```bash
    git push origin feature-branch
    ```
5. Create a pull request.

## License

This project is licensed under the [MIT License](LICENSE).

