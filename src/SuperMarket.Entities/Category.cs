using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Entities
{
    public class Category
    {
        public Category()
        {
            products = new List<Product>() { };
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> products { get; set; }

         

    }
}
