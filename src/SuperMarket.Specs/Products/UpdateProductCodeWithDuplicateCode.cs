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
using System.Linq;
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
    public class UpdateProductCodeWithDuplicateCode : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private UpdateProductDto _dto;
        private UnitOfWork _unitOfWork;
        private ProductRepository _repository;
        private ProductService _sut;
        private Action expected;

        public UpdateProductCodeWithDuplicateCode(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
        }

        [Given(": دسته بندی با عنوان ‘ لبنیات’ در  فهرست دسته بندی کالاها وجود دارد.")]
        public void Given()
        {
            AddACategory();
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
            AddAProduct();
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
            AddANewProduct();
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
            ChangeProductCodeToDuplicateCode();

            expected = () => _sut.Update(_product.Id, _dto);
        }

        [Then("تنها یک کالا با کد ‘105’ در فهرست کالاها باید وجود داشته باشد")]

        public void Then()
        {
            _dataContext.Products.Where(_ => _.Code == _dto.Code).
                Should().HaveCount(1);

        }

        [And("خطایی با عنوان ‘ کد ویرایش شده تکراری است’ باید رخ دهد ")]
        public void AndThen()
        {
            expected.Should().ThrowExactly<DuplicateProductCodeException>();
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
        private void AddANewProduct()
        {
            var product = new Product()
            {
                Name = "ماست کاله",
                Price = 5000,
                CategoryId = _category.Id,
                Code = 105,
                MinimumStock = 1,
                MaximumStock = 10
            };
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }
        private void ChangeProductCodeToDuplicateCode()
        {
            _dto = new UpdateProductDto()
            {
                Id = _product.Id,
                Name = _product.Name,
                Price = _product.Price,
                CategoryId = _product.CategoryId,
                Code = 105,
                MinimumStock = _product.MinimumStock,
                MaximumStock = _product.MaximumStock

            };
        }


    }
}
