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

            if (descriptionListView == null)
            {
                throw new Exception("descriptionListView not found in XAML");
            }
        }

        public void UpdateDescriptions(List<string> descriptions)
        {
            if (descriptionListView != null)
            {
                descriptionListView.Items.Clear();
                foreach (var description in descriptions)
                {
                    descriptionListView.Items.Add(new { Description = description });
                }
            }
        }
    }
}

