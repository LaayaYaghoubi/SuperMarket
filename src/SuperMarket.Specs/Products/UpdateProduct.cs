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
    [Scenario("ویرایش  کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " کالاها را مدیریت  کنم  ",
       InOrderTo = " کالاها را ویرایش کنم"
     )]
    public class UpdateProduct : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private UpdateProductDto _dto;
        private UnitOfWork _unitOfWork;
        private ProductRepository _repository;
        private ProductService _sut;


        public UpdateProduct(ConfigurationFixture configuration)
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
            _category = new Category()
            {
                Name = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }
        [And("کالا با عنوان ‘شیرکاله’ " +
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

        [When("کالا با عنوان ‘شیرکاله ‘ " +
            "و قیمت ‘3500’ " +
            "و با دسته بندی ‘ لبنیات’" +
            " و کد ‘101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’ " +
            " در فهرست دسته بندی کالا را به کالا" +
            " با عنوان ‘شیر سویا کاله  ‘ " +
            "و قیمت ‘3500’ " +
            "و با دسته بندی ‘ لبنیات’" +
            " و کد ‘101’ " +
            "وحداقل موجودی ‘1’ " +
            "و حداکثر موجودی ‘10’" +
            "   ویرایش می کنم.")]

        public void When()
        {
            _dto = new UpdateProductDto()
            {
                Name = _product.Name,   
                Price = 5000,
                CategoryId = _product.CategoryId,
                Id = _product.Id,
                MinimumStock = _product.MinimumStock,
                MaximumStock = _product.MaximumStock

            };
            _sut.Update(_product.Id, _dto);
        }

        [Then("کالا با عنوان ‘ شیر سویا کاله ‘ " +
            "و قیمت ‘3500’" +
            " و با دسته بندی ‘ لبنیات’" +
            " و کد ‘101’ " +
            "در فهرست کالاها باید  وجود داشته باشد.")]

        public void Then()
        {
            var expected = _dataContext.Products.FirstOrDefault(_ => _.Id == _product.Id);

            expected.Name.Should().Be(_dto.Name);
            expected.Id.Should().Be(_dto.Id);
            expected.Price.Should().Be(_dto.Price);
            expected.MinimumStock.Should().Be(_dto.MinimumStock);
            expected.MaximumStock.Should().Be(_dto.MaximumStock);
            expected.CategoryId.Should().Be(_dto.CategoryId);
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
