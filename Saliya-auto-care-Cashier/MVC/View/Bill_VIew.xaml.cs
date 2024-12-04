using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Bill_VIew : UserControl
    {
        public static SharedName SharedDataInstance { get; set; }  // the shared name from the Dashboard

        public class SharedName : INotifyPropertyChanged
        {
            private string customerName;
            public string CustomerName
            {
                get => customerName;
                set
                {
                    if (customerName != value)
                    {
                        customerName = value;
                        OnPropertyChanged(nameof(CustomerName));
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
            if (SharedDataInstance != null)
            {
                DataContext = SharedDataInstance; // Bind to shared data
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
