using BookStore.Persistence.EF;
using BookStore.Specs.Infrastructure;
using FluentAssertions;
using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistence.EF;
using SuperMarket.Persistence.EF.Categories;
using SuperMarket.Services.Categories;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using System;
using Xunit;
using static BookStore.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("حذف دسته بندی")]
    [Feature("",
       AsA = "فروشنده",
       IWantTo = "دسته بندی کالاها را مدیریت کنم",
       InOrderTo = "دسته بندی کالاها را حذف کنم"
       )]
    public class DeleteCategoryWithProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private UnitOfWork _unitOfWork;
        private CategoryRepository _categoryRepository;
        private CategoryService _sut;
        private Action expected;

        public DeleteCategoryWithProduct(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }
        [Given(": دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی کالا وجود داشته باشد.")]
        public void Given()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And(":  کالا با عنوان ‘شیر کاله’ " +
            "و قیمت ‘3500’ " +
            "و با عنوان دسته بندی ‘ لبنیات’" +
            " و کد ‘101’ " +
            " وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’ " +
            " در دسته بندی کالاها وجود داشته باشد.")]

        public void And()
        {
            _product = new Product()
            {
                Id = 101,
                Name = "شیر کاله",
                Price = 3500,
                CategoryId = _category.Id,
                MinimumStock = 1,
                MaximumStock = 10,
            };

            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("دسته بندی با عنوان ‘لبنیات’ در فهرست دسته بندی کالا را حذف می کنم")]
        public void When()
        {
            expected = () => _sut.Delete(_category.Id);
        }

        [Then("فهرست دسته بندی کالا با عنوان ‘لبنیات’ باید وجود داشته باشد.")]
        public void Then()
        {
            _dataContext.Categories.Should().Contain(_ => _.Name == _category.Name);
        }

        [And("خطایی با عنوان ‘ دسته بندی حاوی کالا است’ باید رخ دهد.")]
        public void AndThen()
        {
            expected.Should().ThrowExactly<CategoryContainProductException>();
        }


        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                 _ => Given()
               , _ => And()
               , _ => When()
               , _ => Then()
               , _ => AndThen()
               );
        }
    }
}