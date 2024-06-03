using API.Attributes;
using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.ProductDTO;
using DataAccess.Entity;
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
        public async Task<ResponseDTO> List(string? name, int? CategoryID, [Required] int page = 1)
        {
            User? user = (User?) HttpContext.Items["user"];
            bool isAdmin = user != null && user.RoleId == (int) RoleEnum.Admin;
            ResponseDTO result = await _service.List(isAdmin, name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpPost]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO> Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseDTO result = await _service.Create(DTO);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO> Detail([Required] Guid ProductID)
        {
            ResponseDTO response = await _service.Detail(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO> Update([Required] Guid ProductID, [Required] ProductCreateUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Update(ProductID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{ProductID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO> Delete([Required] Guid ProductID)
        {
            ResponseDTO response = await _service.Delete(ProductID);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
