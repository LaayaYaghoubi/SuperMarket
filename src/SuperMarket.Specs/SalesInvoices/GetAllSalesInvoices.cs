using BookStore.Persistence.EF;
using BookStore.Specs.Infrastructure;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using System;
using static BookStore.Specs.BDDHelper;
using FluentAssertions;
using Xunit;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Persistence.EF.SalesInvoices;
using SuperMarket.Services.SalesInvoices;
using System.Collections.Generic;

namespace SuperMarket.Specs.SalesInvoices
{
    [Scenario("مشاهده ی فاکتور فروش  کالا")]
    [Feature("",
     AsA = "فروشنده ",
     IWantTo = "  کالاها را مدیریت  کنم ",
     InOrderTo = " کالاهای فروخته شده را مشاهده کنم"
   )]
    public class GetAllSalesInvoices : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private SalesInvoice _salesInvoice; 
        private UnitOfWork _unitOfWork;
        private SalesInvoiceRepository _repository;
        private SalesInvoiceService _sut;
        private IList<GetAllSalesInvoiceDto> expected;

        public GetAllSalesInvoices(ConfigurationFixture configuration)
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
        [And(" فاکتور فروش کالا با عنوان مشتری ‘ آقای چناری’" +
           "  و تاریخ  ‘همان روز " +
           " و کد کالا ‘101’" +
           " و تعداد کالا ‘ 2 ‘ " +
           "و قیمت کل ‘ 7000’" +
           " در فهرست فاکتورهای فروش وجود داشته باشد.")]

        public void AndGiven()
        {
            _salesInvoice = new SalesInvoice()
            {
                TotalPrice = 7000,
                ClientName = "آقای چناری",
                DateOfSale = DateTime.Now,
                NumberOfProducts = 2,  
                ProductId = _product.Id,
            };
            _dataContext.Manipulate(_ => _.SalesInvoices.Add(_salesInvoice));
          
        }

        [When(" درخواست مشاهده ی فاکتور های فروش را می دهم.")]

        public void When()
        {
            expected = _sut.GetAll();
        }

        [Then(" فاکتور فروش کالا با عنوان مشتری ‘ آقای چناری’" +
          "  و تاریخ  ‘همان روز " +
          " و کد کالا ‘101’" +
          " و تعداد کالا ‘ 2 ‘ " +
          "و قیمت کل ‘ 7000’" +
          " باید نمایش داده شود..")]

        public void Then()
        {
            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.ClientName == _salesInvoice.ClientName);
            expected.Should().Contain(_ => _.ProductId == _salesInvoice.ProductId);
            expected.Should().Contain(_ => _.NumberOfProducts == _salesInvoice.NumberOfProducts);
            expected.Should().Contain(_ => _.TotalPrice == _salesInvoice.TotalPrice);
            expected.Should().Contain(_ => _.DateOfSale == _salesInvoice.DateOfSale);

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