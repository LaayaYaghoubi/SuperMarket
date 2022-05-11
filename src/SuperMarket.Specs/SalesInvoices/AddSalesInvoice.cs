using BookStore.Persistence.EF;
using BookStore.Specs.Infrastructure;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using System;
using System.Linq;
using static BookStore.Specs.BDDHelper;
using FluentAssertions;
using Xunit;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Persistence.EF.SalesInvoices;
using SuperMarket.Services.SalesInvoices;

namespace SuperMarket.Specs.SalesInvoices
{
    [Scenario("فاکتور فروش  کالا")]
    [Feature("",
     AsA = "فروشنده ",
     IWantTo = "  کالاها را مدیریت  کنم ",
     InOrderTo = " کالاهای فروخته شده را ثبت کنم"
   )]
    public class AddSalesInvoice : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private AddSalesInvoiceDto _dto;
        private UnitOfWork _unitOfWork;
        private SalesInvoiceRepository _repository;
        private SalesInvoiceService _sut;

        public AddSalesInvoice(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSalesInvoiceRepository(_dataContext);
            _sut = new SalesInvoiceAppService(_repository, _unitOfWork);
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
            "و حداکثر موجودی ‘100’ " +
            "و  موجودی ‘4’ " +
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
        [And("هیچ فاکتور فروش کالایی در فهرست فاکتورهای فروش  کالا وجود نداشته باشد.")]

        public void AndGiven()
        {

        }

        [When(" فاکتور فروش کالا با عنوان مشتری ‘ آقای چناری’" +
            "  و تاریخ  ‘همان روز " +
            " و کد کالا ‘101’" +
            " و تعداد کالا ‘ 2 ‘ " +
            "و قیمت کل ‘ 7000’" +
            " را وارد می کنم.")]

        public void When()
        {
            _dto = new AddSalesInvoiceDto()
            {
                ClientName = "آقای چناری",
                DateOfSale = DateTime.Now,
                NumberOfProducts = 2,
                TotalPrice = 7000,
                ProductId = _product.Id,    
            };
            _sut.Add(_dto);
        }

        [Then(" فاکتور فروش کالا با عنوان مشتری ‘ آقای چناری’" +
          "  و تاریخ  ‘همان روز " +
          " و کد کالا ‘101’" +
          " و تعداد کالا ‘ 2 ‘ " +
          "و قیمت کل ‘ 7000’" +
          " باید د فهرست فاکتورهای فروش وجود داشته باشد.")]

        public void Then()
        {
            var expected = _dataContext.SalesInvoices.FirstOrDefault();
            expected.ClientName.Should().Be(_dto.ClientName);
            expected.NumberOfProducts.Should().Be(_dto.NumberOfProducts);
            expected.ProductId.Should().Be(_dto.ProductId);
            expected.DateOfSale.Should().Be(_dto.DateOfSale);
            expected.TotalPrice.Should().Be(_dto.TotalPrice);

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