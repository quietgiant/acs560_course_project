using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        public String CreateNewProduct(Product NewProduct)
        {

            var Jsoncontent = JsonConvert.SerializeObject(NewProduct);

            HttpResponseMessage response = productClient.PostAsync("api/product", new StringContent(Jsoncontent, Encoding.UTF8, "application/json")).Result;
            var temp = response.Content.ReadAsStringAsync().Result;
            var myStringStatus = JsonConvert.DeserializeObject<String>(response.Content.ReadAsStringAsync().Result);


            // Call back that closes the form
            return myStringStatus;


        }
        public Product GetProductByUPC(string UPC)
        {


            Product ProductsInStore = new Product();
            HttpResponseMessage response = productClient.GetAsync("api/product/"+UPC).Result;
            var temp = response.Content.ReadAsStringAsync().Result;
            ProductsInStore = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);

            return ProductsInStore;

        }

    }
}
