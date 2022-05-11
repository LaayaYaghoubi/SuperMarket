using BookStore.Persistence.EF;
using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.SalesInvoices;
using SuperMarket.Services.SalesInvoices;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Services.SalesInvoices.Exceptions;
using SuperMarket.Tests.Tools.Categories;
using SuperMarket.Tests.Tools.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SupertMarket.Services.Test.Unit.SalesInvoices
{
    public class SalesInvoiceServiceTests
    {
        private readonly SalesInvoiceService _sut;
        private readonly SalesInvoiceRepository _repository;
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        public SalesInvoiceServiceTests()
        {
            _dataContext =
                  new EFInMemoryDatabase()
                  .CreateDataContext<EFDataContext>();
            _repository = new EFSalesInvoiceRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new SalesInvoiceAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_salesinvoice_properly()
        {
            Product product = CreateAndAddAProduct();
            AddSalesInvoiceDto dto = CreateASalesInvoiceForCreatedProduct(product);

            _sut.Add(dto);

            var expected = _dataContext.SalesInvoices.FirstOrDefault();
            expected.ClientName.Should().Be(dto.ClientName);
            expected.NumberOfProducts.Should().Be(dto.NumberOfProducts);
            expected.ProductId.Should().Be(dto.ProductId);
            expected.DateOfSale.Should().Be(dto.DateOfSale);
            expected.TotalPrice.Should().Be(dto.TotalPrice);
        }

        [Fact]
        public void Add_throws_exception_ProductStockReachedMinimumStockException_if_salesinvoiceproducts_reach_minstock_product()
        {
            Product product = CreateAndAddAProduct();
            AddSalesInvoiceDto dto = CreateASalesInvoiceForProductWithMaxStock(product);

            Action expected = () => _sut.Add(dto);

            expected.Should().ThrowExactly<ProductStockReachedMinimumStockException>();
        }
        [Fact]
        public void Update_updates_salesinvoice_properly()
        {
            Product product = CreateAndAddAProduct();
            SalesInvoice salesInvoice = CreateAndAddAsalesInvoice(product);
            UpdateSalesInvoiceDto dto = ChangeCreatedSalesInvoice(salesInvoice);

            _sut.Update(salesInvoice.Id, dto);

            var expected = _dataContext.SalesInvoices.
               FirstOrDefault(_ => _.Id == salesInvoice.Id);
            expected.ClientName.Should().Be(dto.ClientName);
            expected.NumberOfProducts.Should().Be(dto.NumberOfProducts);
            expected.ProductId.Should().Be(dto.ProductId);
            expected.DateOfSale.Should().Be(dto.DateOfSale);
            expected.TotalPrice.Should().Be(dto.TotalPrice);
        }

        private static UpdateSalesInvoiceDto ChangeCreatedSalesInvoice(SalesInvoice salesInvoice)
        {
            return new UpdateSalesInvoiceDto()
            {
                TotalPrice = salesInvoice.TotalPrice,
                ClientName = salesInvoice.ClientName,
                DateOfSale = salesInvoice.DateOfSale,
                NumberOfProducts = 4,
                ProductId = salesInvoice.ProductId,
            };
        }

        private SalesInvoice CreateAndAddAsalesInvoice(Product product)
        {
            SalesInvoice salesInvoice = new SalesInvoice()
            {
                TotalPrice = 7000,
                ClientName = "MrYaghoubi",
                DateOfSale = DateTime.Now,
                NumberOfProducts = 2,
                ProductId = product.Id,
            };
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(salesInvoice));
            return salesInvoice;
        }

        private static AddSalesInvoiceDto CreateASalesInvoiceForProductWithMaxStock(Product product)
        {
            return new AddSalesInvoiceDto()
            {
                ClientName = "MrChenari",
                DateOfSale = DateTime.Now,
                NumberOfProducts = 50,
                TotalPrice = 7000,
                ProductId = product.Id,
            };
        }
        private static AddSalesInvoiceDto CreateASalesInvoiceForCreatedProduct(Product product)
        {
            return new AddSalesInvoiceDto()
            {
                ClientName = "MrChenari",
                DateOfSale = DateTime.Now,
                NumberOfProducts = 2,
                TotalPrice = 7000,
                ProductId = product.Id,
            };
        }
        private Product CreateAndAddAProduct()
        {
            Category category = new CategoryBuilder().CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            Product product = new ProductBuilder().
                WithCategoryId(category.Id).WithStock(10).CreateProduct();
            _dataContext.Manipulate(_ => _.Products.Add(product));
            return product;
        }
    }
}
