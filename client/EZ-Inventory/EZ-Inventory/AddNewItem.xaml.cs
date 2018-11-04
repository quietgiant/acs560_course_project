using System;
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

namespace EZ_Inventory
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddNewItem: Window
    {
        public AddNewItem()
        {
            InitializeComponent();
        }

        private void Button_AddItem_Click(object sender, RoutedEventArgs e)
        {
            int UPC;
            double UnitCost;
            double RetailPrice;
            try
            {
                UPC = Int32.Parse(Input_UPC.Text);
            }
            catch (Exception ex)
            {
                Input_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "UPC is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                UnitCost = double.Parse(Input_UnitCost.Text);
            }
            catch(Exception ex)
            {
                Input_UnitCost.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "Unit Cost is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                RetailPrice = double.Parse(Input_RetailPrice.Text);
            }
            catch (Exception ex)
            {
                Input_RetailPrice.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "Retail Price is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
