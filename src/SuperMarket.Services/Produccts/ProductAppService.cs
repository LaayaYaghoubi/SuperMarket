using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Produccts.Contracts;
using SuperMarket.Services.Produccts.Exceptions;
using SuperMarket.Services.Products.Contracts;
using System.Collections.Generic;

namespace SuperMarket.Services.Produccts
{
    public class ProductAppService : ProductService
    {
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(
            ProductRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddProductDto dto)
        {
            var isCodeDuplicate = _repository.IsExistProductCode(dto.Code);
            if (isCodeDuplicate)
            {
                throw new DuplicateProductCodeException();
            }
            var product = new Product()
            {
                Name = dto.Name,
                Price = dto.Price,
                Code = dto.Code,
                MaximumStock = dto.MaximumStock,
                MinimumStock = dto.MinimumStock,
                CategoryId = dto.CategoryId,
            };
            _repository.Add(product);
            _unitOfWork.Commit();
        }

        public IList<GetAllProductsDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdateProductDto dto)
        {
            var product = _repository.FindById(id);
            var isCodeDuplicate = _repository.IsExistProductCode(dto.Code);
            var DuplicatedCodeProduct = _repository.FindByCode(dto.Code);
            if (product == null)
            {
                throw new ThereIsNoProductWithThisIdException();
            }
            else if (isCodeDuplicate && id != DuplicatedCodeProduct.Id)
            {
                throw new DuplicateProductCodeException();
            }
                product.Code = dto.Code;
                product.MaximumStock = dto.MaximumStock;
                product.MinimumStock = dto.MinimumStock;
                product.Name = dto.Name;
                product.Price = dto.Price;
                product.CategoryId = dto.CategoryId;

                _repository.Update(product);
                _unitOfWork.Commit();
            
           
            
        }
    }
}
