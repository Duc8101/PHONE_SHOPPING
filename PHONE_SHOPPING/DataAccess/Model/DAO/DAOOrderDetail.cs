using DataAccess.Entity;

namespace DataAccess.Model.DAO
{
    public class DAOOrderDetail : PHONE_SHOPPINGContext
    {
        public async Task CreateOrderDetail(OrderDetail detail)
        {
            await OrderDetails.AddAsync(detail);
            await SaveChangesAsync();
        }
    }
}
