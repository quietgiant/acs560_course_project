using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace EZ_Inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
        }

 
        public void callback(string upc)
        {
            this.Dispatcher.Invoke(() =>
            {
                Input_ViewInventoryEnterUPC.Text = upc;
            });
          
        }
    

        private void Btn_ViewInventoryTab_Click(object sender, RoutedEventArgs e)
        {
            Tab_ViewInventory.IsSelected = true;

           

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

        private void Input_ViewInventoryEnterUPC_GotFocus(object sender, RoutedEventArgs e)
        {
            BarcodeReader myBarcodeReader = new BarcodeReader("com4");
            myBarcodeReader.activateBarcodeReadToTextBox(callback);
        }
    }
}
