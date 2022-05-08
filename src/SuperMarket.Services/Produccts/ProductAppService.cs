﻿using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Produccts.Contracts;
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
    }
}
