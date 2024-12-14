using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Saliya_auto_care_Cashier.MVC.View;

namespace Saliya_auto_care_Cashier.MVVM.View
{
    public partial class PaintJobs_View : UserControl
    {
        private Categories_View categoriesView;
        private Bill_VIew billView;

        public PaintJobs_View()
        {
            InitializeComponent();
            LoadViews();

        }

        private void LoadViews()
        {
            try
            {
                categoriesView = new Categories_View();
                CatContainer.Navigate(categoriesView);
                categoriesView.CategoriesSelected += OnCategoriesSelected;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Categories View: {ex.Message}");
            }

            try
            {
                billView = new Bill_VIew();
                BillContainer.Navigate(billView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Bill View: {ex.Message}");
            }

            try
            {
                CalContainer.Navigate(new System.Uri("MVC/View/Cal_View.xaml", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Calculator View: {ex.Message}");
            }
        }

        private void OnCategoriesSelected(object sender, List<string> categories)
        {
            if (billView != null)
            {
                billView.UpdateDescriptions(categories);
            }
        }
    }
}

