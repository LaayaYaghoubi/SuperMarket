using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;

using SuperMarket.Services.PurchaseVouchers.Contracts;
using SuperMarket.Services.PurchaseVouchers.Exceptions;
using System.Collections.Generic;

namespace SuperMarket.Services.PurchaseVouchers
{
    public class PurchaseVoucherAppService : PurchaseVoucherService
    {
        private readonly PurchaseVoucherRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public PurchaseVoucherAppService(
            PurchaseVoucherRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddPurchaseVoucherDto dto)
        {
            var purchaseVoucher = new PurchaseVoucher()
            {
                Name = dto.Name,
                ProductId = dto.ProductId,  
                DateOfPurchase = dto.DateOfPurchase,
                NumberOfProducts = dto.NumberOfProducts,
                TotalPrice = dto.TotalPrice,
                ExpirationDate = dto.ExpirationDate,
            };
            var product = _repository.FindProductById(dto.ProductId);
            product.Stock = product.Stock + dto.NumberOfProducts;
            product.ExpirationDate = dto.ExpirationDate;

            if (product.Stock > product.MaximumStock)
            {
                throw new ProductStockReachedMaximumStockException();
            }

            _repository.Add(purchaseVoucher);
            _repository.UpdateProduct(product);
            _unitOfWork.Commit();

          
        }

        public IList<GetAllPurchaseVoucherDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdatePurchaseVoucherDto dto)
        {
            var purchaseVoucher = _repository.FindVoucherById(id);
            purchaseVoucher.Id = dto.Id;
            purchaseVoucher.Name = dto.Name;
            purchaseVoucher.TotalPrice = dto.TotalPrice;
            purchaseVoucher.NumberOfProducts = dto.NumberOfProducts;    
            purchaseVoucher.ProductId = dto.ProductId;  
            purchaseVoucher.DateOfPurchase = dto.DateOfPurchase;
            purchaseVoucher.ExpirationDate = dto.ExpirationDate;

            var product = _repository.FindProductById(dto.ProductId);
            product.Stock = product.Stock + dto.NumberOfProducts;
            product.ExpirationDate = dto.ExpirationDate;

            if (product.Stock > product.MaximumStock)
            {
                throw new ProductStockReachedMaximumStockException();
            }

            _repository.Update(purchaseVoucher);
            _repository.UpdateProduct(product);
            _unitOfWork.Commit();
        }
    }
}
