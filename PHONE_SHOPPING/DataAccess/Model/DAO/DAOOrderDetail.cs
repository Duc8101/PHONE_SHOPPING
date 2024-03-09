using DataAccess.Entity;

namespace DataAccess.Model.DAO
{
    public class DAOOrderDetail : BaseDAO
    {
        public DAOOrderDetail(PHONE_SHOPPINGContext context) : base(context)
        {
        }

        public async Task CreateOrderDetail(OrderDetail detail)
        {
            await _context.OrderDetails.AddAsync(detail);
            await _context.SaveChangesAsync();
        }
    }
}
