using BookStore.Persistence.EF;
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

namespace SupertMarket.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;


        public CategoryServiceTests()
        {
            _dataContext =
                  new EFInMemoryDatabase()
                  .CreateDataContext<EFDataContext>();
            _repository = new EFCategoryRepository(_dataContext);
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            var dto = new AddCategoryDto()
            {
                Name = "dairy"
            };

            _sut.Add(dto);

            _dataContext.Categories.Should()
              .Contain(_ => _.Name == dto.Name);
        }
        [Fact]
        public void GetAll_returns_all_categories()
        {
            var categories = new List<Category>
            {
                new Category { Name = "dairy"},
                new Category { Name = "dairy2"}, 
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));

            var expected = _sut.GetAll();

            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Name == "dairy");
            expected.Should().Contain(_ => _.Name == "dairy2");
         
        }
    }
}
