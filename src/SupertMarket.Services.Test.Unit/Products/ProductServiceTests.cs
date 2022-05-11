using BookStore.Persistence.EF;
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
using SuperMarket.Tests.Tools.Categories;
using SuperMarket.Tests.Tools.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SupertMarket.Services.Test.Unit.Products
{
    public class ProductServiceTests
    {
        private readonly ProductService _sut;
        private readonly ProductRepository _repository;
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        public ProductServiceTests()
        {
            _dataContext =
                  new EFInMemoryDatabase()
                  .CreateDataContext<EFDataContext>();
            _repository = new EFProductRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_product_properly()
        {
            AddProductDto dto = CreateAProduct();

            _sut.Add(dto);

            var expected = _dataContext.Products.FirstOrDefault();
            expected.CategoryId.Should().Be(dto.CategoryId);
            expected.Name.Should().Be(dto.Name);
            expected.Price.Should().Be(dto.Price);
            expected.Id.Should().Be(dto.Id);
            expected.MinimumStock.Should().Be(dto.MinimumStock);
            expected.MaximumStock.Should().Be(dto.MaximumStock);

        }

        [Fact]
        public void Add_throws_DuplicateProductIdException_if_new_product_id_is_duplicated()
        {
            Product product = CreateAndAddAProduct();
            AddProductDto newdto = CreateAProductWithSameId(product);

            Action expected = () => _sut.Add(newdto);

            expected.Should().ThrowExactly<DuplicateProductIdException>();

        }

        private static AddProductDto CreateAProductWithSameId(Product product)
        {
            return new AddProductDto()
            {
                Name = "butter",
                Price = 3400,
                CategoryId = product.CategoryId,
                Id = product.Id,
                MinimumStock = product.MinimumStock,
                MaximumStock = product.MaximumStock
            };
        }

        private Product CreateAndAddAProduct()
        {
            var category = new CategoryBuilder().CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = new ProductBuilder()
                .WithCategoryId(category.Id).
                CreateProduct();
            AddProductDto dto = new AddProductDto()
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Id = product.Id,
                MinimumStock = product.MinimumStock,
                MaximumStock = product.MaximumStock
            };
            _dataContext.Manipulate(_ => _.Products.Add(product));
            return product;
        }

        private AddProductDto CreateAProduct()
        {
            var category = new CategoryBuilder().CreateCategory();
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = new ProductBuilder()
                .WithCategoryId(category.Id).
                CreateProduct();
            AddProductDto dto = new AddProductDto()
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Id = product.Id,
                MinimumStock = product.MinimumStock,
                MaximumStock = product.MaximumStock
            };
            return dto;
        }
    }
}
