using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.PurchaseVouchers.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/purchase-voucher")]
    [ApiController]
    public class PurchaseVoucherController : ControllerBase
    {
        private readonly PurchaseVoucherService _service;

        public PurchaseVoucherController(PurchaseVoucherService service)
        {
            _service = service;
        }
        [HttpPost]
        public void Add(AddPurchaseVoucherDto PurchaseVoucher)
        {
            _service.Add(PurchaseVoucher);
        }

        [HttpGet]
        public IList<GetAllPurchaseVoucherDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut]
        public void Update(int id, UpdatePurchaseVoucherDto PurchaseVoucher)
        {
            _service.Update(id, PurchaseVoucher);
        }

    }
}
