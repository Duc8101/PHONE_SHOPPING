using API.Attributes;
using API.Services.Products;
using Common.Base;
using Common.DTO.ProductDTO;
using Common.Enum;
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
        public ResponseBase Home(string? name, int? CategoryID, [Required] int page = 1)
        {
            ResponseBase result = _service.List(false, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("List")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Manager(string? name, int? CategoryID, [Required] int page = 1)
        {
            ResponseBase result = _service.List(true, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpPost]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase result = _service.Create(DTO);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Detail([Required] Guid ProductID)
        {
            ResponseBase response = _service.Detail(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Update([Required] Guid ProductID, [Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Update(ProductID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Delete([Required] Guid ProductID)
        {
            ResponseBase response = _service.Delete(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }


    }
}
