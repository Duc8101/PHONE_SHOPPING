using API.Services;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService service;
        public CategoryController(CategoryService service)
        {
            this.service = service;
        }

        [HttpGet("All")]
        public async Task<ResponseDTO<List<CategoryListDTO>?>> List()
        {
            ResponseDTO<List<CategoryListDTO>?> response = await service.ListAll();
            Response.StatusCode = response.Code;
            return response;
        }

    }
}
