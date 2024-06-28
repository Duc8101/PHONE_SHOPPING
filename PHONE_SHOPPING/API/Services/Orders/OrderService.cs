using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.Const;
using Common.DTO.CartDTO;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.Entity;
using Common.Enum;
using Common.Pagination;
using DataAccess.DBContext;
using DataAccess.Util;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Orders
{
    public class OrderService : BaseService, IOrderService
    {
        public OrderService(IMapper mapper, PhoneShoppingContext context) : base(mapper, context)
        {

        }

        public async Task<ResponseBase<List<CartListDTO>?>> Create(OrderCreateDTO DTO, Guid userId)
        {
            try
            {
                List<Cart> list = _context.Carts.Include(c => c.Product).Where(c => c.UserId == userId && c.IsCheckout == false && c.IsDeleted == false).ToList();
                List<CartListDTO> data = _mapper.Map<List<CartListDTO>>(list);
                if (DTO.Address == null || DTO.Address.Trim().Length == 0)
                {
                    return new ResponseBase<List<CartListDTO>?>(data, "You have to input address", (int)HttpStatusCode.Conflict);
                }
                foreach (CartListDTO item in data)
                {
                    Product? product = _context.Products.Include(p => p.Category).SingleOrDefault(p => p.ProductId == item.ProductId && p.IsDeleted == false);
                    if (product == null)
                    {
                        return new ResponseBase<List<CartListDTO>?>(data, "Product " + item.ProductName + " not exist!!!", (int)HttpStatusCode.NotFound);
                    }
                    if (product.Quantity < item.Quantity)
                    {
                        return new ResponseBase<List<CartListDTO>?>(data, "Product " + item.ProductName + " not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                    }
                }
                string body = UserUtil.BodyEmailForAdminReceiveOrder();
                List<string> emails = _context.Users.Where(u => u.RoleId == (int)RoleEnum.Admin).Select(u => u.Email).ToList();
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
                order.UserId = userId;
                _context.Orders.Add(order);
                _context.SaveChanges();
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
                    _context.OrderDetails.Add(detail);
                    _context.SaveChanges();
                }
                foreach (Cart cart in list)
                {
                    cart.IsCheckout = true;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                }
                return new ResponseBase<List<CartListDTO>?>(data, "Check out successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<CartListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        private IQueryable<Order> getQuery(Guid? UserID, string? status)
        {
            IQueryable<Order> query = _context.Orders.Include(u => u.User);
            if (UserID.HasValue)
            {
                query = query.Where(o => o.UserId == UserID);
            }
            if (status != null && status.Trim().Length > 0)
            {
                query = query.Where(o => o.Status == status.Trim());
            }
            query = query.OrderBy(o => o.Status == OrderConst.STATUS_PENDING ? 0 : 1).ThenByDescending(o => o.UpdateAt);
            return query;
        }

        public ResponseBase<Pagination<OrderListDTO>?> List(Guid? UserID, string? status, bool isAdmin, int page)
        {
            try
            {
                IQueryable<Order> query = getQuery(UserID, status);
                List<Order> list = query.Skip(PageSizeConst.MAX_ORDER_IN_PAGE * (page - 1)).Take(PageSizeConst.MAX_ORDER_IN_PAGE)
                    .ToList();
                List<OrderListDTO> result = _mapper.Map<List<OrderListDTO>>(list);
                int count = query.Count();
                int number = (int)Math.Ceiling((double)count / PageSizeConst.MAX_ORDER_IN_PAGE);
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
                Pagination<OrderListDTO> data = new Pagination<OrderListDTO>()
                {
                    PageSelected = page,
                    NumberPage = number,
                    Results = result,
                    FIRST_URL = firstURL,
                    LAST_URL = lastURL,
                    NEXT_URL = nextURL,
                    PRE_URL = preURL,
                };
                return new ResponseBase<Pagination<OrderListDTO>?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<Pagination<OrderListDTO>?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase<OrderDetailDTO?> Detail(Guid OrderID)
        {
            try
            {
                Order? order = _context.Orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Category).FirstOrDefault(o => o.OrderId == OrderID);
                if (order == null)
                {
                    return new ResponseBase<OrderDetailDTO?>(null, "Not found order", (int)HttpStatusCode.NotFound);
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
                return new ResponseBase<OrderDetailDTO?>(data, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseBase<OrderDetailDTO?>> Update(Guid OrderID, OrderUpdateDTO DTO)
        {
            try
            {
                Order? order = _context.Orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Category).FirstOrDefault(o => o.OrderId == OrderID);
                if (order == null)
                {
                    return new ResponseBase<OrderDetailDTO?>(null, "Not found order", (int)HttpStatusCode.NotFound);
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
                    return new ResponseBase<OrderDetailDTO?>(data, "Order was approved or rejected", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Status.Trim() == OrderConst.STATUS_REJECTED || DTO.Status.Trim() == OrderConst.STATUS_PENDING)
                {
                    if (DTO.Status.Trim() == OrderConst.STATUS_REJECTED && (DTO.Note == null || DTO.Note.Trim().Length == 0))
                    {
                        return new ResponseBase<OrderDetailDTO?>(data, "When status is " + OrderConst.STATUS_REJECTED + ", you have to note the reason why rejected", (int)HttpStatusCode.Conflict);
                    }
                    order.Status = DTO.Status.Trim();
                    order.UpdateAt = DateTime.Now;
                    order.Note = DTO.Note == null || DTO.Note.Trim().Length == 0 ? null : DTO.Note.Trim();
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    data.Status = order.Status;
                    data.Note = order.Note;
                    return new ResponseBase<OrderDetailDTO?>(data, "Update successful");
                }
                if (DTO.Status.Trim() == OrderConst.STATUS_APPROVED)
                {
                    order.Status = DTO.Status.Trim();
                    order.Note = DTO.Note == null || DTO.Note.Trim().Length == 0 ? null : DTO.Note.Trim();
                    data.Status = order.Status;
                    data.Note = order.Note;
                    foreach (OrderDetail item in list)
                    {
                        Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == item.ProductId && p.IsDeleted == false);
                        if (product == null)
                        {
                            return new ResponseBase<OrderDetailDTO?>(data, "Product " + item.Product.ProductName + " not exist!!!", (int)HttpStatusCode.Conflict);
                        }
                        if (product.Quantity < item.Quantity)
                        {
                            return new ResponseBase<OrderDetailDTO?>(data, "Product " + item.Product.ProductName + " not have enough quantity!!!", (int)HttpStatusCode.Conflict);
                        }
                    }
                    string body = UserUtil.BodyEmailForApproveOrder(list);
                    await UserUtil.sendEmail("[PHONE SHOPPING] Notification for approve order", body, order.User.Email);
                    foreach (OrderDetail item in list)
                    {
                        item.Product.Quantity = item.Product.Quantity - item.Quantity;
                        item.Product.UpdateAt = DateTime.Now;
                        _context.SaveChanges();
                    }
                    order.UpdateAt = DateTime.Now;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    return new ResponseBase<OrderDetailDTO?>(data, "Update successful");
                }
                return new ResponseBase<OrderDetailDTO?>(data, "Status update must be " + OrderConst.STATUS_APPROVED + "," + OrderConst.STATUS_REJECTED + " or " + OrderConst.STATUS_PENDING, (int)HttpStatusCode.Conflict);
            }
            catch (Exception ex)
            {
                return new ResponseBase<OrderDetailDTO?>(null, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
