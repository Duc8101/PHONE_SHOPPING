using DataAccess.Const;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOOrder : BaseDAO
    {
        public DAOOrder(PHONE_SHOPPINGContext context) : base(context)
        {
        }

        public async Task CreateOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Order> getQuery(Guid? UserID, string? status)
        {
            IQueryable<Order> query = _context.Orders.Include(u => u.User);
            if(UserID.HasValue)
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

        public async Task<List<Order>> getList(Guid? UserID, string? status, int page)
        {
            IQueryable<Order> query = getQuery(UserID, status);
            return await query.Skip(PageSizeConst.MAX_ORDER_IN_PAGE * (page - 1)).Take(PageSizeConst.MAX_ORDER_IN_PAGE).ToListAsync();
        }

        public async Task<int> getNumberPage(Guid? UserID, string? status)
        {
            IQueryable<Order> query = getQuery(UserID, status);
            int count = await query.CountAsync();
            return (int)Math.Ceiling((double)count / PageSizeConst.MAX_ORDER_IN_PAGE);
        }

        public async Task<Order?> getOrder(Guid OrderID)
        {
            return await _context.Orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(o => o.Product).ThenInclude(o => o.Category).SingleOrDefaultAsync(o => o.OrderId == OrderID);
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
