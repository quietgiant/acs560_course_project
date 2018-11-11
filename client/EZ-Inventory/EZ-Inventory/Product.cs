using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZ_Inventory
{
    public class Product
    {

        public int  ID { get; set; }
        public int UPC { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public bool IsActive { get; set; }
        public double UnitCost { get; set; }
        public double RetailPrice { get; set; }
        public int UnitsInStock { get; set; }

    }
}
