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
using System.Linq;
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
    public class AddProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private AddProductDto _dto;
        private UnitOfWork _unitOfWork;
        private ProductRepository _repository;
        private ProductService _sut;

        public AddProduct(ConfigurationFixture configuration)
            : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
        }
        [Given("هیچ کالایی در فهرست کالا وجود ندارد.")]
        public void Given()
        {

        }

        [And(": دسته بندی با عنوان ‘ لبنیات’ در  فهرست دسته بندی کالاها وجود دارد.")]
        public void And()
        {
            _category = new Category()
            {
                Name = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("کالا با عنوان ‘شیر کاله ‘ " +
            "و قیمت ‘3500’ " +
            "و با عنوان دسته بندی ‘ لبنیات’ " +
            "و کد ‘101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’  تعریف می کنم ")]

        public void When()
        {
            _dto = new AddProductDto()
            {
                Name = "شیر کاله",
                Price = 3500,
                CategoryId = _category.Id,
                Id = 101,
                MinimumStock = 1,
                MaximumStock = 10
            };
            _sut.Add(_dto);
        }
        [Then("کالا با عنوان ‘شیرکاله ‘ " +
            "و قیمت ‘3500’" +
            " و با دسته بندی ‘ لبنیات’ " +
            "و کد کالا ‘ 101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’ " +
            " باید در فهرست کالاها وجود داشته باشد.")]
        public void Then()
        {
            var expected = _dataContext.Products.FirstOrDefault();

            expected.Name.Should().Be(_dto.Name);
            expected.Id.Should().Be(_dto.Id);
            expected.Price.Should().Be(_dto.Price);
            expected.MinimumStock.Should().Be(_dto.MinimumStock);
            expected.CategoryId.Should().Be(_dto.CategoryId);
            expected.MaximumStock.Should().Be(_dto.MaximumStock);
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
