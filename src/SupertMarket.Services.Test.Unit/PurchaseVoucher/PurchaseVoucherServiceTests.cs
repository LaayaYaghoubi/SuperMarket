using BookStore.Persistence.EF;
using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.PurchaseVouchers;
using SuperMarket.Services.PurchaseVouchers;
using SuperMarket.Services.PurchaseVouchers.Contracts;
using SuperMarket.Services.PurchaseVouchers.Exceptions;
using SuperMarket.Tests.Tools.Categories;
using SuperMarket.Tests.Tools.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SupertMarket.Services.Test.Unit.PurchaseVoucher
{
    public class PurchaseVoucherServiceTests
    {
        private readonly PurchaseVoucherService _sut;
        private readonly PurchaseVoucherRepository _repository;
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        public PurchaseVoucherServiceTests()
        {
            _dataContext =
                  new EFInMemoryDatabase()
                  .CreateDataContext<EFDataContext>();
            _repository = new EFPurchaseVoucherRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new PurchaseVoucherAppService(_repository, _unitOfWork);
        }
        [Fact]
        public void Add_adds_purchaseVoucher_properly()
        {
            Category category = CreateAndAddACategory();
            Product product = CreateAndAddAProduct(category);
            AddPurchaseVoucherDto dto = CreateAPurchaseVoucherForCreatedProduct(product);

            _sut.Add(dto);

            var expected = _dataContext.PurchaseVouchers.FirstOrDefault();
            expected.Name.Should().Be(dto.Name);
            expected.ProductId.Should().Be(dto.ProductId);
            expected.TotalPrice.Should().Be(dto.TotalPrice);
            expected.NumberOfProducts.Should().Be(dto.NumberOfProducts);
            expected.DateOfPurchase.Should().Be(dto.DateOfPurchase);
            expected.Product.Stock.Should().Be(product.Stock);
            expected.Product.ExpirationDate.Should().Be(dto.ExpirationDate);
        }
        [Fact]
        public void Add_throws_exception_ProductStockReachedMaximumStockException_if_sum_of_number_Of_PurchaseVoucher_Products_and_stock_products_becomes_greater_than_maximum_stock()
        {
            Category category = CreateAndAddACategory();
            Product product = CreateAndAddAProduct(category);
            AddPurchaseVoucherDto dto = CreateAPurchaseVoucherForCreatedProductWithMaximimStockValue(product);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<ProductStockReachedMaximumStockException>();
        }

        private static AddPurchaseVoucherDto CreateAPurchaseVoucherForCreatedProductWithMaximimStockValue(Product product)
        {
            return new AddPurchaseVoucherDto()
            {
                Name = product.Name,
                ProductId = product.Id,
                DateOfPurchase = DateTime.Now,
                ExpirationDate = DateTime.Parse("2022-05-27T05:21:13.390Z"),
                NumberOfProducts = 50,
                TotalPrice = 175000,
            };
        }

        private static AddPurchaseVoucherDto CreateAPurchaseVoucherForCreatedProduct(Product product)
        {
            return new AddPurchaseVoucherDto()
            {
                Name = product.Name,
                ProductId = product.Id,
                DateOfPurchase = DateTime.Now,
                ExpirationDate = DateTime.Parse("2022-05-27T05:21:13.390Z"),
                NumberOfProducts = 2,
                TotalPrice = 7000,
            };
        }
        private Product CreateAndAddAProduct(Category category)
        {
            var product = new ProductBuilder().
                WithCategoryId(category.Id).CreateProduct();
            _dataContext.Manipulate(_ => _.Products.Add(product));
            return product;
        }
        private Category CreateAndAddACategory()
        {
            var category = new CategoryBuilder().CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            return category;
        }
    }
}
