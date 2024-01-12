using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOCart : BaseDAO
    {
        public async Task<List<Cart>> getList(Guid UserID)
        {
            return await context.Carts.Where(c => c.UserId == UserID && c.IsDeleted == false).ToListAsync();
        }

        public async Task UpdateCart(Cart cart)
        {
            context.Carts.Update(cart);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCart(Cart cart)
        {
            cart.IsDeleted = true;
            await UpdateCart(cart);
        }

        public async Task CreateCart(Cart cart)
        {
            await context.Carts.AddAsync(cart);
            await context.SaveChangesAsync();
        }
    }
}
