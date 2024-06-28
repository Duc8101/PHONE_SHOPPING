using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.DTO.CartDTO;
using Common.Entity;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Carts
{
    public class CartService : BaseService, ICartService
    {
        public CartService(IMapper mapper, PhoneShoppingContext context) : base(mapper, context)
        {

        }

        public ResponseBase<List<CartListDTO>?> List(Guid UserID)
        {
            try
            {
                List<Cart> list = _context.Carts.Include(c => c.Product).Where(c => c.UserId == UserID && c.IsCheckout == false && c.IsDeleted == false).ToList();
                List<CartListDTO> data = _mapper.Map<List<CartListDTO>>(list);
                return new ResponseBase<List<CartListDTO>?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase<bool> Create(CartCreateDTO DTO, Guid userId)
        {
            try
            {
                Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == DTO.ProductId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase<bool>(false, "Not found product", (int)HttpStatusCode.NotFound);
                }
                Cart? cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == DTO.ProductId && c.IsCheckout == false && c.IsDeleted == false);
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        CartId = Guid.NewGuid(),
                        UserId = userId,
                        ProductId = DTO.ProductId,
                        Quantity = 1,
                        IsCheckout = false,
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        IsDeleted = false,
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }
                else
                {
                    cart.Quantity++;
                    cart.UpdateAt = DateTime.Now;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase<bool>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase<bool> Delete(Guid productId, Guid userId)
        {
            try
            {
                User? user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    return new ResponseBase<bool>(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase<bool>(false, "Product not exist", (int)HttpStatusCode.NotFound);
                }
                Cart? cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId && c.IsCheckout == false && c.IsDeleted == false);
                if (cart == null)
                {
                    return new ResponseBase<bool>(false, "Cart not exist", (int)HttpStatusCode.NotFound);
                }
                else
                {
                    if (cart.Quantity == 1)
                    {
                        cart.IsDeleted = true;
                    }
                    else
                    {
                        cart.Quantity--;
                        cart.UpdateAt = DateTime.Now;
                    }
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase<bool>(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
