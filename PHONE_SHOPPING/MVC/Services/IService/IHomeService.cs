using DataAccess.DTO;

namespace MVC.Services.IService
{
    public interface IHomeService
    {
        Task<ResponseDTO<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page);
    }
}
