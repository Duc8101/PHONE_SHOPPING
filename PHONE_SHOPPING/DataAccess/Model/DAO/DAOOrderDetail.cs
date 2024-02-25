using DataAccess.Entity;

namespace DataAccess.Model.DAO
{
    public class DAOOrderDetail : BaseDAO
    {
        public async Task CreateOrderDetail(OrderDetail detail)
        {
            await context.OrderDetails.AddAsync(detail);
            await context.SaveChangesAsync();
        }
    }
}
