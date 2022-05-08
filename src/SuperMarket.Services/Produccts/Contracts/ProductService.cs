using SuperMarket.Infrastructure.Application;
using SuperMarket.Services.Produccts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.Products.Contracts
{
    public interface ProductService : Service
    {
        void Add(AddProductDto dto);
        void Update(int id, UpdateProductDto dto);

        IList<GetAllProductsDto> GetAll();
    }
}
