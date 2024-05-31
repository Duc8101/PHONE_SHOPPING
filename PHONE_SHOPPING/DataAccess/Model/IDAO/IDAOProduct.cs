using DataAccess.Entity;

namespace DataAccess.Model.IDAO
{
    public interface IDAOProduct
    {
        Task<List<Product>> getList(string? name, int? CategoryID, int page);
        Task<int> getNumberPage(string? name, int? CategoryID);
        Task<Product?> getProduct(Guid ProductID);
        Task<bool> isExist(string ProductName);
        Task CreateProduct(Product product);
        Task<bool> isExist(string ProductName, Guid ProductID);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);
        Task SaveChanges();
    }
}
