using DataAccess.Entity;

namespace DataAccess.Model.IDAO
{
    public interface IDAOOrder
    {
        Task CreateOrder(Order order);
        Task<List<Order>> getList(Guid? UserID, string? status, int page);
        Task<int> getNumberPage(Guid? UserID, string? status);
        Task<Order?> getOrder(Guid OrderID);
        Task UpdateOrder(Order order);
    }
}
