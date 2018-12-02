using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace EZ_Inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string SettingsFilePath = "EZ-InventorySettings.txt";
        private BarcodeReader myBarcodeReader = null;
        public MainWindow()
        {
            InitializeComponent();
            loadSettingsToSettingsTab();
        }

        private void loadSettingsToSettingsTab()
        {
            try
            {
                string readText = File.ReadAllText(SettingsFilePath);
                JObject json = JObject.Parse(readText);
       
                Input_ComPort.Text = (string)json["ComPort"];
            }
            catch (Exception e)
            {
                Input_ComPort.Text = "com4";
            }
        }
     
        public void callback_Input_ViewInventoryEnterUPC(string upc)
        {
            this.Dispatcher.Invoke(() =>
            {
                Input_ViewInventoryEnterUPC.Text = upc;
            });

        }
        public void callback_Input_RestockViewEnterUPC(string upc)
        {
            this.Dispatcher.Invoke(() =>
            {
                Input_RestockView_UPC.Text = upc;
            });

        }
        public void callback_Input_AuditInventoryEnterUPC(string upc)
        {
            this.Dispatcher.Invoke(() =>
            {
                Input_AuditInventoryView_UPC.Text = upc;
            });

        }


        private void Btn_ViewInventoryTab_Click(object sender, RoutedEventArgs e)
        {
            Tab_ViewInventory.IsSelected = true;
            ProductService getItems = new ProductService();
            try
            {
                List<Product> myProducts = getItems.getAllProducts();

                //Grid_ItemsInInventory.DataContext = myProducts;
                Grid_ItemsInInventory.ItemsSource = myProducts;
            }
            catch
            {

            }



        }
        private void Btn_ViewRestockInventoryTab_Click(object sender, RoutedEventArgs e)
        {
            Tab_RestockItem.IsSelected = true;
            
        }
        private void Btn_AuditItemQuanityTab_Click(object sender, RoutedEventArgs e)
        {
            Tab_AuditItems.IsSelected = true;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // filter the results by each new char
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
        private void QuantityInputValidaition(object sender, TextCompositionEventArgs e)
        {
            int QuanityLength = 3;
            Regex regex = new Regex("[^0-9]+");
        IntegerUpDown CurrentTextBox = (IntegerUpDown)sender;
            int NumOfChar = CurrentTextBox.Text.Length;
            if (NumOfChar >= QuanityLength)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = regex.IsMatch(e.Text);
            }

        }
        private void Input_ViewInventoryEnterUPC_GotFocus(object sender, RoutedEventArgs e)
        {
            string ComPort = Input_ComPort.Text;
            if (ComPort != null && ComPort != "")
            {
                 myBarcodeReader = new BarcodeReader(ComPort);
                myBarcodeReader.activateBarcodeReadToTextBox(Input_ComPort, callback_Input_ViewInventoryEnterUPC);
            }
        }
        private void Input_RestockViewEnterUPC_GotFocus(object sender, RoutedEventArgs e)
        {
            string ComPort = Input_ComPort.Text;
            if (ComPort != null && ComPort != "")
            {
                myBarcodeReader = new BarcodeReader(ComPort);
                myBarcodeReader.activateBarcodeReadToTextBox(Input_ComPort, callback_Input_RestockViewEnterUPC);
            }
        }
        private void Input_AuditInventoryViewEnterUPC_GotFocus(object sender, RoutedEventArgs e)
        {
            string ComPort = Input_ComPort.Text;
            if (ComPort != null && ComPort != "")
            {
                myBarcodeReader = new BarcodeReader(ComPort);
                myBarcodeReader.activateBarcodeReadToTextBox(Input_ComPort, callback_Input_AuditInventoryEnterUPC);
            }
        }
        private void Btn_AddItem_Click(object sender, RoutedEventArgs e)
        {

            AddNewItem AddItemWindow = new AddNewItem(Grid_ItemsInInventory, Input_ComPort);
            
            AddItemWindow.ShowDialog();

        }

        private void Btn_SettingsTab_Click(object sender, RoutedEventArgs e)
        {
            Tab_Settings.IsSelected = true;
            
        }

        private void Btn_saveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveSettingsFromSettingsTab();
        }
        private void SaveSettingsFromSettingsTab()
        {
            JObject Settings = new JObject(
    new JProperty("ComPort", Input_ComPort.Text));

            using (StreamWriter file = File.CreateText(SettingsFilePath))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                Settings.WriteTo(writer);
            }
        }

        private void Input_ViewInventoryEnterUPC_LostFocus(object sender, RoutedEventArgs e)
        {
            try{
                myBarcodeReader.CloseConnection();
                myBarcodeReader.killBarcodeReaderThread();
            }
            catch(Exception ex)
            {

            }
            
        }

        private void Btn_Restock_SearchUPC_Click(object sender, RoutedEventArgs e)
        {
            ProductService getItems = new ProductService();
            Input_RestockView_Name.Text = "";
            Input_RestockView_RetailCost.Text = "";
            Input_RestockView_UnitCost.Text = "";
            Input_RestockView_Vendor.Text = "";
            Input_RestockView_Active.Text = "";
            Input_RestockView_Instock.Text = "";
            try
            {
                Product myProducts = getItems.GetProductByUPC(Input_RestockView_UPC.Text);
                if (myProducts == null)
                {
                    Input_RestockView_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    Input_RestockView_UPC.BorderBrush = null;
                    Input_RestockView_Name.Text = myProducts.Name;
                    Input_RestockView_RetailCost.Text = myProducts.RetailPrice.ToString();
                    Input_RestockView_UnitCost.Text = myProducts.UnitCost.ToString();
                    Input_RestockView_Vendor.Text = myProducts.Vendor.ToString();
                    Input_RestockView_Active.Text = myProducts.IsActive.ToString();
                    Input_RestockView_Instock.Text = myProducts.UnitsInStock.ToString();
                }

            }
            catch (Exception ex) {

            }


        }

        private void Btn_ViewInvenory_Search_Click(object sender, RoutedEventArgs e)
        {
            ProductService getItems = new ProductService();
            List<Product> myProducts = new List<Product>();
            try
            {
                Input_RestockView_UPC.BorderBrush = null;
                myProducts.Add(getItems.GetProductByUPC(Input_ViewInventoryEnterUPC.Text));
                //Grid_ItemsInInventory.DataContext = myProducts;
                if (myProducts[0] != null)
                {
                    Grid_ItemsInInventory.ItemsSource = myProducts;
                }
                else
                {
                    Input_ViewInventoryEnterUPC.BorderBrush = System.Windows.Media.Brushes.Red;
                }
               
            }
            catch (Exception ex) {
                Input_ViewInventoryEnterUPC.BorderBrush = System.Windows.Media.Brushes.Red;
            }

        }

        private void Btn_AuditInventoryView_SearchUPC_Click(object sender, RoutedEventArgs e)
        {
            ProductService getItems = new ProductService();
            Input_AuditInventoryView_Name.Text = "";
            Input_AuditInventoryView_RetailCost.Text = "";
            Input_AuditInventoryView_UnitCost.Text = "";
            Input_AuditInventoryView_Vendor.Text = "";
            Input_AuditInventoryView_Active.Text = "";
            Input_AuditInventoryView_Instock.Text = "";
            try
            {
                Product myProducts = getItems.GetProductByUPC(Input_AuditInventoryView_UPC.Text);
                if (myProducts == null)
                {
                    Input_AuditInventoryView_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    Input_AuditInventoryView_UPC.BorderBrush = null;
                    Input_AuditInventoryView_Name.Text = myProducts.Name;
                    Input_AuditInventoryView_RetailCost.Text = myProducts.RetailPrice.ToString();
                    Input_AuditInventoryView_UnitCost.Text = myProducts.UnitCost.ToString();
                    Input_AuditInventoryView_Vendor.Text = myProducts.Vendor.ToString();
                    Input_AuditInventoryView_Active.Text = myProducts.IsActive.ToString();
                    Input_AuditInventoryView_Instock.Text = myProducts.UnitsInStock.ToString();
                }

            }
            catch (Exception ex)
            {

            }

        }

        private void Btn_RestockView_UpdateQuantity_Click(object sender, RoutedEventArgs e)
        {
            Product NewProduct = new Product();
            NewProduct.UPC = Input_RestockView_UPC.Text.ToString();
            NewProduct.Name = Input_RestockView_Name.Text.ToString();

            NewProduct.UnitCost = double.Parse(Input_RestockView_UnitCost.Text.ToString());


            NewProduct.RetailPrice = double.Parse(Input_RestockView_RetailCost.Text.ToString());

            NewProduct.Vendor = Input_RestockView_Vendor.Text.ToString();
            NewProduct.UnitsInStock = int.Parse(Input_RestockView_Instock.Text) + int.Parse(Input_RestockView_ValidatedQuanity.Text);
            NewProduct.IsActive = true;

            ProductService NewProductService = new ProductService();
            NewProductService.UpdateProduct(NewProduct);
            Input_RestockView_ValidatedQuanity.Text = "0";
            Btn_Restock_SearchUPC_Click(null, null);
        }
        private void Btn_AuditInventoryView_UpdateQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (Input_AuditInventoryView_Instock.Text != Input_AuditInventoryView_ValidatedQuanity.Text) {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("The entered quanity is different than the one in our reccords.\n Do you want to update to the new number?", "New Quanity", MessageBoxButton.YesNo,MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Product NewProduct = new Product();
                        NewProduct.UPC = Input_AuditInventoryView_UPC.Text.ToString();
                        NewProduct.Name = Input_AuditInventoryView_Name.Text.ToString();

                        NewProduct.UnitCost = double.Parse(Input_AuditInventoryView_UnitCost.Text.ToString());


                        NewProduct.RetailPrice = double.Parse(Input_AuditInventoryView_RetailCost.Text.ToString());

                        NewProduct.Vendor = Input_AuditInventoryView_Vendor.Text.ToString();
                        NewProduct.IsActive = true;
                        NewProduct.UnitsInStock = int.Parse(Input_AuditInventoryView_ValidatedQuanity.Text);
                        ProductService NewProductService = new ProductService();
                        NewProductService.UpdateProduct(NewProduct);
                        break;
                    case MessageBoxResult.No:

                        break;

                }

            }
            else
            {
                string message = "The new stock quanity matches the recorded quantity.";
                string title = "Values Are Even";
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.None);
            }
            Btn_AuditInventoryView_SearchUPC_Click(null,null);
        }
    }
}
