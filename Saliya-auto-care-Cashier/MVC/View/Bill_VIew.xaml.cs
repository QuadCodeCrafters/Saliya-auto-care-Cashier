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
using Saliya_auto_care_Cashier.MVC.Model;
using System.Printing;
using System.Windows.Xps;


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

        private readonly DatabaseStringModel conn;  // DatabaseStringModel

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

            conn = new DatabaseStringModel(); // conn

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

            //MyCommand = new RelayCommand(Buttonprint_Click);

            AmountPaidText.Text = "Rs 0.00";
            ChargesText.Text = "Rs 0.00";

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
                    taxListView.Items.Add(new { Tax = "10%" });//need to change according to admin's choice


                    decimal amount = (item.Price * 10) / 100 + item.Price; // Price + 10% tax need to change according to admin's choice
                    amountListView.Items.Add(new { Amount = amount }); // futer in here the Amount need to be Amount = Price * qty + (qty * tax) need to be add  


                    // Calculate subtotal after adding the new item
                    CalculateSubtotal();

                    // Calculate sales Tax after adding the new item
                    CalculateSalestax();

                    // Calculate the Discount after adding the new item(all)
                    CalculateDiscount();

                    // Calculate the Total after adding the new item(all)
                    CalculateTotal();

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
            double subtotal = 0;

            foreach (var item in amountListView.Items.Cast<dynamic>())
            {
                subtotal += (double)item.Amount;
            }

            SubtotalText = $"Rs {subtotal:N2}";
        }




        //the methods for calculating the sales tax
        //in here i assumed that the tax is like Vat (requirment change)
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
                totalTax += (decimal)(item.Price * 10) / 100; // 10% of the price need to change according to admin's choice
            }

            SalesTaxText = $"Rs {totalTax:N2}";
        }


        //the methods to calculate the discount
        private string discountText = "Rs 0.00";
        public string DiscountText
        {
            get => discountText;
            set
            {
                if (discountText != value)
                {
                    discountText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DiscountText)));
                }
            }
        }
        private void CalculateDiscount()
        {
            double discount = 0;
            double subtotalValue = Convert.ToDouble(SubtotalText.Replace("Rs", "").Trim());

            if (subtotalValue > 500.00) // need to change according to admin's choice
            {
                discount += (subtotalValue * 10) / 100; //need to change according to admin's choice
                DiscountText = $"Rs {discount:N2}";

            }
            else
            {

                DiscountText = "Rs 0.00";
            }

        }



        //the methods to calculate the total
        private string totalText = "Rs 0.00";
        public string TotalText
        {
            get => totalText;
            set
            {
                if (totalText != value)
                {
                    totalText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalText)));
                }
            }
        }
        private void CalculateTotal()
        {
            double Total = 0;
            double subtotalValue = Convert.ToDouble(SubtotalText.Replace("Rs", "").Trim());
            double discountValue = Convert.ToDouble(DiscountText.Replace("Rs", "").Trim());
            double chargesValue = Convert.ToDouble(ChargesText.Text.Replace("Rs", "").Trim());

            Total = (subtotalValue - discountValue) + chargesValue;

            TotalText = $"Rs {Total:N2}";

        }



        //the methods to calculate the difference
        private void txtamount_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateBalance();
        }

        private void txtCharges_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTotal();
            CalculateBalance();
        }

        //the methods to calculate the balance Due
        private string balanceText = "Rs 0.00";
        public string BalanceText
        {
            get => balanceText;
            set
            {
                if (balanceText != value)
                {
                    balanceText = value;
                    OnPropertyChanged(nameof(BalanceText));
                }
            }
        }
        private void CalculateBalance()
        {
            double Balance = 0;
            double TotalValue = Convert.ToDouble(TotalText.Replace("Rs", "").Trim());
            double PaidValue = Convert.ToDouble(AmountPaidText.Text.Replace("Rs", "").Trim());

            Balance = PaidValue - TotalValue;

            BalanceText = $"Rs {Balance:N2}";

        }


        public void Buttonprint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;

                // Scroll the ScrollViewer to the top i added this is the Scroll was in the bottom the above content will not be print
                var scrollViewer = FindName("BillScrollViewer") as ScrollViewer;
                scrollViewer?.ScrollToVerticalOffset(0);

                // Collect data
                var invoiceID = InvoiceNo;
                var customerName = name.CustomerName;
                var customerAddress = address.CustomerAddress;
                var vehicleType = type.VehicleType;
                var vehicleNumber = number.VehicleNumber;
                var date = DateTime.Now;
                var subtotal = Convert.ToDecimal(SubtotalText.Replace("Rs", "").Trim());
                var salesTax = Convert.ToDecimal(SalesTaxText.Replace("Rs", "").Trim());
                var discount = Convert.ToDecimal(DiscountText.Replace("Rs", "").Trim());
                var Charges = Convert.ToDecimal(ChargesText.Text.Replace("Rs", "").Trim());
                var totalAmount = Convert.ToDecimal(TotalText.Replace("Rs", "").Trim());
                var paidAmount = Convert.ToDecimal(AmountPaidText.Text.Replace("Rs", "").Trim());
                var balance = Convert.ToDecimal(BalanceText.Replace("Rs", "").Trim());



                // Helper function for safe decimal parsing
                decimal SafeParseDecimal(dynamic value)
                {
                    if (value == null) return 0;

                    // If value contains a percentage symbol, parse it correctly
                    string strValue = value.ToString();
                    if (strValue.EndsWith("%"))
                    {
                        strValue = strValue.TrimEnd('%');
                        if (decimal.TryParse(strValue, out decimal percentValue))
                        {
                            return percentValue / 100; // Convert percentage to decimal (e.g., "10%" => 0.1)
                        }
                    }

                    if (decimal.TryParse(strValue, out decimal result))
                    {
                        return result;
                    }

                    return 0;
                }




                // Collect item details
                var items = descriptionListView.Items.Cast<dynamic>().Select((item, index) => (
                    Description: (string)item.Description,
                    Quantity: (int)((dynamic)quantityListView.Items[index]).Quantity,
                    Price: SafeParseDecimal(((dynamic)priceListView.Items[index]).Price),
                    Tax: SafeParseDecimal(((dynamic)taxListView.Items[index]).Tax),
                    Amount: SafeParseDecimal(((dynamic)amountListView.Items[index]).Amount)
                )).ToList();

                // Save to database
                var Bill = new Invoice();

                Bill.SaveInvoice(
                    invoiceID,
                    customerName,
                    customerAddress,
                    vehicleType,
                    vehicleNumber,
                    date,
                    subtotal,
                    salesTax,
                    discount,
                    totalAmount,
                    Charges,
                    paidAmount,
                    balance,
                    items
                );

                // Print the invoice
                if (!string.IsNullOrEmpty(Dashboard.SelectedPrinter))
                {
                    try
                    {
                        // Create PrintServer and get the print queue
                        using (PrintServer printServer = new PrintServer())
                        {
                            using (PrintQueue printQueue = printServer.GetPrintQueue(Dashboard.SelectedPrinter))
                            {
                                XpsDocumentWriter writer = PrintQueue.CreateXpsDocumentWriter(printQueue);
                                writer.Write(Invoice);
                            }
                        }
                        MessageBox.Show($"Invoice printed successfully on {Dashboard.SelectedPrinter}", "Print Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error printing: {ex.Message}", "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No printer selected. Please select a printer from the dashboard.", "Printer Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while printing the invoice: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.IsEnabled = true;
            }
        }


        public void Buttonhistory_Click(object sender, RoutedEventArgs e)
        {
            // Find the Dashboard  and show the dialog
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("HistoryButtonDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }
    }
}
