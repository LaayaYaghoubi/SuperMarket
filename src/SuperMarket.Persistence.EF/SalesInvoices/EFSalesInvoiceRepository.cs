using SuperMarket.Entities;
using SuperMarket.Services.SalesInvoices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.SalesInvoices
{
    public class EFSalesInvoiceRepository : SalesInvoiceRepository
    {
        private readonly EFDataContext _dataContext;
        public EFSalesInvoiceRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(SalesInvoice salesInvoice)
        {
           _dataContext.SalesInvoices.Add(salesInvoice);
        }

        public void Delete(SalesInvoice salesInvoice)
        {
            _dataContext.SalesInvoices.Remove(salesInvoice);
        }

        public SalesInvoice FindInvoiceById(int id)
        {
            return _dataContext.SalesInvoices.Find(id);
        }

        public Product FindProductById(int productId)
        {
           return _dataContext.Products.Find(productId);
        }

        public IList<GetAllSalesInvoiceDto> GetAll()
        {
            return _dataContext.SalesInvoices
                .Select(_ => new GetAllSalesInvoiceDto
                {
                    Id = _.Id,
                    ClientName = _.ClientName,  
                    Count = _.Count,
                    DateOfSale = _.DateOfSale,
                    ProductId = _.ProductId,
                    TotalPrice = _.TotalPrice,  

                }).ToList();

        }

        public void Update(SalesInvoice salesInvoice)
        {
           _dataContext.SalesInvoices.Update(salesInvoice);
        }

        public void UpdateProduct(Product product)
        {
          _dataContext.Products.Update(product);    
        }
    }
}
