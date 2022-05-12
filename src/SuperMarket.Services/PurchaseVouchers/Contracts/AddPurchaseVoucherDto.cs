using System;
namespace SuperMarket.Services.PurchaseVouchers.Contracts
{
    public class AddPurchaseVoucherDto
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
        public DateTime DateOfPurchase { get; set; }   
    }
}
