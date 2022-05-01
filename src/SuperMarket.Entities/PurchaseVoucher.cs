using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Entities
{
    public class PurchaseVoucher
    {
        public PurchaseVoucher()
        {
            Product = new List<Product> { };
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Product { get; set;}
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }  
      



    }
}
