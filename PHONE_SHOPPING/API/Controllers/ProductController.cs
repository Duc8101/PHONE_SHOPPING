using API.Attributes;
using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Enum;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> List(string? name, int? CategoryID, [Required] bool isAdmin = false, [Required] int page = 1)
        {
            ResponseDTO<PagedResultDTO<ProductListDTO>?> result = await _service.List(isAdmin, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpPost]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO<bool>> Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseDTO<bool> result = await _service.Create(DTO);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO<ProductListDTO?>> Detail([Required] Guid ProductID)
        {
            ResponseDTO<ProductListDTO?> response = await _service.Detail(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO<ProductListDTO?>> Update([Required] Guid ProductID, [Required] ProductCreateUpdateDTO DTO)
        {
            ResponseDTO<ProductListDTO?> response = await _service.Update(ProductID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> Delete([Required] Guid ProductID)
        {
            ResponseDTO<PagedResultDTO<ProductListDTO>?> response = await _service.Delete(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
