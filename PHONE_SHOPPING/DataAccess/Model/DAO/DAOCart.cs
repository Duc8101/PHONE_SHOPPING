using DataAccess.DTO.CartDTO;
using DataAccess.Entity;
using DataAccess.Model.IDAO;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Model.DAO
{
    public class DAOCart : BaseDAO, IDAOCart
    {
        public DAOCart(PHONE_SHOPPINGContext context) : base(context)
        {
        }

        public async Task<List<Cart>> getList(Guid UserID)
        {
            return await _context.Carts.Include(c => c.Product).Where(c => c.UserId == UserID && c.IsCheckout == false && c.IsDeleted == false).ToListAsync();
        }

        public async Task UpdateCart(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCart(Cart cart)
        {
            cart.IsDeleted = true;
            await UpdateCart(cart);
        }

        public async Task CreateCart(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> getCart(CartCreateRemoveDTO DTO)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == DTO.UserId && c.ProductId == DTO.ProductId && c.IsCheckout == false && c.IsDeleted == false);
        }
    }
}
