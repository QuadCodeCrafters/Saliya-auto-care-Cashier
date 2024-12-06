using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System;
using System.Windows;
using System.IO;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Bill_VIew : UserControl, INotifyPropertyChanged
    {
        // Singleton instance for Bill_VIew
        private static Bill_VIew instance;
        public static Bill_VIew Instance
        {
            get
            {
                if (instance == null)
                    instance = new Bill_VIew();
                return instance;
            }
        }

        public static Shared name { get; set; }
        public static Sharedaddress address { get; set; }
        public static Sharedtype type { get; set; }
        public static Sharednumber number { get; set; }

        private static string InvoiceFilePath = "LastInvoiceID.txt"; // Path to store last invoice number
        private static int currentInvoiceNumber = 1; // Default invoice number

        // Bind this property to the TextBlock in XAML
        public string InvoiceNo
        {
            get { return $"SA{currentInvoiceNumber:000}"; }
            set
            {
                // Increment the invoice number when set explicitly
                currentInvoiceNumber++;
                OnPropertyChanged(nameof(InvoiceNo));
                SaveInvoiceNumber();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Class for Customer Name
        public class Shared : INotifyPropertyChanged //for the name
        {
            private string customerName;

            public string CustomerName
            {
                get => customerName;
                set
                {
                    if (customerName != value)  // Check if the name is different
                    {
                        customerName = value;
                        OnPropertyChanged(nameof(CustomerName)); // Notify UI about the change

                        // Increment the InvoiceNo instantly when CustomerName changes
                        Bill_VIew.Instance.InvoiceNo = Bill_VIew.Instance.InvoiceNo; // This triggers real-time update
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Class for Customer Address
        public class Sharedaddress : INotifyPropertyChanged //for the address
        {
            private string customerAddress;

            public string CustomerAddress
            {
                get => customerAddress;
                set
                {
                    if (customerAddress != value)  // Check if the address is different
                    {
                        customerAddress = value;
                        OnPropertyChanged(nameof(CustomerAddress)); // Notify UI about the change
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Class for Vehicle Type
        public class Sharedtype : INotifyPropertyChanged //for the address
        {
            private string vehicleType;

            public string VehicleType
            {
                get => vehicleType;
                set
                {
                    if (vehicleType != value)  // Check if the address is different
                    {
                        vehicleType = value;
                        OnPropertyChanged(nameof(VehicleType)); // Notify UI about the change
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Class for Vehicle Number
        public class Sharednumber : INotifyPropertyChanged //for the address
        {
            private string vehicleNumber;

            public string VehicleNumber
            {
                get => vehicleNumber;
                set
                {
                    if (vehicleNumber != value)  // Check if the address is different
                    {
                        vehicleNumber = value;
                        OnPropertyChanged(nameof(VehicleNumber)); // Notify UI about the change
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private TextBlock dateTextBlock;

        public Bill_VIew()
        {
            InitializeComponent();
            LoadLastInvoiceNumber();

            if (name != null && address != null && number != null && type != null)
            {
                DataContext = this; // Bind to the current in the bill view
            }

            dateTextBlock = FindName("date") as TextBlock;
            descriptionListView = FindName("descriptionListView") as ListView;

            if (dateTextBlock != null)
            {
                dateTextBlock.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            }
        }

        private static void SaveInvoiceNumber()
        {
            try
            {
                File.WriteAllText(InvoiceFilePath, currentInvoiceNumber.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save invoice number: {ex.Message}");
            }
        }

        private static void LoadLastInvoiceNumber()
        {
            try
            {
                if (File.Exists(InvoiceFilePath))
                {
                    var lastInvoiceStr = File.ReadAllText(InvoiceFilePath);
                    if (int.TryParse(lastInvoiceStr, out int lastInvoice))
                    {
                        currentInvoiceNumber = lastInvoice;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load last invoice number: {ex.Message}");
            }
        }

        public void UpdateDescriptions(List<string> descriptions)
        {
            if (descriptionListView != null)
            {
                descriptionListView.Items.Clear();
                foreach (var description in descriptions)
                {
                    descriptionListView.Items.Add(new
                    {
                        Description = description,
                    });

                    quantityListView.Items.Add(new
                    {
                        Quantity = 100,  // Default value, update as needed
                    });

                    priceListView.Items.Add(new
                    {
                        Price = 10000.00,  // Default value, update as needed
                    });

                    taxListView.Items.Add(new
                    {
                        Tax = 100,    // Default value, update as needed
                    });

                    amountListView.Items.Add(new
                    {
                        Amount = 78770.00  // Default value, update as needed
                    });
                }
            }
        }
    }
}
