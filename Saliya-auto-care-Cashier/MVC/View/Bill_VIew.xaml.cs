using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Windows.Input;
using Saliya_auto_care_Cashier.MVVM.View;
using System.Data.Common;
using Saliya_auto_care_Cashier.Notifications;


namespace Saliya_auto_care_Cashier.MVC.View
{

    public partial class Bill_VIew : UserControl, INotifyPropertyChanged
    {
        public ICommand MyCommand { get; private set; }
        private static readonly string InvoiceFilePath = "LastInvoiceID.txt";

        public static Sharedname name { get; set; } = new Sharedname();

        public static SharedPrice sharedPrice { get; set; } = new SharedPrice();
        public static SharedTax sharedTax { get; set; } = new SharedTax();
        public static SharedProduct sharedProduct { get; set; } = new SharedProduct();
        public static SharedTotal sharedTotal { get; set; } = new SharedTotal();
        public static SharedQty sharedQty { get; set; } = new SharedQty();

        public static Sharedaddress address { get; set; }
        public static Sharedtype type { get; set; }
        public static Sharednumber number { get; set; }

        private string invoiceNo;

        public string InvoiceNo
        {
            get => invoiceNo;
            set
            {
                if (invoiceNo != value)
                {
                    invoiceNo = value;
                    OnPropertyChanged(nameof(InvoiceNo));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Class for Customer Name
        public class Sharedname : INotifyPropertyChanged
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

        // Class for Customer Address
        public class Sharedaddress : INotifyPropertyChanged
        {
            private string customerAddress;

            public string CustomerAddress
            {
                get => customerAddress;
                set
                {
                    if (customerAddress != value)
                    {
                        customerAddress = value;
                        OnPropertyChanged(nameof(CustomerAddress));
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
        public class Sharedtype : INotifyPropertyChanged
        {
            private string vehicleType;

            public string VehicleType
            {
                get => vehicleType;
                set
                {
                    if (vehicleType != value)
                    {
                        vehicleType = value;
                        OnPropertyChanged(nameof(VehicleType));
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
        public class Sharednumber : INotifyPropertyChanged
        {
            private string vehicleNumber;

            public string VehicleNumber
            {
                get => vehicleNumber;
                set
                {
                    if (vehicleNumber != value)
                    {
                        vehicleNumber = value;
                        OnPropertyChanged(nameof(VehicleNumber));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        //class for Price
        public class SharedPrice : INotifyPropertyChanged
        {
            private double price;

            public double Price
            {
                get => price;
                set
                {
                    if (price != value)
                    {
                        price = value;
                        OnPropertyChanged(nameof(Price));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //class for Tax
        public class SharedTax : INotifyPropertyChanged
        {
            private double tax;

            public double Tax
            {
                get => tax;
                set
                {
                    if (tax != value)
                    {
                        tax = value;
                        OnPropertyChanged(nameof(Tax));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //class for Product
        public class SharedProduct : INotifyPropertyChanged
        {
            private string plateNumber;

            public string Description
            {
                get => plateNumber;
                set
                {
                    if (plateNumber != value)
                    {
                        plateNumber = value;
                        OnPropertyChanged(nameof(Description));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //class for Amount
        public class SharedTotal : INotifyPropertyChanged
        {
            private double total;

            public double Amount
            {
                get => total;
                set
                {
                    if (total != value)
                    {
                        total = value;
                        OnPropertyChanged(nameof(Amount));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //class for Qty
        public class SharedQty : INotifyPropertyChanged
        {
            private double qty;

            public double Quantity
            {
                get => qty;
                set
                {
                    if (qty != value)
                    {
                        qty = value;
                        OnPropertyChanged(nameof(Quantity));
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

            if (name != null && address != null && number != null && type != null && sharedPrice != null && sharedTax != null && sharedProduct != null && sharedTotal != null)
            {
                DataContext = this;
            }

            dateTextBlock = FindName("date") as TextBlock;
            descriptionListView = FindName("descriptionListView") as ListView;

            if (dateTextBlock != null)
            {
                dateTextBlock.Text = DateTime.Now.ToString("MMMM dd, yyyy");
            }

            sharedPrice.PropertyChanged += SharedPrice_PropertyChanged;
            sharedTax.PropertyChanged += SharedTax_PropertyChanged;
            sharedProduct.PropertyChanged += SharedProduct_PropertyChanged;
            sharedTotal.PropertyChanged += SharedTotal_PropertyChanged;
            sharedQty.PropertyChanged += SharedQty_PropertyChanged;

            // Read the last invoice number from the file
            InvoiceNo = LoadLastInvoiceID();

            // Subscribe to CustomerName changes
            name.PropertyChanged += Name_PropertyChanged;

            MyCommand = new RelayCommand(Buttonprint_Click);

        }

        private void SharedProduct_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SharedProduct.Description))
            {
                // Clear existing items and add the updated product
                descriptionListView.Items.Clear();
                descriptionListView.Items.Add(new { Description = sharedProduct.Description });
            }
        }

        private void SharedQty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SharedQty.Quantity))
            {
                // Clear existing items and add the updated product
                quantityListView.Items.Clear();
                quantityListView.Items.Add(new { Quantity = sharedQty.Quantity });
            }
        }

        private void SharedTotal_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(sharedTotal.Amount))
            {
                // Clear existing items and add the updated product
                amountListView.Items.Clear();
                amountListView.Items.Add(new { Amount = sharedTotal.Amount });
            }
        }



        private void SharedPrice_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SharedPrice.Price))
            {
                // Update priceListView with the new price
                priceListView.Items.Clear();
                priceListView.Items.Add(new { Price = sharedPrice.Price });
            }
        }

        private void SharedTax_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SharedTax.Tax))
            {
                // Update taxListView with the new tax
                taxListView.Items.Clear();
                taxListView.Items.Add(new { Tax = sharedTax.Tax });
            }
        }

        private void Name_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Sharedname.CustomerName) && !string.IsNullOrWhiteSpace(name.CustomerName))
            {
                // Increment and update InvoiceNo
                InvoiceNo = GenerateNextInvoiceNo(InvoiceNo);

                // Save the updated InvoiceNo to the file
                SaveLastInvoiceID(InvoiceNo);
            }
        }

        private string LoadLastInvoiceID()
        {
            try
            {
                // Ensure the file exists
                if (!File.Exists(InvoiceFilePath))
                {
                    File.WriteAllText(InvoiceFilePath, "SA000");
                }

                // Read the last invoice number
                var lastInvoice = File.ReadAllText(InvoiceFilePath).Trim();

                // Validate the structure of the invoice number
                if (!string.IsNullOrEmpty(lastInvoice) && lastInvoice.StartsWith("SA") && int.TryParse(lastInvoice.Substring(2), out _))
                {
                    return lastInvoice;
                }
                else
                {
                    // Default value if the file is invalid
                    return "SA000";
                }
            }
            catch (Exception ex)
            {
                // Log or handle file read exceptions
                MessageBox.Show($"Error reading last invoice ID: {ex.Message}");
                return "SA000";
            }
        }

        private void SaveLastInvoiceID(string invoiceNo)
        {
            try
            {
                File.WriteAllText(InvoiceFilePath, invoiceNo);
            }
            catch (Exception ex)
            {
                // Log or handle file write exceptions
                MessageBox.Show($"Error saving last invoice ID: {ex.Message}");
            }
        }

        private string GenerateNextInvoiceNo(string currentInvoiceNo)
        {
            // Extract the numeric part of the invoice number
            int currentNumber = int.Parse(currentInvoiceNo.Substring(2));
            int nextNumber = currentNumber + 1;

            // Generate the next invoice number with leading zeros
            return $"SA{nextNumber:D3}";
        }

        public void UpdateDescriptions(List<string> descriptions)
        {
            if (descriptionListView != null)
            {
               // descriptionListView.Items.Clear();
                foreach (var description in descriptions)
                {
                    descriptionListView.Items.Add(new
                    {
                        Description = description,
                    });

                    quantityListView.Items.Add(new
                    {
                        Quantity = 100,
                    });

                    priceListView.Items.Add(new
                    {
                        Price = 10000.00,
                    });

                    taxListView.Items.Add(new
                    {
                        Tax = 100,
                    });

                    amountListView.Items.Add(new
                    {
                        Amount = 78770.00,
                    });
                }
            }
        }

        public void Billclear_Click(object sender, RoutedEventArgs e)
        {
            CustomerName.Text = string.Empty;
            Customeraddress.Text = string.Empty;
            Customervehicletype.Text = string.Empty;
            Customervehiclenumber.Text = string.Empty;
            descriptionListView.Items.Clear();
            amountListView.Items.Clear();
            quantityListView.Items.Clear();
            priceListView.Items.Clear();
            taxListView.Items.Clear();

        }

        public void Buttonprint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Create a copy of the UserControl for printing
                    UserControl printContent = new UserControl();
                    printContent.Content = this.Content;

                    // Remove any ScrollViewer to ensure all content is visible
                    RemoveScrollViewers(printContent);

                    // Measure and arrange
                    printContent.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    printContent.Arrange(new Rect(new Point(0, 0), printContent.DesiredSize));

                    // Print the content
                    printDialog.PrintVisual(printContent, "Invoice");

                    ShowAgain();
                }
                else
                {
                    Notificationbox.ShowError();
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        public void RemoveScrollViewers(DependencyObject parent)
        {
            for (int i = VisualTreeHelper.GetChildrenCount(parent) - 1; i >= 0; i--)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is ScrollViewer)
                {
                    if (parent is Panel panel)
                    {
                        panel.Children.Remove(child as UIElement);
                        panel.Children.Add((child as ScrollViewer).Content as UIElement);
                    }
                    else if (parent is ContentControl contentControl)
                    {
                        contentControl.Content = (child as ScrollViewer).Content;
                    }
                }
                else
                {
                    RemoveScrollViewers(child);
                }
            }
        }

        public void ShowAgain()
        {
            Notificationbox.ShowSuccess();
            UserControl newUser = new Bill_VIew();
            this.Content = newUser; 
        }

    }
}
