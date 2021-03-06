using BookStore.Specs.Infrastructure;
using FluentAssertions;
using SuperMarket.Persistence.EF;
using SuperMarket.Services.Categories.Contracts;
using System.Linq;
using Xunit;
using static BookStore.Specs.BDDHelper;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Persistence.EF.Categories;
using BookStore.Persistence.EF;
using SuperMarket.Services.Categories;

namespace SuperMarket.Specs.Categories
{
    [Scenario("تعریف دسته بندی")]
    [Feature("",
     AsA = "فروشنده ",
     IWantTo = " دسته بندی کالاها را مدیریت  کنم  ",
     InOrderTo = "دسته بندی های کالا را تعریف کنم"
   )]
    public class AddCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private AddCategoryDto _dto;
        private UnitOfWork _unitOfWork;
        private CategoryRepository _categoryRepository;
        private CategoryService _sut;
        
        public AddCategory(ConfigurationFixture configuration) 
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _categoryRepository = new EFCategoryRepository(_dataContext);
           _sut = new CategoryAppService(_categoryRepository,_unitOfWork);
        }

        [Given("هیچ دسته بندی در فهرست دسته بندی کالا وجود ندارد.")]
        public void Given()
        {

        }
        [When("دسته بندی با عنوان ‘لبنیات’ تعریف می کنم ")]
        public void When()
        {
            CreateACategory();

            _sut.Add(_dto);
        }
        [Then("دسته بندی با عنوان ‘لبنیات’ در فهرست دسته بندی کالا باید وجود داشته باشد.")]
        public void Then()
        {
            var expected = _dataContext.Categories.FirstOrDefault();

            expected.Name.Should().Be(_dto.Name);  
        }
        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
        private void CreateACategory()
        {
            _dto = new AddCategoryDto()
            {
                Name = "لبنیات"
            };
        }
    }
}
