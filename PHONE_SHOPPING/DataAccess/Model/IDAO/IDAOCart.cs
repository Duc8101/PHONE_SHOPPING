using DataAccess.DTO.CartDTO;
using DataAccess.Entity;

namespace DataAccess.Model.IDAO
{
    public interface IDAOCart
    {
        Task<List<Cart>> getList(Guid UserID);
        Task UpdateCart(Cart cart);
        Task DeleteCart(Cart cart);
        Task CreateCart(Cart cart);
        Task<Cart?> getCart(CartCreateRemoveDTO DTO);
    }
}
