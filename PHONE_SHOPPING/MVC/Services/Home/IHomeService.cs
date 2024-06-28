using Common.Base;

namespace MVC.Services.Home
{
    public interface IHomeService
    {
        Task<ResponseBase<Dictionary<string, object>?>> Index(string? name, int? CategoryID, int? page);
    }
}
