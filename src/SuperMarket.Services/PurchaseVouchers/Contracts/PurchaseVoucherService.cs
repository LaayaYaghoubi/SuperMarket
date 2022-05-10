using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.PurchaseVouchers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseVouchers.Contracts
{
    public interface PurchaseVoucherService : Service
    {
        void Add(AddPurchaseVoucherDto dto);
        void Update(int id, UpdatePurchaseVoucherDto dto);
    }
}
