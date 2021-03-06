﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EZ_Inventory
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddNewItem: Window
    {
        private DataGrid ItemsDatagrid;
        private TextBox Input_ComPort;
        private BarcodeReader myBarcodeReader;
        public AddNewItem(DataGrid ItemsDatagrid,TextBox Input_ComPort)
        {
            this.ItemsDatagrid = ItemsDatagrid;
            this.Input_ComPort = Input_ComPort;
            InitializeComponent();
        }
        public void callback_Input_ViewInventoryEnterUPC(string upc)
        {
            this.Dispatcher.Invoke(() =>
            {
                Input_UPC.Text = upc;
            });

        }
        private void Input_UPC_GotFocus(object sender, RoutedEventArgs e)
        {
            string ComPort = Input_ComPort.Text;
            if (ComPort != null && ComPort != "")
            {
                 myBarcodeReader = new BarcodeReader(ComPort);
                myBarcodeReader.activateBarcodeReadToTextBox(Input_ComPort, callback_Input_ViewInventoryEnterUPC);
            }
        }
        private void UPCInputValidaition(object sender, TextCompositionEventArgs e)
        {
            int UPCLengthLimit = 13;
            Regex regex = new Regex("[^0-9]+");
            TextBox CurrentTextBox = (TextBox)sender;
            int NumOfChar = CurrentTextBox.Text.Length;
            if (NumOfChar >= UPCLengthLimit)
            {

                e.Handled = true;
            }
            else
            {
                e.Handled = regex.IsMatch(e.Text);
            }

        }
        private void DoubleValidaition(object sender, TextCompositionEventArgs e)
        {
           
            int UPCLengthLimit = 13;
            Regex regex = new Regex("[^0-9]+");
            TextBox CurrentTextBox = (TextBox)sender;
            int NumOfChar = CurrentTextBox.Text.Length;
            if (NumOfChar >= UPCLengthLimit)
            {
                e.Handled = true;
            }
            else if (e.Text == "." && !CurrentTextBox.Text.Contains("."))
            {
                e.Handled = false;

            }
            else if(CurrentTextBox.Text.Contains(".") && !regex.IsMatch(e.Text)) {
                int numOfDigitsAfterDecimal = CurrentTextBox.Text.Length - CurrentTextBox.Text.IndexOf(".") - 1;
                if (numOfDigitsAfterDecimal <2)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = regex.IsMatch(e.Text);
            }

        }

        private bool isTextboxValueAnInt(TextBox Input)
        {
            
            return Int64.TryParse(Input.Text, out var Result);
        }
        private bool isTextboxValueAFloat(TextBox Input)
        {

            return double.TryParse(Input.Text, out var Result);
        }
       
        private void Input_UPC_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                myBarcodeReader.CloseConnection();
                myBarcodeReader.killBarcodeReaderThread();
            }
            catch (Exception ex)
            {

            }

        }
        private void Button_AddItem_Click(object sender, RoutedEventArgs e)
        {
            long UPC =0;
            double UnitCost = (double) 0.00;
            double RetailPrice = (double)0.00;
            string name = Input_Name.Text;
            string vendor = Input_Vendor.Text;

      
       

            if (!isTextboxValueAnInt(Input_UPC)) {
                Input_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "UPC is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else
            {
                UPC = Int64.Parse(Input_UPC.Text);
            }


            if (!isTextboxValueAFloat(Input_UnitCost))
            {
                Input_UnitCost.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "Unit Cost is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UnitCost = double.Parse(Input_UnitCost.Text);
            }

            if (!isTextboxValueAFloat(Input_RetailPrice))
            {
                Input_UnitCost.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "Retail Price is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                RetailPrice = double.Parse(Input_RetailPrice.Text);
            }
            if (isTextboxValueAnInt(Input_UPC) && Input_Name.Text != "" && isTextboxValueAFloat(Input_UnitCost) && isTextboxValueAFloat(Input_RetailPrice))
            {
                Product NewProduct = new Product();
                NewProduct.UPC = UPC.ToString();
                NewProduct.Name = Input_Name.Text;

                NewProduct.UnitCost = UnitCost;

           
                NewProduct.RetailPrice = RetailPrice;

                NewProduct.Vendor = vendor;
                NewProduct.IsActive = true;

                ProductService NewProductService = new ProductService();

                Product ValidtateDoesNotExist = NewProductService.GetProductByUPC(Input_UPC.Text,false);
                if (ValidtateDoesNotExist != null)
                {
                    string message = "This item already exists in our records. Please edit the item, if you intend to change the data.";
                    string title = "UPC Already Exists";
                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    NewProductService.CreateNewProduct(NewProduct);

                    ProductService getItems = new ProductService();
                    List<Product> myProducts = getItems.getAllProducts();

                    //Grid_ItemsInInventory.DataContext = myProducts;
                    ItemsDatagrid.ItemsSource = myProducts;


                    this.Close();
                }
            }
        }

        
    }
}
