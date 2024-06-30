using API.Attributes;
using API.Services.Categories;
using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Enum;
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
        public ResponseBase List()
        {
            ResponseBase response = _service.ListAll();
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("Paged")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase List(string? name, [Required] int page = 1)
        {
            ResponseBase response = _service.ListPaged(name, page);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Create([Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{ID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Detail([Required] int ID)
        {
            ResponseBase response = _service.Detail(ID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public ResponseBase Update([Required] int ID, [Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Update(ID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }


    }
}
