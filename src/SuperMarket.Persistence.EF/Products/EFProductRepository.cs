using SuperMarket.Entities;
using SuperMarket.Services.Produccts.Contracts;
using SuperMarket.Services.Products.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly EFDataContext _dataContext;
        public EFProductRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Product product)
        {
           _dataContext.Products.Add(product);  
        }

        public Product FindById(int id)
        {
            return _dataContext.Products.Find(id); 
        }

        public IList<GetAllProductsDto> GetAll()
        {
            return _dataContext.Products
                  .Select(_ => new GetAllProductsDto
                  {
                      Id = _.Id,
                      Name = _.Name,
                      Price = _.Price,  
                      MinimumStock = _.MinimumStock,    
                      MaximumStock = _.MaximumStock,
                      CategoryId = _.CategoryId,
                  }).ToList();
        }

        public bool IsExistProductId(int id)
        {
            return _dataContext.Products.Any(_ => _.Id == id); 
        }

        public void Update(Product product)
        {
            _dataContext.Products.Update(product);
        }
    }
}
