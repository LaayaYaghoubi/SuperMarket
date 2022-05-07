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

        public Category FindById(int id)
        {
            return _dataContext.Categories.Find(id);
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
