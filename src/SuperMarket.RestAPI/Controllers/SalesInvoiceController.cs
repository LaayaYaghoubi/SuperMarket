using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.SalesInvoices.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/sales-invoices")]
    [ApiController]
    public class SalesInvoiceController : ControllerBase
    {
        private readonly SalesInvoiceService _service;

        public SalesInvoiceController(SalesInvoiceService service)
        {
            _service = service;
        }
        [HttpPost]
        public void Add(AddSalesInvoiceDto SalesInvoice)
        {
            _service.Add(SalesInvoice);
        }

        [HttpGet]
        public IList<GetAllSalesInvoiceDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut]
        public void Update(int id, UpdateSalesInvoiceDto SalesInvoice)
        {
            _service.Update(id, SalesInvoice);
        }
        [HttpDelete]
        public void Delete(int id)
        {
            _service.Delete(id);
        }
    }
}
