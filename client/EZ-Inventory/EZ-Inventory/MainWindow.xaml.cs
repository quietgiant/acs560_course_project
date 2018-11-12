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
   


        private void Btn_ViewInventoryTab_Click(object sender, RoutedEventArgs e)
        {
            Tab_ViewInventory.IsSelected = true;
            ProductService getItems = new ProductService();
           List<Product> myProducts =  getItems.getAllProducts();
    
            //Grid_ItemsInInventory.DataContext = myProducts;
            Grid_ItemsInInventory.ItemsSource = myProducts;




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
            try
            {
                Product myProducts = getItems.GetProductByUPC(Input_RestockView_UPC.Text);
                Input_RestockView_Name.Text = myProducts.Name;
                Input_RestockView_RetailCost.Text = myProducts.RetailPrice.ToString();
                Input_RestockView_UnitCost.Text = myProducts.UnitCost.ToString();
                Input_RestockView_Vendor.Text = myProducts.Vendor.ToString();
                Input_RestockView_Active.Text = myProducts.IsActive.ToString();
                Input_RestockView_Instock.Text = myProducts.UnitCost.ToString();
            }
            catch (Exception ex) { }


        }

        private void Btn_ViewInvenory_Search_Click(object sender, RoutedEventArgs e)
        {
            ProductService getItems = new ProductService();
            List<Product> myProducts = new List<Product>();
            try
            {
                myProducts.Add(getItems.GetProductByUPC(Input_ViewInventoryEnterUPC.Text));
            }
            catch (Exception ex) { }
            //Grid_ItemsInInventory.DataContext = myProducts;
            Grid_ItemsInInventory.ItemsSource = myProducts;
        }
    }
}
