using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Bill_VIew : UserControl
    {
        public static Shared name { get; set; }
        public static Sharedaddress address { get; set; }
        public static Shared type { get; set; }
        public static Shared number { get; set; }

        // Class for Customer Name
        public class Shared : INotifyPropertyChanged //for the name
        {
            private string customerName;

            public string CustomerName
            {
                get => customerName;
                set
                {
                    if (customerName != value)  // check if the name is not equal to the new value
                    {
                        customerName = value;
                        OnPropertyChanged(nameof(CustomerName)); // Notify UI about the change
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
                    if (customerAddress != value)  // check if the address is not equal to the new value
                    {
                        customerAddress = value;
                        OnPropertyChanged(nameof(CustomerAddress)); // Notify UI about the change
                        MessageBox.Show(CustomerAddress);
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

            // Bind both CustomerName and CustomerAddress at the same time by setting the DataContext
            if (name != null && address != null)
            {
                DataContext = this; // Bind to the current instance of Bill_VIew
            }

            dateTextBlock = FindName("date") as TextBlock;
            descriptionListView = FindName("descriptionListView") as ListView;

            if (dateTextBlock != null)
            {
                dateTextBlock.Text = DateTime.Now.ToString("MMMM dd, yyyy");
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
