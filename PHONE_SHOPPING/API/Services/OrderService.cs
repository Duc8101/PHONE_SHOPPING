using AutoMapper;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO;
using DataAccess.Model.DAO;
using DataAccess.DTO.OrderDTO;
using DataAccess.Entity;
using System.Net;
using DataAccess.Const;
using DataAccess.Model;

namespace API.Services
{
    public class OrderService : BaseService
    {
        private readonly DAOUser daoUser = new DAOUser();
        private readonly DAOCart daoCart = new DAOCart();
        private readonly DAOProduct daoProduct = new DAOProduct();
        private readonly DAOOrder daoOrder = new DAOOrder();
        private readonly DAOOrderDetail daoDetail = new DAOOrderDetail();
        public OrderService(IMapper mapper) : base(mapper)
        {

        }

        public async Task<ResponseDTO<List<CartListDTO>?>> Create(OrderCreateDTO DTO)
        {
            try
            {
                User? user = await daoUser.getUser(DTO.UserId);
                if (user == null)
                {
                    return new ResponseDTO<List<CartListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await daoCart.getList(DTO.UserId);
                List<CartListDTO> data = mapper.Map<List<CartListDTO>>(list);
                if(DTO.Address == null || DTO.Address.Trim().Length == 0)
                {
                    return new ResponseDTO<List<CartListDTO>?>(data, "You have to input address", (int) HttpStatusCode.Conflict);
                }
                foreach(CartListDTO item in data)
                {
                    Product? product = await daoProduct.getProduct(item.ProductId);
                    if(product == null)
                    {
                        return new ResponseDTO<List<CartListDTO>?>(data, "Product " + item.ProductName + " not exist!!!", (int)HttpStatusCode.Conflict);
                    }
                    if(product.Quantity < item.Quantity)
                    {
                        return new ResponseDTO<List<CartListDTO>?>(data, "Product " + item.ProductName + " not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                    }
                }
                string body = UserUtil.BodyEmailForAdminReceiveOrder();
                List<string> emails = await daoUser.getEmailAdmins();
                if(emails.Count > 0)
                {
                    foreach (string email in emails)
                    {
                        await UserUtil.sendEmail("[PHONE SHOPPING] Notification for new order", body, email);
                    }
                }
                Order order = mapper.Map<Order>(DTO);
                order.OrderId = Guid.NewGuid();
                order.Address = DTO.Address.Trim();
                order.Status = OrderConst.STATUS_PENDING;
                order.CreatedAt = DateTime.Now;
                order.UpdateAt = DateTime.Now;
                order.IsDeleted = false;
                order.Note = null;
                await daoOrder.CreateOrder(order);
                foreach (CartListDTO item in data)
                {
                    OrderDetail detail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        CreatedAt = DateTime.Now,
                        UpdateAt = DateTime.Now,
                        IsDeleted = false
                    };
                    await daoDetail.CreateOrderDetail(detail);
                }
                foreach (Cart cart in list)
                {
                    cart.IsCheckout = true;
                    await daoCart.UpdateCart(cart);
                }
                return new ResponseDTO<List<CartListDTO>?>(data, "Check out successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
