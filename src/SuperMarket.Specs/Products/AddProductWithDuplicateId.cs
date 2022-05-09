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
using SuperMarket.Services.Produccts.Exceptions;
using SuperMarket.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static BookStore.Specs.BDDHelper;

namespace SuperMarket.Specs.Products
{
    [Scenario("تعریف  کالا")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = " کالاها را مدیریت  کنم  ",
      InOrderTo = " کالاها را تعریف کنم"
    )]
    public class AddProductWithDuplicateId : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private AddProductDto _dto;
        private UnitOfWork _unitOfWork;
        private ProductRepository _repository;
        private ProductService _sut;
        private Action expected;

        public AddProductWithDuplicateId(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
        }

        [Given(": دسته بندی با عنوان ‘ لبنیات’ در  فهرست دسته بندی کالاها وجود دارد")]
        public void Given()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالا با عنوان ‘شیر کاله’ " +
            "و قیمت ‘3500’ " +
            "و با دسته بندی ‘ لبنیات" +
            " و کد ‘101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’  " +
            "در فهرست کالاها وجود داشته باشد")]

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

        [When("کالا با عنوان ‘ماست کاله’" +
            " و قیمت ‘5000’" +
            " و با عنوان دسته بندی ‘ لبنیات’" +
            " و کد ‘101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’" +
            "   تعریف می کنم")]
        public void When()
        {
            _dto = new AddProductDto()
            {
                Name = "ماست کاله",
                Price = 5000,
                CategoryId = _category.Id,
                Id = 101,
                MinimumStock = 1,
                MaximumStock = 10
            };
           expected =()=> _sut.Add(_dto);
        }

        [Then("تنها یک کالا با کد ‘101’ در فهرست کالاها باید وجود داشته باشد.")]
        public void Then()
        {
            _dataContext.Products.Where(_ => _.Id == _dto.Id)
                .Should().HaveCount(1);
        }

        [And("خطایی با عنوان ‘ کد کالا اضافه شده به فهرست کالاها تکراری است’ باید رخ دهد.")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateProductIdException>();
        }

        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                _ => Given()
              , _ => And()
              , _ => When()
              , _ => Then()
              , _ => ThenAnd());
        }
    }
}