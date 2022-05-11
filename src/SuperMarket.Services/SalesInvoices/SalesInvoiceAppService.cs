using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.PurchaseVouchers.Exceptions;
using SuperMarket.Services.SalesInvoices.Contracts;
using SuperMarket.Services.SalesInvoices.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices
{
    public class SalesInvoiceAppService : SalesInvoiceService
    {

        private readonly SalesInvoiceRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public SalesInvoiceAppService(
            SalesInvoiceRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddSalesInvoiceDto dto)
        {
            var salesInvoice = new SalesInvoice()
            {
               ClientName = dto.ClientName, 
               ProductId = dto.ProductId,
               NumberOfProducts = dto.NumberOfProducts,
               TotalPrice = dto.TotalPrice,
               DateOfSale = dto.DateOfSale,
            };
            var product = _repository.FindProductById(dto.ProductId);
            product.Stock = product.Stock - dto.NumberOfProducts;

            if (product.Stock <= product.MinimumStock)
            {
                throw new ProductStockReachedMinimumStockException();
            }

            _repository.Add(salesInvoice);
            _repository.UpdateProduct(product);
            _unitOfWork.Commit();


        }

        public void Delete(int id)
        {
            var salesInvoice = _repository.FindInvoiceById(id);
            var product = _repository.FindProductById(salesInvoice.ProductId);
            product.Stock = product.Stock + salesInvoice.NumberOfProducts;
            _repository.Delete(salesInvoice);
            _repository.UpdateProduct(product);
            _unitOfWork.Commit();
        }

        public IList<GetAllSalesInvoiceDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id ,UpdateSalesInvoiceDto dto)
        {
            var salesInvoice = _repository.FindInvoiceById(id);
            if(salesInvoice == null)
            {
                throw new ThereIsNoPurchaseVoucherWithThisIdException();
            }

            salesInvoice.NumberOfProducts = dto.NumberOfProducts;
            salesInvoice.DateOfSale = dto.DateOfSale;
            salesInvoice.ClientName = dto.ClientName;
            salesInvoice.ProductId = dto.ProductId;
            salesInvoice.TotalPrice = dto.TotalPrice;
            

            var product = _repository.FindProductById(dto.ProductId);
            product.Stock = product.Stock - dto.NumberOfProducts;     

            if (product.Stock <= product.MinimumStock)
            {
                throw new ProductStockReachedMinimumStockException();
            }

            _repository.Update(salesInvoice);
            _repository.UpdateProduct(product);
            _unitOfWork.Commit();
        }
    }
}
