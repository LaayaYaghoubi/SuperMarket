using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }  
        public DateTime ExpirationDate { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

    }
}
