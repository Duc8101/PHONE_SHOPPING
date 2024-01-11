using API.Services;
using DataAccess.DTO;
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
        public async Task<ResponseDTO<Dictionary<string, object>?>> List(int? CategoryID, [Required] int page = 1)
        {
            ResponseDTO<Dictionary<string, object>?> result = await service.List(CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }
    }
}
