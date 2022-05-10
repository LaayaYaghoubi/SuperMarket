using SuperMarket.Entities;
using SuperMarket.Services.PurchaseVouchers.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.PurchaseVouchers
{
    public class EFPurchaseVoucherRepository : PurchaseVoucherRepository
    {
        private readonly EFDataContext _dataContext;
        public EFPurchaseVoucherRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(PurchaseVoucher purchaseVoucher)
        {
            _dataContext.PurchaseVouchers.Add(purchaseVoucher);
          
        }

        public Product FindProductById(int productId)
        {
          return  _dataContext.Products.Find(productId);
        }

        public PurchaseVoucher FindVoucherById(int id)
        {
            return _dataContext.PurchaseVouchers.Find(id);

        }

        public void Update(PurchaseVoucher purchaseVoucher)
        {
            _dataContext.PurchaseVouchers.Update(purchaseVoucher);
        }

        public void UpdateProduct(Product product)
        {
            _dataContext.Products.Update(product);
        }
    }
}
