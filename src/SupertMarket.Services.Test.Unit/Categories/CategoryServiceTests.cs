using BookStore.Persistence.EF;
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
using System.Collections.Generic;
using System.Linq;
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
            AddCategoryDto category = CreateACategory();

            _sut.Add(category);

            _dataContext.Categories.Should()
              .Contain(_ => _.Name == category.Name);
        }

        [Fact]
        public void Add_throws_exception_DuplicateCategoryNameException_if__category_name_duplicated()
        {
            AddACategory();
            AddCategoryDto newCategory = CreateACategoryWithSameName();

            Action expected = () => _sut.Add(newCategory);

            expected.Should().ThrowExactly<DuplicateCategoryNameException>();
        }

        [Fact]
        public void GetAll_returns_all_categories()
        {
            CreateAndAddTwoCarwgories();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(2);
            expected.Should().Contain(_ => _.Name == "dairy");
            expected.Should().Contain(_ => _.Name == "dairy2");
        }

        [Fact]
        public void Update_updates_category_based_on_its_id()
        {
            Category category = CreateAndAddACategory();
            UpdateCategoryDto categoryChanges = ChangeCreatedCategory();

            _sut.Update(category.Id, categoryChanges);

            var expected = _dataContext.Categories.FirstOrDefault(_ => _.Id == category.Id);
            expected.Name.Should().Be(categoryChanges.Name);
        } 

        [Fact]
        public void Update_throws_exception_DuplicateCategoryNameException_if_updated_category_name_duplicated()
        {
            List<Category> categories = CreateAndAddTwoCategories();
            UpdateCategoryDto categoryChanges = ChangeACategoryNameToDuolicateCategoryName();

            Action expected = () => _sut.Update(categories[1].Id, categoryChanges);

            expected.Should().ThrowExactly<DuplicateCategoryNameException>();
        }

        [Fact]
        public void Update_throws_exception_ThereIsNoCtegoryWithThisIdException_if_selected_category_doesnt_exist()
        {
            int DummyId = 123; 
            UpdateCategoryDto categoryChanges = ChangeCreatedCategory();

            Action expected = () => _sut.Update(DummyId, categoryChanges);

            expected.Should().ThrowExactly<ThereIsNoCtegoryWithThisIdException>();
        }

        [Fact]
        public void delete_deletes_category_based_on_its_id()
        {
            Category category = CreateAndAddACategory();

            _sut.Delete(category.Id);

            _dataContext.Categories.Should().NotContain(category);
        }

        [Fact]
        public void Delete_throws_exception_if_selected_category_doesnt_exist_while_deleting()
        {
            int DummyId = 123;

            Action expected = () => _sut.Delete(DummyId);

            expected.Should().ThrowExactly<ThereIsNoCtegoryWithThisIdException>();
        }

        [Fact]
        public void Delete_throws_exception_CategoryContainProductException_if_selected_category_to_delete_contains_product()
        {
            Category category = CreateAndAddACategory();
            CreateAndAddAProductToCreatedCategory(category);

            Action expected = () => _sut.Delete(category.Id);

            expected.Should().ThrowExactly<CategoryContainProductException>();

        }

        private void CreateAndAddAProductToCreatedCategory(Category category)
        {
            Product product = new Product()
            {
                Id = 101,
                Name = "شیر کاله",
                Price = 3500,
                CategoryId = category.Id,
                MinimumStock = 1,
                MaximumStock = 10,
            };
            _dataContext.Manipulate(_ => _.Products.Add(product));
        }
        private static AddCategoryDto CreateACategory()
        {
            return new AddCategoryDto()
            {
                Name = "dairy"
            };
        }
        private static AddCategoryDto CreateACategoryWithSameName()
        {
            return new AddCategoryDto()
            {
                Name = "dairy"
            };
        }
        private void AddACategory()
        {
            var category = new Category()
            {
                Name = "dairy"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }
        private void CreateAndAddTwoCarwgories()
        {
            var categories = new List<Category>
            {
                new Category { Name = "dairy"},
                new Category { Name = "dairy2"},
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));
        }
        private static UpdateCategoryDto ChangeCreatedCategory()
        {
            return new UpdateCategoryDto()
            {
                Name = "Editeddairy"
            };
        }
        private Category CreateAndAddACategory()
        {
            var category = new Category()
            {
                Name = "dairy"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            return category;
        }
        private static UpdateCategoryDto ChangeACategoryNameToDuolicateCategoryName()
        {
            return new UpdateCategoryDto()
            {
                Name = "dairy"
            };
        }
        private List<Category> CreateAndAddTwoCategories()
        {
            var categories = new List<Category>
            {
                new Category { Name = "dairy"},
                new Category { Name = "dairy2"},
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));
            return categories;
        }

    }
}

