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
    [Scenario("ویرایش دسته بندی")]
    [Feature("",
      AsA = "فروشنده ",
      IWantTo = " دسته بندی کالاها را مدیریت  کنم  ",
      InOrderTo = "دسته بندی های کالا را ویرایش کنم"
    )]
    public class UpdateCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private UpdateCategoryDto _dto;
        private UnitOfWork _unitOfWork;
        private CategoryRepository _categoryRepository;
        private CategoryService _sut;
        private Category _category;
       
        public UpdateCategory(ConfigurationFixture configuration)
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
        [When("عنوان 'لبنیات' را به 'تنقلات' ویرایش میکنم")]
        public void When()
        {
            ChangeCreatedCategory();

            _sut.Update(_category.Id, _dto);
        }
        [Then("دسته بندی با عنوان ‘تنقلات ’در فهرست دسته بندی کالا باید وجود داشته باشد.")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault(_ => _.Id == _category.Id);

            expected.Name.Should().Be(_dto.Name);
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
        private void ChangeCreatedCategory()
        {
            _dto = new UpdateCategoryDto
            {
                Name = "تنقلات"
            };
        }


    }
}
