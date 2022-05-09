using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseVouchers.Contracts
{
    public interface PurchaseVoucherRepository : Repository
    {
        void Add(PurchaseVoucher purchaseVoucher);
        Product FindProductById(int productId);
        void Update(Product product);
    }
}
