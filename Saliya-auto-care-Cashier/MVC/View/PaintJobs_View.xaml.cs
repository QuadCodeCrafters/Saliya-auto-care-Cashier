using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            PaintButton_Click(PaintButton, null);

        }

        //For load all the views
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

        //for the toggle buttons
        private void PaintButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(PaintButton);

        }

        public void ServiceButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(ServiceButton);
        }

        public void RepairButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(RepairButton);
        }

        public void PartsButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedButton(PartsButton);
        }

        private void SetSelectedButton(Button selectedButton)
        {
            // Reset all buttons
            PaintButton.Background = Brushes.Transparent;
            ServiceButton.Background = Brushes.Transparent;
            RepairButton.Background = Brushes.Transparent;
            PartsButton.Background = Brushes.Transparent;
            // Set clicked button as selected
            selectedButton.Background = Brushes.White;
        }
    }
}

