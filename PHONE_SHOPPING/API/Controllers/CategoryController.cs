using API.Services;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("Paged")]
        public async Task<ResponseDTO<PagedResultDTO<CategoryListDTO>?>> List(string? name, [Required] int page = 1)
        {
            ResponseDTO<PagedResultDTO<CategoryListDTO>?> response = await service.ListPaged(name, page);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> Create([Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseDTO<bool> response = await service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{ID}")]
        public async Task<ResponseDTO<CategoryListDTO?>> Detail([Required] int ID)
        {
            ResponseDTO<CategoryListDTO?> response = await service.Detail(ID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ID}")]
        public async Task<ResponseDTO<CategoryListDTO?>> Update([Required] int ID, [Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseDTO<CategoryListDTO?> response = await service.Update(ID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

    }
}
