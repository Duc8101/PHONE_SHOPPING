using DataAccess.Entity;

namespace DataAccess.Model.IDAO
{
    public interface IDAOOrderDetail
    {
        Task CreateOrderDetail(OrderDetail detail);
    }
}
