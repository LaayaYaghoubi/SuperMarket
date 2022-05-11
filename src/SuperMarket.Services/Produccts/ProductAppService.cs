using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Produccts.Contracts;
using SuperMarket.Services.Produccts.Exceptions;
using SuperMarket.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var isIdDuplicate = _repository.IsExistProductId(dto.Id);
            if (isIdDuplicate)
            {
                throw new DuplicateProductIdException();
            }
            var product = new Product()
            {
                Name = dto.Name,
                Price = dto.Price,
                Id = dto.Id,
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
            var isIdDuplicate = _repository.IsExistProductId(dto.Id);
            if (product == null)
            {
                throw new ThereIsNoProductWithThisIdException();
            }
            else if (isIdDuplicate && id != dto.Id)
            {
                throw new DuplicateProductIdException();
            }
                product.Id = dto.Id;
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
