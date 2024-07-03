using AutoMapper;
using DataAccess.DBContext;

namespace API.Services.Base
{
    public class BaseService
    {
        internal readonly IMapper _mapper;
        internal readonly PHONE_STOREContext _context;
        public BaseService(IMapper mapper, PHONE_STOREContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
