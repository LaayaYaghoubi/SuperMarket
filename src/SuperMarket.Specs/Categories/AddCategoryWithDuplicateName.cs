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
using System.Linq;
using Xunit;
using static BookStore.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("تعریف دسته بندی با عنوان تکراری")]
    [Feature("",
       AsA = "فروشنده ",
     IWantTo = " دسته بندی کالاها را مدیریت  کنم  ",
     InOrderTo = "دسته بندی های کالا را مدیریت کنم"
        )]
    public class AddCategoryWithDuplicateName : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private AddCategoryDto _dto;
        private UnitOfWork _unitOfWork;
        private CategoryRepository _categoryRepository;
        private CategoryService _sut;
        private Category _category;
        private Action expected;
        public AddCategoryWithDuplicateName(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان ‘لبنیات’ در فهرست دسته بندی کالا وجود داشته باشد.")]
        public void Given()
        {
            AddACategory();
        }
        [When("دسته بندی با عنوان ‘لبنیات’ تعریف می کنم ")]
        public void When()
        {
            CreateACategoryWithDuplicateName();
            expected = () => _sut.Add(_dto);
        }
        [Then("تنها یک دسته بندی با عنوان’ لبنیات’ در فهرست دسته بندی کالا باید وجود داشته باشد.")]
        public void Then()
        {
            _dataContext.Categories.Where(_ => _.Name == _dto.Name)
                .Should().HaveCount(1);
        }
        [And("خطایی با عنوان ‘عنوان دسته بندی کالا تکراری است’ باید رخ دهد.")]
        public void And()
        {
            expected.Should().ThrowExactly<DuplicateCategoryNameException>();
        }
        [Fact]
        public void Run()
        {
            Runner.RunScenario(
                 _ => Given()
               , _ => When()
               , _ => Then()
               , _ => And());
        } 
        public void AddACategory()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }
        private void CreateACategoryWithDuplicateName()
        {
            _dto = new AddCategoryDto()
            {
                Name = "لبنیات"
            };
        }
    }
}
