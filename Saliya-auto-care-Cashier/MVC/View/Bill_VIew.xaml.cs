using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Saliya_auto_care_Cashier.MVC.View
{
    public partial class Bill_VIew : UserControl
    {
 
        private TextBlock dateTextBlock;

        public Bill_VIew()
        {
            InitializeComponent();
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

