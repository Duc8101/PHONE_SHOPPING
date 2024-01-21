using API.Services;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService service;
        public ProductController(ProductService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> List(string? name, int? CategoryID, [Required] bool isAdmin = false,[Required] int page = 1)
        {
            ResponseDTO<PagedResultDTO<ProductListDTO>?> result = await service.List(isAdmin,name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpPost]
        public async Task<ResponseDTO<bool>> Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseDTO<bool> result = await service.Create(DTO);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("{ProductID}")]
        public async Task<ResponseDTO<ProductListDTO?>> Detail([Required] Guid ProductID)
        {
            ResponseDTO<ProductListDTO?> response = await service.Detail(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
