using API.Attributes;
using API.Services.IService;
using DataAccess.DTO;
using DataAccess.DTO.CategoryDTO;
using DataAccess.Enum;
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
        public async Task<ResponseDTO> List()
        {
            ResponseDTO response = await _service.ListAll();
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("Paged")]
        public async Task<ResponseDTO> List(string? name, [Required] int page = 1)
        {
            ResponseDTO response = await _service.ListPaged(name, page);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO> Create([Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{ID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO> Detail([Required] int ID)
        {
            ResponseDTO response = await _service.Detail(ID);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{ID}")]
        [Role(RoleEnum.Admin)]
        [Authorize]
        public async Task<ResponseDTO> Update([Required] int ID, [Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseDTO response = await _service.Update(ID, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

    }
}
