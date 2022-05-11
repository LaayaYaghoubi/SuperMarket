using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Tests.Tools.Categories
{
    public class CategoryBuilder
    {
     
       Category category = new Category();
        public CategoryBuilder()
        {
            category = new Category()
            {
                Name = "Dairy",
            };
        }
       
        public CategoryBuilder WithName(string name)
        {
            category.Name = name;
            return this;
        }
     
        public Category Create()
        {
            return category;
        }
    }

}
