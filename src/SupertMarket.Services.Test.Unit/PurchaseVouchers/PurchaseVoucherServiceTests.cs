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

namespace SupertMarket.Services.Test.Unit.PurchaseVouchers
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

        [Fact]
        public void GetAll_returns_all_purchase_vouchers_properly()
        {
            Category category = CreateAndAddACategory();
            Product product = CreateAndAddAProduct(category);
            PurchaseVoucher purchaseVoucher = CreateAndAddAPurchaseVoucher(product);

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == purchaseVoucher.Name);
            expected.Should().Contain(_ => _.Id == purchaseVoucher.Id);
            expected.Should().Contain(_ => _.ProductId == purchaseVoucher.ProductId);
            expected.Should().Contain(_ => _.NumberOfProducts == purchaseVoucher.NumberOfProducts);
            expected.Should().Contain(_ => _.TotalPrice == purchaseVoucher.TotalPrice);
            expected.Should().Contain(_ => _.DateOfPurchase == purchaseVoucher.DateOfPurchase);
            expected.Should().Contain(_ => _.ExpirationDate == purchaseVoucher.ExpirationDate);
        }

        [Fact]
        public void Update_updates_purchaseVoucher_properly()

        {
            Category category = CreateAndAddACategory();
            Product product = CreateAndAddAProduct(category);
            PurchaseVoucher purchaseVoucher = CreateAndAddAPurchaseVoucher(product);
            UpdatePurchaseVoucherDto dto = ChangeCreatedPurchaseVoucher(purchaseVoucher);

            _sut.Update(purchaseVoucher.Id, dto);

            var expected = _dataContext.PurchaseVouchers
               .FirstOrDefault(_ => _.Id == purchaseVoucher.Id);
            expected.Name.Should().Be(dto.Name);
            expected.ProductId.Should().Be(dto.ProductId);
            expected.DateOfPurchase.Should().Be(dto.DateOfPurchase);
            expected.NumberOfProducts.Should().Be(dto.NumberOfProducts);
            expected.TotalPrice.Should().Be(dto.TotalPrice);
            expected.ExpirationDate.Should().Be(dto.ExpirationDate);
        }
        [Fact]
        public void Update_Throws_exception_ThereIsNoPurchaseVoucherWithThisIdException_if_selected_voucher_does_not_exist()
        {
            int FakeId = 345;
            Category category = CreateAndAddACategory();
            Product product = CreateAndAddAProduct(category);
            PurchaseVoucher purchaseVoucher = CreateAndAddAPurchaseVoucher(product);
            UpdatePurchaseVoucherDto dto = ChangeCreatedPurchaseVoucher(purchaseVoucher);

            Action expected =()=> _sut.Update(FakeId, dto);

            expected.Should().ThrowExactly<ThereIsNoPurchaseVoucherWithThisIdException>();

        }

        private static UpdatePurchaseVoucherDto ChangeCreatedPurchaseVoucher(PurchaseVoucher purchaseVoucher)
        {
            return new UpdatePurchaseVoucherDto()
            {
                Name = purchaseVoucher.Name,
                DateOfPurchase = purchaseVoucher.DateOfPurchase,
                ProductId = purchaseVoucher.ProductId,
                NumberOfProducts = 30,
                TotalPrice = 105000,
                ExpirationDate = DateTime.Parse("2022-05-30T05:21:13.390Z")
            };
        }
        private PurchaseVoucher CreateAndAddAPurchaseVoucher(Product product)
        {
            PurchaseVoucher purchaseVoucher = new PurchaseVoucher()
            {
                Name = product.Name,
                ProductId = product.Id,
                DateOfPurchase = DateTime.Now,
                ExpirationDate = DateTime.Parse("2022-05-27T05:21:13.390Z"),
                NumberOfProducts = 2,
                TotalPrice = 7000,
            };
            _dataContext.Manipulate(_ => _.PurchaseVouchers.Add(purchaseVoucher));
            return purchaseVoucher;
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
