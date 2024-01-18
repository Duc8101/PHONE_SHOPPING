using DataAccess.Const;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOOrder : BaseDAO
    {
        public async Task CreateOrder(Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }

        private IQueryable<Order> getQuery(Guid? UserID, string? status)
        {
            IQueryable<Order> query = context.Orders.Include(u => u.User).Where(o => o.IsDeleted == false);
            if(UserID != null)
            {
                query = query.Where(o => o.UserId == UserID);
            }
            if(status != null && status.Trim().Length > 0)
            {
                query = query.Where(o => o.Status == status.Trim());
            }
            query = query.OrderBy(o => (o.Status == OrderConst.STATUS_PENDING) ? 0 : 1).ThenByDescending(o => o.UpdateAt);
            return query;
        }
    }
}
