using SuperMarket.Entities;
using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Produccts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Products.Contracts
{
    public interface ProductRepository : Repository
    {
        void Add(Product product);
        bool IsExistProductCode(int id);
        Product FindById(int id);
        void Update(Product product);
        IList<GetAllProductsDto> GetAll();
        Product FindByCode(int code);
    }
}
