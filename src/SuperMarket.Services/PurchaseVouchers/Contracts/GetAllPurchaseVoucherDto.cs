using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseVouchers.Contracts
{
    public class GetAllPurchaseVoucherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
