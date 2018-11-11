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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        private bool isTextboxValueAnInt(TextBox Input)
        {
            
            return Int32.TryParse(Input.Text, out var Result);
        }
        private bool isTextboxValueAFloat(TextBox Input)
        {

            return float.TryParse(Input.Text, out var Result);
        }
        private void Button_AddItem_Click(object sender, RoutedEventArgs e)
        {
            int UPC =0;
            float UnitCost = (float) 0.00;
            float RetailPrice = (float)0.00;
            string name = Input_Name.Text;
            string vendor = "";

      
       

            if (!isTextboxValueAnInt(Input_UPC)) {
                Input_UPC.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "UPC is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else
            {
                UPC = Int32.Parse(Input_UPC.Text);
            }


            if (!isTextboxValueAFloat(Input_UnitCost))
            {
                Input_UnitCost.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "Unit Cost is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UnitCost = float.Parse(Input_UnitCost.Text);
            }

            if (!isTextboxValueAFloat(Input_RetailPrice))
            {
                Input_UnitCost.BorderBrush = System.Windows.Media.Brushes.Red;
                string message = "Retail Price is not in the correct format";
                string title = "Unable To Add Item";
                MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                RetailPrice = float.Parse(Input_RetailPrice.Text);
            }
            if (isTextboxValueAnInt(Input_UPC) && isTextboxValueAFloat(Input_UnitCost) && isTextboxValueAFloat(Input_RetailPrice))
            {
                Product NewProduct = new Product();
                NewProduct.UPC = UPC;
                NewProduct.Name = Input_Name.Text;

                var jsonObject = new JObject();
                jsonObject.Add("Float64", UnitCost);
                NewProduct.UnitCost = jsonObject;

                var jsonObject2 = new JObject();
                jsonObject2.Add("Float64", RetailPrice);
                NewProduct.RetailPrice = jsonObject2;

                NewProduct.Vendor = vendor;
                NewProduct.IsActive = true;

                ProductService NewProductService = new ProductService();
                NewProductService.CreateNewProduct(NewProduct);

            }
        }
    }
}
