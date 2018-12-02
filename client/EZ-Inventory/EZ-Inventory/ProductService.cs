using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EZ_Inventory
{
    class ProductService
    {
        HttpClient productClient;

        public ProductService()
        {
            productClient = new HttpClient();
            productClient.BaseAddress = new Uri("https://inventory-management-219002.appspot.com");
        }

        public  List<Product> getAllProducts()
        {
            List<Product> ProductsInStore = new List<Product>();
            HttpResponseMessage response =  productClient.GetAsync("api/product").Result;
            var temp = response.Content.ReadAsStringAsync().Result;
            ProductsInStore = JsonConvert.DeserializeObject<List<Product>>( response.Content.ReadAsStringAsync().Result);
            
            return ProductsInStore;


        }
        public Product CreateNewProduct(Product NewProduct)
        {

            var Jsoncontent = JsonConvert.SerializeObject(NewProduct);

            HttpResponseMessage response = productClient.PostAsync("api/product", new StringContent(Jsoncontent, Encoding.UTF8, "application/json")).Result;
            var temp = response.Content.ReadAsStringAsync().Result;
            var myStringStatus = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);


            // Call back that closes the form
            return myStringStatus;


        }
        public Product GetProductByUPC(string UPC)
        {


            Product ProductsInStore = new Product();
            HttpResponseMessage response = productClient.GetAsync("api/product/"+UPC).Result;
            var temp = response.Content.ReadAsStringAsync().Result;
            try
            {
                ProductsInStore = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
            }
            catch {
                if (temp == "sql: no rows in result set\n")
                {

                    string message = "Unable To Find Item. This UPC does not exist in the inventory.";
                    string title = "Unable To Find Item";
                    MessageBoxResult result = System.Windows.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
                }
               
            }
            return ProductsInStore;

        }
        public Product UpdateProduct(Product NewProduct)
        {
            var Jsoncontent = JsonConvert.SerializeObject(NewProduct);

            HttpResponseMessage response = productClient.PutAsync("api/product", new StringContent(Jsoncontent, Encoding.UTF8, "application/json")).Result;
            var temp = response.Content.ReadAsStringAsync().Result;
            var myStringStatus = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);


            // Call back that closes the form
            return myStringStatus;
        }
    }
}
