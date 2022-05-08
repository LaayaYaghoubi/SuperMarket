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
    [Scenario("ویرایش  کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " کالاها را مدیریت  کنم  ",
       InOrderTo = " کالاها را ویرایش کنم"
     )]
    public class UpdateProductIdToDuplicateId : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private UpdateProductDto _dto;
        private UnitOfWork _unitOfWork;
        private ProductRepository _categoryRepository;
        private ProductService _sut;
        private Action expected;

        public UpdateProductIdToDuplicateId(ConfigurationFixture configuration) : base(configuration)
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
        [And("کالا با عنوان ‘شیر’ " +
            "و قیمت ‘3500’ " +
            "و با دسته بندی ‘ لبنیات’ " +
            "و کد ‘101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’ " +
            " در فهرست کالاها وجود داشته باشد")]

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
        [And("کالا با عنوان ‘ماست کاله’" +
            " و قیمت ‘5000’" +
            " و با دسته بندی ‘ لبنیات’ " +
            "و کد ‘105’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’ " +
            " در فهرست کالاها وجود داشته باشد ")]
        public void AndGiven()
        {
            var product = new Product()
            {
                Name = "ماست کاله",
                Price = 5000,
                CategoryId = _category.Id,
                Id = 105,
                MinimumStock = 1,
                MaximumStock = 10
            };
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }

        [When("کالا با عنوان ‘شیر کاله’ " +
            "و قیمت ‘3500’" +
            " و با دسته بندی ‘ لبنیات’ " +
            "و کد ‘101’ " +
            "وحداقل موجودی ‘1’" +
            " و حداکثر موجودی ‘10’ " +
            " در فهرست دسته بندی کالا " +
            "را به کالا با " +
            "عنوان ‘شیر کاله ‘ " +
            "و با قیمت ‘3500’ " +
            "و با دسته بندی ‘ لبنیات’ " +
            "و کد ‘105’" +
            " وحداقل موجودی ‘1’" +
            " و حداکثر موجودی ‘10’  " +
            "ویرایش می کنم.")]

        public void When()
        {
            _dto = new UpdateProductDto()
            {
                Name = _product.Name,
                Price = _product.Price,
                CategoryId = _product.CategoryId,
                Id = 105,
                MinimumStock = _product.MinimumStock,
                MaximumStock = _product.MaximumStock

            };
           expected =()=> _sut.Update(_product.Id, _dto);
        }

        [Then("تنها یک کالا با کد ‘105’ در فهرست کالاها باید وجود داشته باشد")]

        public void Then()
        {
            _dataContext.Products.Where(_ => _.Id == _dto.Id).
                Should().HaveCount(1);

        }

        [And("خطایی با عنوان ‘ کد ویرایش شده تکراری است’ باید رخ دهد ")]
        public void AndThen()
        {
            expected.Should().ThrowExactly<DuplicateProductIdException>();
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
               , _ => AndThen());
        }


    }
}
