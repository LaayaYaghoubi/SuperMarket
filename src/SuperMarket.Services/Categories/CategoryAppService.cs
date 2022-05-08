using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Categories.Contracts;
using SuperMarket.Services.Categories.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Categories
{
    public class CategoryAppService : CategoryService
    {

        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public CategoryAppService(
            CategoryRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddCategoryDto dto)
        {
            bool isNameDuplicate = _repository.IsExistCategoryName(dto.Name);
               

            if (isNameDuplicate)
            {
                throw new DuplicateCategoryNameException();
            }

            Category category = new Category()
            {
                Name = dto.Name,
            };
            _repository.Add(category);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var category = _repository.FindById(id);
            var isCategoryWithProduct = _repository.IsCategoryWithProduct(id);
            if (isCategoryWithProduct)
            {
                throw new CategoryContainProductException();
            }
            _repository.Delete(category);
            _unitOfWork.Commit();
        }

        public IList<GetCategoryDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdateCategoryDto dto)
        {
            Category category = _repository.FindById(id);
            bool isNameDuplicate = _repository.IsExistCategoryName(dto.Name);
            if (isNameDuplicate)
            {
                throw new DuplicateCategoryNameException();
            }
            category.Name = dto.Name;
            _repository.Update(category);
            _unitOfWork.Commit();
        }
    }
}
