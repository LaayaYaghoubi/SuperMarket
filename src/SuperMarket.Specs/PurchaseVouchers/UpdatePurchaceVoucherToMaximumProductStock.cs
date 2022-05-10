using BookStore.Persistence.EF;
using BookStore.Specs.Infrastructure;
using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.PurchaseVouchers;
using SuperMarket.Services.PurchaseVouchers;
using SuperMarket.Services.PurchaseVouchers.Contracts;
using SuperMarket.Services.PurchaseVouchers.Exceptions;
using System;
using System.Linq;
using Xunit;
using static BookStore.Specs.BDDHelper;

namespace SuperMarket.Specs.PurchaseVouchers
{
    [Scenario("ویرایش سند ورود کالا")]
    [Feature("",
        AsA = "فروشنده",
        IWantTo = "کالا ها را مدیریت کنم ",
        InOrderTo = "ورود کالاها را ویرایش کنم"
        )]
    public class UpdatePurchaceVoucherToMaximumProductStock : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private PurchaseVoucher _purchaseVoucher;
        private UpdatePurchaseVoucherDto _dto;
        private UnitOfWork _unitOfWork;
        private PurchaseVoucherRepository _repository;
        private PurchaseVoucherService _sut;
        private Action expected;
        public UpdatePurchaceVoucherToMaximumProductStock(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFPurchaseVoucherRepository(_dataContext);
            _sut = new PurchaseVoucherAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان ‘ لبنیات’ در  فهرست دسته بندی کالاها وجود دارد")]
        public void Given()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با با عنوان ‘شیرکاله’" +
           " و قیمت ‘3500’" +
           " و با دسته بندی ‘ لبنیات’" +
           " و کد کالا ‘101’ " +
           "وحداقل موجودی ‘1’ " +
            "و  موجودی ‘4’ " +
           "و حداکثر موجودی ‘100’ " +
           " در فهرست کالاها وجود داشته باشد.")]

        public void And()
        {
            _product = new Product()
            {
                Name = "شیر کاله",
                Price = 3500,
                CategoryId = _category.Id,
                Id = 101,
                MinimumStock = 1,
                MaximumStock = 100,
                Stock = 4
            };
            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }


        [And("سند ورود کالا با عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-27 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 20 ‘ " +
            "و قیمت کل ‘ 70000’" +
            "  در فهرست ورود کالاها وجود داشته باشد.")]


        public void AndGiven()
        {
            _purchaseVoucher = new PurchaseVoucher()
            {
                Name = "خرید شیر کاله",
                DateOfPurchase = DateTime.Now,
                ProductId = _product.Id,
                NumberOfProducts = 20,
                TotalPrice = 70000,
                ExpirationDate = DateTime.Parse("2022-05-27T05:21:13.390Z")
            };
            _dataContext.Manipulate(_ => _.PurchaseVouchers.Add(_purchaseVoucher));
        }

        [When("سند ورود کالا با عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-27 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 20 ‘ " +
            "و قیمت کل ‘ 70000’" +
            "در فهرست ورود کالا ها به سند ورود کالا با" +
            "عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-30 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 100 ‘ " +
            "و قیمت کل ‘ 350000’" +
            " ویرایش می کنم")]

        public void When()
        {
            _dto = new UpdatePurchaseVoucherDto()
            {
              
                Name = _purchaseVoucher.Name,
                DateOfPurchase = _purchaseVoucher.DateOfPurchase,
                ProductId = _product.Id,
                NumberOfProducts = 100,
                TotalPrice = 350000,
                ExpirationDate = DateTime.Parse("2022-05-30T05:21:13.390Z")
            };
           expected=()=> _sut.Update(_purchaseVoucher.Id, _dto);
        }

        [Then("کالا با با عنوان ‘شیرکاله’" +
           " و قیمت ‘3500’" +
           " و با دسته بندی ‘ لبنیات’" +
           " و کد کالا ‘101’ " +
           "وحداقل موجودی ‘1’ " +
            "و  موجودی ‘4’ " +
           "و حداکثر موجودی ‘100’ " +
           " باید در فهرست کالاها وجود داشته باشد.")]

        public void Then()
        {
            var expectedProduct = _dataContext.Products.
                  FirstOrDefault(_ => _.Id == _purchaseVoucher.ProductId);
            expectedProduct.Name.Should().Be(_product.Name);
            expectedProduct.Id.Should().Be(_product.Id);
            expectedProduct.CategoryId.Should().Be(_product.CategoryId);
            expectedProduct.MinimumStock.Should().Be(_product.MinimumStock);
            expectedProduct.MaximumStock.Should().Be(_product.MaximumStock);
            expectedProduct.Price.Should().Be(_product.Price);
            expectedProduct.Stock.Should().Be(_product.Stock);
        }

        [And("خطایی با عنوان‘موجودی کالا از حداکثر موجودی بیشتر شده است’ باید رخ دهد ")]
        public void AndThen()
        {
            expected.Should().ThrowExactly<ProductStockReachedMaximumStockException>();
        }
       

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                 _ => Given()
               , _ => And()
               , _ => AndGiven()
               , _ => When()
               , _ => Then()
               , _ => AndThen()

               );
        }

    }
}
