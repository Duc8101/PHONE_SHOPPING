using API.Services;
using DataAccess.DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<ProductListDTO>> List()
        {
            /* List<Product> list = context.Products.Include(p => p.Category).ToList();
             List<ProductListDTO> result = mapper.Map<List<ProductListDTO>>(list);*/
            return await service.List();
        }
    }
}
