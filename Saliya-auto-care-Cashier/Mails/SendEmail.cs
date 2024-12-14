using System;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace Saliya_auto_care_Cashier.Mails
{
    internal class SendEmail
    {
        public async Task SendRegistratio(string customerEmail, string customerName)
        {
            try
            {
                // Create the email message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Saliya Auto Care", "your-email@example.com")); // Replace with your email
                message.To.Add(new MailboxAddress(customerName, customerEmail));
                message.Subject = "Welcome to Saliya Auto Care";

                // HTML content for the email
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif; color: #333; background-color: #f4f4f4; padding: 20px;'>
                            <div style='max-width: 600px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);'>
                                <div style='background: #1E88E5; padding: 10px; text-align: center;'>
                                    <h1 style='color: #fff;'>Saliya Auto Care</h1>
                                </div>
                                <div style='padding: 20px;'>
                                    <h2>Welcome, {customerName}!</h2>
                                    <p>Thank you for registering with <strong>Saliya Auto Care</strong>. We're excited to serve you!</p>
                                    <p>Explore our services including vehicle repairs, paint jobs, spare parts, and more.</p>
                                    <p style='text-align: center;'>
                                        <a href='http://your-website-link.com' style='text-decoration: none; background: #1E88E5; color: white; padding: 10px 20px; border-radius: 5px;'>Visit Us</a>
                                    </p>
                                </div>
                                <div style='text-align: center; padding: 10px; font-size: 12px; color: #888;'>
                                    <p>&copy; 2024 Saliya Auto Care. All rights reserved.</p>
                                </div>
                            </div>
                        </body>
                        </html>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                // Send the email
                using (var smtpClient = new SmtpClient())
                {
                    await smtpClient.ConnectAsync("smtp.example.com", 587, MailKit.Security.SecureSocketOptions.StartTls); // Replace with your SMTP server
                    await smtpClient.AuthenticateAsync("your-email@example.com", "your-email-password"); // Replace with your email credentials
                    await smtpClient.SendAsync(message);
                    await smtpClient.DisconnectAsync(true);
                }

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
