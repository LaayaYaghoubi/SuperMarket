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
using System.Collections.Generic;
using Xunit;
using static BookStore.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("نمایش دسته بندی ها")]
    [Feature("",
     AsA = "فروشنده ",
     IWantTo = " دسته بندی کالاها را مدیریت  کنم  ",
     InOrderTo = "دسته بندی های کالا را نمایش دهم"
   )]
    public class GetAllCategories : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private UnitOfWork _unitOfWork;
        private CategoryRepository _categoryRepository;
        private CategoryService _sut;
        private Category _category;
        IList<GetCategoryDto> expected;
        public GetAllCategories(ConfigurationFixture configuration)
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
            AddACategory();
        }
        [When("درخواست مشاهده فهرست دسته بندی کالاها را می دهم")]
        public void When()
        {
          expected = _sut.GetAll();
        }
        [Then("فهرست دسته بندی کالا ها با عنوان ‘لبنیات’ نمایش داده می شود.")]
        public void Then()
        {
            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Name == _category.Name);

        }
        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
        private void AddACategory()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }
    }
}
