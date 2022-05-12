using SuperMarket.Entities;
using System;

namespace SuperMarket.Tests.Tools.Products
{
    public class ProductBuilder
    {
        Product product = new Product();
        Category category = new Category()
        {
            Name = "Dairy"
        };
        public ProductBuilder()
        {
            product = new Product()
            {
                Code = 104,
                Name = "Milk",
                Price = 3400,
                CategoryId = category.Id,
                MaximumStock = 50,
                MinimumStock = 2,
            };
        }
        public ProductBuilder WithCode(int code)
        {
            product.Code = code;
            return this;
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
      
        public ProductBuilder WithCategoryId(int categoryId)
        {
            product.CategoryId = categoryId;
            return this;
        }
        public ProductBuilder WithCategoryName(string name)
        {
            product.Category.Name = name;
            return this;
        }
        public ProductBuilder WithPurchaseVoucher(decimal totalPrice, 
            int numberOfProducts, DateTime expirationDate)
        {
            product.PurchaseVoucher = new PurchaseVoucher()
            {
                Name = product.Name,
                TotalPrice = totalPrice,
                Count = numberOfProducts,
                ProductId = product.Code,
                DateOfPurchase = DateTime.Now
        };
            return this;
        }

        public ProductBuilder WithSalesInvoice(decimal totalPrice,
            int numberOfProducts, string clientName)
        {
            product.SalesInvoice = new SalesInvoice()
            {
                ClientName = clientName,
                TotalPrice = totalPrice,
                Count = numberOfProducts,
                ProductId = product.Code,
                DateOfSale = DateTime.Now
            };
            return this;
        }
        public Product CreateProduct()
        {
            return product;
        }
    }

    }

