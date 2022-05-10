using BookStore.Persistence.EF;
using BookStore.Specs.Infrastructure;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.PurchaseVouchers;
using SuperMarket.Services.PurchaseVouchers.Contracts;
using SuperMarket.Services.PurchaseVouchers;
using System;
using System.Linq;
using static BookStore.Specs.BDDHelper;
using FluentAssertions;
using Xunit;
using SuperMarket.Services.PurchaseVouchers.Exceptions;

namespace SuperMarket.Specs.PurchaseVouchers
{
    [Scenario("ورود  کالا")]
    [Feature("",
     AsA = "فروشنده ",
     IWantTo = "  کالاها را مدیریت  کنم  ",
     InOrderTo = " کالاهای وارد شده را ثبت کنم"
   )]
    public class AddPurchaseVoucherWithMaximumProductStock : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private AddPurchaseVoucherDto _dto;
        private UnitOfWork _unitOfWork;
        private PurchaseVoucherRepository _repository;
        private PurchaseVoucherService _sut;
        private Action expected;

        public AddPurchaseVoucherWithMaximumProductStock(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFPurchaseVoucherRepository(_dataContext);
            _sut = new PurchaseVoucherAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان ‘لبنیات’ در فهرست دسته بندی وجود دارد.")]

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
        [And("هیچ سند ورود کالایی در فهرست سندهای ورود کالا وجود نداشته باشد.")]

        public void AndGiven()
        {

        }

        [When("سند ورود کالا با عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-27 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 100 ‘ " +
            "و قیمت کل ‘ 350000’" +
            " را وارد می کنم.")]

        public void When()
        {
            _dto = new AddPurchaseVoucherDto()
            {
                Name = "خرید شیر کاله",
                DateOfPurchase = DateTime.Now,
                ProductId = _product.Id,
                NumberOfProducts = 100,
                TotalPrice = 350000,
                ExpirationDate = DateTime.Parse("2022-05-27T05:21:13.390Z")
            };
             expected =()=>  _sut.Add(_dto);
        }

        [Then(" خطایی با عنوان‘موجودی کالا از حداکثر موجودی بیشتر شده است’ باید رخ دهد")]
        public void Then()
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
               );
        }
    }
}