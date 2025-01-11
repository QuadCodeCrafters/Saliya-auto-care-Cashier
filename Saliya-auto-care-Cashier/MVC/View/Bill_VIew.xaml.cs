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
using System.Linq;
using Saliya_auto_care_Cashier.Styles;


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

        public void Updateitems(List<(string Name, decimal Price)> items)
        {
            if (descriptionListView != null)
            {
                foreach (var item in items)
                {
                    bool itemExists = descriptionListView.Items
                        .Cast<dynamic>()
                        .Any(existingItem => existingItem.Description == item.Name);

                    if (itemExists)
                    {
                        //Need to change there was an font issue
                        //var result = CustomMessageBox.Show(
                        //    $"The product '{item.Name}' is already in the bill.\nDo you want to add it again?",
                        //    "Product Exists"
                        //);

                        var result = CustomMessageBox.Show(
                            $"The product is already in the bill.\n   Do you want to add it again?",
                            "Product Exists"
                        );

                        if (result == false)
                        {
                            continue;
                        }
                    }

                    descriptionListView.Items.Add(new { Description = item.Name });
                    quantityListView.Items.Add(new { Quantity = 1 }); // qty need to get from the button Version 1.1
                    priceListView.Items.Add(new { Price = item.Price });
                    taxListView.Items.Add(new { Tax = "10%"});


                    decimal amount = (item.Price * 10) /100 + item.Price; // Price + 10% tax
                    amountListView.Items.Add(new { Amount = amount }); // futer in here the Amount need to be Amount = Price * qty + (qty * tax) need to be add  


                    // Calculate subtotal after adding the new item
                    CalculateSubtotal();

                    // Calculate sales Tax after adding the new item
                    CalculateSalestax();

                    // Calculate Shipping cost after adding the new item(Patrs)
                    CalculateShippingcost();

                    // Calculate the Discount after adding the new item(all)
                    CalculateDiscount();
                }
            }
        }


        public void Billclear_Click(object sender, RoutedEventArgs e)
        {
            //CustomerName.Text = string.Empty;
            //Customeraddress.Text = string.Empty;
            //Customervehicletype.Text = string.Empty;
            //Customervehiclenumber.Text = string.Empty;

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
                    printDialog.PrintVisual(Invoice, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }
        }

        //the methods for calculating the subtotal amount
        //In here i assumed that normaly in a POS system is keeping the lates cash details before adding an another customer 

        private string subtotalText = "Rs 0.00";
        public string SubtotalText
        {
            get => subtotalText;
            set
            {
                if (subtotalText != value)
                {
                    subtotalText = value;
                    OnPropertyChanged(nameof(SubtotalText));
                }
            }
        }
        private void CalculateSubtotal()
        {
            double total = 0;

            foreach (var item in amountListView.Items.Cast<dynamic>())
            {
                total += (double)item.Amount;
            }

            SubtotalText = $"Rs {total:N2}";
        }




        //the methods for calculating the sales tax

        private string salesTaxText = "Rs 0.00";
        public string SalesTaxText
        {
            get => salesTaxText;
            set
            {
                if (salesTaxText != value)
                {
                    salesTaxText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SalesTaxText)));
                }
            }
        }

        private void CalculateSalestax()
        {
            decimal totalTax = 0;
            foreach (var item in priceListView.Items.Cast<dynamic>())
            {
                totalTax += (decimal)(item.Price * 10) / 100; // 10% of the price
            }

            SalesTaxText = $"Rs {totalTax:N2}";
        }


        //the methods to calculate the shipping cost 
        private void CalculateShippingcost() 
        { 
        
        
        }


        //the methods to calculate the discount

        private string DiscountText = "Rs 0.00";

        private void CalculateDiscount()
        {
            DiscountText = $"Rs {200:N2}";
        }

        //the methods to calculate the total
        //the methods to calculate the Amount paid
        //the methods to calculate the balance Due



    }
}
