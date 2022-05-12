using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public class GetAllSalesInvoiceDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateOfSale { get; set; }
    }
}
