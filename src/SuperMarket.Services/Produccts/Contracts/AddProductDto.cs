using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Produccts.Contracts
{
    public class AddProductDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int MinimumStock { get; set; }
        [Required]
        public int MaximumStock { get; set; }
        [Required]
        public int CategoryId { get; set; }
       
    }
}
