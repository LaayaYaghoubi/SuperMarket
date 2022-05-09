using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;

using SuperMarket.Services.PurchaseVouchers.Contracts;

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

            _repository.Add(purchaseVoucher);
            _repository.Update(product);
            _unitOfWork.Commit();
        }
    }
}
