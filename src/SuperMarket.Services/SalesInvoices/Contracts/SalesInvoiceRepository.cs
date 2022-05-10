using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public interface SalesInvoiceRepository : Repository
    {
        Product FindProductById(int productId);
        void Add(SalesInvoice salesInvoice);
        void UpdateProduct(Product product);
        IList<GetAllSalesInvoiceDto> GetAll();
    }
}
