using DataAccess.Entity;

namespace DataAccess.Model.IDAO
{
    public interface IDAOCategory
    {
        Task<List<Category>> getList();
        Task<List<Category>> getList(string? name, int page);
        Task<int> getNumberPage(string? name);
        Task<bool> isExist(string name);
        Task CreateCategory(Category category);
        Task<Category?> getCategory(int ID);
        Task<bool> isExist(string name, int ID);
        Task UpdateCategory(Category category);
    }
}
