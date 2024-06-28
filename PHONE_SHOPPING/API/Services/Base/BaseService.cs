using AutoMapper;
using DataAccess.DBContext;

namespace API.Services.Base
{
    public class BaseService
    {
        internal readonly IMapper _mapper;
        internal readonly PhoneShoppingContext _context;
        public BaseService(IMapper mapper, PhoneShoppingContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
