﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Entities
{
    public class SalesInvoice
    {
      
        public int Id { get; set; }
        public string ClientName { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }  
        public int NumberOfProducts { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateOfSale { get; set; }


    }
}
