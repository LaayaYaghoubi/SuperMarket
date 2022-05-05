using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Entities
{
    public class Product
    {
        public Product()
        {
            PurchaseVoucher = new List<PurchaseVoucher> { };
            SalesInvoice = new List<SalesInvoice> { };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }  
        public DateTime ExpirationDate { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<PurchaseVoucher> PurchaseVoucher { get; set; }
        public int PurchaseVoucherId { get; set; }
        public List<SalesInvoice> SalesInvoice { get; set; }
        public int SalesInvoiceId { get; set; }

    }
}
