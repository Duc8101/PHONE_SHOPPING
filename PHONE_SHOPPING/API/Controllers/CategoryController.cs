using API.Attributes;
using API.Services.Categories;
using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Enum;
using Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("All")]
        public ResponseBase<List<CategoryListDTO>?> List()
        {
            ResponseBase<List<CategoryListDTO>?> response = _service.ListAll();
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("Paged")]
        [Role<Pagination<CategoryListDTO>>(RoleEnum.Admin)]
        [Authorize<Pagination<CategoryListDTO>>]
        public ResponseBase<Pagination<CategoryListDTO>?> List(string? name, [Required] int page = 1)
        {
            ResponseBase<Pagination<CategoryListDTO>?> response = _service.ListPaged(name, page);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role<bool>(RoleEnum.Admin)]
        [Authorize<bool>]
        public ResponseBase<bool> Create([Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseBase<bool> response = _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{ID}")]
        [Role<CategoryListDTO>(RoleEnum.Admin)]
        [Authorize<CategoryListDTO>]
        public ResponseBase<CategoryListDTO?> Detail([Required] int ID)
        {
            ResponseBase<CategoryListDTO?> response = _service.Detail(ID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ID}")]
        [Role<CategoryListDTO>(RoleEnum.Admin)]
        [Authorize<CategoryListDTO>]
        public ResponseBase<CategoryListDTO?> Update([Required] int ID, [Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseBase<CategoryListDTO?> response = _service.Update(ID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }


    }
}
