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
        


    }
}
