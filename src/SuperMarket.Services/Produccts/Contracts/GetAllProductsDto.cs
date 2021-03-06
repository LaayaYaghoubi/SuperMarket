using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Produccts.Contracts
{
    public class GetAllProductsDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }   
        public int CategoryId { get; set; }
    }
}
