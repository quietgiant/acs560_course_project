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


        public void callback_Input_UpdateSearchUPCUI(string upc)
        {
            this.Dispatcher.Invoke(() =>
            {
                TabItem ti = Tab_Main.SelectedItem as TabItem;
                string TabName = ti.Name;
                if (TabName == "Tab_ViewInventory")
                {
                    Input_ViewInventoryEnterUPC.Text = upc;
                }

                else if (TabName == "Tab_RestockItem")
                {

                    Input_RestockView_UPC.Text = upc;
                }
                else if (TabName == "Tab_AuditItems")
                {

                    Input_AuditInventoryView_UPC.Text = upc;
                }
                else if (TabName == "Tab_EditAItem")
                {

                    Input_EditAnItemView_UPC.Text = upc;
                }

            });

        }


        public void callback_input_UpdateQuantityBy1(string UPC)
        {


            this.Dispatcher.Invoke(() =>
            {
                TabItem ti = Tab_Main.SelectedItem as TabItem;
                string TabName = ti.Name;
                if (TabName == "Tab_RestockItem")
                {

                    if (UPC == Input_RestockView_UPC.Text)
                    {

                        Input_RestockView_ValidatedQuanity.Text = (int.Parse(Input_RestockView_ValidatedQuanity.Text) + 1).ToString();

                    }
                    else
                    {
                        string message = "This UPC is different than the searched UPC";
                        string title = "Unable To Update Quanity";
                        MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (TabName == "Tab_AuditItems")
                {

                    if (UPC == Input_AuditInventoryView_UPC.Text)
                    {

                        Input_AuditInventoryView_ValidatedQuanity.Text = (int.Parse(Input_AuditInventoryView_ValidatedQuanity.Text) + 1).ToString();

                    }
                    else
                    {
                        string message = "This UPC is different than the searched UPC";
                        string title = "Unable To Update Quanity";
                        MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
               
            });
        }



        private void Btn_ChangeTab_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string ButtonName = btn.Name.ToString();
            if(ButtonName == "Btn_ViewInventoryTab")
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
            else if (ButtonName == "Btn_ViewRestockInventoryTab")
            {
                Tab_RestockItem.IsSelected = true;
            }
            else if (ButtonName == "Btn_AuditItemQuanityTab")
            {
                Tab_AuditItems.IsSelected = true;
            }
            else if (ButtonName == "Btn_EditAnItemTab")
            {
                Tab_EditAItem.IsSelected = true;
            }
            else if (ButtonName == "Btn_SettingsTab")
            {
                Tab_Settings.IsSelected = true;
            }

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


 
        private void Input_Barcode_UpdateQuanity_GotFocus(object sender, RoutedEventArgs e)
        {
            string ComPort = Input_ComPort.Text;
            if (ComPort != null && ComPort != "")
            {
                myBarcodeReader = new BarcodeReader(ComPort);
                myBarcodeReader.activateBarcodeReadToTextBox(Input_ComPort, callback_input_UpdateQuantityBy1);
            }
        }



        private void Input_SearchForUPC_GotFocus(object sender, RoutedEventArgs e)
        {
            string ComPort = Input_ComPort.Text;
            if (ComPort != null && ComPort != "")
            {
                myBarcodeReader = new BarcodeReader(ComPort);
                myBarcodeReader.activateBarcodeReadToTextBox(Input_ComPort, callback_Input_UpdateSearchUPCUI);
            }
        }
      
        private void Btn_AddItem_Click(object sender, RoutedEventArgs e)
        {

            AddNewItem AddItemWindow = new AddNewItem(Grid_ItemsInInventory, Input_ComPort);
      
            AddItemWindow.ShowDialog();

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
                Product myProducts = getItems.GetProductByUPC(Input_RestockView_UPC.Text,true);
                if (myProducts == null)
                {
                    Input_RestockView_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else
                {

                    Btn_RestockView_UpdateQuantity.Visibility = Visibility.Visible;
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
        private void Btn_EditAnItem_SearchUPC_Click(object sender, RoutedEventArgs e)
        {
            ProductService getItems = new ProductService();
            Input_EditAnItemView_Name.Text = "";
            Input_EditAnItemView_RetailCost.Text = "";
            Input_EditAnItemView_UnitCost.Text = "";
            Input_EditAnItemView_Vendor.Text = "";
            Input_EditAnItemView_Active.Text = "";
            Input_EditAnItemView_NumberInstock.Text = "";
            try
            {
                Product myProducts = getItems.GetProductByUPC(Input_EditAnItemView_UPC.Text, true);
                if (myProducts == null)
                {
                    Input_RestockView_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    Btn_EditAnItemView_UpdateItems.Visibility = Visibility.Visible;
                    Input_EditAnItemView_UPC.BorderBrush = null;
                    Input_EditAnItemView_Name.Text = myProducts.Name;
                    Input_EditAnItemView_RetailCost.Text = myProducts.RetailPrice.ToString();
                    Input_EditAnItemView_UnitCost.Text = myProducts.UnitCost.ToString();
                    Input_EditAnItemView_Vendor.Text = myProducts.Vendor.ToString();

                    if (myProducts.IsActive.ToString() == "True")
                    {
                        Input_EditAnItemView_Active.Text = "True";
                        Btn_EditAnItemView_OpositeActivate.Content = "Deactivate";
                        Btn_EditAnItemView_OpositeActivate.Visibility = Visibility.Visible;
                        
                    }
                    else
                    {
                        Input_EditAnItemView_Active.Text = "False";
                        Btn_EditAnItemView_OpositeActivate.Content = "Activate";
                        Btn_EditAnItemView_OpositeActivate.Visibility = Visibility.Visible;
                    }
                    
                    Input_EditAnItemView_NumberInstock.Text = myProducts.UnitsInStock.ToString();
                }

            }
            catch (Exception ex)
            {

            }


        }
        private void Btn_EditAnItemView_swapActivity(object sender, RoutedEventArgs e)
        {
            if(Input_EditAnItemView_Active.Text == "True")
            {
                Input_EditAnItemView_Active.Text = "False";
                Btn_EditAnItemView_OpositeActivate.Content = "Activate";
            }
            else
            {
                Input_EditAnItemView_Active.Text = "True";
                Btn_EditAnItemView_OpositeActivate.Content = "Deactivate";
            }
        }
        private void Btn_ViewInvenory_Search_Click(object sender, RoutedEventArgs e)
        {
            ProductService getItems = new ProductService();
            List<Product> myProducts = new List<Product>();
            try
            {
                Input_RestockView_UPC.BorderBrush = null;
                myProducts.Add(getItems.GetProductByUPC(Input_ViewInventoryEnterUPC.Text,true));
                //Grid_ItemsInInventory.DataContext = myProducts;
                if (myProducts[0] != null)
                {
                    Grid_ItemsInInventory.ItemsSource = myProducts;
                }
                else
                {
                    Input_ViewInventoryEnterUPC.BorderBrush = System.Windows.Media.Brushes.Red;
                    try
                    {
                         myProducts = getItems.getAllProducts();

                        //Grid_ItemsInInventory.DataContext = myProducts;
                        Grid_ItemsInInventory.ItemsSource = myProducts;
                    }
                    catch
                    {

                    }

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
                Product myProducts = getItems.GetProductByUPC(Input_AuditInventoryView_UPC.Text,true);
                if (myProducts == null)
                {
                    Input_AuditInventoryView_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                }
                else
                {
                   Btn_AuditInventoryView_AuditInventory.Visibility = Visibility.Visible;
                
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
            NewProduct.UPC = Input_RestockView_UPC.Text.ToString().TrimStart(new Char[] { '0' }); ;
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
        private bool isTextboxValueAnInt(TextBox Input)
        {

            return Int64.TryParse(Input.Text, out var Result);
        }
        private bool isTextboxValueAFloat(TextBox Input)
        {

            return double.TryParse(Input.Text, out var Result);
        }
        private void Btn_EditAnItemView_UpdateItem_Click(object sender, RoutedEventArgs e)
        {
            Product NewProduct = new Product();
            bool ItemCanBeUpdated = true;
            if (Input_EditAnItemView_UPC.Text != "" && Input_EditAnItemView_Name.Text != "" && Input_EditAnItemView_RetailCost.Text!="" 
                && Input_EditAnItemView_UnitCost.Text!= "") {

                if (!isTextboxValueAnInt(Input_EditAnItemView_UPC))
                {
                    ItemCanBeUpdated = false;
                    Input_EditAnItemView_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                    string message = "UPC is not in the correct format";
                    string title = "Unable To Add Item";
                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    NewProduct.UPC = Int64.Parse(Input_EditAnItemView_UPC.Text).ToString().TrimStart(new Char[] { '0' }); ;
                }

                NewProduct.Name = Input_EditAnItemView_Name.Text.ToString();



                if (!isTextboxValueAFloat(Input_EditAnItemView_RetailCost))
                {
                    ItemCanBeUpdated = false;
                    Input_EditAnItemView_RetailCost.BorderBrush = System.Windows.Media.Brushes.Red;
                    string message = "Retail Cost is not in the correct format";
                    string title = "Unable To Add Item";
                    Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    NewProduct.RetailPrice = Double.Parse(Input_EditAnItemView_RetailCost.Text.ToString());
                }
                if (!isTextboxValueAFloat(Input_EditAnItemView_UnitCost))
                {
                    ItemCanBeUpdated = false;
                    Input_EditAnItemView_UnitCost.BorderBrush = System.Windows.Media.Brushes.Red;
                    string message = "Unit Cost is not in the correct format";
                    string title = "Unable To Add Item";
                    Xceed.Wpf.Toolkit.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    NewProduct.UnitCost = Double.Parse(Input_EditAnItemView_UnitCost.Text.ToString());
                }
                NewProduct.Vendor = Input_EditAnItemView_Vendor.Text.ToString();
                NewProduct.UnitsInStock = int.Parse(Input_EditAnItemView_NumberInstock.Text);
                bool b = Input_EditAnItemView_Active.Text == "True";
                NewProduct.IsActive = b;



                if (ItemCanBeUpdated)
                {
                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure you want to update this item?", "Edit Info", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            ProductService NewProductService = new ProductService();
                            NewProductService.UpdateProduct(NewProduct);
                            Xceed.Wpf.Toolkit.MessageBox.Show("Item was updated with the new information.", "Item Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                            break;
                        case MessageBoxResult.No:

                            break;

                    }


                    Input_EditAnItemView_UPC.Text = "";
                    Input_EditAnItemView_Name.Text = "";
                    Input_EditAnItemView_RetailCost.Text = "";
                    Input_EditAnItemView_UnitCost.Text = "";
                    Input_EditAnItemView_Vendor.Text = "";
                    Input_EditAnItemView_Active.Text = "";
                    Input_EditAnItemView_NumberInstock.Text = "";
                }

            }
        }
        private void Btn_AuditInventoryView_UpdateQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (Input_AuditInventoryView_Instock.Text != Input_AuditInventoryView_ValidatedQuanity.Text) {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("The entered quanity is different than the one in our reccords.\n Do you want to update to the new number?", "New Quanity", MessageBoxButton.YesNo,MessageBoxImage.Error);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Product NewProduct = new Product();
                        NewProduct.UPC = Input_AuditInventoryView_UPC.Text.ToString().TrimStart(new Char[] { '0' }); ;
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
