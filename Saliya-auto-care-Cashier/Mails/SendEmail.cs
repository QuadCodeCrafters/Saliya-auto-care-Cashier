using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

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

            MessageBox.Show($"API Key: {apiKey}\nAPI Secret: {apiSecret}\nSender Email: {senderEmail}", "Retrieved Credentials", MessageBoxButton.OK, MessageBoxImage.Information);

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret) || string.IsNullOrEmpty(senderEmail))
            {
                throw new Exception("Missing API credentials. Please check the Credential Manager settings.");
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

            var credential = (CREDENTIAL)Marshal.PtrToStructure(credPointer, typeof(CREDENTIAL));
            string password = Marshal.PtrToStringUni(credential.CredentialBlob);
            CredFree(credPointer);

            MessageBox.Show($"Successfully retrieved {fieldName}: {password}");
            return password;
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

        public string GenerateRegistrationContent(string username)
        {
            return $@"
            <html>
            <body>
                <h1>Welcome, {username}!</h1>
                <p>Thank you for registering with Saliya Auto Care.</p>
                <p>We’re excited to have you on board!</p>
            </body>
            </html>";
        }

        public string GenerateBillContent(string customerName, string billDetails)
        {
            return $@"
            <html>
            <body>
                <h1>Dear {customerName},</h1>
                <p>Thank you for your purchase!</p>
                <p>Here are your bill details:</p>
                <div>{billDetails}</div>
                <p>We look forward to serving you again.</p>
            </body>
            </html>";
        }
    }
}
