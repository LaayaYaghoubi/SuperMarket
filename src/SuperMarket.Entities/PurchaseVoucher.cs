using System;
namespace SuperMarket.Entities
{
    public class PurchaseVoucher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Product Product { get; set; }    
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
        public DateTime DateOfPurchase { get; set; }
    }
}
