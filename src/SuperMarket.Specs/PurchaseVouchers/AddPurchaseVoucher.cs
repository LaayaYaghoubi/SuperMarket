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

namespace SuperMarket.Specs.PurchaseVouchers
{
    [Scenario("ورود  کالا")]
    [Feature("",
     AsA = "فروشنده ",
     IWantTo = " کالاها را مدیریت  کنم  ",
     InOrderTo = " کالاهای وارد شده را ثبت کنم"
   )]
    public class AddPurchaseVoucher : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private AddPurchaseVoucherDto _dto;
        private UnitOfWork _unitOfWork;
        private PurchaseVoucherRepository _repository;
        private PurchaseVoucherService _sut;

        public AddPurchaseVoucher(ConfigurationFixture configuration) 
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
            "و حداکثر موجودی ‘10’ " +
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
                MaximumStock = 10,
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
            " و تعداد کالا ‘ 10 ‘ " +
            "و قیمت کل ‘ 35000’" +
            " را وارد می کنم.")]

        public void When()
        {

           
            _dto = new AddPurchaseVoucherDto()
            {
                Name = "خرید شیر کاله",
                DateOfPurchase = DateTime.Now,
                ProductId = _product.Id,
                NumberOfProducts = 10,
                TotalPrice = 35000,
                ExpirationDate = DateTime.Parse("2022-05-27T05:21:13.390Z")
            };
          

            _sut.Add(_dto);
        }

        [Then("سند ورود کالا با عنوان سند " +
            "‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-27 " +
            "و تعداد کالا ‘ 14 ‘ " +
            "و کد کالا ‘101’ " +
            "و قیمت کل ‘ 35000’ " +
            " در فهرست سندهای ورود کالا باید وجود داشته باشد.")]

        public void Then()
        {
            var expected = _dataContext.PurchaseVouchers.FirstOrDefault();
            expected.Name.Should().Be(_dto.Name);
            expected.ProductId.Should().Be(_dto.ProductId);
            expected.TotalPrice.Should().Be(_dto.TotalPrice);
            expected.NumberOfProducts.Should().Be(_dto.NumberOfProducts);
            expected.DateOfPurchase.Should().Be(_dto.DateOfPurchase);
            expected.Product.Stock.Should().Be(14); 

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
    }
}