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
                Id = 104,
                Name = "Milk",
                Price = 3400,
                CategoryId = category.Id,
                MaximumStock = 50,
                MinimumStock = 2,
            };
        }
        public ProductBuilder WithId(int id)
        {
            product.Id = id;
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
                NumberOfProducts = numberOfProducts,
                ExpirationDate = expirationDate,
                ProductId = product.Id,
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
                NumberOfProducts = numberOfProducts,
                ProductId = product.Id,
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

