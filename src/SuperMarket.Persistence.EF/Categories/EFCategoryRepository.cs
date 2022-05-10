using SuperMarket.Entities;
using SuperMarket.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Categories
{
    public class EFCategoryRepository : CategoryRepository
    {
        private readonly EFDataContext _dataContext;
        public EFCategoryRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Category category)
        {
            _dataContext.Categories.Add(category);
        }

        public void Delete(Category category)
        {
           _dataContext.Categories.Remove(category);    
        }

        public Category FindById(int id)
        {
            return _dataContext.Categories.Find(id);
        }

        public IList<GetCategoryDto> GetAll()
        {
            return _dataContext.Categories
                  .Select(_ => new GetCategoryDto
                  {
                      Id = _.Id,
                      Name = _.Name
                  }).ToList();
        }

        public bool IsCategoryExist(int id)
        {
          return _dataContext.Categories.Any(_ => _.Id == id);
        }

        public bool IsCategoryWithProduct(int id)
        {
            return _dataContext.Categories.Any(_ => _.Id == id && _.products.Count> 0);
            
        }

        public bool IsExistCategoryName(string name)
        {
            return _dataContext.Categories.Any(_ => _.Name == name);
        }

        public void Update(Category category)
        {
            _dataContext.Update(category);
        }
    }

    
    }

