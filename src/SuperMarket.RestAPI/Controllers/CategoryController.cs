using Microsoft.AspNetCore.Mvc;
using SuperMarket.Services.Categories.Contracts;
using System.Collections.Generic;

namespace SuperMarket.RestAPI.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }
        [HttpPost]
        public void Add(AddCategoryDto category)
        {
            _service.Add(category);
        }

        [HttpGet]
        public IList<GetCategoryDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpPut]
        public void Update(int id, UpdateCategoryDto category)
        {
            _service.Update(id, category);
        }
        [HttpDelete]
        public void Delete(int id)
        {
            _service.Delete(id);
        }


    }
}
