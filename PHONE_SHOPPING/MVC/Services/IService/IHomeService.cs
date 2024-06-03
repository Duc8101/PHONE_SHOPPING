using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IHomeService
    {
        Task<ResponseDTO> Index(string? name, int? CategoryID, int? page);
    }
}
