using DataAccess.DTO.CartDTO;
using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOCart : BaseDAO
    {
        public async Task<List<Cart>> getList(Guid UserID)
        {
            return await context.Carts.Include(c => c.Product).Where(c => c.UserId == UserID && c.IsCheckout == false && c.IsDeleted == false).ToListAsync();
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

        public async Task<Cart?> getCart(CartCreateRemoveDTO DTO)
        {
            return await context.Carts.FirstOrDefaultAsync(c => c.UserId == DTO.UserId && c.ProductId == DTO.ProductId && c.IsCheckout == false && c.IsDeleted == false);
        }
    }
}
