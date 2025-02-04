using GMap.NET.MapProviders;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Saliya_auto_care_Cashier.Mails
{
    public class EmailService
    {
        private readonly string apiKey;
        private readonly string apiSecret;
        private readonly string senderEmail;
        private readonly string senderName = "Saliya Auto Care";

        public EmailService()
        {
            // get the  credentials from Windows Credential Manager
            apiKey = GetCredential("SaliyaAutoCare/apiKey", "API Key");
            apiSecret = GetCredential("SaliyaAutoCare/apiSecret", "API Secret");
            senderEmail = GetCredential("SaliyaAutoCare/Email", "Sender Email");

            // for debugging
            MessageBox.Show($"API Key: {apiKey}\nAPI Secret: {apiSecret}\nSender Email: {senderEmail}", "Retrieved Credentials", MessageBoxButton.OK, MessageBoxImage.Information);

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret) || string.IsNullOrEmpty(senderEmail))
            {
                MessageBox.Show("Missing API credentials. Please check the Credential Manager settings.");
            }
        }

        public async Task<bool> SendEmailAsync(string recipientEmail, string recipientName, string subject, string htmlContent)
        {
            try
            {
                var message = new JObject
                {
                    { "From", new JObject { { "Email", senderEmail }, { "Name", senderName } } },
                    { "To", new JArray { new JObject { { "Email", recipientEmail }, { "Name", recipientName } } } },
                    { "Subject", subject },
                    { "HTMLPart", htmlContent }
                };

                MailjetClient client = new MailjetClient(apiKey, apiSecret);
                MailjetRequest request = new MailjetRequest
                {
                    Resource = SendV31.Resource,
                }
                .Property(Send.Messages, new JArray { message });

                MailjetResponse response = await client.PostAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show($"Email send failed. Status: {response.StatusCode}, Error: {response.GetData()}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An exception occurred while sending the email: {ex.Message}");
                return false;
            }
        }

        // I modified the GetCredential method because the keys had some strange chinese letters

        // Windows Credential Manager using raw binary data with UTF-16 array and in here i converted  them in to  windows string values

        private string GetCredential(string target, string fieldName)
        {
            IntPtr credPointer;
            bool success = CredRead(target, CRED_TYPE.GENERIC, 0, out credPointer);

            if (!success)
            {
                MessageBox.Show($"Failed to retrieve {fieldName} from Credential Manager.\nError Code: {Marshal.GetLastWin32Error()}",
                                "Credential Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            try
            {
                // Cast the pointer to the CREDENTIAL structure
                var credential = (CREDENTIAL)Marshal.PtrToStructure(credPointer, typeof(CREDENTIAL));

                // Decode the CredentialBlob to a string
                if (credential.CredentialBlob != IntPtr.Zero && credential.CredentialBlobSize > 0)
                {
                    byte[] credentialBytes = new byte[credential.CredentialBlobSize];
                    Marshal.Copy(credential.CredentialBlob, credentialBytes, 0, (int)credential.CredentialBlobSize);

                    // Convert the byte array to a string (UTF-16 encoding for Windows strings)
                    string decodedCredential = System.Text.Encoding.Unicode.GetString(credentialBytes);
                    return decodedCredential;
                }

                return null;
            }
            finally
            {
                // Free the memory allocated for the credential
                CredFree(credPointer);
            }
        }


        [DllImport("Advapi32.dll", SetLastError = true)]
        private static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr credential);

        [DllImport("Advapi32.dll", SetLastError = true)]
        private static extern void CredFree(IntPtr buffer);

        private enum CRED_TYPE
        {
            GENERIC = 1,
            DOMAIN_PASSWORD = 2,
            DOMAIN_CERTIFICATE = 3,
            DOMAIN_VISIBLE_PASSWORD = 4,
            GENERIC_CERTIFICATE = 5
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CREDENTIAL
        {
            public uint Flags;
            public CRED_TYPE Type;
            public string TargetName;
            public string Comment;
            public long LastWritten;
            public uint CredentialBlobSize;
            public IntPtr CredentialBlob;
            public uint Persist;
            public uint AttributeCount;
            public IntPtr Attributes;
            public string TargetAlias;
            public string UserName;
        }

        //for the Registration mail
        public string RegistrationContent(string username)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "RegistrationTemplate.html");

            if (!File.Exists(templatePath))
            {
                MessageBox.Show($"Email template not found: {templatePath}");
            }

            string htmlContent = File.ReadAllText(templatePath);

            // The username from the registration model
            htmlContent = htmlContent.Replace("{username}", username);
            htmlContent = htmlContent.Replace("{year}", DateTime.Now.Year.ToString());

            return htmlContent;
        }

        //for the SendPickupMail
        public string PickupMailContent(string DriverName, string CustomerName, string CustomerPhoneNumber)
        {
            string PickupMail = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", "CarrierServiceDriverTemplate.html");

            if (!File.Exists(PickupMail))
            {
                MessageBox.Show($"Email template not found: {PickupMail}");
            }

            string htmlContent = File.ReadAllText(PickupMail);

            // Get current date and time
            string currentDate = DateTime.Now.ToString("MMMM dd, yyyy");
            string currentTime = DateTime.Now.ToString("hh:mm tt");

            // Replace placeholders in the template
            htmlContent = htmlContent.Replace("{Driver Name}", DriverName);
            htmlContent = htmlContent.Replace("{Pickup Date}", currentDate);
            htmlContent = htmlContent.Replace("{Pickup Time}", currentTime);
            htmlContent = htmlContent.Replace("{Customer Name}", CustomerName);
            htmlContent = htmlContent.Replace("{Customer Phone Number}", CustomerPhoneNumber);
            htmlContent = htmlContent.Replace("{Your Company Name}", "Saliya Auto Care ®");

            return htmlContent;
        }
    }
}