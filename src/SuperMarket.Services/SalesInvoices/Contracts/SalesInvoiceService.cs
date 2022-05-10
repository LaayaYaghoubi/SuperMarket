using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.SalesInvoices.Contracts
{
    public interface SalesInvoiceService : Service
    {
        void Add(AddSalesInvoiceDto dto);
        IList<GetAllSalesInvoiceDto> GetAll();
        void Update(int id, UpdateSalesInvoiceDto dto);
        void Delete(int id);
    }
}
