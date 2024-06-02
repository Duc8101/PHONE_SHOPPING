using AutoMapper;
using DataAccess.Model;

namespace API.Services.Service
{
    public class BaseService
    {
        internal readonly IMapper _mapper;
        internal readonly PHONE_SHOPPINGContext _context;
        public BaseService(IMapper mapper, PHONE_SHOPPINGContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
