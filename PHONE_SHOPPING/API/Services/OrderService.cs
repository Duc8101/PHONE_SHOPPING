using AutoMapper;
using DataAccess.Const;
using DataAccess.DTO;
using DataAccess.DTO.CartDTO;
using DataAccess.DTO.OrderDetailDTO;
using DataAccess.DTO.OrderDTO;
using DataAccess.Entity;
using DataAccess.Model;
using DataAccess.Model.DAO;
using System.Net;

namespace API.Services
{
    public class OrderService : BaseService
    {
        private readonly DAOUser _daoUser;
        private readonly DAOCart _daoCart;
        private readonly DAOProduct _daoProduct;
        private readonly DAOOrder _daoOrder;
        private readonly DAOOrderDetail _daoDetail;
        public OrderService(IMapper mapper, DAOUser daoUser, DAOCart daoCart, DAOProduct daoProduct, DAOOrder daoOrder, DAOOrderDetail daoDetail) : base(mapper)
        {
            _daoUser = daoUser;
            _daoCart = daoCart;
            _daoProduct = daoProduct;
            _daoOrder = daoOrder;
            _daoDetail = daoDetail;
        }

        public async Task<ResponseDTO<List<CartListDTO>?>> Create(OrderCreateDTO DTO)
        {
            try
            {
                User? user = await _daoUser.getUser(DTO.UserId);
                if (user == null)
                {
                    return new ResponseDTO<List<CartListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                }
                List<Cart> list = await _daoCart.getList(DTO.UserId);
                List<CartListDTO> data = _mapper.Map<List<CartListDTO>>(list);
                if (DTO.Address == null || DTO.Address.Trim().Length == 0)
                {
                    return new ResponseDTO<List<CartListDTO>?>(data, "You have to input address", (int)HttpStatusCode.Conflict);
                }
                foreach (CartListDTO item in data)
                {
                    Product? product = await _daoProduct.getProduct(item.ProductId);
                    if (product == null)
                    {
                        return new ResponseDTO<List<CartListDTO>?>(data, "Product " + item.ProductName + " not exist!!!", (int)HttpStatusCode.NotFound);
                    }
                    if (product.Quantity < item.Quantity)
                    {
                        return new ResponseDTO<List<CartListDTO>?>(data, "Product " + item.ProductName + " not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                    }
                }
                string body = UserUtil.BodyEmailForAdminReceiveOrder();
                List<string> emails = await _daoUser.getEmailAdmins();
                if (emails.Count > 0)
                {
                    foreach (string email in emails)
                    {
                        await UserUtil.sendEmail("[PHONE SHOPPING] Notification for new order", body, email);
                    }
                }
                Order order = _mapper.Map<Order>(DTO);
                order.OrderId = Guid.NewGuid();
                order.Address = DTO.Address.Trim();
                order.Status = OrderConst.STATUS_PENDING;
                order.CreatedAt = DateTime.Now;
                order.UpdateAt = DateTime.Now;
                order.IsDeleted = false;
                order.Note = null;
                await _daoOrder.CreateOrder(order);
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
                    await _daoDetail.CreateOrderDetail(detail);
                }
                foreach (Cart cart in list)
                {
                    cart.IsCheckout = true;
                    await _daoCart.UpdateCart(cart);
                }
                return new ResponseDTO<List<CartListDTO>?>(data, "Check out successful");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<PagedResultDTO<OrderListDTO>?>> List(Guid? UserID, string? status, bool isAdmin, int page)
        {
            try
            {
                if (UserID != null)
                {
                    User? user = await _daoUser.getUser(UserID.Value);
                    if (user == null)
                    {
                        return new ResponseDTO<PagedResultDTO<OrderListDTO>?>(null, "Not found user", (int)HttpStatusCode.NotFound);
                    }
                }
                List<Order> list = await _daoOrder.getList(UserID, status, page);
                List<OrderListDTO> result = _mapper.Map<List<OrderListDTO>>(list);
                int number = await _daoOrder.getNumberPage(UserID, status);
                int prePage = page - 1;
                int nextPage = page + 1;
                string preURL;
                string nextURL;
                string firstURL;
                string lastURL;
                if (isAdmin)
                {
                    if (status == null || status.Trim().Length == 0)
                    {
                        preURL = "/ManagerOrder?page=" + prePage;
                        nextURL = "/ManagerOrder?page=" + nextPage;
                        firstURL = "/ManagerOrder";
                        lastURL = "/ManagerOrder?page=" + number;
                    }
                    else
                    {
                        preURL = "/ManagerOrder?status=" + status.Trim() + "&page=" + prePage;
                        nextURL = "/ManagerOrder?status=" + status.Trim() + "&page=" + nextPage;
                        firstURL = "/ManagerOrder?status=" + status.Trim();
                        lastURL = "/ManagerOrder?status=" + status.Trim() + "&page=" + number;
                    }
                }
                else
                {
                    preURL = "/MyOrder?page=" + prePage;
                    nextURL = "/MyOrder?page=" + nextPage;
                    firstURL = "/MyOrder";
                    lastURL = "/MyOrder?page=" + number;
                }
                PagedResultDTO<OrderListDTO> data = new PagedResultDTO<OrderListDTO>()
                {
                    PageSelected = page,
                    NumberPage = number,
                    Results = result,
                    FIRST_URL = firstURL,
                    LAST_URL = lastURL,
                    NEXT_URL = nextURL,
                    PRE_URL = preURL,
                };
                return new ResponseDTO<PagedResultDTO<OrderListDTO>?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<PagedResultDTO<OrderListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<OrderDetailDTO?>> Detail(Guid OrderID)
        {
            try
            {
                Order? order = await _daoOrder.getOrder(OrderID);
                if (order == null)
                {
                    return new ResponseDTO<OrderDetailDTO?>(null, "Not found order", (int)HttpStatusCode.NotFound);
                }
                List<OrderDetail> list = order.OrderDetails.ToList();
                List<DetailDTO> DTOs = _mapper.Map<List<DetailDTO>>(list);
                OrderDetailDTO data = new OrderDetailDTO()
                {
                    OrderId = OrderID,
                    UserId = order.UserId,
                    Username = order.User.Username,
                    Status = order.Status,
                    Address = order.Address,
                    Note = order.Note,
                    OrderDate = order.CreatedAt,
                    DetailDTOs = DTOs,
                };
                return new ResponseDTO<OrderDetailDTO?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDTO<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO)
        {
            try
            {
                Order? order = await _daoOrder.getOrder(OrderID);
                if (order == null)
                {
                    return new ResponseDTO<OrderDetailDTO?>(null, "Not found order", (int)HttpStatusCode.NotFound);
                }
                List<OrderDetail> list = order.OrderDetails.ToList();
                List<DetailDTO> DTOs = _mapper.Map<List<DetailDTO>>(list);
                OrderDetailDTO data = new OrderDetailDTO()
                {
                    OrderId = OrderID,
                    UserId = order.UserId,
                    Username = order.User.Username,
                    Status = order.Status,
                    Address = order.Address,
                    Note = order.Note,
                    OrderDate = order.CreatedAt,
                    DetailDTOs = DTOs,
                };
                if (order.Status == OrderConst.STATUS_REJECTED || order.Status == OrderConst.STATUS_APPROVED)
                {
                    return new ResponseDTO<OrderDetailDTO?>(data, "Order was approved or rejected", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Status.Trim() == OrderConst.STATUS_REJECTED || DTO.Status.Trim() == OrderConst.STATUS_PENDING)
                {
                    order.Status = DTO.Status.Trim();
                    order.UpdateAt = DateTime.Now;
                    order.Note = DTO.Note == null || DTO.Note.Trim().Length == 0 ? null : DTO.Note.Trim();
                    await _daoOrder.UpdateOrder(order);
                    data.Status = order.Status;
                    data.Note = order.Note;
                    return new ResponseDTO<OrderDetailDTO?>(data, "Update successful");
                }
                if(DTO.Status.Trim() == OrderConst.STATUS_APPROVED)
                {
                    order.Status = DTO.Status.Trim();
                    order.Note = DTO.Note == null || DTO.Note.Trim().Length == 0 ? null : DTO.Note.Trim();
                    data.Status = order.Status;
                    data.Note = order.Note;
                    foreach (OrderDetail item in list)
                    {
                        Product? product = await _daoProduct.getProduct(item.ProductId);
                        if (product == null)
                        {
                            return new ResponseDTO<OrderDetailDTO?>(data, "Product " + item.Product.ProductName + " not exist!!!", (int)HttpStatusCode.NotFound);
                        }
                        if (product.Quantity < item.Quantity)
                        {
                            return new ResponseDTO<OrderDetailDTO?>(data, "Product " + item.Product.ProductName + " not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                        }
                    }
                    string body = UserUtil.BodyEmailForApproveOrder(list);
                    await UserUtil.sendEmail("[PHONE SHOPPING] Notification for approve order", body, order.User.Email);
                    foreach (OrderDetail item in list)
                    {
                        item.Product.Quantity = item.Product.Quantity - item.Quantity;
                        item.Product.UpdateAt = DateTime.Now;
                        await _daoProduct.SaveChanges();
                    }
                    order.UpdateAt = DateTime.Now;
                    await _daoOrder.UpdateOrder(order);
                    return new ResponseDTO<OrderDetailDTO?>(data, "Update successful");
                }
                return new ResponseDTO<OrderDetailDTO?>(data, "Status update must be " + OrderConst.STATUS_APPROVED + "," + OrderConst.STATUS_REJECTED + " or " + OrderConst.STATUS_PENDING, (int)HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
