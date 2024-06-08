using AutoMapper;
using DataAccess;

namespace API.Services.Service
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
