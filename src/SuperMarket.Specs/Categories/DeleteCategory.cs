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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class DeleteCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private UnitOfWork _unitOfWork;
        private CategoryRepository _categoryRepository;
        private CategoryService _sut;

        public DeleteCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_categoryRepository, _unitOfWork);
        }
        [Given("دسته بندی با عنوان ‘لبنیات’ در فهرست دسته بندی کالا وجود داشته باشد.")]
        public void Given()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("دسته بندی با عنوان ‘لبنیات’در فهرست دسته بندی کالا را حذف می کنم.")]
        public void When()
        {
            _sut.Delete(_category.Id);
        }

        [Then("فهرست دسته بندی کالا با عنوان ‘لبنیات’نباید وجود داشته باشد.")]
        public void Then()
        {
            _dataContext.Categories.Should().NotContain(_ => _.Name == _category.Name); 
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
