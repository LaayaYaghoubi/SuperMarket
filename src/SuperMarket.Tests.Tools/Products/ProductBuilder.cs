using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Tests.Tools.Products
{
    public class ProductBuilder
    {
        Product product = new Product();
        Category category = new Category();
        public ProductBuilder()
        {
            product = new Product()
            {
                Name = "Milk",
                Price = 3400,
                CategoryId = category.Id,
                MaximumStock = 50,
                MinimumStock = 10,
            };
        }
        public ProductBuilder WithName(string name)
        {
            product.Name = name;
            return this;
        }
        public ProductBuilder WithPrice(decimal price)
        {
            product.Price = price;
            return this;
        }
        public ProductBuilder WithStock(int stock)
        {
            product.Stock = stock;  
            return this;
        }
        public ProductBuilder WithMinimumStock(int minstock)
        {
            product.MinimumStock = minstock;
            return this;
        }
        public ProductBuilder WithMaximumStock(int maxstock)
        {
            product.MaximumStock = maxstock;
            return this;
        }
        public ProductBuilder WithExpirationDate(DateTime date)
        {
            product.ExpirationDate = date;
            return this;
        }
        public ProductBuilder WithCategoryId(int categoryId)
        {
            product.CategoryId = categoryId;
            return this;
        }
        public ProductBuilder WithPurchaseVoucher(string name, int productId, decimal totalPrice)
        {
            product.PurchaseVoucher = new PurchaseVoucher
            {
              Name = name,
              ProductId = productId,    
              TotalPrice = totalPrice,

            };
            return this;
            
        }



    }

    }

