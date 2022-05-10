using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
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

        public IList<GetAllSalesInvoiceDto> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
