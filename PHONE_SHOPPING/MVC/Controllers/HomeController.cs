using DataAccess.Base;
using DataAccess.DTO.CategoryDTO;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.IService;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _service;
        public HomeController(IHomeService service)
        {
            _service = service;
        }
        public async Task<bool> Index()
        {
            ResponseBase result = await _service.Index(null, null, null);
            return ((List<CategoryListDTO>?) result.Data) == null;
        }

    }
}
