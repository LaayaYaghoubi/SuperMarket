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
using System;
using System.Collections.Generic;
using Xunit;
using static BookStore.Specs.BDDHelper;

namespace SuperMarket.Specs.PurchaseVouchers
{
    [Scenario("مشاهده  کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = " کالاها را مدیریت  کنم  ",
      InOrderTo = " کالاها را مشاهده کنم"
    )]
    public class GetAllPurchaseVouchers : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private PurchaseVoucher _purchaseVoucher;
        private UnitOfWork _unitOfWork;
        private PurchaseVoucherRepository _repository;
        private PurchaseVoucherService _sut;
        private IList<GetAllPurchaseVoucherDto> expected;

        public GetAllPurchaseVouchers(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFPurchaseVoucherRepository(_dataContext);
            _sut = new PurchaseVoucherAppService(_repository, _unitOfWork);
        }


        [Given(": دسته بندی با عنوان ‘ لبنیات’ در  فهرست دسته بندی کالاها وجود دارد.")]
        public void Given()
        {
            AddACategory();
        }

        [And("کالا با عنوان ‘شیرکاله’ " +
            "و قیمت ‘3500’ " +
            "و با دسته بندی ‘ لبنیات’ " +
            "و کد ‘101’" +
            " وحداقل موجودی ‘1’" +
            " و حداکثر موجودی ‘10’" +
            "  در فهرست کالاها وجود داشته باشد.")]

        public void And()
        {
            AddAProduct();
        }

        [And("سند ورود کالا با عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-27 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 10 ‘ " +
            "و قیمت کل ‘ 35000’" +
            " در فهرست سندهای ورود کالا وجود داشته باشد")]
        public void AndGiven()
        {
            AddAPurchaseVoucher();
        }

        [When("درخواست مشاهده فهرست کالاها را می دهم")]

        public void When()
        {
            expected = _sut.GetAll();
        }

        [Then("سند ورود کالا با عنوان سند ‘ خرید شیر کاله’" +
           "  و تاریخ انقضا کالا ‘ 2022-05-27 " +
           " و کد کالا ‘101’" +
           " و تعداد کالا ‘ 10 ‘ " +
           "و قیمت کل ‘ 35000’" +
           " باید نمایش داده شود.")]

        public void Then()
        {
            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _purchaseVoucher.Name);
            expected.Should().Contain(_ => _.Id == _purchaseVoucher.Id);
            expected.Should().Contain(_ => _.ProductId == _purchaseVoucher.ProductId);
            expected.Should().Contain(_ => _.Count == _purchaseVoucher.Count);
            expected.Should().Contain(_ => _.TotalPrice == _purchaseVoucher.TotalPrice);
            expected.Should().Contain(_ => _.DateOfPurchase == _purchaseVoucher.DateOfPurchase);
    
        }

        [Fact]
        public void Run()
        {
            Given();
            And();
            AndGiven();
            When();
            Then();
        }

        private void AddACategory()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }
        private void AddAProduct()
        {
            _product = new Product()
            {
                Name = "شیر کاله",
                Price = 3500,
                CategoryId = _category.Id,
                Code = 101,
                MinimumStock = 1,
                MaximumStock = 10
            };
            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }
        private void AddAPurchaseVoucher()
        {
            _purchaseVoucher = new PurchaseVoucher()
            {
                Name = "خرید شیر کاله",
                DateOfPurchase = DateTime.Now,
                ProductId = _product.Id,
                Count = 10,
                TotalPrice = 35000,

            };
            _dataContext.Manipulate(_ => _.PurchaseVouchers.Add(_purchaseVoucher));
        }
    }
}

