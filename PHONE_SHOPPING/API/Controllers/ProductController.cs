using API.Attributes;
using API.Services.IService;
using DataAccess.Base;
using DataAccess.DTO.ProductDTO;
using DataAccess.Enum;
using DataAccess.Pagination;
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

        [HttpGet("List")]
        public async Task<ResponseBase<Pagination<ProductListDTO>?>> Home(string? name, int? CategoryID, [Required] int page = 1)
        {
            ResponseBase<Pagination<ProductListDTO>?> result = await _service.List(false, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("List")]
        [Role<Pagination<ProductListDTO>>(RoleEnum.Admin)]
        [Authorize<Pagination<ProductListDTO>>]
        public async Task<ResponseBase<Pagination<ProductListDTO>?>> Manager(string? name, int? CategoryID, [Required] int page = 1)
        {
            ResponseBase<Pagination<ProductListDTO>?> result = await _service.List(true, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpPost]
        [Role<bool>(RoleEnum.Admin)]
        [Authorize<bool>]
        public async Task<ResponseBase<bool>> Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase<bool> result = await _service.Create(DTO);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("{ProductID}")]
        [Role<ProductListDTO>(RoleEnum.Admin)]
        [Authorize<ProductListDTO>]
        public async Task<ResponseBase<ProductListDTO?>> Detail([Required] Guid ProductID)
        {
            ResponseBase<ProductListDTO?> response = await _service.Detail(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ProductID}")]
        [Role<ProductListDTO>(RoleEnum.Admin)]
        [Authorize<ProductListDTO>]
        public async Task<ResponseBase<ProductListDTO?>> Update([Required] Guid ProductID, [Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase<ProductListDTO?> response = await _service.Update(ProductID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{ProductID}")]
        [Role<Pagination<ProductListDTO>>(RoleEnum.Admin)]
        [Authorize<Pagination<ProductListDTO>>]
        public async Task<ResponseBase<Pagination<ProductListDTO>?>> Delete([Required] Guid ProductID)
        {
            ResponseBase<Pagination<ProductListDTO>?> response = await _service.Delete(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }


    }
}
