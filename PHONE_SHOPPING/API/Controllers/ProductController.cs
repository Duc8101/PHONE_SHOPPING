using API.Attributes;
using API.Services.Products;
using Common.Base;
using Common.DTO.ProductDTO;
using Common.Enum;
using Common.Pagination;
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
        public ResponseBase<Pagination<ProductListDTO>?> Home(string? name, int? CategoryID, [Required] int page = 1)
        {
            ResponseBase<Pagination<ProductListDTO>?> result = _service.List(false, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("List")]
        [Role<Pagination<ProductListDTO>>(RoleEnum.Admin)]
        [Authorize<Pagination<ProductListDTO>>]
        public ResponseBase<Pagination<ProductListDTO>?> Manager(string? name, int? CategoryID, [Required] int page = 1)
        {
            ResponseBase<Pagination<ProductListDTO>?> result = _service.List(true, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpPost]
        [Role<bool>(RoleEnum.Admin)]
        [Authorize<bool>]
        public ResponseBase<bool> Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase<bool> result = _service.Create(DTO);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("{ProductID}")]
        [Role<ProductListDTO>(RoleEnum.Admin)]
        [Authorize<ProductListDTO>]
        public ResponseBase<ProductListDTO?> Detail([Required] Guid ProductID)
        {
            ResponseBase<ProductListDTO?> response = _service.Detail(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ProductID}")]
        [Role<ProductListDTO>(RoleEnum.Admin)]
        [Authorize<ProductListDTO>]
        public ResponseBase<ProductListDTO?> Update([Required] Guid ProductID, [Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase<ProductListDTO?> response = _service.Update(ProductID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{ProductID}")]
        [Role<bool>(RoleEnum.Admin)]
        [Authorize<bool>]
        public ResponseBase<bool> Delete([Required] Guid ProductID)
        {
            ResponseBase<bool> response = _service.Delete(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }


    }
}
