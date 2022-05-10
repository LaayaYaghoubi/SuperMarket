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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class UpdatePurchaseVoucher : EFDataContextDatabaseFixture
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
        public UpdatePurchaseVoucher(ConfigurationFixture configuration) 
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
            "در فهرست ورود کالا ها به سند ورود کالا با"+
            "عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-30 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 30 ‘ " +
            "و قیمت کل ‘ 105000’" +  
            " ویرایش می کنم")]

        public void When()
        {
            _dto = new UpdatePurchaseVoucherDto()
            {
                Id = _purchaseVoucher.Id,   
                Name = _purchaseVoucher.Name,
                DateOfPurchase =_purchaseVoucher.DateOfPurchase,
                ProductId = _product.Id,
                NumberOfProducts = 30,
                TotalPrice = 105000,
                ExpirationDate = DateTime.Parse("2022-05-30T05:21:13.390Z")
            };
            _sut.Update(_purchaseVoucher.Id, _dto);
        }
        [Then("سند ورود کالا با عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-27 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 20 ‘ " +
            "و قیمت کل ‘ 70000’" +
            "در فهرست ورود کالا ها به سند ورود کالا با" +
            "عنوان سند ‘ خرید شیر کاله’" +
            "  و تاریخ انقضا کالا ‘ 2022-05-30 " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 30 ‘ " +
            "و قیمت کل ‘ 105000’" +
            "  در فهرست سندهای ورود کالا" +
            "باید وجود داشته باشد")]

        public void Then()
        {
            var expected = _dataContext.PurchaseVouchers
                .FirstOrDefault(_ => _.Id == _purchaseVoucher.Id);
            expected.Name.Should().Be(_dto.Name);
            expected.ProductId.Should().Be(_dto.ProductId); 
            expected.DateOfPurchase.Should().Be(_dto.DateOfPurchase);   
            expected.NumberOfProducts.Should().Be(_dto.NumberOfProducts);   
            expected.TotalPrice.Should().Be(_dto.TotalPrice);   
            expected.ExpirationDate.Should().Be(_dto.ExpirationDate);
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
