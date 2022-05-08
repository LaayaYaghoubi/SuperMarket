using BookStore.Persistence.EF;
using BookStore.Specs.Infrastructure;
using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Products;
using SuperMarket.Services.Produccts;
using SuperMarket.Services.Produccts.Contracts;
using SuperMarket.Services.Products.Contracts;
using System.Collections.Generic;
using Xunit;
using static BookStore.Specs.BDDHelper;

namespace SuperMarket.Specs.Products
{
    [Scenario("مشاهده  کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = " کالاها را مدیریت  کنم  ",
      InOrderTo = " کالاها را مشاهده کنم"
    )]
    public class GetAllProducts : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Product _product;
        private Category _category;
        private GetAllProductsDto _dto;
        private UnitOfWork _unitOfWork;
        private ProductRepository _categoryRepository;
        private ProductService _sut;
        private IList<GetAllProductsDto> expected;
        public GetAllProducts(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_categoryRepository, _unitOfWork);
        }

        [Given(": دسته بندی با عنوان ‘ لبنیات’ در  فهرست دسته بندی کالاها وجود دارد.")]
        public void Given()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
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
            _product = new Product()
            {
                Name = "شیر کاله",
                Price = 3500,
                CategoryId = _category.Id,
                Id = 101,
                MinimumStock = 1,
                MaximumStock = 10
            };
            _dataContext.Manipulate(_ => _.Products.Add(_product)); 
        }

        [When("درخواست مشاهده فهرست کالاها را می دهم")]

        public void When()
        {
           expected = _sut.GetAll();
        }

        [Then("فهرست کالا ها با  کالا با عنوان ‘شیرکاله’ " +
            "و قیمت ‘3500’" +
            " و با دسته بندی ‘ لبنیات’ " +
            "و کد ‘101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’  " +
            "باید نمایش داده شود.")]

        public void Then()
        {
            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _product.Name);
            expected.Should().Contain(_ => _.Id == _product.Id);
            expected.Should().Contain(_ => _.CategoryId == _product.CategoryId);
            expected.Should().Contain(_ => _.MinimumStock == _product.MinimumStock);
            expected.Should().Contain(_ => _.MaximumStock == _product.MaximumStock);
        }

        [Fact]
        public void Run()
        {
            Given();
            And();
            When();
            Then();
        }
    }
}

