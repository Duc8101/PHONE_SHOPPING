using DataAccess.Base;

namespace MVC.Services.IService
{
    public interface IHomeService
    {
        Task<ResponseBase> Index(string? name, int? CategoryID, int? page);
    }
}
