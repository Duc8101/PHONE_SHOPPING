using DataAccess.Base;

namespace MVC.Services.IService
{
    public interface IHomeService
    {
        Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page);
    }
}
