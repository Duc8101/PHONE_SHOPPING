using API.Services.IService;
using AutoMapper;
using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.Entity;
using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Service
{
    public class CartService : BaseService, ICartService
    {
        public CartService(IMapper mapper, PHONE_SHOPPINGContext context) : base(mapper, context)
        {

        }

        public async Task<ResponseDTO> List(Guid UserID)
        {
            try
            {
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == UserID);
                if (user == null)
                {
                    return new ResponseDTO(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await _context.Carts.Include(c => c.Product).Where(c => c.UserId == UserID && c.IsCheckout == false && c.IsDeleted == false).ToListAsync();
                List<CartListDTO> data = _mapper.Map<List<CartListDTO>>(list);
                return new ResponseDTO(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Create(CartCreateRemoveDTO DTO)
        {
            try
            {
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == DTO.UserId);
                if (user == null)
                {
                    return new ResponseDTO(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                Product? product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == DTO.ProductId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseDTO(false, "Not found product", (int)HttpStatusCode.NotFound);
                }
                Cart? cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == DTO.UserId && c.ProductId == DTO.ProductId && c.IsCheckout == false && c.IsDeleted == false);
                if (cart == null)
                {
                    cart = new Cart()
                    {
                        CartId = Guid.NewGuid(),
                        UserId = DTO.UserId,
                        ProductId = DTO.ProductId,
                        Quantity = 1,
                        IsCheckout = false,
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        IsDeleted = false,
                    };
                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    cart.Quantity++;
                    cart.UpdateAt = DateTime.Now;
                    _context.Carts.Update(cart);
                    await _context.SaveChangesAsync();
                }
                return new ResponseDTO(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Remove(CartCreateRemoveDTO DTO)
        {
            try
            {
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == DTO.UserId);
                if (user == null)
                {
                    return new ResponseDTO(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                Product? product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == DTO.ProductId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseDTO(false, "Product not exist", (int)HttpStatusCode.Conflict);
                }
                Cart? cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == DTO.UserId && c.ProductId == DTO.ProductId && c.IsCheckout == false && c.IsDeleted == false);
                if (cart == null)
                {
                    return new ResponseDTO(false, "Cart not exist", (int)HttpStatusCode.Conflict);
                }
                else
                {
                    if (cart.Quantity == 1)
                    {
                        cart.IsDeleted = true;
                        _context.Carts.Update(cart);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        cart.Quantity--;
                        cart.UpdateAt = DateTime.Now;
                        _context.Carts.Update(cart);
                        await _context.SaveChangesAsync();
                    }
                }
                return new ResponseDTO(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO> Delete(Guid UserID)
        {
            try
            {
                User? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == UserID);
                if (user == null)
                {
                    return new ResponseDTO(false, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await _context.Carts.Include(c => c.Product).Where(c => c.UserId == UserID && c.IsCheckout == false && c.IsDeleted == false).ToListAsync();
                foreach (Cart cart in list)
                {
                    cart.IsDeleted = true;
                    _context.Carts.Update(cart);
                    await _context.SaveChangesAsync();
                }
                return new ResponseDTO(false, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
