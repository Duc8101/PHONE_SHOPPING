using DataAccess.Entity;

namespace DataAccess.Model.DAO
{
    public class DAOOrder : BaseDAO
    {
        public async Task CreateOrder(Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
    }
}
