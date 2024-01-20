﻿using API.Services;
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
        public async Task<ResponseDTO<PagedResultDTO<ProductListDTO>?>> List(string? name, int? CategoryID, [Required] bool isAdmin = false,[Required] int page = 1)
        {
            ResponseDTO<PagedResultDTO<ProductListDTO>?> result = await service.List(isAdmin,name, CategoryID, page);
            Response.StatusCode = result.Code;
            return result;
        }
    }
}
